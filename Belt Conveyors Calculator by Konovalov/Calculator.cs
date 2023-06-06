using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Belt_Conveyors_Calculator_by_Konovalov.AdditonMath;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    class Calculator
    {
        public readonly int[] standartBeltWidth = new int[] { 650, 800, 1000, 1200, 1400 };
       // private int _producticity = 0;
        //private int _widthOfBelt = 0;
        private int _angleOfConveyor = 0;
        /private int _weightOfV_Roller = 15;
        private int _weightOfI_Roller =  10;
        private int _stepOfWorkingRoller = 1000;
        private int _stepOfIdleRoller = 3000;
        private int _thicknessOfBelt = 20;
        private int _lenghtOfConveyor = 10;
        private double _speedOfBeltLinear = 1.5;
        private double _lenghtOfConvProjection;
        private int _extendedMethodEnginePower;

        public int LenghtOfConv { get; set;}
        public double LenghtOfConvProjection { get { return _lenghtOfConvProjection; }  set { _lenghtOfConvProjection = value * Math.Cos(_angleOfConveyor); } }
        public int Productivity { get; private set; }   
        public int WidthOfBelt { get; private set; }
        public int AngleOfConveyor { get; private set; }
        public double SpeedOfConveyor { get; private set; }  
        public int SimpleMethodEnginePower { get { return CalculateSimpleEnginePower(); }  }
       // public int ExtendedMethodEnginePower { get; private set; }

        //public Calculator()
        //{
        //    Productivity = _producticity;
        //    LenghtOfConv = _lenghtOfConveyor;
        //    AngleOfConveyor = _angleOfConveyor;
        //    WidthOfBelt = _widthOfBelt;
        //    SpeedOfConveyor = _speedOfBeltLinear;
        //}
        
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
            if (widthOfBelt <= 200)
            {
                MessageBox.Show("Incorrect value");
            }
            else
            {
                WidthOfBelt = widthOfBelt;
            }           
        }  
        
        public void SetAngle(int angle)
        {
            if(_angleOfConveyor < 0)
            {
                MessageBox.Show("Incorrect value!");
            }
            else
            {
                AngleOfConveyor = angle;
            }
        }

        public void SetLenght(int lenght)
        {
            if (lenght <= 0)
            {
                MessageBox.Show("Incorrect value!");
            }
            else
            {
                LenghtOfConv = lenght;
            }
        }

        private int CalculateSimpleEnginePower()
        {
            double _powerUseful = ((Productivity * ProjectionLengthOfConveyor(LenghtOfConv, AngleOfConveyor) * 0.042 / 367) + (Productivity * RelativeHeightOfConveyor(LenghtOfConv, AngleOfConveyor)/ 367));
            double _powerParasite = (SpeedOfConveyor * ProjectionLengthOfConveyor(LenghtOfConv, AngleOfConveyor) * 0.042);
            return  (int)(_powerUseful + _powerParasite);
        }
    }
}
