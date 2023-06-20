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
using System.Windows.Shapes;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public string provider = "ALEX\\SQLEXPRESS";
        public string initialCatalog = "reducers";

        public Window1()
        {
            InitializeComponent();
        }        

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;            
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void initialCatalogTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
           initialCatalog = initialCatalogTextBox.Text;
        }

        private void dataSourseTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            provider = dataSourseTextBox.Text;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dataSourseTextBox.Text = provider;
            initialCatalogTextBox.Text = initialCatalog;
        }

        private void dataSourseTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (dataSourseTextBox.Text != provider)
            {
                okButton.IsEnabled = true;
            }           
        }

        private void initialCatalogTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (initialCatalogTextBox.Text != initialCatalog)
            {
                okButton.IsEnabled = true;
            }
        }
    }
}
