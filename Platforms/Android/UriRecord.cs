using Android.Nfc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTestNfcAndroid.Platforms.Android
{
    internal class UriRecord : IParsedNdefRecord
    {

        static UriRecord _uriRecord = new UriRecord();
        private readonly string _uri;

        public UriRecord(string uri)
        {
            this._uri = uri;
        }

        public UriRecord()
        {
        }

        public static UriRecord GetInstance()
        {
            return _uriRecord;
        }

        internal bool IsUri(NdefRecord record)
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

        public UriRecord Parse(NdefRecord record)
        {
            var tnf = record.Tnf;
            if (tnf == NdefRecord.TnfWellKnown)
                return ParseWellKnown(record);
            else if (tnf == NdefRecord.TnfAbsoluteUri)
                return ParseAbsolute(record);
            else
                throw new Exception("Unknown tnf");
        }

        private UriRecord ParseAbsolute(NdefRecord record)
        {
            /*
                byte[] payload = record.getPayload();
                Uri uri = Uri.parse(new String(payload, Charset.forName("UTF-8")));
                return new UriRecord(uri);
            */

            var payload = record.GetPayload();
            Encoding enc = Encoding.GetEncoding("UTF-8");
            string text = enc.GetString(payload);
            return new UriRecord(text);

        }

        private UriRecord ParseWellKnown(NdefRecord record)
        {
            //Preconditions.CheckArgument(Arrays.Equals(record.GetTypeInfo(), NdefRecord.RtdUri));

            if (record.GetTypeInfo()[0] != NdefRecord.RtdUri[0])
                throw new Exception();

            //                 Preconditions.checkArgument(Arrays.equals(record.getType(), NdefRecord.RTD_URI));
            //              byte[] payload = record.getPayload();
            /// *
            // * payload[0] contains the URI Identifier Code, per the
            // * NFC Forum "URI Record Type Definition" section 3.2.2.
            // *
            // * payload[1]...payload[payload.length - 1] contains the rest of
            // * the URI.
            // */
            //        String prefix = URI_PREFIX_MAP.get(payload[0]);
            //        byte[] fullUri =
            //            Bytes.concat(prefix.getBytes(Charset.forName("UTF-8")), Arrays.copyOfRange(payload, 1,
            //                payload.length));
            //        Uri uri = Uri.parse(new String(fullUri, Charset.forName("UTF-8")));
            //        return new UriRecord(uri);

            var payload = record.GetPayload();
            string prefix = UriMap(payload[0]);
            Encoding enc = Encoding.GetEncoding("UTF-8");
            string text = enc.GetString(payload, 1, payload.Length - 1);
            return new UriRecord(prefix + text);
        }

        static Dictionary<byte, string> MAP = new Dictionary<byte, string>()
        {
            {(byte) 0x00, "" },
            {(byte)  0x01, "http://www."},
            {(byte)  0x02, "https://www."},
            {(byte)  0x03, "http://"},
            {(byte)  0x04, "https://"},
            {(byte)  0x05, "tel:"},
            {(byte)  0x06, "mailto:"},
            {(byte)  0x07, "ftp://anonymous:anonymous@"},
            {(byte)  0x08, "ftp://ftp."},
            {(byte)  0x09, "ftps://"},
            {(byte)  0x0A, "sftp://"},
            {(byte)  0x0B, "smb://"},
            {(byte)  0x0C, "nfs://"},
            {(byte)  0x0D, "ftp://"},
            {(byte)  0x0E, "dav://"},
            {(byte)  0x0F, "news:"},
            {(byte)  0x10, "telnet://"},
            {(byte)  0x11, "imap:"},
            {(byte)  0x12, "rtsp://"},
            {(byte)  0x13, "urn:"},
            {(byte)  0x14, "pop:"},
            {(byte)  0x15, "sip:"},
            {(byte)  0x16, "sips:"},
            {(byte)  0x17, "tftp:"},
            {(byte)  0x18, "btspp://"},
            {(byte)  0x19, "btl2cap://"},
            {(byte)  0x1A, "btgoep://"},
            {(byte)  0x1B, "tcpobex://"},
            {(byte)  0x1C, "irdaobex://"},
            {(byte)  0x1D, "file://"},
            {(byte)  0x1E, "urn:epc:id:"},
            {(byte)  0x1F, "urn:epc:tag:"},
            {(byte)  0x20, "urn:epc:pat:"},
            {(byte)  0x21, "urn:epc:raw:"},
            {(byte)  0x22, "urn:epc:"},
            {(byte)  0x23, "urn:nfc:"}
        };

        public static string UriMap(byte code)
        {
            MAP.TryGetValue(code, out string valore);
            return valore;
        }

        public string Str()
        {
            return this._uri;
        }
    }
}