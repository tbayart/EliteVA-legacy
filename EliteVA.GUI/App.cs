using EliteVA.GUI.Views;
using System;
using System.Windows;
using System.Windows.Threading;

namespace EliteVA.GUI
{
    internal class App : Application
    {
        private LandingPadView _landingPads;
        private IntPtr _vaWindowHandle;

        public App()
        {
            _vaWindowHandle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _landingPads = new LandingPadView { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            _landingPads.Background = System.Windows.Media.Brushes.Transparent;
            _landingPads.Closing += LandingPadsClosingOverride;
        }

        public void ShowLandingPads(string stationName, StationType stationType, int landingPad)
        {
            Dispatcher.Invoke(() => ShowLandingPadsInternal(stationName, stationType, landingPad));
        }

        private void ShowLandingPadsInternal(string stationName, StationType stationType, int landingPad)
        {
            var vaScreen = System.Windows.Forms.Screen.FromHandle(_vaWindowHandle);
            _landingPads.Show(stationName, stationType, landingPad, vaScreen);
        }

        public void HideLandingPads()
        {
            Dispatcher.Invoke(() => _landingPads.Hide());
        }

        public new void Shutdown()
        {
            Dispatcher.Invoke(() => base.Shutdown());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _landingPads.Closing -= LandingPadsClosingOverride;
            _landingPads.Close();
            base.OnExit(e);
        }

        private void LandingPadsClosingOverride(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            HideLandingPads();
        }
    }
}
