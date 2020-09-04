using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace task_scheduler.libs
{
    public partial class TaskbarIcon : FrameworkElement, IDisposable
    {
        private WindowProcedureHandler messageHandler;

        internal string WindowId { get; private set; }

        internal IntPtr MessageWindowHandle { get; private set; }

        private uint taskbarRestartMessageId;

        public static readonly object SyncRoot = new object();
        
        public TaskbarIcon()
        {
            var iconData = NotifyIconData.CreateDefault(MessageWindowHandle);

            lock (SyncRoot)
            {
                var _res = WinApi.Shell_NotifyIcon(NotifyCommand.Modify, ref iconData);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public WindowClass CreateMessageWindow()
        {
            WindowClass wc;

            wc.style = 0;
            wc.lpfnWndProc = messageHandler;
            wc.cbClsExtra = 0;
            wc.cbWndExtra = 0;
            wc.hInstance = IntPtr.Zero;
            wc.hIcon = IntPtr.Zero;
            wc.hCursor = IntPtr.Zero;
            wc.hbrBackground = IntPtr.Zero;
            wc.lpszMenuName = string.Empty;
            wc.lpszClassName = WindowId;

            // Register the window class
            WinApi.RegisterClass(ref wc);

            // Get the message used to indicate the taskbar has been restarted
            // This is used to re-add icons when the taskbar restarts
            taskbarRestartMessageId = WinApi.RegisterWindowMessage("TaskbarCreated");

            // Create the message window
            MessageWindowHandle = WinApi.CreateWindowEx(0, WindowId, "", 0, 0, 0, 1, 1, IntPtr.Zero, IntPtr.Zero,
                IntPtr.Zero, IntPtr.Zero);

            if (MessageWindowHandle == IntPtr.Zero)
            {
                throw new Win32Exception("Message window handle was not a valid pointer");
            }

            return wc;
        }
    }
}
