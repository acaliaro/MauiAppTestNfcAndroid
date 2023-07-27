using Android.Nfc;
using AndroidX.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTestNfcAndroid.Platforms.Android
{
    internal class TextRecord : IParsedNdefRecord
    {
        static TextRecord _textRecord = new TextRecord();
        private readonly string _text;

        public TextRecord(string text)
        {
            this._text = text;
        }

        public TextRecord()
        {
        }

        public static TextRecord GetInstance()
        {
            return _textRecord;
        }

        internal bool IsText(NdefRecord record)
        {

            try
            {
                Parse(record);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TextRecord Parse(NdefRecord record)
        {
            Preconditions.CheckArgument(record.Tnf == NdefRecord.TnfWellKnown);
            var typeInfo = record.GetTypeInfo();
            if (record.GetTypeInfo()[0] != NdefRecord.RtdText[0])
                throw new Exception();
            //Preconditions.CheckArgument(Arrays.Equals(record.GetTypeInfo()[0], NdefRecord.RtdText[0]));
            // Preconditions.CheckArgument(Array.Equals(record.GetType(), NdefRecord.RtdText));

            var payload = record.GetPayload();
            string textEncoding = (payload[0] & 0200) == 0 ? "UTF-8" : "UTF-16";
            int languageCodeLength = payload[0] & 0077;
            Encoding enc = Encoding.GetEncoding("US-ASCII");
            Encoding encText = Encoding.GetEncoding(textEncoding);
            string languageCode = enc.GetString(payload, 0, languageCodeLength);
            string text = encText.GetString(payload, languageCodeLength + 1, payload.Length - languageCodeLength - 1);
            return new TextRecord(text);
            /*
            String textEncoding = ((payload[0] & 0200) == 0) ? "UTF-8" : "UTF-16";
            int languageCodeLength = payload[0] & 0077;
            String languageCode = new String(payload, 1, languageCodeLength, "US-ASCII");
            String text =
                new String(payload, languageCodeLength + 1,
                    payload.length - languageCodeLength - 1, textEncoding);
            return new TextRecord(languageCode, text);
            */

        }

        public string Str()
        {
            return _text;
        }
    }

}