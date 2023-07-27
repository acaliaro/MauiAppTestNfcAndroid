using Android.Nfc;
using AndroidX.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Java.Util;

namespace MauiAppTestNfcAndroid.Platforms.Android
{
    internal class SmartPoster : IParsedNdefRecord
    {
        static SmartPoster _smartPoster = new SmartPoster();
        private IEnumerable<IParsedNdefRecord> _uri;
        private IParsedNdefRecord _title;
        public static SmartPoster GetInstance()
        {
            return _smartPoster;
        }

        public string Str()
        {
            string s = "";
            foreach (var uri in _uri)
                s += uri.Str() + " ";

            s = s.Trim();

            s += " " + _title.Str();
            return s;
            //return _uri + " " + _title;
        }

        internal bool IsPoster(NdefRecord record)
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

        internal SmartPoster Parse(NdefRecord record)
        {

            /*
             *        
             *        Preconditions.checkArgument(record.getTnf() == NdefRecord.TNF_WELL_KNOWN);
    Preconditions.checkArgument(Arrays.equals(record.getType(), NdefRecord.RTD_SMART_POSTER));
    try {
        NdefMessage subRecords = new NdefMessage(record.getPayload());
        return parse(subRecords.getRecords());
    } catch (FormatException e) {
        throw new IllegalArgumentException(e);
    }
    */
            Preconditions.CheckArgument(record.Tnf == NdefRecord.TnfWellKnown);
            //Preconditions.CheckArgument(Arrays.Equals(record.GetTypeInfo(), NdefRecord.RtdSmartPoster));
            if (record.GetTypeInfo()[0] != NdefRecord.RtdSmartPoster[0])
                throw new Exception();
            NdefMessage subRecords = new NdefMessage(record.GetPayload());
            return Parse(subRecords.GetRecords());
        }

        private SmartPoster Parse(NdefRecord[] ndefRecord)
        {
            var records = NdefMessageParser.GetInstance().GetRecords(ndefRecord);
            var uri = records.Where(o => o is UriRecord);
            var title = records.FirstOrDefault(o => o is TextRecord);

            return new SmartPoster(uri, title);

        }

        byte[] ActionRecordType = { (byte)'a', (byte)'c', (byte)'t' };


        public SmartPoster(IEnumerable<IParsedNdefRecord> uri, IParsedNdefRecord title)
        {
            this._uri = uri;
            this._title = title;
        }

        public SmartPoster()
        {
        }

        //enum ReccomendedAction { Unknown=-1, DoAction=0, SaveForLater = 1, OpenForEditing = 2};

        //private ReccomendedAction GetRecommendedAction(NdefRecord[] records)
        //{
        //    var record = GetByType(ActionRecordType, records);
        //    if (record == null)
        //        return ReccomendedAction.Unknown;

        //    var action = record.GetPayload()[0];
        //    if(ReccomendedAction.lo)



        //}

        private NdefRecord GetByType(byte[] actionRecordType, NdefRecord[] records)
        {

            foreach (var record in records)
            {
                if (Arrays.Equals(actionRecordType, record.GetType()))
                    return record;
            }

            return null;
        }
    }
}