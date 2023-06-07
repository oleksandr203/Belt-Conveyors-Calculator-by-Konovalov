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
        public readonly int[] standartBeltWidth = new int[] { 650, 800, 1000, 1200, 1400};
        private int _producticity = 200;        
        private int _angleOfConveyor = 10;
        private int _lenghtOfConveyor = 15;
        private double _speedOfBeltLinear = 1.5;
        private int _desitity = 1600;
        private int _weightOfV_Roller = 20;
        private int _weightOfI_Roller = 20;       
        private int _stepOfWorkingRoller = 1000;
        private int _stepOfIdleRoller = 3000;
        private int _thicknessOfBelt = 20;       
        private double _lenghtOfConvProjection;
        private double _simpleMethodEnginePower;
        private double _extendedMethodEnginePower;       

        public int Density { get; private set; }
        public int LenghtOfConveyor { get; private set;}        
        public int Productivity { get; private set; }   
        public int WidthOfBelt { get; private set; }
        public int AngleOfConveyor { get; private set; }
        public double SpeedOfConveyor { get; private set; }  
        
        public double SimpleMethodEnginePower { get; private set; }  
        public double ExtendedMethodEnginePower { get; private set; }

        public Calculator()
        {
            Productivity = _producticity;
            LenghtOfConveyor = _lenghtOfConveyor;
            AngleOfConveyor = _angleOfConveyor;
            WidthOfBelt = standartBeltWidth[0];
            SpeedOfConveyor = _speedOfBeltLinear;
        }

        public void SetDensity(int density)
        {
            if(density < 0)
            {
                MessageBox.Show("Incorrect input");                
            }
            else
            {
                Density = density;
            }
        }

        public void SetProductivity(int productivity)
        {
            if(productivity < 0)
            {
                MessageBox.Show("Incorrect value");               
            }
            else
            {
                Productivity = productivity;
            }
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
            if(angle < 0)
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
                LenghtOfConveyor = lenght;
            }
        }

        public void SetSpeed(double speed)
        {
            if((speed < 0) || (speed > 5))
            {
                MessageBox.Show("Incorrect value!");                
            }
            else
            {
                SpeedOfConveyor = speed;
            }
        }
        private double GetForseOfV_Roller()
        {
            if(WidthOfBelt == 650 || WidthOfBelt == 800)
            {
                _weightOfV_Roller = 18;
            }
            else               
            {
                _weightOfV_Roller = 24;
            }
            return _weightOfV_Roller * 9.8 / 10 / 1.2;
        }

        private double GetForseOfI_Roller()
        {
            if (WidthOfBelt == 650 || WidthOfBelt == 800)
            {
                _weightOfI_Roller = 15;
            }
            else
            {
                _weightOfI_Roller = 18;
            }
            return _weightOfI_Roller * 9.8 / 10 / 1.2;
        }

        public int GetForseWeightOfBelt()
        {
            return (int)(1.13 * 0.001 * WidthOfBelt * 2 * _thicknessOfBelt * 10);
            
        }

        public double ForseForAllRollers()
        {
            return GetForseOfV_Roller() + GetForseOfI_Roller();
        }

        public void CalculateSimpleEnginePower()
        {
            double powerUseful = ((Productivity * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.042 / 367) + (Productivity * HeightOfConveyor(LenghtOfConveyor, AngleOfConveyor)/ 367));
            double powerParasite = (SpeedOfConveyor * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.042);
            SimpleMethodEnginePower = (powerUseful + powerParasite) * 1.1 / 0.8;               
        }

        public void CalculateExtendedEnginePower()
        {
            var a = CoefficientOfLenght(LenghtOfConveyor);
            var b = ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor);
            var c = LoadOfCargoPerMeter(Productivity, SpeedOfConveyor);
            var d = ForseForAllRollers();
            var e = GetForseWeightOfBelt();
            var f = HeightOfConveyor(LenghtOfConveyor, AngleOfConveyor);

            double forseOnDrivePulley = (CoefficientOfLenght(LenghtOfConveyor) * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.045 * 
                (LoadOfCargoPerMeter(Productivity, SpeedOfConveyor) + ForseForAllRollers() + 2 * GetForseWeightOfBelt()))
                + LoadOfCargoPerMeter(Productivity, SpeedOfConveyor) * HeightOfConveyor(LenghtOfConveyor, AngleOfConveyor);

            ExtendedMethodEnginePower = 1.1 * forseOnDrivePulley / 0.8 / 1000;
        }
    }
}
