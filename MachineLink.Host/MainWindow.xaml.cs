using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MachineLink.Host
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceHost _host;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (sender, args) => StartButton_OnClick(this, null);
        }
        
        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _host = new ServiceHost(typeof(Service.MachineLinkService), new Uri("http://192.168.0.2:20101/MachineLink"));
            
            // Service host is opened on the UI thread
            _host.AddServiceEndpoint(typeof(Service.IMachineLinkService), new BasicHttpBinding(), "MachineLink");
            
            // Enable metadata exchange
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior() { HttpGetEnabled = true };
            
            _host.Description.Behaviors.Add(smb);

            // Enable exeption details
            ServiceDebugBehavior sdb = _host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.IncludeExceptionDetailInFaults = true;

            _host.Opened += (s, args) => StatusText.Text = "Service is running";
            _host.Closed += (s, args) => StatusText.Text = "Service is stopped";

            _host.Open();

            // Keep track of the UI thread id
            this.Title = Thread.CurrentThread.ManagedThreadId.ToString();
        }

        private void StopButton_OnClick(object sender, RoutedEventArgs e)
        {
            _host.Close();
        }
    }
}
