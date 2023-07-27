using Android.Nfc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTestNfcAndroid.Platforms.Android
{
    internal class Payload : IParsedNdefRecord
    {
        private NdefRecord record;

        public Payload(NdefRecord record)
        {
            this.record = record;
        }

        public string Str()
        {
            return System.Text.Encoding.Default.GetString(this.record.GetPayload());
        }
    }
}