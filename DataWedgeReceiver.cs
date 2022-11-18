using System;
using Android.Content;
using scannerwerWithIntent.model;

namespace Inventory.Droid
{
    [BroadcastReceiver]
    public class DataWedgeReceiver : BroadcastReceiver
    {
        // This intent string contains the source of the data as a string
        private static string SOURCE_TAG = "com.motorolasolutions.emdk.datawedge.source";
        // This intent string contains the barcode symbology as a string
        private static string LABEL_TYPE_TAG = "com.motorolasolutions.emdk.datawedge.label_type";
        // This intent string contains the captured data as a string
        // (in the case of MSR this data string contains a concatenation of the track data)
        private static string DATA_STRING_TAG = "com.motorolasolutions.emdk.datawedge.data_string";
        // Intent Action for our operation
        public static string IntentAction = "barcodescanner.RECVR";
        public static string IntentCategory = "android.intent.category.DEFAULT";

        Helpermodel helpermodel = Helpermodel.GetInstance();

        public event EventHandler<String> scanDataReceived;

        public override void OnReceive(Context context, Intent i)
        {
            // check the intent action is for us
            if (i.Action.Equals(IntentAction))
            {
                // define a string that will hold our output
                String Out = "";
                // get the source of the data
                String source = i.GetStringExtra(SOURCE_TAG);
                // save it to use later
                if (source == null)
                    source = "scanner";
                // get the data from the intent
                String data = i.GetStringExtra(DATA_STRING_TAG);
                // let's define a variable for the data length
                int data_len = 0;
                // and set it to the length of the data
                if (data != null)
                    data_len = data.Length;
                // check if the data has come from the barcode scanner
                if (source.Equals("scanner"))
                {
                    // check if there is anything in the data
                    if (data != null && data.Length > 0)
                    {
                        // we have some data, so let's get it's symbology
                        String sLabelType = i.GetStringExtra(LABEL_TYPE_TAG);
                        // check if the string is empty
                        if (sLabelType != null && sLabelType.Length > 0)
                        {
                            // format of the label type string is LABEL-TYPE-SYMBOLOGY
                            // so let's skip the LABEL-TYPE- portion to get just the symbology
                            sLabelType = sLabelType.Substring(11);
                        }
                        else
                        {
                            // the string was empty so let's set it to "Unknown"
                            sLabelType = "Unknown";
                        }

                        // let's construct the beginning of our output string
                        Out = data.ToString() + "\r\n";
                    }
                }

                if (scanDataReceived != null)
                {
                    scanDataReceived(this, Out);
                }
            }
        }
    }
}
