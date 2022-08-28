using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EliteVA.GUI
{
    public class WindowManager : IDisposable
    {
        private readonly ILogger<WindowManager> _logger;
        private Thread _thread;
        private App _app;

        public WindowManager(ILogger<WindowManager> logger)
        {
            _logger = logger;
            _thread = new Thread(AppWorker);
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
            // Ensure App is running
            while (_app == null) Task.Delay(100).Wait();
        }

        public void LandingPadShow(string stationName, string stationTypeString, int landingPad)
        {
            if (Enum.TryParse<StationType>(stationTypeString, out var stationType) == false)
                throw new ArgumentException($"Unsupported StationType [{stationTypeString}]");

            LandingPadShow(stationName, stationType, landingPad);
        }

        public void LandingPadShow(string stationName, StationType stationType, int landingPad)
        {
            _app.ShowLandingPads(stationName, stationType, landingPad);
        }

        public void LandingPadHide()
        {
            _app.HideLandingPads();
        }

        private void AppWorker()
        {
            _logger.Log(LogLevel.Debug, "WindowManagerWorker starting");
            _app = new App();
            _app.Run();
            _logger.Log(LogLevel.Debug, "WindowManagerWorker stopped");
        }

        public void Dispose()
        {
            try
            {
                _logger.Log(LogLevel.Debug, "Disposing");
                _app.Shutdown();
                _thread.Join();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Dispose failed");
            }
        }
    }
}
