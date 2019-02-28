using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Support.V4.App;

namespace Henspe.Droid
{
    public class Adresses
    {
        static Adress[] builtInAdresses = {
            new Adress { problem = "42 " },
            new Adress { problem = "9 "}
        };

        // Array of flash cards that make up the flash card deck:
        private Adress[] adresses;

        // Create an instance copy using the built-in flash cards:
        public Adresses() { adresses = builtInAdresses; }

        // Indexer (read only) for accessing a flash card:
        public Adress this[int i] { get { return adresses[i]; } }

        // Returns the number of flash cards in the deck:
        public int NumAdresses { get { return adresses.Length; } }

    }

    public class Adress
    {
        // Math problem for this flash card:
        public string problem;
    }

    public class FirstFragment : Android.Support.V4.App.Fragment
    {
        string text;
        public global::Android.Views.View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            global::Android.Views.View v = inflater.Inflate(Resource.Layout.HenspeRow, container, false);

            TextView tv = (TextView)v.FindViewById(Resource.Id.description);
            tv.Text = "Adresse";

            ImageView image = (ImageView)v.FindViewById(Resource.Id.image);

            image.SetImageResource(Resource.Drawable.ic_adresse);

            return v;
        }

        public static FirstFragment newInstance(String text)
        {
            FirstFragment f = new FirstFragment();
            Bundle b = new Bundle();
            b.PutString("msg", text);
            f.Arguments = (b);
            return f;
        }
    }

    class AdressAdapter : FragmentPagerAdapter
    {
        // Underlying model data (flash card deck):
        public Adresses adresses;

        // Constructor accepts a deck of flash cards:
        public AdressAdapter(Android.Support.V4.App.FragmentManager fm, Adresses adresses)
            : base(fm)
        {
            this.adresses = adresses;
        }

        // Returns the number of cards in the deck:
        public override int Count { get { return adresses.NumAdresses; } }

        // Returns a new fragment for the flash card at this position:
        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return FirstFragment.newInstance("FirstFragment, Instance 1");

            //  return null;// (Android.Support.V4.App.Fragment)  AdressFragment.newInstance(adresses[position].Adress);
        }

        // Display the problem number in the PagerTitleStrip:
        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            if (position == 1)
                return new Java.Lang.String("Din posisjon");
            else
                return new Java.Lang.String("Angitt posisjon");
        }
    }
}