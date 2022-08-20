using System.Windows;
using System.Windows.Threading;

namespace EliteVA.GUI
{
    internal class App : Application
    {
        private Window _landingPads;

        public App()
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _landingPads = new LandingPadMap { WindowStartupLocation = WindowStartupLocation.CenterScreen };
            _landingPads.Closing += LandingPadsClosingOverride;
        }

        public void ShowLandingPads()
        {
            Dispatcher.InvokeAsync(() => _landingPads.Show());
        }

        public void HideLandingPads()
        {
            Dispatcher.InvokeAsync(() => _landingPads.Hide());
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
