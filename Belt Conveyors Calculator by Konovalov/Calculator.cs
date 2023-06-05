using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    class Calculator
    {
       // public readonly int[] standartBeltWidth = new int[] { 650, 800, 1000, 1200, 1400 };
        int _producticity;
        int _widthOfBelt;
        int _angleOfConveyor;
        double _speedOfBeltLinear;
        public int Productivity { get; private set; }   
        public int WidthOfBelt { get; private set; }
        public int AngleOfConveyor { get; private set; }
        public int SpeedOfConveyor { get; private set; }               

        public void SetProductivity(int productivity)
        {
            if(productivity < 0)
            {
                MessageBox.Show("Incorrect value");
            }
            Productivity = productivity;
        }
        public void SetWidthOfBelt(int widthOfBelt)
        {
            if (widthOfBelt < 0)
            {
                MessageBox.Show("Incorrect value");
            }
            WidthOfBelt = widthOfBelt;
        }
    }
}
