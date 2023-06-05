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

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculator calculator = new Calculator();
        public MainWindow()
        {
            InitializeComponent();   
            
        }

        private void resultTextBlock_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            
            resultTextBlock.Text = $"Result: Productivity: {calculator.Productivity} tons per hour, current belt is {calculator.WidthOfBelt}, test {Calculator.AngleInRadian(18)}";
            
        }

        private void productivityValue_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {            
            
            
        }

        private void productivityValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void productivityValue_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                calculator.SetProductivity(Convert.ToInt32(productivityValue.Text));
            }
            catch(System.FormatException)
            {
                MessageBox.Show("Only numbers must be");
            }
           
        }       

        private void widthComboBoxList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                calculator.SetWidthOfBelt(Convert.ToInt32(widthComboBoxList.SelectionBoxItem.ToString()));

            }
            catch { }
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }        
    }
}
