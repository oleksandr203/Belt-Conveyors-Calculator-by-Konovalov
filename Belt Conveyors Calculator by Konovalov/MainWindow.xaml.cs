using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;
using System.Windows;
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
            productivityValue.Text = calculator.Productivity.ToString();
            widthComboBoxList.SelectedIndex = 0;
            btnSelectReducer.IsEnabled = false;
            lenghtOfConveyor.Text = calculator.LenghtOfConveyor.ToString();
            angleOfBelt.Text = calculator.AngleOfConveyor.ToString();
            speedOfBelt.Text = calculator.SpeedOfConveyor.ToString();
        }
        private async Task ConnectDB()
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
            connection.Open();           
            string sql = "Select * From Models";
            SqlCommand sqlCommand = new SqlCommand(sql, connection);
            using(SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
            {
                while (sqlDataReader.Read())
                {
                    Reducer reducer = new Reducer((int)sqlDataReader["Id"], (string)sqlDataReader["Name"], (int)sqlDataReader["Torque"], (double)sqlDataReader["Ratio"]); 
                    calculator.reducerList.Add(reducer);
                }
            }
        }

        private async void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            calculator.CalculateSimpleEnginePower();
            calculator.CalculateExtendedEnginePower();
            resultTextBlock.Text = $"Required power of engine: \r\n" +
                $"result of simple method: {calculator.SimpleMethodEnginePower:F2} kWt\r\n" +
                $"result of extension method: {calculator.ExtendedMethodEnginePower:F2} kWt.\r\n" +
                $"Standart electric motor: {calculator.SelectMotorPower()} kWt";            
            statusBar_1.Content = "Done!";
            await Task.Run(() => ConnectDB());
            btnSelectReducer.IsEnabled = true;            
        }

        private void productivityValue_LostFocus(object sender, RoutedEventArgs e)
        {
            if (productivityValue.Text != "")
            {
                try
                {
                    calculator.SetProductivity(Convert.ToInt32(productivityValue.Text));
                }
                catch (System.FormatException)
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                productivityValue.Text = "0";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (lenghtOfConveyor.Text != "")
            {
                try
                {
                    calculator.SetLenght(Convert.ToInt32(lenghtOfConveyor.Text));
                }
                catch (FormatException)
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                lenghtOfConveyor.Text = "0";
            }
        }

        private void widthComboBoxList_LostFocus(object sender, RoutedEventArgs e)
        {
            calculator.SetWidthOfBelt(Convert.ToInt32(widthComboBoxList.Text));
        }

        private void angleOfBelt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (angleOfBelt.Text != "")
            {
                try
                {
                    calculator.SetAngle(Convert.ToInt32(angleOfBelt.Text));
                }
                catch
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                angleOfBelt.Text = "0";
            }
        }

        private void speedOfBelt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (speedOfBelt.Text != "")
            {
                try
                {
                    calculator.SetSpeed(Convert.ToDouble(speedOfBelt.Text));
                }
                catch
                {
                    MessageBox.Show("Only numbers must be");
                }
            }
            else
            {
                speedOfBelt.Text = "0";
            }
        }

        private void btnSelectReducer_Click(object sender, RoutedEventArgs e)
        {
            calculator.SelectReducer();
        }

        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            statusBar_1.Content = "Ready, enter data please";
        }
    }
}
