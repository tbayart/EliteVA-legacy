using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EliteVA.GUI.Views
{
    /// <summary>
    /// Interaction logic for LandingPadView.xaml
    /// </summary>
    public partial class LandingPadView : Window
    {
        private const double _windowScale = 0.5D;
        private Dictionary<StationType, List<Vector>> _padCoordinates;

        public LandingPadView()
        {
            InitializeComponent();
            _padCoordinates = InitializeCoordinates();
        }

        /// <summary>
        /// Initializes and show the LandingPads window on the specified screen.
        /// </summary>
        /// <param name="stationName">The station's name to display.</param>
        /// <param name="stationType">The station's type.</param>
        /// <param name="padNumber">The pad number.</param>
        /// <param name="screen">The screen where the window must be displayed on.</param>
        public void Show(string stationName, StationType stationType, int padNumber, System.Windows.Forms.Screen screen)
        {
            if (stationType == StationType.Ocellus || stationType == StationType.Orbis || stationType == StationType.AsteroidBase)
                stationType = StationType.Coriolis;
            else if (stationType == StationType.MegaShip)
                stationType = StationType.FleetCarrier;

            var coords = _padCoordinates[stationType][padNumber - 1];
            var background = LoadImage(stationType);

            Title = stationName;
            LandingPadsImage.Source = background;
            // calculate window size
            var screenRatio = (double)screen.WorkingArea.Width / screen.WorkingArea.Height;
            var imageRatio = background.Width / background.Height;
            if (imageRatio >= screenRatio)
            {
                Width = screen.WorkingArea.Width * _windowScale;
                Height = Width / imageRatio;
            }
            else
            {
                Height = screen.WorkingArea.Height * _windowScale;
                Width = Height * imageRatio;
            }
            // resize icon indicating target pad
            coords.X *= Width;
            coords.Y *= Height;
            LandingPadIcon.Width = Width * 0.08;
            LandingPadIcon.Height = Height * 0.08;
            // place icon on target pad
            Canvas.SetLeft(LandingPadIcon, coords.X - LandingPadIcon.Width * 0.5);
            Canvas.SetTop(LandingPadIcon, coords.Y - LandingPadIcon.Height * 0.5);
            // show window on screen before setting its position
            Show();
            Left = (screen.WorkingArea.Width - Width) * 0.5D;
            Top = (screen.WorkingArea.Height - Height) * 0.5D;
        }

        /// <summary>
        /// Load LandingPads Coordinates from resource into cache dictionary.
        /// </summary>
        /// <returns>The dictionary with cached coordinates.</returns>
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

        /// <summary>
        /// Load the image for the provided <see cref="StationType"/>.
        /// </summary>
        /// <param name="stationType">The station's type.</param>
        /// <returns>A <see cref="ImageSource"/> instance.</returns>
        private ImageSource LoadImage(StationType stationType)
        {
            var uri = new Uri($"pack://application:,,,/EliteVA.GUI;component/Medias/LandingPads_{stationType}.png");
            return new System.Windows.Media.Imaging.BitmapImage(uri);
        }

        /// <summary>
        /// Enumerate LandingPads coordinates from resource.
        /// </summary>
        /// <returns>Yield return data line per line.</returns>
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
