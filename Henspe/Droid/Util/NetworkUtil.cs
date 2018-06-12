using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Henspe.Droid.Util
{
    public class NetworkUtil
    {
        public static bool CheckIfHasNetwork(Context context)
        {
            return CheckifConnectedToWifi(context) || CheckIfConnectedToMobileData(context);
        }

        public static bool CheckifConnectedToWifi(Context context)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo wifiInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Wifi);
            return wifiInfo.IsConnected;
        }

        public static bool CheckIfConnectedToMobileData(Context context)
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)context.GetSystemService(Context.ConnectivityService);
            NetworkInfo mobileDataInfo = connectivityManager.GetNetworkInfo(ConnectivityType.Mobile);
            return mobileDataInfo.IsConnected;
        }
    }
}