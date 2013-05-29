using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Shapes;
using Awesomium.Core;
using Application = System.Windows.Forms.Application;
using Path = System.IO.Path;

namespace WPF_Google_Music
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Elysium.Controls.Window
    {
        private GlobalHotKey hotKey;

        public string SessionPath 
        {
            get
            {
                return String.Format("{0}{1}Cache", Path.GetDirectoryName(Application.ExecutablePath),
                                     Path.DirectorySeparatorChar, Guid.NewGuid());
            }
        }

        public MainWindow()
        {
            var webSession = WebCore.CreateWebSession(SessionPath, new WebPreferences()
                {
                    Javascript = true,
                    AllowInsecureContent = true,
                    WebAudio = true,
                    Plugins = true
                });
            InitializeComponent();
            Browser.WebSession = webSession;
            hotKey = new GlobalHotKey(this);
        }

        private void Browser_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void GlobalKeyPressed(Keys key, ModifierKeys modifier)
        {
            if (modifier != ModifierKeys.None) return;

            if (key == Keys.MediaPlayPause)
            {
                PlayPressed();
            } else if (key == Keys.MediaNextTrack)
            {
                NextPressed(); ;
            } else if (key == Keys.MediaPreviousTrack)
            {
                PrevPressed();
            }
        }

        public void PlayPressed()
        {
            Browser.ExecuteJavascript("alert('Play pressed!');");
        }

        public void NextPressed()
        {
            Browser.ExecuteJavascript("alert('Next pressed!');");
        }

        public void PrevPressed()
        {
            Browser.ExecuteJavascript("alert('Prev pressed!');");
        }
    }
}
