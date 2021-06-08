﻿using System;
using System.Drawing;
using System.IO;

namespace EliteVA.VoiceAttackProxy.Process
{
    public class VoiceAttackActiveWindow
    {
        private readonly dynamic _proxy;

        internal VoiceAttackActiveWindow(dynamic proxy, IServiceProvider services)
        {
            _proxy = proxy;
        }

        /// <summary>
        /// The title of the window
        /// </summary>
        public string Title => _proxy.Utility.ActiveWindowTitle( );

        /// <summary>
        /// The name of the process of the window
        /// </summary>
        public string ProcessName => _proxy.Utility.ActiveWindowProcessName();

        /// <summary>
        /// The id of the process of the window
        /// </summary>
        public int ProcessId => _proxy.Utility.ActiveWindowProcessID();

        /// <summary>
        /// The path of the process of the window
        /// </summary>
        public FileInfo Path => new FileInfo(_proxy.Utility.ActiveWindowPath());

        /// <summary>
        /// The size of the window
        /// </summary>
        public Size WindowSize => new Size(_proxy.Utility.ActiveWindowWidth(), _proxy.Utility.ActiveWindowHeight());

        /// <summary>
        /// The top left coordinates of the window, relative to the screen
        /// </summary>
        /// <remarks>Top left is 0,0</remarks>
        public Point TopLeft => new Point(_proxy.Utility.ActiveWindowLeft(), _proxy.Utility.ActiveWindowTop());

        /// <summary>
        /// Gets the mouse position relative to the window
        /// </summary>
        /// <remarks>Top left is 0,0</remarks>
        public Point MouseRelative => new Point(_proxy.Utility.MousePositionWindowX(), _proxy.Utility.MousePositionWindowY());

        /// <summary>
        /// Gets the mouse position on the screen
        /// </summary>
        /// <remarks>Top left is 0,0</remarks>
        public Point MouseAbsolute => new Point(_proxy.Utility.MousePositionScreenX(), _proxy.Utility.MousePositionScreenY());
    }
}