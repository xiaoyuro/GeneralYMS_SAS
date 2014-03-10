using GeneralYMS_SAS.App_Data;
using GeneralYMS_SAS.Model;
using MahApps.Metro.Controls;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;

namespace GeneralYMS_SAS
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : MetroWindow
    {
        YmsDb Db;

        public Login()
        {
            InitializeComponent();
            Loaded += Login_Loaded;
        }

        void Login_Loaded(object sender, RoutedEventArgs e)
        {
            Db = new YmsDb();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserLogin();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UserLogin()
        {
            string username = tbUserName.Text;

            string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(tbPassWord.Password, "MD5");

            IQueryable<UserList> Users = null;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                Users = Db.UserList.Where(p => p.UserName == username && p.UserPass == password);
                Thread.Sleep(2000);
            };

            worker.RunWorkerCompleted += (o, a) =>
            {
                CheckUser(Users);

                btnLogin.IsEnabled = true;
                LoadingBar.IsBusy = false;
            };

            btnLogin.IsEnabled = false;
            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private void CheckUser(IQueryable<UserList> Users)
        {
            try
            {
                if (Users.Any())
                {
                    //插入登录时间
                    var LoginUser = Users.FirstOrDefault();

                    var WorkGroup = Db.UserRole.Where(x => x.UserRoleId == LoginUser.UserRoleId).FirstOrDefault().WorkGroupId;
                    if (!WorkGroup.HasPower("管理员"))
                    {
                        this.ShowMessage("无法登录！您没有管理员权限！");
                        return;
                    }

                    LoginUser.LastLoginTime = DateTime.Now;
                    Db.SaveChanges();

                    //保存Cookie
                    SetSession(LoginUser);

                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
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
            App.Current.Resources["MyUser"] = loginUser;
        }

        private void CheckNetwork(string host)
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send(host);
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
