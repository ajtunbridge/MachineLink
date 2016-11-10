#region Using directives

using System;
using System.ServiceModel;
using System.Windows;
using MachineLink.DesktopClient.MachineLinkService;

#endregion

namespace MachineLink.DesktopClient
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MachineLinkServiceClient _svc = new MachineLinkServiceClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ReponseText.Text = "working....";

            Progress.Visibility = Visibility.Visible;

            if (_svc.InnerChannel.State == CommunicationState.Faulted)
            {
                _svc.Abort();
                _svc = new MachineLinkServiceClient();
            }

            _svc.BeginTestMethod(TestMethodCallback, _svc);

            //_svc.BeginDownloadProgram(DownloadProgramCallback, _svc);
        }

        private void TestMethodCallback(IAsyncResult r)
        {
            try
            {
                var response = _svc.EndTestMethod(r);

                Dispatcher.Invoke((Action) delegate
                {
                    ReponseText.Text = response;
                    Progress.Visibility = Visibility.Hidden;
                });
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke((Action)delegate
                {
                    ReponseText.Text = ex.ToString();
                    Progress.Visibility = Visibility.Hidden;
                });
            }
        }

        private void DownloadProgramCallback(IAsyncResult r)
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
                Dispatcher.Invoke((Action) delegate
                {
                    ReponseText.Text = ex.ToString();
                    Progress.Visibility = Visibility.Hidden;
                });
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
        }
    }
}