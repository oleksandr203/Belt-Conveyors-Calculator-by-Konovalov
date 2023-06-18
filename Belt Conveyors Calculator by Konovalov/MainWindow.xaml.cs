using Microsoft.Data.SqlClient;
using System.Configuration;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calculator;
        StringBuilder resultSB = new StringBuilder();
        SqlConnection connection = new SqlConnection();
        string provider;       
        string catalog;       

        public MainWindow()
        {
            InitializeComponent();           
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            calculator = new Calculator();            
            widthComboBoxList.ItemsSource = calculator.standartBeltWidth;
            productivityValueTextBox.Text = calculator.Productivity.ToString();
            widthComboBoxList.SelectedIndex = 0;
            lenghtOfConveyorTextBox.Text = calculator.LenghtOfConveyor.ToString();
            angleOfBeltTextBox.Text = calculator.AngleOfConveyor.ToString();
            speedOfBeltTextBox.Text = calculator.SpeedOfConveyor.ToString();
            TextBoxRatioOrDiametr.Text = calculator.Ratio.ToString();            
            AppSettingsReader ar = new AppSettingsReader();
            catalog = (string)ar.GetValue("initialCatalog", typeof(string));
            provider = (string)ar.GetValue("provider", typeof(string));
            await Task.Run(() => ConnectDB(catalog, provider));
        }

        private Task ConnectDB(string _initialCatalog, string _dataSourse)
        {            
            var cStringBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = _initialCatalog,
                DataSource = _dataSourse,
                ConnectTimeout = 10,
                IntegratedSecurity = true,
                TrustServerCertificate = true
            };           
            connection.ConnectionString = cStringBuilder.ConnectionString;
            try
            {
                TryOpenConnection(connection);
            }
            catch(Exception ex)
            {
                this.Dispatcher.Invoke(() => statusBar_1.Content = ex.Message);
            } 
            if (connection.State != ConnectionState.Open)
            {   
                 calculator.FillListOfReducerByConfigBase();
                 this.Dispatcher.Invoke(() => statusBar_1.Content = "Using inner DB");
                 this.Dispatcher.Invoke(() => bdButtonBorder.BorderBrush = Brushes.Orange);
            }
            return Task.CompletedTask;
        }

        private void TryOpenConnection(SqlConnection _connection)
        {
            _connection.Open();
            string sql = "Select * From Models";
            SqlCommand sqlCommand = new SqlCommand(sql, _connection);
            using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    Reducer reducer = new Reducer((int)sqlDataReader["Id"], (string)sqlDataReader["Name"], (int)sqlDataReader["Torque"], (double)sqlDataReader["Ratio"]);
                    calculator.reducerList.Add(reducer);
                }
            }            
            this.Dispatcher.Invoke(() => bdButtonBorder.BorderBrush = Brushes.Green);           
            this.Dispatcher.Invoke(() => statusBar_1.Content = "Ready");
            this.Dispatcher.Invoke(() => bdButton.Content = "DB connected");
            
           //this.Dispatcher.Invoke(() => bdButton.IsEnabled = false);            
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            resultSB.Clear();
            calculator.CalculateSimpleEnginePower();
            calculator.CalculateExtendedEnginePower();
            resultTextBlock.Text = $"Required power (simple method): {calculator.SimpleMethodEnginePower:F2} kW\r\n" +
                $"Required power (extension method): {calculator.ExtendedMethodEnginePower:F2} kW *";                
            resultTextBlockMotor.Text = $"Standart electric motor: {calculator.SelectMotorPower()} kW {calculator.rpmBase[2]} rpm";
            statusBar_1.Content = "Done!";
            calculator.SelectReducer();            
            resultTextBlockReducer.Text = calculator.FittingReducer;
            GetRatioOrPulleyDiamenet();
            textBlockHeadPulley.Text = $"Diameter of pulley: {((double)calculator.HeadPulleyDiameter/1000):F2} m";
            textBlockRatio.Text = $"Ratio of reducer: {calculator.Ratio:F1}";
            TextBlockAddiotionInfo.Text = $" step of idlers: {calculator._stepOfWorkingIdler} mm,\n step of return idlers: {calculator._stepOfIdleIdler} mm" +
                $",\n thickness of belt: {calculator._thicknessOfBelt} mm";
            FillResultSb();
            statusBar_1.Content = "Done!";
            textBlockTest.Text = resultSB.ToString();
        }
        
        private void FillResultSb()
        {
            resultSB.AppendLine($"***Main characteristics of conveyor by Belt Conveyor Calculator by Konovalov*** \n");           
            resultSB.AppendLine("Calculation based on:");
            resultSB.AppendLine($"\tProducticity: \t\t{calculator.Productivity} tonns per hour");
            resultSB.AppendLine($"\tWidth of belt: \t\t{calculator.WidthOfBelt} mm");
            resultSB.AppendLine($"\tLenght of conveyor: \t{calculator.LenghtOfConveyor} m");
            resultSB.AppendLine($"\tAngle of conveyor: \t{calculator.AngleOfConveyor} degree");
            resultSB.AppendLine($"\tSpeed of belt: \t\t{calculator.SpeedOfConveyor} m/s \n" +
                $"\tStep of idlers: \t\t{calculator._stepOfWorkingIdler} mm,\n " +
                $"\tStep of return idlers: \t{calculator._stepOfIdleIdler} mm,\n" +
                $"\tThickness of belt: \t\t{calculator._thicknessOfBelt} mm");
            resultSB.AppendLine($"Required power (simple method): \t{calculator.SimpleMethodEnginePower:F2} kW \n" +
                $"Required power (extension method): \t{calculator.ExtendedMethodEnginePower:F2} kW \n" +
                $"Standart power of engine: \t\t{calculator.SelectMotorPower()} kW \n" +
                $"Fitting reducer: \t\t\t{calculator.FittingReducer} \n" +
                $"Ratio: \t\t\t\t{calculator.Ratio} (motor speed: {calculator.rpmBase[2]})\n" +
                $"Diameter of Head Pulley: \t\t{calculator.HeadPulleyDiameter} mm \n" +
                $" ");
            
            resultSB.AppendLine($"\tDate: {DateTime.Now}");
        }

        public void FillCalcReduser(Reducer reducer)
        {
            calculator.reducerList.Add(reducer);
        }

        private void GetRatioOrPulleyDiamenet()
        {
            if (ratioOrDiameterComboBox.SelectedIndex == 0)
            {
                calculator.CalculateHaedPulleyCharacteristics();
            }
            else
            if (ratioOrDiameterComboBox.SelectedIndex == 1)
            {
                calculator.CalculateRatio();
            }
        }


        private void productivityValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (productivityValueTextBox.Text != "")
            {
                try
                {
                    calculator.SetProductivity(Convert.ToInt32(productivityValueTextBox.Text));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                productivityValueTextBox.Text = "0";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lenghtOfConveyorTextBox.Text != "")
            {
                try
                {
                    calculator.SetLenght(Convert.ToInt32(lenghtOfConveyorTextBox.Text));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                lenghtOfConveyorTextBox.Text = "0";
            }
        }

        private void widthComboBoxList_LostFocus(object sender, RoutedEventArgs e)
        {
            calculator.SetWidthOfBelt(Convert.ToInt32(widthComboBoxList.Text));
        }

        private void angleOfBelt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (angleOfBeltTextBox.Text != "")
            {
                try
                {
                    calculator.SetAngle(Convert.ToInt32(angleOfBeltTextBox.Text));
                }
                catch
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                angleOfBeltTextBox.Text = "0";
            }
        }

        private void speedOfBelt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (speedOfBeltTextBox.Text != "")
            {
                try
                {
                    calculator.SetSpeed(Convert.ToDouble(speedOfBeltTextBox.Text));
                }
                catch
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                speedOfBeltTextBox.Text = "0";
            }
        }
        
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            statusBar_1.Content = "Ready, enter data please";
        }        

        private void ratioTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ratioOrDiameterComboBox.SelectedIndex == 0)
            {
                try
                {
                    calculator.SetRatio(Convert.ToDouble(TextBoxRatioOrDiametr.Text));
                }
                catch
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else if(ratioOrDiameterComboBox.SelectedIndex == 1)
            {
                try
                {
                    calculator.SetDiameterOfHeadPulley(Convert.ToInt32(TextBoxRatioOrDiametr.Text));
                }
                catch
                {
                    MessageBox.Show("Only numbers must be");
                }
            }            
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.Key == System.Windows.Input.Key.Escape)
            {                
                if(MessageBox.Show("Are you sure?", "Exit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.Close();
                }                
            }
        }

        private void bdButton_Click(object sender, RoutedEventArgs e)
        {
            if(connection.State != ConnectionState.Open)
            {
                Window1 win = new Window1();
                win.ShowDialog();
                if(win.DialogResult == true)
                {
                    ConnectDB(win.initialCatalog, win.provider);
                }                
            }                        
        }
    }
}
