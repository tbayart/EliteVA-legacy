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
            _landingPads.Close();
            base.OnExit(e);
        }
    }
}
