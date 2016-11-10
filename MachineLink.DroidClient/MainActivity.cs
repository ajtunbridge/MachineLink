using System;
using System.ServiceModel;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace MachineLink.DroidClient
{
    [Activity(Label = "CPE - MachineLink", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static readonly EndpointAddress EndPoint = new EndpointAddress("http://192.168.0.2:20101/MachineLink/MachineLink");

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.DownloadButton);

            button.Click += DownloadButton_Click;
        }

        private static BasicHttpBinding CreateBasicHttp()
        {
            BasicHttpBinding binding = new BasicHttpBinding
            {
                Name = "BasicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 0, 10);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            var client = new MachineLinkServiceClient(CreateBasicHttp(), EndPoint);

            var textView = FindViewById<TextView>(Resource.Id.ResponseText);

            textView.Text = "working...";

            client.TestMethodCompleted += (o, args) =>
            {
                if (args.Error != null)
                {
                    RunOnUiThread(() =>
                        textView.Text = args.Error.Message);
                }
                else
                {
                    RunOnUiThread(() => textView.Text = args.Result);
                }
            };

            client.TestMethodAsync();
        }
    }
}

