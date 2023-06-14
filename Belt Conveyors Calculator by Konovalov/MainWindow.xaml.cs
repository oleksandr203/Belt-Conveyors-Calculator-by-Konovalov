using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calculator;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            calculator = new Calculator();
            widthComboBoxList.ItemsSource = calculator.standartBeltWidth;
            productivityValueTextBox.Text = calculator.Productivity.ToString();
            widthComboBoxList.SelectedIndex = 0;
            lenghtOfConveyorTextBox.Text = calculator.LenghtOfConveyor.ToString();
            angleOfBeltTextBox.Text = calculator.AngleOfConveyor.ToString();
            speedOfBeltTextBox.Text = calculator.SpeedOfConveyor.ToString();
            TextBoxRatioOrDiametr.Text = calculator.Ratio.ToString();
        }

        private void ConnectDB()//to realize right configuration
        {
            var cStringBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "reducers",
                DataSource = @"COKONOVALOV",
                ConnectTimeout = 25,
                IntegratedSecurity = true,
                TrustServerCertificate = true
            };
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = cStringBuilder.ConnectionString;
            try
            {
                connection.Open();
                string sql = "Select * From Models";
                SqlCommand sqlCommand = new SqlCommand(sql, connection);
                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {
                        Reducer reducer = new Reducer((int)sqlDataReader["Id"], (string)sqlDataReader["Name"], (int)sqlDataReader["Torque"], (double)sqlDataReader["Ratio"]);
                        calculator.reducerList.Add(reducer);
                    }
                }
            }
            catch(SqlException ex) { MessageBox.Show(ex.Message); }           
        }

        private async void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            calculator.CalculateSimpleEnginePower();
            calculator.CalculateExtendedEnginePower();
            resultTextBlock.Text = $"Required power (simple method): {calculator.SimpleMethodEnginePower:F2} kW\r\n" +
                $"Required power (extension method): {calculator.ExtendedMethodEnginePower:F2} kW *";                
            resultTextBlockMotor.Text = $"Standart electric motor: {calculator.SelectMotorPower()} kW {calculator.rpmBase[2]} rpm";
            statusBar_1.Content = "Done!";            
            await Task.Run(() => ConnectDB());
            calculator.SelectReducer();
            resultTextBlockReducer.Text = calculator.FittingReducer;

            GetRatioOrPulleyDiamenet();
            textBlockHeadPulley.Text = $"Diameter of pulley: {((double)calculator.HeadPulleyDiameter/1000):F2} m";
            textBlockRatio.Text = $"Ratio of reducer: {calculator.Ratio:F1}";
            TextBlockAddiotionInfo.Text = $"Step of idlers = {calculator._stepOfWorkingIdler} mm, step of return  = {calculator._stepOfIdleIdler} mm, thickness of belt = {calculator._thicknessOfBelt} mm";
        }

        private void GetRatioOrPulleyDiamenet()
        {
            if (ratioOrDiameterComboBox.SelectedIndex == 0)
            {
                //calculator.SetDiameterOfHeadPulley(0);
                calculator.CalculateHaedPulleyCharacteristics();
            }
            else
            if (ratioOrDiameterComboBox.SelectedIndex == 1)
            {
                //calculator.SetRatio(0);
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
                    calculator.SetDiameterOfHeadPulley (Convert.ToInt32(TextBoxRatioOrDiametr.Text));
                }
                catch
                {
                    MessageBox.Show("Only numbers must be");
                }
            }            
        }
    }
}
