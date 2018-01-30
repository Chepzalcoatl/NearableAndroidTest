using Android.App;
using Android.Widget;
using Android.OS;
using EstimoteSdk;
using System;

namespace NearableAndroidTest
{
    [Activity(Label = "NearableAndroidTest", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity, BeaconManager.IServiceReadyCallback
    {
        int count = 1;
        BeaconManager beaconManager;
        string scanId;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SystemRequirementsChecker.CheckWithDefaultDialogs(this);
            Estimote.EnableDebugLogging(true);
            Estimote.Initialize(this, "test-nm6", "9e7bddbce8bbbfc9d6427ece93bf279d");
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            button.Click += delegate { button.Text = $"{count++} clicks!"; };

			// Create your application here
			beaconManager = new BeaconManager(this);
			beaconManager.Nearable += (sender, e) =>{
                var items = e.Nearables;
                if (items.Count > 0){
                    Toast.MakeText(this, string.Format("Found {0} nearables.", items.Count), ToastLength.Short).Show();
                }
			};

            beaconManager.Connect(this);
        }


		protected override void OnStop()
		{
			base.OnStop();
            beaconManager.StopNearableDiscovery(scanId);
		}


		protected override void OnDestroy()
		{
			base.OnDestroy();
			beaconManager.Disconnect();
		}

        public void OnServiceReady()
        {
            scanId = beaconManager.StartNearableDiscovery();
        }
    }
}

