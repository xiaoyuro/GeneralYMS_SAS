using GeneralYMS_SAS.App_Data;
using GeneralYMS_SAS.Model;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GeneralYMS_SAS
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        YmsDb Db;

        UserList LoginUser;

        public MainWindow()
        {
            InitializeComponent();

            Db = new YmsDb();

            LoginUser = App.Current.Resources["MyUser"] as UserList;

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            BindUserInfo();
        }

        private void btnLogOff_Click(object sender, RoutedEventArgs e)
        {
            LogOff();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            bool result = this.ConfirmMessage("请确认是否关闭程序?");
            if (result)
            {
                this.Close();
            }
        }

        private void NavBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame frm = FindName("frmContent") as Frame;
            Button btn = sender as Button;
            switch (btn.Tag.ToString())
            {
                case "首页":
                    frm.NavigationService.Navigate(new AppHome());
                    break;

                case "后退":
                    if (frm.NavigationService.CanGoBack)
                        frm.NavigationService.GoBack();
                    break;

                case "前进":
                    if (frm.NavigationService.CanGoForward)
                        frm.NavigationService.GoForward();
                    break;

                case "刷新":
                    frm.NavigationService.Refresh();
                    break;
            }
        }

        private void MetroWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            frmContent.NavigationService.Refresh();
        }

        //绑定用户信息
        private void BindUserInfo()
        {
            //tbUserName.Text = LoginUser.UserRealName;

            //tbArea.Text = GetUserArea(LoginUser.UserAreaId);
        }

        private void LogOff()
        {
            bool result = this.ConfirmMessage("请确认是否注销当前用户?");
            if (!result)
            {
                return;
            }

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                Thread.Sleep(1000);
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                LoadingBar.IsBusy = false;
                Login loginwindow = new Login();
                loginwindow.Show();
                this.Close();
            };

            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private string GetUserArea(int userAreaId)
        {
            YmsDb Db = new YmsDb();
            var Area = Db.Area.Where(p => p.AreaId == userAreaId);
            var AreaName = Area.Any() ? Area.FirstOrDefault().AreaName : "全国区域";
            return string.Format("[{0}]", AreaName);
        }
    }
}
