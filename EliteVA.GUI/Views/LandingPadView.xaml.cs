using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace EliteVA.GUI.Views
{
    /// <summary>
    /// Interaction logic for LandingPadView.xaml
    /// </summary>
    public partial class LandingPadView : Window
    {
        private Dictionary<StationType, List<Vector>> _padCoordinates;

        public LandingPadView()
        {
            InitializeComponent();
            _padCoordinates = InitializeCoordinates();
            SetLandingPad(StationType.Coriolis, 1);
        }

        public void SetLandingPad(StationType stationType, int padNumber)
        {
            if (stationType == StationType.Ocellus || stationType == StationType.Orbis || stationType == StationType.AsteroidBase)
                stationType = StationType.Coriolis;

            var uri = new Uri($"pack://application:,,,/EliteVA.GUI;component/Medias/LandingPads_{stationType}.png");
            LandingPadsImage.Source = new BitmapImage(uri);
            var coords = _padCoordinates[stationType][padNumber - 1];
            coords.X *= LandingPadsImage.ActualWidth;
            coords.Y *= LandingPadsImage.ActualHeight;
            LandingPadIcon.Width = LandingPadsImage.ActualWidth * 0.08;
            LandingPadIcon.Height = LandingPadsImage.ActualHeight * 0.08;
            Canvas.SetLeft(LandingPadIcon, coords.X - LandingPadIcon.ActualWidth * 0.5);
            Canvas.SetTop(LandingPadIcon, coords.Y - LandingPadIcon.ActualHeight * 0.5);
        }

        private Dictionary<StationType, List<Vector>> InitializeCoordinates()
        {
            var padCoordinates = new Dictionary<StationType, List<Vector>>();
            string header = null;
            foreach (var line in LandingPadsCoords())
            {
                if (header == null)
                {
                    header = line;
                    continue;
                }
                var data = line.Split(',');
                var stationType = (StationType)Enum.Parse(typeof(StationType), data[0]);
                var relX = double.Parse(data[4], System.Globalization.CultureInfo.InvariantCulture);
                var relY = double.Parse(data[5], System.Globalization.CultureInfo.InvariantCulture);
                var coord = new Vector(relX, relY);
                if (padCoordinates.ContainsKey(stationType) == false)
                    padCoordinates[stationType] = new List<Vector>();
                padCoordinates[stationType].Add(coord);
            }
            return padCoordinates;
        }

        private IEnumerable<string> LandingPadsCoords()
        {
            using (var stream = GetType().Assembly.GetManifestResourceStream("EliteVA.GUI.Medias.LandingPadsCoords.csv"))
            using (var reader = new StreamReader(stream))
            {
                while (reader.EndOfStream == false)
                    yield return reader.ReadLine();
            }
        }
    }
}
