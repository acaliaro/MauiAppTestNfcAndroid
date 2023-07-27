using Android.Nfc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTestNfcAndroid.Platforms.Android
{
    internal class NdefMessageParser
    {
        static NdefMessageParser _ndefMessageParser = new NdefMessageParser();

        public static NdefMessageParser GetInstance()
        {
            return _ndefMessageParser;
        }

        public List<IParsedNdefRecord> Parse(NdefMessage message)
        {
            return GetRecords(message.GetRecords());
        }

        public List<IParsedNdefRecord> GetRecords(NdefRecord[] records)
        {

            var elements = new List<IParsedNdefRecord>();

            foreach (var record in records)
            {
                if (UriRecord.GetInstance().IsUri(record))
                    elements.Add(UriRecord.GetInstance().Parse(record));
                else if (TextRecord.GetInstance().IsText(record))
                    elements.Add(TextRecord.GetInstance().Parse(record));
                else if (SmartPoster.GetInstance().IsPoster(record))
                    elements.Add(SmartPoster.GetInstance().Parse(record));
                else
                {
                    var rec = new Payload(record);
                    elements.Add(rec);
                }
            }

            return elements;

        }
    }
}
