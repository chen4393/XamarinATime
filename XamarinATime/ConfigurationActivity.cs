using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using Java.Util;
using Android.Widget;
using Android.Views.InputMethods;
using Java.Lang;
using Android.Util;
using Android.Graphics;
using Android.Content.Res;

namespace XamarinATime
{
    [Activity(Label = "ConfigurationActivity")]
    public class ConfigurationActivity : Activity
    {
        public const string PREFS_NAME = "MyPrefsFile";
        public const string EXTRA_MESSAGE = "ATimeActivity";
        public const string EXTRA_MESSAGE_1 = "ATimeActivity_1";
        public const string EXTRA_MESSAGE_2 = "ATimeActivity_2";
        public const string EXTRA_MESSAGE_3 = "ATimeActivity_3";
        public const string EXTRA_MESSAGE_4 = "ATimeActivity_4";
        public const string EXTRA_MESSAGE_5 = "ATimeActivity_5";
        public const string EXTRA_MESSAGE_6 = "ATimeActivity_6";
        public const string EXTRA_MESSAGE_7 = "ATimeActivity_7";
        public const string EXTRA_MESSAGE_8 = "ATimeActivity_8";

        private LocationManager locationManager;
        private string provider;

        private double currentLongitude = 44.98;
        private double currentLatitude = -93.24;
        private double offsetFromUtc = -5;

        private Calendar current_cal;
        private DatePicker datepicker;

        private string message_lat;
        private string message_lon;
        private string message_off;

        private bool afterConfig = false;
        private bool wantToday;

        private int userYear = Calendar.Instance.Get(CalendarField.Year);
        private int userMonth = Calendar.Instance.Get(CalendarField.Month);
        private int userDay = Calendar.Instance.Get(CalendarField.DayOfMonth);

        private EditText inputLatitude;
        private EditText inputLongitude;
        private EditText inputOffset;

        private Button submit;
        private Button currentLocation;
        private Button currentDate;
        private Button findMyLocation;
        private Button about;

        private string message_year;
        private string message_month;
        private string message_day;
        private string message_wantToday;
        private string speech_Available;
        private string message_config;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.configuration);

            locationManager = (LocationManager)GetSystemService(LocationService);
            bool enabled_GPS = false;
            enabled_GPS = locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            if (enabled_GPS)
            {
                provider = LocationManager.GpsProvider;
            }
            else
            {
                provider = LocationManager.NetworkProvider;
            }

            Location location = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
            if (location != null)
            {
                currentLatitude = location.Latitude;
                currentLatitude = System.Math.Round(currentLatitude * 100000.00) / 100000.00;
                currentLongitude = location.Longitude;
                currentLongitude = System.Math.Round(currentLongitude * 100000.00) / 100000.00;
            }
            else
            {
                currentLatitude = 44.98;
                currentLongitude = -93.24;
            }

            Log.Warn(MainActivity.TAG, "enabled_GPS == true? " + (enabled_GPS == true).ToString());
            Log.Warn(MainActivity.TAG, "location != null? " + (location != null).ToString());
            Log.Warn(MainActivity.TAG, "currentLatitude: " + currentLatitude.ToString());
            Log.Warn(MainActivity.TAG, "currentLongitude: " + currentLongitude.ToString());

            current_cal = Calendar.Instance;
            TimeZone tz = current_cal.TimeZone;
            Date date = current_cal.Time;
            offsetFromUtc = (tz.GetOffset(date.Time)) / 3600000.0;

            datepicker = FindViewById<DatePicker>(Resource.Id.datePicker_1);
            datepicker.Init(current_cal.Get(CalendarField.Year), current_cal.Get(CalendarField.Month),
                            current_cal.Get(CalendarField.DayOfMonth), null);

            ISharedPreferences settings = GetSharedPreferences(PREFS_NAME, 0);
            message_lat = settings.GetString("myLatitude", currentLatitude.ToString());
            message_lon = settings.GetString("myLongitude", currentLongitude.ToString());
            message_off = settings.GetString("myOffset", offsetFromUtc.ToString());
            userYear = settings.GetInt("myYear", userYear);
            userMonth = settings.GetInt("myMonth", userMonth);
            userDay = settings.GetInt("myDay", userDay);
            Init();
        }

        protected override void OnPause()
        {
            base.OnPause();
            StoreConfig();
        }

        protected override void OnResume()
        {
            base.OnResume();
            locationManager = (LocationManager)GetSystemService(Context.LocationService);
            bool enabled_GPS = locationManager.IsProviderEnabled(LocationManager.GpsProvider);
            if (enabled_GPS)
            {
                provider = LocationManager.GpsProvider;
            }
            else
            {
                provider = LocationManager.NetworkProvider;
            }

            Location location = locationManager.GetLastKnownLocation(LocationManager.NetworkProvider);
            if (location != null)
            {
                currentLatitude = location.Latitude;
                currentLatitude = System.Math.Round(currentLatitude * 100000.00) / 100000.00;
                currentLongitude = location.Longitude;
                currentLongitude = System.Math.Round(currentLongitude * 100000.00) / 100000.00;
            }
            else
            {
                currentLatitude = 44.98;
                currentLongitude = -93.24;
            }

            current_cal = Calendar.Instance;
            TimeZone tz = current_cal.TimeZone;
            Date date = current_cal.Time;
            offsetFromUtc = (tz.GetOffset(date.Time)) / 3600000.0;

            datepicker = FindViewById<DatePicker>(Resource.Id.datePicker_1);
            /*
            datepicker.Init(current_cal.Get(CalendarField.Year), current_cal.Get(CalendarField.Month),
                            current_cal.Get(CalendarField.DayOfMonth), null);
            */
            
            datepicker.Init(userYear, userMonth, userDay, null);

            ISharedPreferences settings = GetSharedPreferences(PREFS_NAME, 0);
            message_lat = settings.GetString("myLatitude", currentLatitude.ToString());
            message_lon = settings.GetString("myLongitude", currentLongitude.ToString());
            message_off = settings.GetString("myOffset", offsetFromUtc.ToString());
            userYear = settings.GetInt("myYear", userYear);
            userMonth = settings.GetInt("myMonth", userMonth);
            userDay = settings.GetInt("myDay", userDay);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            //ApplicationContext.GetSharedPreferences(PREFS_NAME, 0).Edit().Clear().Commit();
        }

        protected void StoreConfig()
        {
            ApplicationContext.GetSharedPreferences(PREFS_NAME, 0).Edit().Clear().Commit();

            ISharedPreferences settings = GetSharedPreferences(PREFS_NAME, 0);
            ISharedPreferencesEditor editor = settings.Edit();
            editor.PutInt("myYear", userYear);
            editor.PutInt("myMonth", userMonth);
            editor.PutInt("myDay", userDay);
            Log.Warn(MainActivity.TAG, "userMonth: " + userMonth);
            Log.Warn(MainActivity.TAG, "userDay: " + userDay);

            datepicker = FindViewById<DatePicker>(Resource.Id.datePicker_1);
            datepicker.Init(userYear, userMonth, userDay, null);

            bool result = CheckBeforeStore(message_lat, message_lon, message_off);

            if (result)
            {
                editor.PutString("myLatitude", message_lat);
                editor.PutString("myLongitude", message_lon);
                editor.PutString("myOffset", message_off);
                editor.Commit();
            }
        }

        private void Init()
        {
            wantToday = true;
            inputLatitude = (EditText)FindViewById(Resource.Id.input_latitude);
            inputLatitude.Text = ("" + message_lat);
            inputLatitude.ImeOptions = ImeAction.Done;

            inputLongitude = (EditText)FindViewById(Resource.Id.input_longitude);
            inputLongitude.Text = ("" + message_lon);
            inputLongitude.ImeOptions = ImeAction.Done;

            inputOffset = (EditText)FindViewById(Resource.Id.input_offset);
            inputOffset.Text = ("" + message_off);
            inputOffset.ImeOptions = ImeAction.Done;

            currentLocation = (Button)FindViewById(Resource.Id.current_loc);
            currentLocation.Click += delegate
            {
                inputLatitude.Text = ("" + currentLatitude);
                inputLongitude.Text = ("" + currentLongitude);
                inputOffset.Text = ("" + offsetFromUtc);
                ColorStateList mList = currentLocation.TextColors;
                int color = mList.DefaultColor;
                if (color.Equals(Color.Red))
                {
                    currentLocation.SetTextColor(Color.Black);
                }
            };
            currentDate = (Button)FindViewById(Resource.Id.current_date);
            currentDate.Click += delegate
            {
                datepicker.UpdateDate(current_cal.Get(CalendarField.Year),
                    current_cal.Get(CalendarField.Month), 
                    current_cal.Get(CalendarField.DayOfMonth));
                ColorStateList mList = currentDate.TextColors;
                int color = mList.DefaultColor;
                if (color.Equals(Color.Red))
                {
                    currentDate.SetTextColor(Color.Black);
                }
            };
            findMyLocation = FindViewById<Button>(Resource.Id.findMyLoc);
            findMyLocation.Click += delegate
            {
                FragmentManager fm = FragmentManager;
                FindMyLocation findMyLocation  = new FindMyLocation();
                findMyLocation.Show(fm, "123");
            };
            about = FindViewById<Button>(Resource.Id.about_atime);
            about.Click += delegate
            {
                FragmentManager fm = FragmentManager;
                AboutFragment aboutFragment = new AboutFragment();
                aboutFragment.Show(fm, "123");
            };
            submit = (Button)FindViewById(Resource.Id.submit);
            submit.Click += delegate
            {
                SendData();
                
            };

            datepicker = FindViewById<DatePicker>(Resource.Id.datePicker_1);
            datepicker.Init(userYear, userMonth, userDay, null);

            bool result = CheckBeforeStore(message_lat, message_lon, message_off);
            if (!result)
            {
                return;
            }
            double userLatitude = Double.ParseDouble(message_lat);
            double userLongitude = Double.ParseDouble(message_lon);
            double userOffset = Double.ParseDouble(message_off);
            if (Math.Abs(currentLatitude - userLatitude) > 0.01 ||
                Math.Abs(currentLongitude - userLongitude) > 0.01 ||
                Math.Abs(offsetFromUtc - userOffset) > 0.1)
            {
                currentLocation.SetTextColor(Color.Red);
            }

            if (userYear != current_cal.Get(CalendarField.Year) ||
                userMonth != current_cal.Get(CalendarField.Month) ||
                userDay != current_cal.Get(CalendarField.DayOfMonth))
            {
                currentDate.SetTextColor(Color.Red);
            }
        }

        private void SendData()
        {
            afterConfig = true;

            userYear = datepicker.Year;
            userMonth = datepicker.Month;
            userDay = datepicker.DayOfMonth;

            message_lat = inputLatitude.Text.ToString();
            message_lon = inputLongitude.Text.ToString();
            message_off = inputOffset.Text.ToString();
            message_year = Integer.ToString(userYear);
            message_month = Integer.ToString(userMonth);
            message_day = Integer.ToString(userDay);

            if (userYear == (current_cal.Get(CalendarField.Year)) && 
                userMonth == (current_cal.Get(CalendarField.Month)) && 
                userDay == (current_cal.Get(CalendarField.DayOfMonth)))
            {
                wantToday = true;
            }
            else
            {
                wantToday = false;
            }

            message_wantToday = Boolean.ToString(wantToday);
            message_config = Boolean.ToString(afterConfig);
            Intent intent = new Intent(this, typeof(MainActivity));
            intent.SetFlags(ActivityFlags.ClearTop);
            bool result = CheckInput(message_lat, message_lon, message_off);
            if (result)
            {
                intent.PutExtra(EXTRA_MESSAGE, message_lat.ToString());
                intent.PutExtra(EXTRA_MESSAGE_1, message_lon.ToString());
                intent.PutExtra(EXTRA_MESSAGE_2, message_off.ToString());
                intent.PutExtra(EXTRA_MESSAGE_3, message_year.ToString());
                intent.PutExtra(EXTRA_MESSAGE_4, message_month.ToString());
                intent.PutExtra(EXTRA_MESSAGE_5, message_day.ToString());
                intent.PutExtra(EXTRA_MESSAGE_6, message_config.ToString());
                intent.PutExtra(EXTRA_MESSAGE_7, message_wantToday.ToString());
                StartActivity(intent);
                Finish();
            }
        }

        private bool CheckInput(string latitude, string longitude, string offset)
        {
            bool result = true;
            if (latitude == null || longitude == null)
            {
                result = false;
                Toast.MakeText(ApplicationContext,
                    "Please input non empty number which is in the range of -90 to 90 for latitude and -180 to 180 for logitute",
                    ToastLength.Long).Show();
                
            }
            if (latitude.Equals("") || latitude.Equals("-") || latitude.Equals(".") ||
                    latitude.Equals(".-") || latitude.Equals("-."))
            {
                result = false;
                Toast.MakeText(ApplicationContext,
                    "Please input a valid latitude number which is in the range of -90 to 90",
                    ToastLength.Long).Show();
            }
            else
            {
                double lat = Double.ParseDouble(latitude);
                if (lat > 90.0 || lat < -90.0)
                {
                    Toast.MakeText(ApplicationContext,
                        "Please input a valid latitude number which is in the range of -90 to 90",
                        ToastLength.Long).Show();
                    result = false;
                }
            }
            if (longitude.Equals("") || longitude.Equals("-") || longitude.Equals(".") ||
                    longitude.Equals(".-") || longitude.Equals("-."))
            {
                result = false;
                Toast.MakeText(ApplicationContext,
                    "Please input a valid longitude number which is in the range of -180 to 180",
                    ToastLength.Long).Show();
            }
            else
            {
                double lon = Double.ParseDouble(longitude);
                if (lon > 180.0 || lon < -180.0)
                {
                    Toast.MakeText(ApplicationContext,
                        "Please input a valid longitude number which is in the range of -180 to 180",
                        ToastLength.Long).Show();
                    result = false;
                }
            }
            if (offset.Equals("") || offset.Equals("-") || offset.Equals(".") ||
                    offset.Equals(".-") || offset.Equals("-."))
            {
                result = false;
                Toast.MakeText(ApplicationContext,
                    "Please input a valid offset number which is in the range of -12 to 12",
                    ToastLength.Long).Show();
            }
            else
            {
                double off = Double.ParseDouble(offset);
                if (off > 12.0 || off < -12.0)
                {
                    Toast.MakeText(ApplicationContext,
                        "Please input a valid offset number which is in the range of -12 to 12",
                        ToastLength.Long).Show();
                    result = false;
                }
            }
            if (result == true)
            {
                double off = Double.ParseDouble(offset);
                if ((off <= (Double.ParseDouble(longitude) / 15 - 3.5)) ||
                    (off >= (Double.ParseDouble(longitude) / 15 + 3.5)))
                {
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("Wrong Configuration ??");
                    alert.SetMessage("Your timezone configuration doesn't match with the longtitude, please check again");
                    
                    alert.SetNeutralButton("OK", (senderAlert, args) =>
                    {
                        Toast.MakeText(this, "Set again!", ToastLength.Short).Show();
                        
                    });
                    

                    Dialog dialog = alert.Create();
                    dialog.Show();

                    result = false;
                }
            }
            return result;
        }
   
        private bool CheckBeforeStore(string latitude, string longitude, string offset)
        {
            bool result = true;
            if (latitude == null || longitude == null)
            {
                result = false;
            }

            if (latitude.Equals("") || latitude.Equals("-") || latitude.Equals(".") || 
                latitude.Equals(".-") || latitude.Equals("-."))
            {
                result = false;
            }
            else
            {
                double lat = Double.ParseDouble(latitude);
                if (lat > 90.0 || lat < -90.0)
                {
                    result = false;
                }
            }

            if (longitude.Equals("") || longitude.Equals("-") || longitude.Equals(".") || 
                longitude.Equals(".-") || longitude.Equals("-."))
            {
                result = false;
            }
            else
            {
                double lon = Double.ParseDouble(longitude);
                if (lon > 180.0 || lon < -180.0)
                {
                    result = false;
                }
            }

            if (offset.Equals("") || offset.Equals("-") || offset.Equals(".") || 
                offset.Equals(".-") || offset.Equals("-."))
            {
                result = false;
            }
            else
            {
                double off = Double.ParseDouble(offset);
                if (off > 12.0 || off < -12.0)
                {
                    result = false;
                }
            }

            if (result == true)
            {
                double off = Double.ParseDouble(offset);
                if ((off <= (Double.ParseDouble(longitude) / 15 - 3.5)) || 
                    (off >= (Double.ParseDouble(longitude) / 15 + 3.5)))
                {
                    result = false;
                }
            }
            return result;
        }
    }

}