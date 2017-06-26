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
    public class FindMyLocation : DialogFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View v = inflater.Inflate(Resource.Layout.FindMyLocation, container, false);
            Dialog.SetTitle("Location Help");
            TextView detail_1 = (TextView)v.FindViewById(Resource.Id.text_first);
            TextView detail_2 = (TextView)v.FindViewById(Resource.Id.text_second);
            TextView detail_3 = (TextView)v.FindViewById(Resource.Id.text_detail3);
            TextView detail_4 = (TextView)v.FindViewById(Resource.Id.text_detail4);
            TextView detail_5 = (TextView)v.FindViewById(Resource.Id.text_detail5);

            detail_1.Text = ("Latitude, longitude, and your time zone are essential for find your exact auspicious time. " + "Click the \"Current Location\" button to use your phone's current location (this is more precise if GPS is enabled).Or you might want to use a location where you will be at a certain time."
                + "\n" + "You may use these helpful sites to find your location information:");

            detail_2.TextFormatted = Html.FromHtml(("<a href=\"http://www.zipinfo.com/search/zipcode.htm\">Find latitude and longitude based on your zip code</a> "));
            detail_2.MovementMethod = LinkMovementMethod.Instance;

            detail_3.TextFormatted = Html.FromHtml("<a href=\"http://www.mapsofworld.com/lat_long\">Find latitude and longitude based on your city</a> ");
            detail_3.MovementMethod = LinkMovementMethod.Instance;

            detail_4.TextFormatted = Html.FromHtml("<a href=\"http://itouchmap.com/latlong.html\">Find latitude and longitude using Google Maps</a> ");
            detail_4.MovementMethod = LinkMovementMethod.Instance;

            detail_5.TextFormatted = Html.FromHtml("<a href=\"http://www.timeanddate.com/worldclock\">Find your time zone</a> ");
            detail_5.MovementMethod = LinkMovementMethod.Instance;

            Button dialogButton = v.FindViewById<Button>(Resource.Id.button_1);
            dialogButton.Click += delegate
            {
                Dialog.Dismiss();
            };

            Dialog.Show();
            return v;
        }
    }
}