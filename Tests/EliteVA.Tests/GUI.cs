using EliteVA.GUI;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EliteVA.Tests
{
    public class GUI
    {
        [Fact]
        public async Task ShowHideWindow()
        {
            var logger = Mock.Of<ILogger<WindowManager>>();
            var wm = new WindowManager(logger);
            var coords = new LandingPadsCoords();

            Console.WriteLine("Window should be shown while landing pads are iterated");
            foreach (var stationType in coords.StationTypes)
            {
                var padCount = coords.GetPadCount(stationType);
                for (int padNumber = 1; padNumber <= padCount; padNumber++)
                {
                    wm.LandingPadShow($"Test {stationType}", stationType, padNumber);
                    await Task.Delay(500);
                }
            }
            wm.LandingPadHide();
            Console.WriteLine("Window should now be hidden");
            await Task.Delay(1000);
        }
    }
}
