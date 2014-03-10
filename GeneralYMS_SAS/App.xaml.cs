using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;

namespace GeneralYMS_SAS
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        public static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

        //只允许打开单个程序
        protected override void OnStartup(StartupEventArgs e)
        {
            Process self = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(self.ProcessName).Length > 1)
            {
                Application.Current.Shutdown();
                SwitchToThisWindow(Process.GetProcessesByName(self.ProcessName)[0].MainWindowHandle, true);
                return;
            }
            else
            {
                base.OnStartup(e);
            }
        }
    }
}
