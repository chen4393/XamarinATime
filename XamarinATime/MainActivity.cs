using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using System.Text.RegularExpressions;

namespace XamarinATime
{
    [Activity(Label = "XamarinATime", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public const string TAG = "Warning";

        private Calendar current_cal;

        private int today_year, today_month, today_day, today_of_year;

        public TextView timeDisplay, configureText;

        private string displayDate;

        private double user_offset = -5.0;

        private SimpleDateFormat formater = new SimpleDateFormat("MMM-dd");

        private SunTime suntime;
        private double user_latitude;
        private double user_longitude;

        TextView[] upleft = new TextView[9];
        TextView[] upright = new TextView[9];
        TextView[] belowleft = new TextView[9];
        TextView[] belowright = new TextView[9];

        private string[] sunday_lastnight = { "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL", "LABH" };
        private string[] sunday_today = { "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG" };
        private string[] sunday_tonight = { "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL", "LABH", "UDWEG", "SHUBH" };
        private string[] sunday_tomorrow = { "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL", "LABH", "AMRIT" };

        private string[] monday_lastnight = { "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL", "LABH", "UDWEG", "SHUBH" };
        private string[] monday_today = { "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL", "LABH", "AMRIT" };
        private string[] monday_tonight = { "CHAL", "ROG", "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL" };
        private string[] monday_tomorrow = { "ROG", "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH", "ROG" };

        private string[] tuesday_lastnight = { "CHAL", "ROG", "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL" };
        private string[] tuesday_today = { "ROG", "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH", "ROG" };
        private string[] tuesday_tonight = { "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL" };
        private string[] tuesday_tomorrow = { "LABH", "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL", "LABH" };

        private string[] wednesday_lastnight = { "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL" };
        private string[] wednesday_today = { "LABH", "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL", "LABH" };
        private string[] wednesday_tonight = { "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL", "LABH", "UDWEG" };
        private string[] wednesday_tomorrow = { "SHUBH", "ROG", "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH" };

        private string[] thursday_lastnight = { "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL", "LABH", "UDWEG" };
        private string[] thursday_today = { "SHUBH", "ROG", "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH" };
        private string[] thursday_tonight = { "AMRIT", "CHAL", "ROG", "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT" };
        private string[] thursday_tomorrow = { "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL" };

        private string[] friday_lastnight = { "AMRIT", "CHAL", "ROG", "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT" };
        private string[] friday_today = { "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL" };
        private string[] friday_tonight = { "ROG", "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG" };
        private string[] friday_tomorrow = { "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL" };

        private string[] saturday_lastnight = { "ROG", "KAAL", "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG" };
        private string[] saturday_today = { "KAAL", "SHUBH", "ROG", "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL" };
        private string[] saturday_tonight = { "LABH", "UDWEG", "SHUBH", "AMRIT", "CHAL", "ROG", "KAAL", "LABH" };
        private string[] saturday_tomorrow = { "UDWEG", "CHAL", "LABH", "AMRIT", "KAAL", "SHUBH", "ROG", "UDWEG" };

        private int[] lastnight_time = new int[8];
        private int[] today_time = new int[8];
        private int[] tonight_time = new int[8];
        private int[] tomorrow_time = new int[8];

        private int day_user;
        private int flagrise;
        private int flagset;
        private int day_of_week;

        private int sunsetTime_yesterdayDefault;
        private int sunriseTime_todayDefault;
        private int sunsetTime_todayDefault;
        private int sunriseTime_tomorrowDefault;
        private int sunsetTime_tomorrowDefault;

        private int mYear;
        private int mDayOfYear;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            this.RequestWindowFeature(WindowFeatures.NoTitle);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            TimeZone timeZone = TimeZone.GetTimeZone("GMT-05:00");
            current_cal = Calendar.GetInstance(timeZone);
            today_year = current_cal.Get(CalendarField.Year);
            today_month = current_cal.Get(CalendarField.Month);
            today_day = current_cal.Get(CalendarField.DayOfMonth);
            today_day = current_cal.Get(CalendarField.DayOfYear);
            mYear = current_cal.Get(CalendarField.Year);
            mDayOfYear = current_cal.Get(CalendarField.DayOfYear);
            Init();
        }

        private void Init()
        {
            timeDisplay = (TextView)FindViewById<TextView>(Resource.Id.timeDisplay);
            configureText = (TextView)FindViewById<TextView>(Resource.Id.configure_text);

            configureText.Text = "Using current time and location";

            current_cal.Set(today_year, today_month, today_day);

            Date dis_date = new Date();
            displayDate = formater.Format(dis_date);
            timeDisplay.Text = displayDate;

            user_latitude = 44.96987;
            user_longitude = -93.22678;
            user_offset = -5.00;

            Log.Warn(TAG, "this is a warning message");
            Log.Error(TAG, "this is an error message");
            
            InitPanel();
            CalculateTimeSequence();

            day_of_week = current_cal.Get(CalendarField.DayOfWeek);
            //Log.Warn(TAG, today_year + " " + today_month + " " + today_day);
            //Log.Warn(TAG, day_of_week.ToString());
            SetSequence(day_of_week);
            DrawMaker();
            Button about = FindViewById<Button>(Resource.Id.About);
            about.Click += delegate
            {
                FragmentManager fm = FragmentManager;
                AboutFragment aboutFragment = new AboutFragment();
                aboutFragment.Show(fm, "123");
            };

            Button configuration = FindViewById<Button>(Resource.Id.Configuration);
            configuration.Click += delegate
            {
                /*
                Context context = ApplicationContext;
                string text = "Configure the location and date";
                ToastLength duration = ToastLength.Short;
                Toast toast = Toast.MakeText(context, text, duration);
                toast.Show();

                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Configure Ausp Time");
                alert.SetMessage("Alert Dialog");
                alert.SetPositiveButton("Good", (senderAlert, args) => {
                    //change value write your own set of instructions
                    //you can also create an event for the same in xamarin
                    //instead of writing things here
                });
                alert.SetNegativeButton("Not doing great", (senderAlert, args) => {
                    //perform your own task for this conditional button click
                });
                RunOnUiThread(() => {
                    alert.Show();
                });
                */
                Intent intent = new Intent(this, typeof(ConfigurationActivity));
                StartActivity(intent);
                Finish();
            };
        }

        private void InitPanel()
        {
            upright[0] = (TextView)FindViewById(Resource.Id.text_today_1);
            upright[1] = (TextView)FindViewById(Resource.Id.text_today_2);
            upright[2] = (TextView)FindViewById(Resource.Id.text_today_3);
            upright[3] = (TextView)FindViewById(Resource.Id.text_today_4);
            upright[4] = (TextView)FindViewById(Resource.Id.text_today_5);
            upright[5] = (TextView)FindViewById(Resource.Id.text_today_6);
            upright[6] = (TextView)FindViewById(Resource.Id.text_today_7);
            upright[7] = (TextView)FindViewById(Resource.Id.text_today_8);
            upright[8] = (TextView)FindViewById(Resource.Id.text_today_9);

            upleft[0] = (TextView)FindViewById(Resource.Id.text_lastnight_1);
            upleft[1] = (TextView)FindViewById(Resource.Id.text_lastnight_2);
            upleft[2] = (TextView)FindViewById(Resource.Id.text_lastnight_3);
            upleft[3] = (TextView)FindViewById(Resource.Id.text_lastnight_4);
            upleft[4] = (TextView)FindViewById(Resource.Id.text_lastnight_5);
            upleft[5] = (TextView)FindViewById(Resource.Id.text_lastnight_6);
            upleft[6] = (TextView)FindViewById(Resource.Id.text_lastnight_7);
            upleft[7] = (TextView)FindViewById(Resource.Id.text_lastnight_8);
            upleft[8] = (TextView)FindViewById(Resource.Id.text_lastnight_9);

            belowleft[0] = (TextView)FindViewById(Resource.Id.text_tonight_1);
            belowleft[1] = (TextView)FindViewById(Resource.Id.text_tonight_2);
            belowleft[2] = (TextView)FindViewById(Resource.Id.text_tonight_3);
            belowleft[3] = (TextView)FindViewById(Resource.Id.text_tonight_4);
            belowleft[4] = (TextView)FindViewById(Resource.Id.text_tonight_5);
            belowleft[5] = (TextView)FindViewById(Resource.Id.text_tonight_6);
            belowleft[6] = (TextView)FindViewById(Resource.Id.text_tonight_7);
            belowleft[7] = (TextView)FindViewById(Resource.Id.text_tonight_8);
            belowleft[8] = (TextView)FindViewById(Resource.Id.text_tonight_9);

            belowright[0] = (TextView)FindViewById(Resource.Id.text_belowright_1);
            belowright[1] = (TextView)FindViewById(Resource.Id.text_belowright_2);
            belowright[2] = (TextView)FindViewById(Resource.Id.text_belowright_3);
            belowright[3] = (TextView)FindViewById(Resource.Id.text_belowright_4);
            belowright[4] = (TextView)FindViewById(Resource.Id.text_belowright_5);
            belowright[5] = (TextView)FindViewById(Resource.Id.text_belowright_6);
            belowright[6] = (TextView)FindViewById(Resource.Id.text_belowright_7);
            belowright[7] = (TextView)FindViewById(Resource.Id.text_belowright_8);
            belowright[8] = (TextView)FindViewById(Resource.Id.text_belowright_9);

            upleft[0].Text = "Last night";
            upright[0].Text = "Today";
            belowleft[0].Text = "Tonight";
            belowright[0].Text = "Tomorrow";
        }

        private void CalculateTimeSequence()
        {
            TimeZone timeZone = TimeZone.GetTimeZone("GMT-05:00");
            current_cal = Calendar.GetInstance(timeZone);
            suntime = new SunTime(user_latitude, user_longitude, user_offset, current_cal);
            sunriseTime_todayDefault = suntime.sunriseTime;
            sunsetTime_todayDefault = suntime.sunsetTime;
            flagrise = suntime.flagrise;
            flagset = suntime.flagset;

            Log.Warn(TAG, "current_cal: " + current_cal.ToString());

            Calendar c_temp_yesterday = new GregorianCalendar(current_cal.Get(CalendarField.Year),
                                                              current_cal.Get(CalendarField.Month), 
                                                              current_cal.Get(CalendarField.DayOfMonth) - 1);
            suntime = new SunTime(user_latitude, user_longitude, user_offset, c_temp_yesterday);
            sunsetTime_yesterdayDefault = suntime.sunsetTime;

            Calendar c_temp_tomorrow = new GregorianCalendar(current_cal.Get(CalendarField.Year),
                                                             current_cal.Get(CalendarField.Month), 
                                                             current_cal.Get(CalendarField.DayOfMonth) + 1);
            suntime = new SunTime(user_latitude, user_longitude, user_offset, c_temp_tomorrow);
            sunriseTime_tomorrowDefault = suntime.sunriseTime;
            sunsetTime_tomorrowDefault = suntime.sunsetTime;

            int temp_1 = (86400 - sunsetTime_yesterdayDefault + sunriseTime_todayDefault) / 8;   // lastnight
            int temp_2 = (sunsetTime_todayDefault - sunriseTime_todayDefault) / 8;               // today
            int temp_3 = (86400 - sunsetTime_todayDefault + sunriseTime_tomorrowDefault) / 8;    // tonight
            int temp_4 = (sunsetTime_tomorrowDefault - sunriseTime_tomorrowDefault) / 8;         // tomorrow

            for (int i = 0; i < 8; i++)
            {
                lastnight_time[i] = sunsetTime_yesterdayDefault + temp_1 * i;
                today_time[i] = sunriseTime_todayDefault + temp_2 * i;
                tonight_time[i] = sunsetTime_todayDefault + temp_3 * i;
                tomorrow_time[i] = sunriseTime_tomorrowDefault + temp_4 * i;
            }
        }

        private void SetSequence(int day)
        {
            timeDisplay.SetTextColor(Color.Black);
            switch (day)
            {
                case 1:
                    for (int i = 0; i < 8; i++)
                    {
                        upleft[i + 1].Text = sunday_lastnight[i] + " " + SplitTime(lastnight_time[i]);
                        upright[i + 1].Text = sunday_today[i] + " " + SplitTime(today_time[i]);
                        belowleft[i + 1].Text = sunday_tonight[i] + " " + SplitTime(tonight_time[i]);
                        belowright[i + 1].Text = sunday_tomorrow[i] + " " + SplitTime(tomorrow_time[i]);
                    }
                    PaintColor();
                    UpdateClockBackgroundColor(1);
                    break;

                case 2:
                    for (int i = 0; i < 8; i++)
                    {
                        upleft[i + 1].Text = monday_lastnight[i] + " " + SplitTime(lastnight_time[i]);
                        upright[i + 1].Text = monday_today[i] + " " + SplitTime(today_time[i]);
                        belowleft[i + 1].Text = monday_tonight[i] + " " + SplitTime(tonight_time[i]);
                        belowright[i + 1].Text = monday_tomorrow[i] + " " + SplitTime(tomorrow_time[i]);
                    }
                    PaintColor();
                    UpdateClockBackgroundColor(2);
                    break;

                case 3:
                    for (int i = 0; i < 8; i++)
                    {
                        upleft[i + 1].Text = tuesday_lastnight[i] + " " + SplitTime(lastnight_time[i]);
                        upright[i + 1].Text = tuesday_today[i] + " " + SplitTime(today_time[i]);
                        belowleft[i + 1].Text = tuesday_tonight[i] + " " + SplitTime(tonight_time[i]);
                        belowright[i + 1].Text = tuesday_tomorrow[i] + " " + SplitTime(tomorrow_time[i]);
                    }
                    PaintColor();
                    UpdateClockBackgroundColor(3);
                    break;

                case 4:
                    for (int i = 0; i < 8; i++)
                    {
                        upleft[i + 1].Text = wednesday_lastnight[i] + " " + SplitTime(lastnight_time[i]);
                        upright[i + 1].Text = wednesday_today[i] + " " + SplitTime(today_time[i]);
                        belowleft[i + 1].Text = wednesday_tonight[i] + " " + SplitTime(tonight_time[i]);
                        belowright[i + 1].Text = wednesday_tomorrow[i] + " " + SplitTime(tomorrow_time[i]);
                    }
                    PaintColor();
                    UpdateClockBackgroundColor(4);
                    break;

                case 5:
                    for (int i = 0; i < 8; i++)
                    {
                        upleft[i + 1].Text = thursday_lastnight[i] + " " + SplitTime(lastnight_time[i]);
                        upright[i + 1].Text = thursday_today[i] + " " + SplitTime(today_time[i]);
                        belowleft[i + 1].Text = thursday_tonight[i] + " " + SplitTime(tonight_time[i]);
                        belowright[i + 1].Text = thursday_tomorrow[i] + " " + SplitTime(tomorrow_time[i]);
                    }
                    PaintColor();
                    UpdateClockBackgroundColor(5);
                    break;

                case 6:
                    for (int i = 0; i < 8; i++)
                    {
                        upleft[i + 1].Text = friday_lastnight[i] + " " + SplitTime(lastnight_time[i]);
                        upright[i + 1].Text = friday_today[i] + " " + SplitTime(today_time[i]);
                        belowleft[i + 1].Text = friday_tonight[i] + " " + SplitTime(tonight_time[i]);
                        belowright[i + 1].Text = friday_tomorrow[i] + " " + SplitTime(tomorrow_time[i]);
                    }
                    PaintColor();
                    UpdateClockBackgroundColor(6);
                    break;

                case 7:
                    for (int i = 0; i < 8; i++)
                    {
                        upleft[i + 1].Text = saturday_lastnight[i] + " " + SplitTime(lastnight_time[i]);
                        upright[i + 1].Text = saturday_today[i] + " " + SplitTime(today_time[i]);
                        belowleft[i + 1].Text = saturday_tonight[i] + " " + SplitTime(tonight_time[i]);
                        belowright[i + 1].Text = saturday_tomorrow[i] + " " + SplitTime(tomorrow_time[i]);
                    }
                    PaintColor();
                    UpdateClockBackgroundColor(7);
                    break;

                default:
                    break;
            }
        }

        private string SplitTime(int time)
        {

            string results = null;

            if (time > 86400)
            {
                time = time - 86400;
            }
            int hours = (time / 3600);
            int remainder = (time - 3600 * hours);
            int mins = remainder / 60;

            if (hours > 12)
            {
                results = string.Format("{0:D2}:{1:D2}" + " PM", hours - 12, mins);
            }
            else if (hours == 12)
            {
                results = string.Format("12:{0:D2}" + " PM", mins);
            }
            else if (hours > 0 && hours < 12)
            {
                results = string.Format("{0:D2}:{1:D2}" + " AM", hours, mins);
            }
            else if (hours == 0)
            {
                results = string.Format("12:{0:D2}" + " AM", mins);
            }

            return results;
        }

        // put the hand pointer symbol
        private void DrawMaker()
        {
            Calendar calendar = Calendar.GetInstance(TimeZone.Default);

            int year_now = calendar.Get(CalendarField.Year);
            int day_year_now = calendar.Get(CalendarField.DayOfYear);

            for (int i = 1; i < 9; i++)
            {
                char[] delimiterChars = {' '};
                string[] token = upleft[i].Text.ToString().Split(delimiterChars);
                Regex rgx = new Regex("\u261E.*");
                if (rgx.IsMatch(token[0]))
                {
                    upleft[i].Text = token[1] + " " + token[2] + " " + token[3];
                }

                string[] token_1 = upright[i].Text.ToString().Split(delimiterChars);
                if (rgx.IsMatch(token_1[0]))
                {
                    upright[i].Text = token_1[1] + " " + token_1[2] + " " + token_1[3];
                }

                string[] token_2 = belowleft[i].Text.ToString().Split(delimiterChars);
                if (rgx.IsMatch(token_2[0]))
                {
                    belowleft[i].Text = token_2[1] + " " + token_2[2] + " " + token_2[3];
                }
            }

            if (mYear == year_now && mDayOfYear == day_year_now)
            {
                int hour = calendar.Get(CalendarField.HourOfDay);
                int minute = calendar.Get(CalendarField.Minute);
                int second = calendar.Get(CalendarField.Second);
                int temp = hour * 3600 + minute * 60 + second;

                if (temp < today_time[0])
                {  // last day night

                    temp = temp + 86400;

                    if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                    {
                        upleft[1].Text = ("\u261E" + " " + upleft[1].Text);
                    }

                    else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                    {
                        upleft[2].Text = ("\u261E" + " " + upleft[2].Text);
                    }

                    else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                    {
                        upleft[3].Text = ("\u261E" + " " + upleft[3].Text);
                    }

                    else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4] - 20)
                    {
                        upleft[4].Text = ("\u261E" + " " + upleft[4].Text);
                    }

                    else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                    {
                        upleft[5].Text = ("\u261E" + " " + upleft[5].Text);
                    }

                    else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                    {
                        upleft[6].Text = ("\u261E" + " " + upleft[6].Text);
                    }

                    else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                    {
                        upleft[7].Text = ("\u261E" + " " + upleft[7].Text);
                    }

                    else if (temp >= lastnight_time[7] - 20)
                    {
                        upleft[8].Text = ("\u261E" + " " + upleft[8].Text);
                    }
                }

                else if (temp >= today_time[0] && temp < tonight_time[0])
                {

                    if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                    {
                        upright[1].Text = ("\u261E" + " " + upright[1].Text);
                    }

                    else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                    {
                        upright[2].Text = ("\u261E" + " " + upright[2].Text);
                    }

                    else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                    {
                        upright[3].Text = ("\u261E" + " " + upright[3].Text);

                    }

                    else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                    {
                        upright[4].Text = ("\u261E" + " " + upright[4].Text);
                    }

                    else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                    {
                        upright[5].Text = ("\u261E" + " " + upright[5].Text);
                    }

                    else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                    {
                        upright[6].Text = ("\u261E" + " " + upright[6].Text);
                    }

                    else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                    {
                        upright[7].Text = ("\u261E" + " " + upright[7].Text);
                    }

                    else if (temp >= today_time[7] - 20 && temp < tonight_time[0] - 20)
                    {
                        upright[8].Text = ("\u261E" + " " + upright[8].Text);
                    }
                }
                else
                {

                    if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                    {
                        belowleft[1].Text = ("\u261E" + " " + belowleft[1].Text);
                    }

                    else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                    {
                        belowleft[2].Text = ("\u261E" + " " + belowleft[2].Text);
                    }

                    else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                    {
                        belowleft[3].Text = ("\u261E" + " " + belowleft[3].Text);
                    }

                    else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                    {
                        belowleft[4].Text = ("\u261E" + " " + belowleft[4].Text);
                    }

                    else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                    {
                        belowleft[5].Text = ("\u261E" + " " + belowleft[5].Text);
                    }

                    else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                    {
                        belowleft[6].Text = ("\u261E" + " " + belowleft[6].Text);
                    }

                    else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                    {
                        belowleft[7].Text = ("\u261E" + " " + belowleft[7].Text);
                    }

                    else if (temp >= tonight_time[7] - 20)
                    {
                        belowleft[8].Text = ("\u261E" + " " + belowleft[8].Text);
                    }
                }
            }

            else if ((mYear == today_year && mDayOfYear == today_of_year) && (day_year_now == today_of_year + 1 && year_now == today_year))
            {

                int hour = calendar.Get(CalendarField.HourOfDay);
                int minute = calendar.Get(CalendarField.Minute);
                int second = calendar.Get(CalendarField.Second);
                int temp = hour * 3600 + minute * 60 + second;

                temp = temp + 86400;

                if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                {
                    belowleft[1].Text = ("\u261E" + " " + belowleft[1].Text);
                }

                else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                {
                    belowleft[2].Text = ("\u261E" + " " + belowleft[2].Text);
                }

                else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                {
                    belowleft[3].Text = ("\u261E" + " " + belowleft[3].Text);
                }

                else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                {
                    belowleft[4].Text = ("\u261E" + " " + belowleft[4].Text);
                }

                else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                {
                    belowleft[5].Text = ("\u261E" + " " + belowleft[5].Text);
                }

                else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                {
                    belowleft[6].Text = ("\u261E" + " " + belowleft[6].Text);
                }

                else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                {
                    belowleft[7].Text = ("\u261E" + " " + belowleft[7].Text);
                }

                else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                {
                    belowleft[8].Text = ("\u261E" + " " + belowleft[8].Text);
                }
            }
            else if ((mYear == year_now && mDayOfYear == day_year_now - 1) && ((day_year_now == today_of_year && year_now == today_year)))
            {

                int hour = calendar.Get(CalendarField.HourOfDay);
                int minute = calendar.Get(CalendarField.Minute);
                int second = calendar.Get(CalendarField.Second);
                int temp = hour * 3600 + minute * 60 + second;

                temp = temp + 86400;

                if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                {
                    belowleft[1].Text = ("\u261E" + " " + belowleft[1].Text);
                }

                else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                {
                    belowleft[2].Text = ("\u261E" + " " + belowleft[2].Text);
                }

                else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                {
                    belowleft[3].Text = ("\u261E" + " " + belowleft[3].Text);
                }

                else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                {
                    belowleft[4].Text = ("\u261E" + " " + belowleft[4].Text);
                }

                else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                {
                    belowleft[5].Text = ("\u261E" + " " + belowleft[5].Text);
                }

                else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                {
                    belowleft[6].Text = ("\u261E" + " " + belowleft[6].Text);
                }

                else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                {
                    belowleft[7].Text = ("\u261E" + " " + belowleft[7].Text);
                }

                else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                {
                    belowleft[8].Text = ("\u261E" + " " + belowleft[8].Text);
                }

            }
        }

        private void PaintColor()
        {
            for (int i = 1; i < 9; i++)
            {
                if (new Regex("AMRIT.*").IsMatch(upleft[i].Text.ToString())) { upleft[i].SetBackgroundColor(Color.Argb(255, 175, 230, 255)); }
                else if (new Regex("LABH.*").IsMatch(upleft[i].Text.ToString())) { upleft[i].SetBackgroundColor(Color.Argb(255, 120, 200, 200)); }
                else if (new Regex("SHUBH.*").IsMatch(upleft[i].Text.ToString())) { upleft[i].SetBackgroundColor(Color.Argb(255, 0, 165, 190)); }
                else if (new Regex("CHAL.*").IsMatch(upleft[i].Text.ToString())) { upleft[i].SetBackgroundColor(Color.White); }
                else if (new Regex("KAAL.*").IsMatch(upleft[i].Text.ToString())) { upleft[i].SetBackgroundColor(Color.Argb(255, 255, 190, 190)); }
                else if (new Regex("ROG.*").IsMatch(upleft[i].Text.ToString())) { upleft[i].SetBackgroundColor(Color.Argb(255, 255, 128, 128)); }
                else if (new Regex("UDWEG.*").IsMatch(upleft[i].Text.ToString())) { upleft[i].SetBackgroundColor(Color.Argb(255, 255, 0, 0)); }
                else { upleft[i].SetBackgroundColor(Color.Black); }
                upleft[i].SetTextColor(Color.Black);
            }

            for (int i = 1; i < 9; i++)
            {
                if (new Regex("AMRIT.*").IsMatch(upright[i].Text.ToString())) { upright[i].SetBackgroundColor(Color.Argb(255, 175, 230, 255)); }
                else if (new Regex("LABH.*").IsMatch(upright[i].Text.ToString())) { upright[i].SetBackgroundColor(Color.Argb(255, 120, 200, 200)); }
                else if (new Regex("SHUBH.*").IsMatch(upright[i].Text.ToString())) { upright[i].SetBackgroundColor(Color.Argb(255, 0, 165, 190)); }
                else if (new Regex("CHAL.*").IsMatch(upright[i].Text.ToString())) { upright[i].SetBackgroundColor(Color.White); }
                else if (new Regex("KAAL.*").IsMatch(upright[i].Text.ToString())) { upright[i].SetBackgroundColor(Color.Argb(255, 255, 190, 190)); }
                else if (new Regex("ROG.*").IsMatch(upright[i].Text.ToString())) { upright[i].SetBackgroundColor(Color.Argb(255, 255, 128, 128)); }
                else if (new Regex("UDWEG.*").IsMatch(upright[i].Text.ToString())) { upright[i].SetBackgroundColor(Color.Argb(255, 255, 0, 0)); }
                else { upright[i].SetBackgroundColor(Color.Black); }
                upright[i].SetTextColor(Color.Black);
            }

            for (int i = 1; i < 9; i++)
            {
                if (new Regex("AMRIT.*").IsMatch(belowleft[i].Text.ToString())) { belowleft[i].SetBackgroundColor(Color.Argb(255, 175, 230, 255)); }
                else if (new Regex("LABH.*").IsMatch(belowleft[i].Text.ToString())) { belowleft[i].SetBackgroundColor(Color.Argb(255, 120, 200, 200)); }
                else if (new Regex("SHUBH.*").IsMatch(belowleft[i].Text.ToString())) { belowleft[i].SetBackgroundColor(Color.Argb(255, 0, 165, 190)); }
                else if (new Regex("CHAL.*").IsMatch(belowleft[i].Text.ToString())) { belowleft[i].SetBackgroundColor(Color.White); }
                else if (new Regex("KAAL.*").IsMatch(belowleft[i].Text.ToString())) { belowleft[i].SetBackgroundColor(Color.Argb(255, 255, 190, 190)); }
                else if (new Regex("ROG.*").IsMatch(belowleft[i].Text.ToString())) { belowleft[i].SetBackgroundColor(Color.Argb(255, 255, 128, 128)); }
                else if (new Regex("UDWEG.*").IsMatch(belowleft[i].Text.ToString())) { belowleft[i].SetBackgroundColor(Color.Argb(255, 255, 0, 0)); }
                else { belowleft[i].SetBackgroundColor(Color.Black); }
                belowleft[i].SetTextColor(Color.Black);
            }

            for (int i = 1; i < 9; i++)
            {
                if (new Regex("AMRIT.*").IsMatch(belowright[i].Text.ToString())) { belowright[i].SetBackgroundColor(Color.Argb(255, 175, 230, 255)); }
                else if (new Regex("LABH.*").IsMatch(belowright[i].Text.ToString())) { belowright[i].SetBackgroundColor(Color.Argb(255, 120, 200, 200)); }
                else if (new Regex("SHUBH.*").IsMatch(belowright[i].Text.ToString())) { belowright[i].SetBackgroundColor(Color.Argb(255, 0, 165, 190)); }
                else if (new Regex("CHAL.*").IsMatch(belowright[i].Text.ToString())) { belowright[i].SetBackgroundColor(Color.White); }
                else if (new Regex("KAAL.*").IsMatch(belowright[i].Text.ToString())) { belowright[i].SetBackgroundColor(Color.Argb(255, 255, 190, 190)); }
                else if (new Regex("ROG.*").IsMatch(belowright[i].Text.ToString())) { belowright[i].SetBackgroundColor(Color.Argb(255, 255, 128, 128)); }
                else if (new Regex("UDWEG.*").IsMatch(belowright[i].Text.ToString())) { belowright[i].SetBackgroundColor(Color.Argb(255, 255, 0, 0)); }
                else { belowright[i].SetBackgroundColor(Color.Black); }
                belowright[i].SetTextColor(Color.Black);
            }
        }

        private string GetCurrentTimeString()
        {
            string results = null;
            TimeZone timeZone = TimeZone.GetTimeZone("GMT-05:00");
            Calendar calendar = Calendar.GetInstance(timeZone);
            int hour = calendar.Get(CalendarField.HourOfDay);
            int minute = calendar.Get(CalendarField.Minute);
            Log.Warn(TAG, "Time: " + hour.ToString());

            if (hour > 12 && hour < 24)
            {
                results = string.Format("{0:D2}:{1:D2}" + " PM", hour - 12, minute);
            }
            else if (hour == 12)
            {
                results = string.Format("12:{0:D2}" + " PM", minute);
            }
            else if (hour > 0 && hour < 12)
            {
                results = string.Format("{0:D2}:{1:D2}" + " AM", hour, minute);
            }
            else if (hour == 0 || hour == 24)
            {
                results = string.Format("12:{0:D2}" + " AM", minute);
            }
            
            return results;
        }

        private void UpdateClockBackgroundColor(int day)
        {
            Calendar calendar = Calendar.Instance;
            bool isYesNight = false;
            int year_now = calendar.Get(CalendarField.Year);
            int day_year_now = calendar.Get(CalendarField.DayOfYear);
            int hour = calendar.Get(CalendarField.HourOfDay);
            int minute = calendar.Get(CalendarField.Minute);
            int second = calendar.Get(CalendarField.Second);
            // how many seconds we have passed so far in one day
            int temp = hour * 3600 + minute * 60 + second;

            if (year_now == today_year && day_year_now == today_of_year + 1)
            {
                temp = temp + 86400;
            }
            if (day_year_now == today_of_year && year_now == today_year)
            {
                isYesNight = true;
            }

            switch (day)
            {
                case 1:
                    if (isYesNight == false)
                    {
                        if (temp < today_time[0])
                        {
                            temp += 86400;
                            if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));      //LABH		   
                            }

                            else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));         //UDWEG		 
                            }

                            else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));        //SHUBH		    
                            }

                            else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4])
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));        //AMRIT		  
                            }

                            else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                               //CHAL
                            }

                            else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));               //ROG
                            }

                            else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));               //KAAL
                            }

                            else if (temp >= lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));              //LABH
                            }
                        }
                        else if (temp >= today_time[0] && temp < tonight_time[0])
                        {
                            if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                  //UDWEG
                            }
                            else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                             //CHAL
                            }
                            else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));            //LABH
                            }
                            else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));            //AMRIT
                            }
                            else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));           //KAAL
                            }
                            else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));             //SHUBH
                            }
                            else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));          //ROG
                            }
                            else if (temp >= today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));             //UDWEG
                            }
                        }
                        else
                        {
                            if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));           //SHUBH
                            }
                            else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));        //AMRIT
                            }
                            else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                       //CHAL
                            }
                            else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));        //ROG
                            }
                            else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));        //KAAL
                            }
                            else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));        //LABH
                            }
                            else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));            //UDWEG
                            }
                            else if (temp >= tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));          //SHUBH
                            }
                        }
                    }
                    else
                    {
                        temp = temp + 86400;

                        if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));           //SHUBH
                        }

                        else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));        //AMRIT
                        }

                        else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                       //CHAL
                        }

                        else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));        //ROG
                        }

                        else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));        //KAAL
                        }

                        else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));        //LABH
                        }

                        else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));            //UDWEG
                        }

                        else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));          //SHUBH
                        }
                    }
                    break;
                case 2:
                    if (isYesNight == false)
                    {
                        if (temp < today_time[0])
                        {
                            temp = temp + 86400;
                            if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));                  //SHUBH
                            }
                            else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));               //AMRIT
                            }

                            else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                                //CHAL
                            }

                            else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));               //ROG
                            }

                            else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));               //KAAL
                            }

                            else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));               //LABH
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                   //UDWEG
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));              //SHUBH
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }
                        }
                        else if (temp >= today_time[0] - 20 && temp < tonight_time[0] - 20)
                        {
                            if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));   //AMRIT
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));   //KAAL
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));     //SHUBH
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190)); 
                            }

                            else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));   //ROG
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));       //UDWEG
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                   //CHAL
                                                                                               //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));    //LABH
                                                                                                   //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));     //AMRIT
                                                                                                    //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }
                        }
                        else
                        {
                            if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                                                                                                    //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));        //ROG
                                                                                                       //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));       //KAAL
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));       //LABH
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));          //UDWEG
                                                                                                     //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));       //AMRIT
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                                                                                                    //dateDisplay.setBackgroundColor(Color.WHITE);
                            }
                        }
                    }
                    else
                    {
                        temp = temp + 86400;

                        if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                        }

                        else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));        //ROG
                        }

                        else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));       //KAAL
                        }

                        else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));       //LABH
                        }

                        else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));          //UDWEG
                        }

                        else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                        }

                        else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));       //AMRIT
                        }

                        else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                        }

                    }
                    break;
                case 3:
                    if (isYesNight == false)
                    {
                        if (temp < today_time[0])
                        {
                            // lastnight 
                            temp = temp + 86400;
                            if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                                //CHAL
                                                                                                            //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));              //ROG
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));              //KAAL
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));               //LABH
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                  //UDWEG
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0)); 
                            }

                            else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));               //SHUBH
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));                //AMRIT
                                                                                                               //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255)); 
                            }

                            else if (temp >= lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                                //CHAL
                                                                                                            //dateDisplay.setBackgroundColor(Color.WHITE);
                            }
                        }
                        else if (temp >= today_time[0] - 20 && temp < tonight_time[0] - 20)
                        {
                            // today - tonight
                            if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));              //ROG
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128)); 
                            }

                            else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                  //UDWEG
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                              //CHAL
                                                                                                          //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));              //LABH
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));              //AMRIT
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));              //KAAL
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));                //SHUBH
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));              //ROG
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }
                        }
                        else
                        {
                            //tonight - tomorrow
                            if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));             //KAAL
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));               //LABH
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                   //UDWEG
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));                 //SHUBH
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));            //AMRIT
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                           //CHAL
                                                                                                       //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));            //ROG
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));            //KAAL
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }
                        }
                    }
                    else
                    {
                        temp = temp + 86400;

                        if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));             //KAAL
                                                                                                        //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                        }

                        else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));               //LABH
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                        }

                        else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                   //UDWEG
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                        }

                        else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));                 //SHUBH
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                        }

                        else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));            //AMRIT
                                                                                                       //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                        }

                        else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                           //CHAL
                                                                                                   //dateDisplay.setBackgroundColor(Color.WHITE);
                        }

                        else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));            //ROG
                                                                                                       //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                        }

                        else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));            //KAAL
                                                                                                       //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                        }
                    }
                    break;
                case 4:
                    if (isYesNight == false)
                    {
                        if (temp < today_time[0])
                        {
                            // lastnight 
                            temp = temp + 86400;
                            if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));                 //KAAL
                                                                                                                //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));                 //LABH
                                                                                                                //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                    //UDWEG
                                                                                                               //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));                 //SHUBH
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));               //AMRIT
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                               //CHAL
                                                                                                           //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));               //ROG
                                                                                                              //.setBackgroundColor(Color.argb(255,255,128,128)); 
                            }

                            else if (temp >= lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));               //KAAL
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190)); 
                            }
                        }
                        else if (temp >= today_time[0] - 20 && temp < tonight_time[0] - 20)
                        {
                            // today - tonight
                            if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));            //LABH
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200)); 
                            }

                            else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));            //AMRIT
                                                                                                           //.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));             //KAAL 
                                                                                                            //.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));             //SHUBH
                                                                                                          //.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));           //ROG
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));              //UDWEG
                                                                                                         //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                         //CHAL
                                                                                                     //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));          //LABH
                                                                                                         //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }
                        }
                        else
                        {
                            //tonight - tomorrow
                            if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));         //UDWEG
                                                                                                    //.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));       //AMRIT
                                                                                                      //.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                      //CHAL
                                                                                                  //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));  //ROG
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));  //KAAL
                                                                                                 //.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));    //LABH
                                                                                                   //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));       //UDWEG
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }
                        }
                    }
                    else
                    {
                        temp = temp + 86400;

                        if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));         //UDWEG
                                                                                                //.setBackgroundColor(Color.argb(255,255,0,0));
                        }

                        else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                        }

                        else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));       //AMRIT
                                                                                                  //.setBackgroundColor(Color.argb(255,175,230,255));
                        }

                        else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                      //CHAL
                                                                                              //dateDisplay.setBackgroundColor(Color.WHITE);
                        }

                        else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));  //ROG
                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                        }

                        else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));  //KAAL
                                                                                             //.setBackgroundColor(Color.argb(255,255,190,190));
                        }

                        else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));    //LABH
                                                                                               //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                        }

                        else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));       //UDWEG
                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                        }
                    }
                    break;
                case 5:
                    if (isYesNight == false)
                    {
                        if (temp < today_time[0])
                        {
                            // lastnight 
                            temp = temp + 86400;
                            if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                 //UDWEG
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));                  //SHUBH
                                                                                                               //.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));            //AMRIT
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                                //CHAL
                                                                                                            //.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));               //ROG
                                                                                                              //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));           //KAAL
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));              //LABH
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200)); 
                            }

                            else if (temp >= lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));                 //UDWEG
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0)); 
                            }
                        }
                        else if (temp >= today_time[0] - 20 && temp < tonight_time[0] - 20)
                        {
                            // today - tonight
                            if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));              //SHUBH
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));            //ROG
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));              //UDWEG 
                                                                                                         //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                          //CHAL
                                                                                                      //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));           //LABH
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));           //AMRIT
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));       //KAAL
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));           //SHUBH
                                                                                                        //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190)); 
                            }
                        }
                        else
                        {
                            //tonight - tomorrow
                            if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));      //AMRIT
                                                                                                     //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                                                                                                    //dateDisplay.setBackgroundColor(Color.WHITE); 
                            }

                            else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));       //ROG
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128)); 
                            }

                            else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));    //KAAL
                                                                                                   //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));    //LABH
                                                                                                   //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));      //UDWEG
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                                                                                                      //.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));    //AMRIT
                                                                                                   //.setBackgroundColor(Color.argb(255,175,230,255));
                            }
                        }
                    }
                    else
                    {
                        temp = temp + 86400;

                        if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));      //AMRIT
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                        }

                        else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                                                                                                //dateDisplay.setBackgroundColor(Color.WHITE); 
                        }

                        else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));       //ROG
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128)); 
                        }

                        else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));    //KAAL
                                                                                               //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                        }

                        else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));    //LABH
                                                                                               //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                        }

                        else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));      //UDWEG
                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                        }

                        else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                                                                                                  //.setBackgroundColor(Color.argb(255,0,165,190));
                        }

                        else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));    //AMRIT
                                                                                               //.setBackgroundColor(Color.argb(255,175,230,255));
                        }
                    }
                    break;
                case 6:
                    if (isYesNight == false)
                    {
                        if (temp < today_time[0])
                        {
                            // lastnight 
                            temp = temp + 86400;
                            if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));           //AMRIT
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                              //CHAL
                                                                                                          //dateDisplay.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));           //ROG
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));           //KAAL
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));              //LABH
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200)); 
                            }

                            else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));              //UDWEG
                                                                                                         //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));               //SHUBH
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190)); 
                            }

                            else if (temp >= lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));             //AMRIT
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }
                        }
                        else if (temp >= today_time[0] - 20 && temp < tonight_time[0] - 20)
                        {
                            // today - tonight
                            if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                            //CHAL
                                                                                                        //dateDisplay.setBackgroundColor(Color.WHITE); 
                            }

                            else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));            //LABH
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));            //AMRIT 
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));        //KAAL
                                                                                                       //.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));             //SHUBH
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));           //ROG
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));               //UDWEG
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                         //CHAL
                                                                                                     //dateDisplay.setBackgroundColor(Color.WHITE);
                            }
                        }
                        else
                        {
                            //tonight - tomorrow

                            if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));      //ROG
                                                                                                     //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));       //KAAL
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));       //LABH
                                                                                                      //.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));            //UDWEG
                                                                                                       //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0)); 
                            }

                            else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));        //SHUBH
                                                                                                     //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190)); 
                            }

                            else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));      //AMRIT
                                                                                                     //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255)); 
                            }

                            else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                     //CHAL
                                                                                                 //dateDisplay.setBackgroundColor(Color.WHITE); 
                            }

                            else if (temp >= tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));      //ROG
                                                                                                     //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128)); 
                            }
                        }
                    }
                    else
                    {
                        if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));      //ROG
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                        }

                        else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));       //KAAL
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                        }

                        else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));       //LABH
                                                                                                  //.setBackgroundColor(Color.argb(255,120,200,200));
                        }

                        else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));            //UDWEG
                                                                                                   //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0)); 
                        }

                        else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));        //SHUBH
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190)); 
                        }

                        else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));      //AMRIT
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255)); 
                        }

                        else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                     //CHAL
                                                                                             //dateDisplay.setBackgroundColor(Color.WHITE); 
                        }

                        else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));      //ROG
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128)); 

                        }
                    }
                    break;
                case 7:
                    if (isYesNight == false)
                    {
                        if (temp < today_time[0])
                        {
                            // lastnight 
                            temp = temp + 86400;
                            if (temp >= lastnight_time[0] - 20 && temp < lastnight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));           //ROG
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128));
                            }
                            else if (temp >= lastnight_time[1] - 20 && temp < lastnight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));           //KAAL
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }
                            else if (temp >= lastnight_time[2] - 20 && temp < lastnight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));            //LABH
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= lastnight_time[3] - 20 && temp < lastnight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));               //UDWEG
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= lastnight_time[4] - 20 && temp < lastnight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));                //SHUBH
                                                                                                             //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= lastnight_time[5] - 20 && temp < lastnight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));          //AMRIT
                                                                                                         //.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= lastnight_time[6] - 20 && temp < lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                              //CHAL
                                                                                                          //dateDisplay.setBackgroundColor(Color.WHITE);  
                            }

                            else if (temp >= lastnight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));             //ROG
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128)); 
                            }
                        }
                        else if (temp >= today_time[0] - 20 && temp < tonight_time[0] - 20)
                        {
                            // today - tonight
                            if (temp >= today_time[0] - 20 && temp < today_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));            //KAAL
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= today_time[1] - 20 && temp < today_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));            //SHUBH
                                                                                                         //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= today_time[2] - 20 && temp < today_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));            //ROG 
                                                                                                           //dateDisplay.setBackgroundColor(Color.argb(255,255,128,128)); 
                            }

                            else if (temp >= today_time[3] - 20 && temp < today_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));           //UDWEG
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= today_time[4] - 20 && temp < today_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                          //CHAL
                                                                                                      //dateDisplay.setBackgroundColor(Color.WHITE); 
                            }

                            else if (temp >= today_time[5] - 20 && temp < today_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));           //LABH
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200)); 
                            }

                            else if (temp >= today_time[6] - 20 && temp < today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));             //AMRIT
                                                                                                            //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= today_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));           //KAAL
                                                                                                          //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }
                        }
                        else
                        {
                            //tonight - tomorrow

                            if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));      //LABH
                                                                                                     //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }

                            else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));           //UDWEG
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                            }

                            else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                                                                                                      //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                            }

                            else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));         //AMRIT
                                                                                                        //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                            }

                            else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                                                                                                    //.setBackgroundColor(Color.WHITE);
                            }

                            else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));         //ROG
                                                                                                        //.setBackgroundColor(Color.argb(255,255,128,128));
                            }

                            else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));         //KAAL
                                                                                                        //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                            }

                            else if (temp >= tonight_time[7] - 20)
                            {
                                timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));         //LABH
                                                                                                        //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                            }
                        }
                    }
                    else
                    {
                        temp = temp + 86400;
                        if (temp >= tonight_time[0] - 20 && temp < tonight_time[1] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));      //LABH
                                                                                                 //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                        }

                        else if (temp >= tonight_time[1] - 20 && temp < tonight_time[2] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 0, 0));           //UDWEG
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,255,0,0));
                        }

                        else if (temp >= tonight_time[2] - 20 && temp < tonight_time[3] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 0, 165, 190));         //SHUBH
                                                                                                  //dateDisplay.setBackgroundColor(Color.argb(255,0,165,190));
                        }

                        else if (temp >= tonight_time[3] - 20 && temp < tonight_time[4] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 175, 230, 255));         //AMRIT
                                                                                                    //dateDisplay.setBackgroundColor(Color.argb(255,175,230,255));
                        }

                        else if (temp >= tonight_time[4] - 20 && temp < tonight_time[5] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.White);                        //CHAL
                                                                                                //.setBackgroundColor(Color.WHITE);
                        }

                        else if (temp >= tonight_time[5] - 20 && temp < tonight_time[6] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 128, 128));         //ROG
                                                                                                    //.setBackgroundColor(Color.argb(255,255,128,128));
                        }

                        else if (temp >= tonight_time[6] - 20 && temp < tonight_time[7] - 20)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 255, 190, 190));         //KAAL
                                                                                                    //dateDisplay.setBackgroundColor(Color.argb(255,255,190,190));
                        }

                        else if (temp >= tonight_time[7] - 20 && temp <= tomorrow_time[0] + 86400)
                        {
                            timeDisplay.SetBackgroundColor(Color.Argb(255, 120, 200, 200));         //LABH
                                                                                                    //dateDisplay.setBackgroundColor(Color.argb(255,120,200,200));
                        }
                    }
                    break;
                default:break;
            }
        }
    }
}

