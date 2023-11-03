using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Media3D;
using static Belt_Conveyors_Calculator_by_Konovalov.AdditonMath;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    class Calculator
    {
        public readonly int[] standartBeltWidth = new int[] { 650, 800, 1000, 1200, 1400 };
        public readonly double[] randeOfElectricMotors = new double[] { 1.1, 1.5, 2.2, 3, 4, 5.5, 7.5, 11, 15, 18.5, 22, 30, 37, 45, 55, 75, 90, 110, 132, 160, 200, 250, 315};
        public readonly int[] rpmBase = new int[] { 750, 1000, 1500, 3000 };
        public List<Reducer<int, double>> reducerList = new List<Reducer<int, double>>();
        private int _productivity = 200;
        private int _widthOfBelt = 800;
        private int _angleOfConveyor = 10;
        private int _lenghtOfConveyor = 15;
        private double _speedOfBelt = 1.5;
        private int _weightOfV_Idler = 20;
        private int _weightOfI_Idler = 20;
        public readonly int _stepOfWorkingIdler = 1000;
        public readonly int _stepOfIdleIdler = 3000;
        public readonly int _thicknessOfBelt = 20;
        private int[] _speedOfDrive = new int[] { 735, 950, 1450, 3000 };        
        private double _ratio = 31.5;
        private double forceOnDrivePulley = 0;
        private int _headPulleyDiameter = 600;
        private string matchingReductors = "";

        //List<string> matchingReductors = new List<string>();       
        public int Productivity
        {
            get { return _productivity; }
            set
            {
                if (value < 0 | value > 2000)
                {
                    MessageBox.Show("Set Productivity value from 0 to 2000");
                    _productivity = 0;                    
                }
                else
                {
                    _productivity  = value;
                }
            }
        }
        public int LenghtOfConveyor
        {
            get { return _lenghtOfConveyor; }
            set {                
                if (value < 1)
                {
                    MessageBox.Show("Set lenght from 1 to 999 m");
                    _lenghtOfConveyor = 1;
                }
                else
                {
                    _lenghtOfConveyor = value;
                }
            }
        }
        public int WidthOfBelt
        {
            get { return _widthOfBelt; }
            set
            {
                if (value <= 200)
                {
                    MessageBox.Show("Incorrect value");
                }
                else
                {
                    _widthOfBelt = value;
                }
            }
        }
        public string RecommendedWidthOfBelt { get; private set; }
        public int AngleOfConveyor
        {
            get { return _angleOfConveyor; }
            set
            {
                if (value < 0 || value> 19)
                {
                    MessageBox.Show("Set angle from 0 to 19 degrees");
                }
                else
                {
                    _angleOfConveyor = value;
                }
            }
        }
        public double SpeedOfConveyor
        {
            get { return _speedOfBelt; }
            set
            {
                if ((value < 0) || (value > 2))
                {
                    MessageBox.Show("Set speed from 0,1 to 2 m/s");
                }
                else
                {
                    _speedOfBelt = value;
                }
            }
        }
        public double Ratio
        {
            get { return _ratio; }
            set
            {                
                if ((value <= 1) || (value > 50))
                {
                    MessageBox.Show("Set ratio from 1 to 50 m/s");
                }
                else
                {
                    _ratio = value;
                }
            }
        }
        public int HeadPulleyDiameter
        {
            get { return _headPulleyDiameter; }
            set
            {
                if ((value <= 100) || (value > 2500))
                {
                    MessageBox.Show("Set diameter from 100 to 2500 mm");
                }
                else
                {
                    _headPulleyDiameter = value;
                }
            }
        }
        public double SimpleMethodEnginePower { get; private set; }
        public double ExtendedMethodEnginePower { get; private set; }        
        public int CalculatedTorque { get; private set; }
        public string MatchingReducer { get; private set; } = string.Empty;
        public int ForseTakeUp { get; private set; }
       

        public Calculator()
        {
            Productivity = _productivity;
            LenghtOfConveyor = _lenghtOfConveyor;
            AngleOfConveyor = _angleOfConveyor;
            WidthOfBelt = standartBeltWidth[0];
            SpeedOfConveyor = _speedOfBelt;
            Ratio = _ratio;
            HeadPulleyDiameter = _headPulleyDiameter;            
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

        private int GetForseWeightOfBelt()
        {
            return (int)(1.13 * 0.001 * WidthOfBelt * 2 * _thicknessOfBelt * 10);
        }

        private double ForseForAllRollers()
        {
            return GetForseOfV_Roller() + GetForseOfI_Roller();
        }

        private void CalculateSimpleEnginePower()
        {
            double powerUseful = ((Productivity * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.042 / 367) + (Productivity * HeightOfConveyor(LenghtOfConveyor, AngleOfConveyor) / 367));
            double powerParasite = (SpeedOfConveyor * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.042);
            SimpleMethodEnginePower = (powerUseful + powerParasite) * 1.1 / 0.8;
        }

        private void CalculateExtendedEnginePower()
        {
            forceOnDrivePulley = (CoefficientOfLenght(LenghtOfConveyor) * ProjectionLengthOfConveyor(LenghtOfConveyor, AngleOfConveyor) * 0.045 *
                (LoadOfCargoPerMeter(Productivity, SpeedOfConveyor) + ForseForAllRollers() + 2 * GetForseWeightOfBelt()))
                + LoadOfCargoPerMeter(Productivity, SpeedOfConveyor) * HeightOfConveyor(LenghtOfConveyor, AngleOfConveyor);

            ExtendedMethodEnginePower = 1.1 * forceOnDrivePulley / 0.8 / 1000;
        }

        private void CalculateTorque()
        {
            CalculatedTorque = (int)(ExtendedMethodEnginePower * 9549 * 1.2 / _speedOfDrive[3] * 31.5);
        }

        private void MatchTheWidthOfBelt()
        {
            int resultBelt = (int)(1.1*(Math.Sqrt(Productivity/(500*SpeedOfConveyor*1.4*0.9))+0.05)*1000);
            for (int i = 0; i < standartBeltWidth.Length; i++)
            {
                if (standartBeltWidth[i] > resultBelt)
                {
                    RecommendedWidthOfBelt = standartBeltWidth[i].ToString();
                    break;
                }                    
            }  
        }
       
        public void FillListOfReducerByConfigBase()
        {                 
            if(reducerList.Count == 0)
            {
                Reducer<int, double> reducer1 = new Reducer<int, double>(1, "Ц2У-100", 315, 31.5);
                reducerList.Add(reducer1);
                Reducer<int, double> reducer2 = new Reducer<int, double>(2, "Ц2У-160", 630, 31.5);
                reducerList.Add(reducer2);
                Reducer<int, double> reducer3 = new Reducer<int, double>(3, "Ц2У-200", 1250, 31.5);
                reducerList.Add(reducer3);
                Reducer<int, double> reducer4 = new Reducer<int, double>(4, "Ц2У-250", 2500, 31.5);
                reducerList.Add(reducer4);
                Reducer<int, double> reducer5 = new Reducer<int, double>(4, "Ц2У-315", 5000, 31.5);
                reducerList.Add(reducer5);
                Reducer<int, double> reducer6 = new Reducer<int, double>(4, "Ц2У-350", 10000, 31.5);
                reducerList.Add(reducer6);
                Reducer<int, double> reducer7 = new Reducer<int, double>(4, "Ц2У-400", 16300, 31.5);
                reducerList.Add(reducer7);
                Reducer<int, double> reducer8 = new Reducer<int, double>(4, "Ц2У-500", 56000, 31.5);
                reducerList.Add(reducer8);
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
           return "No matching engine is found";
        }

        private void CalculateForceTakeUp()
        {
            ForseTakeUp = (int)(forceOnDrivePulley * 10 / 6);
        }

        private void SelectReducer()
        {
            MatchingReducer = "";
            foreach (var reducer in reducerList)
            {
                if(reducer != null && reducer._maxTorque > CalculatedTorque && reducer._maxTorque / CalculatedTorque < 1.15)
                {
                    MatchingReducer += $"\t{reducer._name}";
                }                
            }
            if (MatchingReducer == "")
                MatchingReducer = "Sorry! No matches";

            // string resultReducer = "";
            //// matchingReductors.Clear();
            // foreach (var reducer in reducerList)
            // {
            //     if (reducer != null && reducer._maxTorque > CalculatedTorque && reducer._maxTorque / CalculatedTorque < 1.15)
            //     {                    
            //         matchingReductors.Add(reducer._name);                     
            //     }
            // } 
            // if (matchingReductors.Count == 0)
            // {
            //     matchingReducer = ($"\tSorry, no match in database");
            // }
            // else
            // {  
            //     foreach(var reducer in matchingReductors)
            //     {    
            //         resultReducer += "\t" + reducer + "\n";
            //     }
            //     matchingReducer = resultReducer.Remove(resultReducer.Length - 1, 1);               
            // }
        }

        private void CalculateRatioOrHaedPulleyCharacteristics(bool RatioOrDiameter)
        {
            if (RatioOrDiameter)
            {
                HeadPulleyDiameter = (int)(1000 * SpeedOfConveyor / (rpmBase[2] / Ratio / 60) / Math.PI);
            }
            else
            {
                Ratio = rpmBase[2] / ((SpeedOfConveyor * 1000 * 60) / (3.14 * HeadPulleyDiameter));                
            }
        }

        public void GetResults(bool getRatio)
        {
            CalculateSimpleEnginePower();
            CalculateExtendedEnginePower();
            CalculateTorque();
            SelectReducer();
            CalculateForceTakeUp();
            CalculateRatioOrHaedPulleyCharacteristics(getRatio);
            MatchTheWidthOfBelt();
        }
    }
}
