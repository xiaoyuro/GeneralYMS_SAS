using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;

namespace GeneralYMS_SAS.Model
{
    public static class YMSUI
    {
        public static RadBusyIndicator GetLoadingBar(this FrameworkElement control)
        {
            System.Windows.Window wm = System.Windows.Window.GetWindow(control);
            return wm.FindName("LoadingBar") as RadBusyIndicator;
        }

        public static string ToFomatString(this object str)
        {
            return "|" + str + "|";
        }

        public static bool HasValidError(this Control fe, DependencyProperty dp)
        {
            BindingExpression be = fe.GetBindingExpression(dp);
            if (be != null)
            {
                be.UpdateSource();
                return be.HasError;
            }
            return false;
        }

        public static void ShowMessage(this FrameworkElement control, string content)
        {
            var window = System.Windows.Window.GetWindow(control);
            RadWindow.Alert(new DialogParameters
            {
                Owner = window,
                Header = "提示",
                OkButtonContent = "关闭",
                Content = content,
                Theme = new Windows8Theme(),
            });
        }

        public static bool ConfirmMessage(this FrameworkElement control, string content)
        {
            var window = System.Windows.Window.GetWindow(control);

            bool flag = false;
            RadWindow.Confirm(new DialogParameters
            {
                Owner = window,
                Header = "提示",
                Content = content,
                Closed = (sender, args) =>
                {
                    if (args.DialogResult == true)
                    {
                        flag = true;
                    }
                },
                OkButtonContent = "确定",
                CancelButtonContent = "取消",
                Theme = new Windows8Theme()
            });
            return flag;
        }

        public static bool ShowWindow(this FrameworkElement control, System.Windows.Window targetpage)
        {
            var window = System.Windows.Window.GetWindow(control);
            targetpage.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            targetpage.Owner = window;

            var overlay = window.FindName("Overlay") as Grid;
            overlay.Visibility = Visibility.Visible;
            bool? result = targetpage.ShowDialog();
            overlay.Visibility = Visibility.Collapsed;
            return result.Value;
        }

        public static void NavigateToPage(this FrameworkElement sourcePage, Page targetpage)
        {
            System.Windows.Window wm = System.Windows.Window.GetWindow(sourcePage);
            var frm = wm.FindName("frmContent") as Frame;
            var sv = wm.FindName("svContent") as ScrollViewer;
            var LoadingBar = wm.FindName("LoadingBar") as RadBusyIndicator;

            LoadingBar.IsBusy = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                Thread.Sleep(new TimeSpan(0, 0, 1));
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                LoadingBar.IsBusy = false;
                frm.Content = targetpage;
                sv.ScrollToTop();
            };
            worker.RunWorkerAsync();
        }
    }
}
