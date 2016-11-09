using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MachineLink.DesktopClient.MachineLinkServiceReference;

namespace MachineLink.DesktopClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MachineLinkServiceClient _svc = new MachineLinkServiceClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Progress.Visibility = Visibility.Visible;

            if (_svc.InnerChannel.State == CommunicationState.Faulted)
            {
                _svc.Abort();
                _svc = new MachineLinkServiceClient();
            }

            _svc.BeginDownloadProgram(Callback, null);
        }

        private void Callback(IAsyncResult r)
        {
            try
            {
                var response = _svc.EndDownloadProgram(r);

                Dispatcher.Invoke((Action) delegate
                {
                    ReponseText.Text = response.Program;
                    Progress.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke((Action)delegate
                {
                    ReponseText.Text = "AN ERROR OCCURRED!";
                    Progress.Visibility = Visibility.Hidden;
                });
            }
        }
    }
}
