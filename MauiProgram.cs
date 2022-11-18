using Android.Content;
using Android.OS;
using Inventory.Droid;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.LifecycleEvents;
using scannerwerWithIntent.model;

namespace scannerwerWithIntent;

public static class MauiProgram
{
    private static string ACTION_DATAWEDGE_FROM_6_2 = "com.symbol.datawedge.api.ACTION";
    private static string EXTRA_CREATE_PROFILE = "com.symbol.datawedge.api.CREATE_PROFILE";
    private static string EXTRA_SET_CONFIG = "com.symbol.datawedge.api.SET_CONFIG";
    private static string EXTRA_PROFILE_NAME = "Inventory DEMO";
    public static DataWedgeReceiver _broadcastReceiver = null;
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
                            .ConfigureLifecycleEvents(events =>
                            {
#if ANDROID
                                events.AddAndroid(android => android
                                    .OnCreate((activity, bundle) => OnCreate())
                                    .OnPause((activity) => OnPause())
                                    .OnResume((activity) => OnResume()));
#endif
                              
                                void OnCreate()
                                {
                                    _broadcastReceiver = new DataWedgeReceiver();
                                    Helpermodel helpermodel = Helpermodel.GetInstance();


                                    _broadcastReceiver.scanDataReceived += (s, scanData) =>
                                    {
                                        helpermodel.ScannedText = scanData;
                                    };
                                    CreateProfile();
                                }
                                void OnResume()
                                {
                                    if (null != _broadcastReceiver)
                                    {
                                        // Register the broadcast receiver
                                        IntentFilter filter = new IntentFilter(DataWedgeReceiver.IntentAction);
                                        filter.AddCategory(DataWedgeReceiver.IntentCategory);
                                        Android.App.Application.Context.RegisterReceiver(_broadcastReceiver, filter);
                                    }
                                }
                                void OnPause()
                                {
                                    if (null != _broadcastReceiver)
                                    {
                                        // Unregister the broadcast receiver
                                        Android.App.Application.Context.UnregisterReceiver(_broadcastReceiver);
                                    }
                                }
                                void CreateProfile()
                                {
                                    String profileName = EXTRA_PROFILE_NAME;
                                    SendDataWedgeIntentWithExtra1(ACTION_DATAWEDGE_FROM_6_2, EXTRA_CREATE_PROFILE, profileName);

                                    //  Now configure that created profile to apply to our application
                                    Bundle profileConfig = new Bundle();
                                    profileConfig.PutString("PROFILE_NAME", EXTRA_PROFILE_NAME);
                                    profileConfig.PutString("PROFILE_ENABLED", "true"); //  Seems these are all strings
                                    profileConfig.PutString("CONFIG_MODE", "UPDATE");
                                    Bundle barcodeConfig = new Bundle();
                                    barcodeConfig.PutString("PLUGIN_NAME", "BARCODE");
                                    barcodeConfig.PutString("RESET_CONFIG", "true"); //  This is the default but never hurts to specify
                                    Bundle barcodeProps = new Bundle();
                                    barcodeConfig.PutBundle("PARAM_LIST", barcodeProps);
                                    profileConfig.PutBundle("PLUGIN_CONFIG", barcodeConfig);
                                    Bundle appConfig = new Bundle();
                                    appConfig.PutString("PACKAGE_NAME", Android.App.Application.Context.PackageName);      //  Associate the profile with this app
                                    appConfig.PutStringArray("ACTIVITY_LIST", new String[] { "*" });
                                    profileConfig.PutParcelableArray("APP_LIST", new Bundle[] { appConfig });
                                    SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
                                    //  You can only configure one plugin at a time, we have done the barcode input, now do the intent output
                                    profileConfig.Remove("PLUGIN_CONFIG");
                                    Bundle intentConfig = new Bundle();
                                    intentConfig.PutString("PLUGIN_NAME", "INTENT");
                                    intentConfig.PutString("RESET_CONFIG", "true");
                                    Bundle intentProps = new Bundle();
                                    intentProps.PutString("intent_output_enabled", "true");
                                    intentProps.PutString("intent_action", DataWedgeReceiver.IntentAction);
                                    intentProps.PutString("intent_delivery", "2");
                                    intentConfig.PutBundle("PARAM_LIST", intentProps);
                                    profileConfig.PutBundle("PLUGIN_CONFIG", intentConfig);
                                    SendDataWedgeIntentWithExtra(ACTION_DATAWEDGE_FROM_6_2, EXTRA_SET_CONFIG, profileConfig);
                                }
                                void SendDataWedgeIntentWithExtra(String action, String extraKey, Bundle extras)
                                {
                                    Intent dwIntent = new Intent();
                                    dwIntent.SetAction(action);
                                    dwIntent.PutExtra(extraKey, extras);
                                    Android.App.Application.Context.SendBroadcast(dwIntent);
                                }
                                void SendDataWedgeIntentWithExtra1(String action, String extraKey, String extraValue)
                                {
                                    Intent dwIntent = new Intent();
                                    dwIntent.SetAction(action);
                                    dwIntent.PutExtra(extraKey, extraValue);
                                    Android.App.Application.Context.SendBroadcast(dwIntent);
                                }
                            });

        return builder.Build();
    }
}
