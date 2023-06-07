using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Belt_Conveyors_Calculator_by_Konovalov.AdditonMath;

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
            lenghtOfConveyor.Text = calculator.LenghtOfConveyor.ToString();
            angleOfBelt.Text = calculator.AngleOfConveyor.ToString();
            speedOfBelt.Text = calculator.SpeedOfConveyor.ToString();
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {            
            calculator.CalculateSimpleEnginePower();
            calculator.CalculateExtendedEnginePower();
            resultTextBlock.Text = $"Result: \r\n Result of simple method: {calculator.SimpleMethodEnginePower:F2}kWt\r\n " +
                 $"Result of extension method: {calculator.ExtendedMethodEnginePower:F2} kWt.";              
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
    }
}
