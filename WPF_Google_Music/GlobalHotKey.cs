using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using Elysium.Controls;

namespace WPF_Google_Music
{
    internal class GlobalHotKey : IDisposable
    {
        public const int VKC_PLAY_PAUSE = 0xB3;
        public const int VKC_NEXT_TRACK = 0xB0;
        public const int VKC_PREV_TRACK = 0xB1;
        public const int WM_HOTKEY = 0x0312;

        private readonly Window _mainWindow;
        readonly WindowInteropHelper _host;

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public GlobalHotKey(Window mainWindow)
        {
            _mainWindow = mainWindow;
            _host = new WindowInteropHelper(_mainWindow);

            SetupHotKey(_host.Handle);
            ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        private void ComponentDispatcher_ThreadPreprocessMessage(ref MSG msg, ref bool handled)
        {
            var instanceOfApp = _mainWindow.GetType() == typeof (MainWindow);
            var mainWindow = _mainWindow as MainWindow;

            if (mainWindow == null || !instanceOfApp) return;

            if (msg.message == WM_HOTKEY)
            {
                // get the keys.
                Keys key = (Keys)(((int)msg.lParam >> 16) & 0xFFFF);
                ModifierKeys modifier = (ModifierKeys)((int)msg.lParam & 0xFFFF);
                // TODO Handle the hotkey press
                mainWindow.GlobalKeyPressed(key, modifier);
            }
        }

        private void SetupHotKey(IntPtr handle)
        {
            RegisterHotKey(handle, GetType().GetHashCode(), 0, VKC_PLAY_PAUSE);
            RegisterHotKey(handle, GetType().GetHashCode(), 0, VKC_NEXT_TRACK);
            RegisterHotKey(handle, GetType().GetHashCode(), 0, VKC_PREV_TRACK);
        }

        public void Dispose()
        {
            UnregisterHotKey(_host.Handle, GetType().GetHashCode());
        }
    }
}
