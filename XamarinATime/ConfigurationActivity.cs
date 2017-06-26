using Android.App;
using Android.Content;
using Android.OS;
using Android.Locations;
using Java.Util;
using Android.Widget;
using Android.Views.InputMethods;

namespace XamarinATime
{
    [Activity(Label = "ConfigurationActivity")]
    public class ConfigurationActivity : Activity
    {
        private const string PREFS_NAME = "MyPrefsFile";

        private LocationManager locationManager;
        private string provider;

        private double currentLongitude;
        private double currentLatitude;
        private double offsetFromUtc;

        private Calendar current_cal;
        private DatePicker datepicker;

        private string message_lat;
        private string message_lon;
        private string message_off;

        private bool wantToday;

        private EditText inputLatitude;
        private EditText inputLongitude;
        private EditText inputOffset;

        private Button submit;
        private Button reset;
        private Button currentDate;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.configuration);

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

            Location location = locationManager.GetLastKnownLocation(provider);
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
            datepicker.Init(current_cal.Get(CalendarField.Year), current_cal.Get(CalendarField.Month),
                            current_cal.Get(CalendarField.DayOfMonth), null);

            ISharedPreferences settings = GetSharedPreferences(PREFS_NAME, 0);
            message_lat = settings.GetString("myLatitude", currentLatitude.ToString());
            message_lon = settings.GetString("myLongitude", currentLongitude.ToString());
            message_off = settings.GetString("myOffset", offsetFromUtc.ToString());
            init();
        }

        public void init()
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

            submit = (Button)FindViewById(Resource.Id.submit);
            reset = (Button)FindViewById(Resource.Id.reset);
            currentDate = (Button)FindViewById(Resource.Id.current_date);
        }
    }

}