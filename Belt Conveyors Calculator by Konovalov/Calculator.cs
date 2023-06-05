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
        public readonly int[] standartBeltWidth = new int[] { 650, 800, 1000, 1200, 1400 };

        int _producticity;
        int _widthOfBelt;
        int _angleOfConveyor;
        int _weightOfV_Roller;
        int _weightOfI_Roller;
        int _stepOfWorkingRoller = 1000;
        int _stepOfIdleRoller = 3000;
        int _thicknessOfBelt = 20;
        double _speedOfBeltLinear;
        double _lenghtOfConvProjection;
        public int LenghtOfConv { get; set;}
        public double LenghtOfConvProjection { get { return _lenghtOfConvProjection; }  set { _lenghtOfConvProjection = value * Math.Cos(_angleOfConveyor); } }
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
        public static int ProjectionLengthOfConveyor(int length, int angle)
        {
            return (int)(length * Math.Cos(angle));
        }

        public static double AngleInRadian(int angle)
        {
            return angle * Math.PI / 180;
        }

        public static double RelativeHeightOfConveyor(int length, int angle)
        {
            return length * Math.Sin(angle);
        }

        public static int LoadOfCargoPerMeter(int productivity, double speed)
        {
            return (int)(10 * productivity / 3.6 / speed);
        }

        public static int LoadOfBeltWeight(int widthOfBelt, int thicknessOfBelt)
        {
            return (widthOfBelt * thicknessOfBelt * 2 / 100);
        }
    }
}
