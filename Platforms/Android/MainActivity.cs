using Android.App;
using Android.Content.PM;
using Android.Nfc;
using Android.Content;
using Android.OS;
using System.Text;
using MauiAppTestNfcAndroid.Platforms.Android;

namespace MauiAppTestNfcAndroid;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
[IntentFilter(new[] { NfcAdapter.ActionNdefDiscovered, NfcAdapter.ActionTagDiscovered, Intent.CategoryDefault })]

public class MainActivity : MauiAppCompatActivity
{
    private NfcAdapter _nfcAdapter;

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        _nfcAdapter = NfcAdapter.GetDefaultAdapter(this);

    }

    protected override void OnResume()
    {
        base.OnResume();
        if (_nfcAdapter == null)
        {
            var alert = new AlertDialog.Builder(this).Create();
            alert.SetMessage("NFC is not supported on this device.");
            alert.SetTitle("NFC Unavailable");
            alert.Show();
        }
        else
        {
            var tagDetected = new IntentFilter(NfcAdapter.ActionTagDiscovered);
            var ndefDetected = new IntentFilter(NfcAdapter.ActionNdefDiscovered);
            var techDetected = new IntentFilter(NfcAdapter.ActionTechDiscovered);

            var filters = new[] { ndefDetected, tagDetected, techDetected };

            var intent = new Intent(this, this.GetType()).AddFlags(ActivityFlags.SingleTop);

            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);

            _nfcAdapter.EnableForegroundDispatch(this, pendingIntent, filters, null);
        }
    }

    protected override void OnPause()
    {
        base.OnPause();

        if (_nfcAdapter != null)
            _nfcAdapter.DisableForegroundDispatch(this);
    }

    protected override void OnNewIntent(Intent intent)
    {
        if (intent.Action == NfcAdapter.ActionTagDiscovered)
        {

            List<string> tags = new List<string>();

            var id = intent.GetByteArrayExtra(NfcAdapter.ExtraId);
            string reverseTag = "";

            if (id != null)
            {
                string data = "";
                for (int ii = 0; ii < id.Length; ii++)
                {
                    if (!string.IsNullOrEmpty(data))
                        data += "-";
                    string value = id[ii].ToString("X2");
                    data += value;
                    reverseTag = value + reverseTag;

                }

                System.Diagnostics.Debug.WriteLine("data = " + data);
                System.Diagnostics.Debug.WriteLine("reverseTag = " + reverseTag);

                tags.Add(data);

            }
            else
                tags.Add(null);

            var tag = intent.GetParcelableExtra(NfcAdapter.ExtraTag) as Tag;
            if (tag != null)
            {

                var rawTagMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraTag);

                // First get all the NdefMessage
                var rawMessages = intent.GetParcelableArrayExtra(NfcAdapter.ExtraNdefMessages);
                if (rawMessages != null)
                {

                    // https://medium.com/@ssaurel/create-a-nfc-reader-application-for-android-74cf24f38a6f

                    foreach (var message in rawMessages)
                    {

                        foreach (var r in NdefMessageParser.GetInstance().Parse((NdefMessage)message))
                        {
                            System.Diagnostics.Debug.WriteLine("TAG: " + r.Str());
                            tags.Add(r.Str());
                        }

                    }
                }
            }

            //MessagingCenter.Send<App, List<string>>((App)Xamarin.Forms.Application.Current, "Tag", tags);

        }
        else if (intent.Action == NfcAdapter.ActionNdefDiscovered)
        {
            System.Diagnostics.Debug.WriteLine("ActionNdefDiscovered");
        }
    }
}
