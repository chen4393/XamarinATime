using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Text;
using Android.Text.Method;

namespace XamarinATime
{
    public class AboutFragment : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View v = inflater.Inflate(Resource.Layout.About, container, false);
            //Dialog.SetContentView(Resource.Id.About);
            Dialog.SetTitle("About Atime");
            TextView detail_1 = v.FindViewById<TextView>(Resource.Id.text_1);
            TextView detail_2 = v.FindViewById<TextView>(Resource.Id.text_2);
            TextView detail_3 = v.FindViewById<TextView>(Resource.Id.text_3);
            TextView detail_4 = v.FindViewById<TextView>(Resource.Id.text_4);
            TextView detail_5 = v.FindViewById<TextView>(Resource.Id.text_5);
            TextView detail_6 = v.FindViewById<TextView>(Resource.Id.text_6);
            TextView detail_7 = v.FindViewById<TextView>(Resource.Id.text_7);
            TextView detail_8 = v.FindViewById<TextView>(Resource.Id.text_8);
            TextView detail_9 = v.FindViewById<TextView>(Resource.Id.text_9);
            TextView detail_10 = v.FindViewById<TextView>(Resource.Id.text_10);
            TextView detail_11 = v.FindViewById<TextView>(Resource.Id.text_11);
            TextView detail_12 = v.FindViewById<TextView>(Resource.Id.text_12);
            TextView detail_13 = v.FindViewById<TextView>(Resource.Id.text_13);
            TextView detail_14 = v.FindViewById<TextView>(Resource.Id.text_14);

            detail_1.Text = "Ace Auspicious Time";
            detail_2.Text = "Version 2.0";
            detail_3.Text = "Copyright \u00A9 2017";
            detail_4.Text = Html.FromHtml("<a href=\"http://pyrahealth.com/component/content/article/35-pyrahealth-general-topic/153-ace-auspicious-time\">Pyrahealth.com</a> ").ToString();
            detail_4.MovementMethod = LinkMovementMethod.Instance;
            detail_5.Text = ("Ace Auspicious Time calculates beneficial times for activities at your location. Using Indian Vedic astrology, the day and night's eight Choghadiya Muhurtas are calculated.");
            detail_6.Text = ("The Choghadiya time intervals have a nature of being good, neutral, or bad for starting an activity. Your location and time zone are used to calculate your sunrise and sunset.");
            detail_7.Text = ("There are seven types of Choghadiyas:");
            detail_8.Text = ("\u2022 AMRIT: nectar. [Moon, good]");
            detail_9.Text = ("\u2022 CHAL: neutral, okay.[Venus, neutral]");
            detail_10.Text = ("\u2022 KAAL: to go after (with hostile intention), persecute [Saturn, bad]");
            detail_11.Text = ("\u2022 LABH: gain. [Mercury, good]");
            detail_12.Text = ("\u2022 ROG: disease [Mars, bad]");
            detail_13.Text = ("\u2022 SHUBH: good [Jupiter, good]");
            detail_14.Text = ("\u2022 UDWEG: regret, fear, distress (separation from a beloved object). [Sun, bad]");

            Button dialogButton = v.FindViewById<Button>(Resource.Id.button_2);
            dialogButton.Click += delegate
            {
                Dialog.Dismiss();
            };
            //Dialog.Show();
            return v;
        }
    }
}