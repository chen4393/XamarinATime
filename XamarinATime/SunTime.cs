using Android.Util;
using Java.Lang;
using Java.Util;

namespace XamarinATime
{

    class SunTime
    {
        public const double PI = 3.141592653589793;
        public double longitude { get; set; }
        public double latitude { get; set; }
        private double utcOffset;

        public int sunriseTime { get; set; }
        public int sunsetTime { get; set; }

        public int test_data;
        public int flagrise { get; set; }
        public int flagset { get; set; }

        private Calendar calendar;
        private int[] timeDetail = new int[8];

        public SunTime()
        {
            this.latitude = 0.0;
            this.longitude = 0.0;
            this.utcOffset = 1.0;
        }

        //create SunTime object for current date
        public SunTime(double latitude, double longitude, double offset, Calendar cal)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.calendar = cal;
            test_data = calendar.Get(CalendarField.Month);
            utcOffset = offset;
            Update();
        }

        private void Update()
        {
            sunriseTime = CalculateTime(1);
            sunsetTime = CalculateTime(2);
        }

        private int CalculateTime(int flag)
        {
            // 1. calculate year of day
            int DayOfYear = calendar.Get(CalendarField.DayOfYear);
            
            // 2. convert the longitude to hour value and calculate an approximate time
            double lngHour = longitude / 15.0;
            double t;
            if (flag == 1)
            {
                t = DayOfYear + ((6.0 - lngHour) / 24.0);     // if rising
            }
            else
            {
                t = DayOfYear + ((18.0 - lngHour) / 24.0);    // if setting
            }
            /*
            string TAG = "Warning";
            Log.Warn(TAG, "DayOfYear: " + DayOfYear.ToString());
            Log.Warn(TAG, "lngHour: " + lngHour.ToString());
            */
            // 3. calculate the Sun's mean anomaly
            double M = (0.9856 * t) - 3.289;
            
            // 4. calculate the Sun's true longitude, and adjust it to the range of (0, 360)
            double L = M + (1.916 * Math.Sin(Deg2Rad(M))) + (0.020 * Math.Sin(Deg2Rad(2 * M))) + 282.634;
            L = FixValue(L, 0, 360);

            // 5. calculate the Sun's right ascension, and adjust it to the range of (0, 360)
            double RA = Rad2Deg(Math.Atan(0.91764 * Math.Tan(Deg2Rad(L))));

            // 6. right ascension value needs to be in the same quadrant as L and need to be converted into hours
            double Lquadrant = (Math.Floor(L / 90.0)) * 90.0;
            double RAquadrant = (Math.Floor(RA / 90.0)) * 90.0;
            RA = RA + (Lquadrant - RAquadrant);
            RA = RA / 15.0;

            // 7. calculate the Sun's declination
            double sinDec = 0.39782 * Math.Sin(Deg2Rad(L));
            double cosDec = Math.Cos(Math.Asin(sinDec));

            // 8. calculate the Sun's local hour angle
            double cosH = (-0.01454 - (sinDec * Math.Sin(Deg2Rad(latitude)))) / (cosDec * Math.Cos(Deg2Rad(latitude)));
            if (cosH > 1)
            {
                flagrise = 100;
            }
            else if (cosH < -1)
            {
                flagset = 100;
            }

            // 9. finish calculating H and convert into hours
            double H;
            if (flag == 1)
            {
                H = 360.0 - Rad2Deg(Math.Acos(cosH));
            }
            else
            {
                H = Rad2Deg(Math.Acos(cosH));
            }
            H = H / 15.0;

            // 10. calculate local mean time of rising/setting
            double T = H + RA - (0.06571 * t) - 6.622;

            // 11. adjust back to UTC
            double UT = T - lngHour;

            UT = UT + utcOffset;
            UT = FixValue(UT, 0, 24);

            return (int)Math.Round(UT * 3600.0);
        }

        private static double Deg2Rad(double angle)
        {
            return PI * angle / 180.0;
        }

        private static double Rad2Deg(double angle)
        {
            return 180.0 * angle / PI;
        }

        private static double FixValue(double value, double min, double max)
        {
            while (value < min)
            {
                value += (max - min);
            }
            while (value >= max)
            {
                value -= (max - min);
            }
            return value;
        }
    }
}