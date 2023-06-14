using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Data;
using System.Data.SqlClient;
using static Belt_Conveyors_Calculator_by_Konovalov.AdditonMath;
using System.Xml;
using System;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    class Calculator
    {
        public readonly int[] standartBeltWidth = new int[] { 650, 800, 1000, 1200, 1400 };
        public readonly double[] randeOfElectricMotors = new double[] { 1.1, 1.5, 2.2, 3, 4, 5.5, 7.5, 11, 15, 18.5, 22, 30, 37, 45, 55, 75, 90, 110, 132, 160, 200, 250, 315};
        public readonly int[] rpmBase = new int[] { 750, 1000, 1500, 3000 };
        public List<Reducer> reducerList = new List<Reducer>();
        private int _producticity = 200;
        private int _angleOfConveyor = 10;
        private int _lenghtOfConveyor = 15;
        private double _speedOfBeltLinear = 1.5;
        private int _weightOfV_Idler = 20;
        private int _weightOfI_Idler = 20;
        public readonly int _stepOfWorkingIdler = 1000;
        public readonly int _stepOfIdleIdler = 3000;
        public readonly int _thicknessOfBelt = 20;
        private int[] _speedOfDrive = new int[] { 735, 950, 1450, 3000 };        
        private double _ratio = 31.5;
        private int _mainPulleyDiameter = 600;

        public int Density { get; private set; }
        public int LenghtOfConveyor { get; private set; }
        public int Productivity { get; private set; }
        public int WidthOfBelt { get; private set; }
        public int AngleOfConveyor { get; private set; }
        public double SpeedOfConveyor { get; private set; }
        public double SimpleMethodEnginePower { get; private set; }
        public double ExtendedMethodEnginePower { get; private set; }
        public int CalculatedTorque { get; private set; }
        public string FittingReducer { get; private set; }
        public double Ratio { get; private set; }
        public int HeadPulleyDiameter { get; private set; }

        public Calculator()
        {
            Productivity = _producticity;
            LenghtOfConveyor = _lenghtOfConveyor;
            AngleOfConveyor = _angleOfConveyor;
            WidthOfBelt = standartBeltWidth[0];
            SpeedOfConveyor = _speedOfBeltLinear;
            Ratio = _ratio;
            HeadPulleyDiameter = _mainPulleyDiameter;
        }
        
        public void SetProductivity(int productivity)
        {
            if (productivity < 0)
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
            string msg = "Set angle from 0 to 19 degrees";
            if (angle < 0 || angle > 19)
            {
                MessageBox.Show(msg);
            }            
            else 
            {
                AngleOfConveyor = angle;
            }
        }

        public void SetLenght(int lenght)
        {
            string msg = "Set lenght from 1 to 999 m"; 
            if (lenght <= 0)
            {
                MessageBox.Show(msg);
            }
            else
            {
                LenghtOfConveyor = lenght;
            }
        }

        public void SetSpeed(double speed)
        {
            string msg = "Set speed from 0,1 to 2 m/s";
            if ((speed < 0) || (speed > 2))
            {
                MessageBox.Show(msg);
            }            
            else
            {
                SpeedOfConveyor = speed;
            }
        }

        public void SetRatio(double ratio)
        {
            string msg = "Set ratio from 1 to 50 m/s";
            if ((ratio <= 1 ) || (ratio > 50))
            {
                MessageBox.Show(msg);
            }
            else
            {
                Ratio = ratio;
            }
        }

        public void SetDiameterOfHeadPulley(int diameter)
        {
            string msg = "Set diameter from 100 to 2500 mm";
            if ((diameter <= 100) || (diameter > 2500))
            {
                MessageBox.Show(msg);
            }
            else
            {
                HeadPulleyDiameter = diameter;
            }
        }

        private double GetForseOfV_Roller()
        {
            if (WidthOfBelt == 650 || WidthOfBelt == 800)
            {
                _weightOfV_Idler = 18;
            }
            else
            {
                _weightOfV_Idler = 24;
            }
            return _weightOfV_Idler * 9.8 / 10 / 1.2;
        }

        private double GetForseOfI_Roller()
        {
            if (WidthOfBelt == 650 || WidthOfBelt == 800)
            {
                _weightOfI_Idler = 15;
            }
            else
            {
                _weightOfI_Idler = 18;
            }
            return _weightOfI_Idler * 9.8 / 10 / 1.2;
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
            double powerUseful = ((Productivity * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.042 / 367) + (Productivity * HeightOfConveyor(LenghtOfConveyor, AngleOfConveyor) / 367));
            double powerParasite = (SpeedOfConveyor * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.042);
            SimpleMethodEnginePower = (powerUseful + powerParasite) * 1.1 / 0.8;
        }

        public void CalculateExtendedEnginePower()
        {
            double forseOnDrivePulley = (CoefficientOfLenght(LenghtOfConveyor) * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.045 *
                (LoadOfCargoPerMeter(Productivity, SpeedOfConveyor) + ForseForAllRollers() + 2 * GetForseWeightOfBelt()))
                + LoadOfCargoPerMeter(Productivity, SpeedOfConveyor) * HeightOfConveyor(LenghtOfConveyor, AngleOfConveyor);

            ExtendedMethodEnginePower = 1.1 * forseOnDrivePulley / 0.8 / 1000;
        }

        public void CalculateTorque()
        {
            CalculatedTorque = (int)(ExtendedMethodEnginePower * 9549 * 1.2 / _speedOfDrive[3] * 31.5);
        }
       
        public void FillListOfReducer()
        {                 
            if(reducerList ==  null)
            {
                Reducer reducer1 = new Reducer(1, "Ц2У-100", 315, 31.5);
                reducerList.Add(reducer1);
                Reducer reducer2 = new Reducer(2, "Ц2У-160", 630, 31.5);
                reducerList.Add(reducer2);
                Reducer reducer3 = new Reducer(3, "Ц2У-200", 1250, 31.5);
                reducerList.Add(reducer3);
                Reducer reducer4 = new Reducer(4, "Ц2У-250", 2500, 31.5);
                reducerList.Add(reducer3);
            }            
        }

        public string SelectMotorPower()
        {
            foreach (var m in randeOfElectricMotors)
            {
                if (ExtendedMethodEnginePower - m <= 0 && ExtendedMethodEnginePower / m < 1.2)
                {
                    return m.ToString();
                }                
            }
           return "No fitting engine is found";
        }

        public void SelectReducer()
        {
            CalculateTorque();
            string resultReducer = "";
            bool isResult = false;
            foreach (var reducer in reducerList)
            {
                if (reducer != null & reducer._maxTorque > CalculatedTorque & reducer._maxTorque / CalculatedTorque < 1.25)
                {
                    resultReducer += $"{reducer._name} fullfills your requires\r\n";                       
                        isResult = true;                        
                }
            } 
            if (!isResult)
            {
                FittingReducer = ($"No fits in data base");
            }
            else
            {
                FittingReducer = resultReducer;
            }
        }

        public void CalculateHaedPulleyCharacteristics()
        {   
            HeadPulleyDiameter = (int)(1000 * SpeedOfConveyor / (rpmBase[2] / Ratio / 60 )/ Math.PI); 
        }

        public void CalculateRatio()
        {            
            double n2 = (SpeedOfConveyor * 1000 * 60) / (3.14 * HeadPulleyDiameter);
            Ratio = rpmBase[2] /n2;
        }
    }
}
