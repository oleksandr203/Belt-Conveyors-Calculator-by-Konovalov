using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belt_Conveyors_Calculator_by_Konovalov
{
    public static class AdditonMath
    {
        public static double ProjectionLengthOfConveyor(int length, int angle)
        {
            return length * Math.Cos(AngleInRadian(angle));
        }

        public static double AngleInRadian(int angle)
        {
            return angle * Math.PI / 180;
        }

        public static double HeightOfConveyor(int length, int angle)
        {
            return length * Math.Sin(AngleInRadian(angle));
        }

        public static int LoadOfCargoPerMeter(int productivity, double speed)
        {
            return (int)(10 * productivity / 3.6 / speed);
        }
       
        public static double CoefficientOfLenght(int lenght)
        {
            if (lenght >= 400)
                return 1.2;
            else
                if (lenght < 400 & lenght >= 300)
                return 1.3;
            else
                if (lenght < 300 & lenght >= 200)
                return 1.4;
            else
                if (lenght < 200 & lenght >= 150)
                return 1.5;
            else
                if (lenght < 150 & lenght >= 125)
                return 1.6;
            else
                if (lenght < 125 & lenght >= 100)
                return 1.7;
            else
                if (lenght < 100 & lenght >= 90)
                return 1.8;
            else
                if (lenght < 90 & lenght >= 80)
                return 1.9;
            else
                if (lenght < 80 & lenght >= 70)
                return 2;
            else
                if (lenght < 70 & lenght >= 60)
                return 2.1;
            else
                if (lenght < 60 & lenght >= 50)
                return 2.2;
            else
                if (lenght < 50 & lenght >= 40)
                return 2.2;
            else
                if (lenght < 40 & lenght >= 30)
                return 2.4;
            else
                if (lenght < 30 & lenght >= 20)
                return 3;
            else
                if (lenght < 20 & lenght >= 10)
                return 3.5;
            else
                return 4;

        }
    }
}
