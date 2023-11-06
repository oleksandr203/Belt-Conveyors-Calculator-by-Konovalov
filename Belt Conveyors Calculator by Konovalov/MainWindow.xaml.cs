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
using Microsoft.Win32;
using System.IO;
using System.Windows.Documents;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calculator = new Calculator();
        StringBuilder resultSB = new StringBuilder();
        SqlConnection connection = new SqlConnection();
        string provider = "";       
        string catalog = "";       

        public MainWindow()
        {
            InitializeComponent();           
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {          
            widthComboBoxList.ItemsSource = calculator.standartBeltWidth;
            productivityValueTextBox.Text = calculator.Productivity.ToString();
            widthComboBoxList.SelectedIndex = 0;
            lenghtOfConveyorTextBox.Text = calculator.LenghtOfConveyor.ToString();
            angleOfBeltTextBox.Text = calculator.AngleOfConveyor.ToString();
            speedOfBeltTextBox.Text = calculator.SpeedOfConveyor.ToString();
            TextBoxRatioOrDiametr.Text = calculator.Ratio.ToString();
            calculator.FillListOfReducerByConfigBase();
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
            try
            {
                connection.ConnectionString = cStringBuilder.ConnectionString;
            }
            catch { }
            
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
                calculator.reducerList.Clear();
                while (sqlDataReader.Read())
                {
                    Reducer<int, double> reducer = new ((int)sqlDataReader["Id"], (string)sqlDataReader["Name"], (int)sqlDataReader["Torque"], (double)sqlDataReader["Ratio"]);
                    calculator.reducerList.Add(reducer);
                }
            }            
            this.Dispatcher.Invoke(() => bdButtonBorder.BorderBrush = Brushes.Green);           
            this.Dispatcher.Invoke(() => statusBar_1.Content = "Ready");
            this.Dispatcher.Invoke(() => bdButton.Content = "DB connected");     
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            resultSB.Clear();
            resultTextBlockReducer.Text = string.Empty;
            calculator.GetResults(ratioOrDiameterComboBox.SelectedIndex == 0);          
            resultTextBlock.Text = $"\tRequired power (simple method): {calculator.SimpleMethodEnginePower:F2} kW\r\n" +
                $"\tRequired power (extension method): {calculator.ExtendedMethodEnginePower:F2} kW *";                
            resultTextBlockMotor.Text = $"\tStandart electric motor: {calculator.SelectMotorPower()} kW {calculator.rpmBase[2]} rpm";                    
            resultTextBlockReducer.Text = calculator.MatchingReducer;
            textBlockHeadPulley.Text = $"\tDiameter of pulley: {((double)calculator.HeadPulleyDiameter/1000):F2} m";
            textBlockRatio.Text = $"\tRatio of reducer: {calculator.Ratio:F1}";
            textForceOfTakeUp.Text = $"\tForce: {calculator.ForseTakeUp} N";
            TextBlockAddiotionInfo.Text = $"\tStep of idlers: {calculator._stepOfWorkingIdler} mm\n" +
                $" \tStep of return idlers: {calculator._stepOfIdleIdler} mm\n" +
                $" \tThickness of belt: {calculator._thicknessOfBelt} mm\n";
            textRecommendBelt.Text = $"\t{calculator.RecommendedWidthOfBelt} mm *";
            FillResultSb();
            statusBar_1.Content = "Done!";           
        }
        
        private void FillResultSb()
        {
            resultSB.AppendLine($"***Main characteristics of conveyor*** \n");           
            resultSB.AppendLine("Calculation based on:\n" +
                $"\tProducticity: {calculator.Productivity, 40} tonns per hour\n" +
                $"\tWidth of belt: {calculator.WidthOfBelt, 40} mm\n" +
                $"\tLenght of conveyor: {calculator.LenghtOfConveyor, 30} m\n" +
                $"\tAngle of conveyor: {calculator.AngleOfConveyor, 30} degree\n" +
                $"\tSpeed of belt: {calculator.SpeedOfConveyor,40} m/s \n" +
                $"\tStep of idlers: {calculator._stepOfWorkingIdler,40} mm\n " +
                $"\tStep of return idlers: {calculator._stepOfIdleIdler,30} mm\n" +
                $"\tThickness of belt: {calculator._thicknessOfBelt,35} mm\n" +
                $"Required power (simple method): \t\t{calculator.SimpleMethodEnginePower:F2} kW \n" +
                $"Required power (extension method): \t{calculator.ExtendedMethodEnginePower:F2} kW \n" +
                $"Standart power of engine: {calculator.SelectMotorPower(),35} kW \n" +
                $"Matching reducer: \n{calculator.MatchingReducer} \n" +
                $"Ratio: {calculator.Ratio,50} (motor speed: {calculator.rpmBase[2]} n-1)\n" +
                $"Force of Take-Up: {calculator.ForseTakeUp, 35} \n" +
                $"Diameter of Head Pulley: {calculator.HeadPulleyDiameter,35} mm \n");            
            resultSB.AppendLine($"\t\t\t\tDate: {DateTime.Now}");
        }

        public void FillCalcReduser(Reducer<int, double> reducer)
        {
            calculator.reducerList.Add(reducer);
        }

        private void productivityValue_LostFocus(object sender, RoutedEventArgs e)
        {

            if (productivityValueTextBox.Text != "")
            {
                try
                {
                    calculator.Productivity = Convert.ToInt32(productivityValueTextBox.Text);
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

        private void lengthValueTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lenghtOfConveyorTextBox.Text != "")
            {
                try
                {
                    calculator.LenghtOfConveyor = Convert.ToInt32(lenghtOfConveyorTextBox.Text);
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
            calculator.WidthOfBelt = Convert.ToInt32(widthComboBoxList.Text);
        }

        private void angleOfBelt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (angleOfBeltTextBox.Text != "")
            {
                try
                {
                    calculator.AngleOfConveyor = Convert.ToInt32(angleOfBeltTextBox.Text);
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
                    calculator.SpeedOfConveyor = Convert.ToDouble(speedOfBeltTextBox.Text);
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
                    calculator.Ratio = Convert.ToDouble(TextBoxRatioOrDiametr.Text);
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
                    calculator.HeadPulleyDiameter = Convert.ToInt32(TextBoxRatioOrDiametr.Text);
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

        private void CommandBinding_CanExecute(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }       

        private void CommandBinding_CanExecute_1(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            if(resultSB.Length > 1)
            e.CanExecute = true;
        }

        private void CommandBinding_Executed_2(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {           
            PrintDialog printDialog = new PrintDialog();
            if(printDialog.ShowDialog() == true)
            {                
                FlowDocument flowDocument = new FlowDocument();
                flowDocument.PageHeight = printDialog.PrintableAreaHeight;                
                flowDocument.ColumnWidth = 800;
                Paragraph paragraph = new Paragraph();
                paragraph.FontSize = 14;
                paragraph.FontFamily = new FontFamily("Palatino Linotype");
                paragraph.TextAlignment = TextAlignment.Left;
                paragraph.Margin = new Thickness(100, 150, 5, 50);
                paragraph.Inlines.Add(new Run(resultSB.ToString()));
                flowDocument.Blocks.Add(paragraph);
                DocumentPaginator paginator = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator;
                printDialog.PrintDocument(paginator, "Result");
            }         
        }

        private void CommandBinding_CanExecute_2(object sender, System.Windows.Input.CanExecuteRoutedEventArgs e)
        {
            if (resultSB.Length > 1)
                e.CanExecute = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("I am a calculator designed for learning and to help! Thank You for using me!", "About me");
        }
    }
}
