using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;

namespace GeneralYMS_SAS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            switch (btn.Tag.ToString())
            {
                case "首页":
                    FrmContent.NavigationService.Navigate(new AppHome());
                    break;

                case "后退":
                    if (FrmContent.NavigationService.CanGoBack)
                        FrmContent.NavigationService.GoBack();
                    break;

                case "前进":
                    if (FrmContent.NavigationService.CanGoForward)
                        FrmContent.NavigationService.GoForward();
                    break;

                case "刷新":
                    FrmContent.NavigationService.Refresh();
                    break;
            }
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            FrmContent.NavigationService.Refresh();
        }
    }
}
