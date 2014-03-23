using GeneralYMS_SAS.App_Data;
using GeneralYMS_SAS.Model;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Web.Security;
using System.Windows;

namespace GeneralYMS_SAS
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : MetroWindow
    {
        YmsDb _db;

        public Login()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            _db = new YmsDb();

            Title = "社区公共服务综合信息平台——统计分析系统" + AppVersion.Version;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserLogin();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UserLogin()
        {
            var username = TbUserName.Text;

            var password = FormsAuthentication.HashPasswordForStoringInConfigFile(TbPassWord.Password, "MD5");

            IQueryable<UserList> users = null;

            var worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                users = _db.UserList.Where(p => p.UserName == username && p.UserPass == password);
                Thread.Sleep(2000);
            };

            worker.RunWorkerCompleted += (o, a) =>
            {
                CheckUser(users);

                BtnLogin.IsEnabled = true;
                LoadingBar.IsBusy = false;
            };

            BtnLogin.IsEnabled = false;
            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private void CheckUser(IQueryable<UserList> users)
        {
            try
            {
                if (users.Any())
                {
                    //插入登录时间
                    var loginUser = users.FirstOrDefault();

                    var workGroup = _db.UserRole.FirstOrDefault(x => x.UserRoleId == loginUser.UserRoleId).WorkGroupId;
                    if (!workGroup.HasPower("管理员"))
                    {
                        this.ShowMessage("无法登录！您没有管理员权限！");
                        return;
                    }

                    loginUser.LastLoginTime = DateTime.Now;
                    _db.SaveChanges();

                    //保存Cookie
                    SetSession(loginUser);

                    var mainWindow = new MainWindow();
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    this.ShowMessage("用户名或密码错误!");
                }
            }
            catch
            {
                CheckNetwork("127.0.0.1");
            }
        }

        private void SetSession(UserList loginUser)
        {
            //1.用户数据
            Application.Current.Resources["MyUser"] = loginUser;
        }

        private void CheckNetwork(string host)
        {
            try
            {
                var ping = new Ping();
                var reply = ping.Send(host);
                if (reply.Status == IPStatus.Success)
                {
                    this.ShowMessage("无法连接到远程数据库或数据库异常，请重新尝试或联系管理员!");
                }
            }

            catch
            {
                this.ShowMessage("网络故障，请重新尝试或联系管理员!");
            }
        }
    }
}
