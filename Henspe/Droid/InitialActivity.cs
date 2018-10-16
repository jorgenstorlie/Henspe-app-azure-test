using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Content.PM;
using Henspe.Droid.Utils;

namespace Henspe.Droid
{
    //Android Launch Screens samples
    //https://android.jlelse.eu/the-complete-android-splash-screen-guide-c7db82bce565
    //https://montemagno.com/beautiful-android-splash-screens/

    [Activity(Label = "Henspe",
        Icon = "@mipmap/ic_launcher",
           RoundIcon = "@mipmap/ic_launcher_round",
        Theme = "@style/SplashTheme",
        ScreenOrientation = ScreenOrientation.Portrait,
        MainLauncher = true)]
    public class InitialActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Intent intent = null;
           
         
            if (UserUtil.settings.instructionsFinished)
                intent = new Intent(this, typeof(MainNew));
            else
           intent = new Intent(this, typeof(OnBoardingActivity));

            intent.AddFlags(ActivityFlags.ClearTop);
            intent.AddFlags(ActivityFlags.SingleTop);
            StartActivity(intent);
            Finish();
        }
    }
}