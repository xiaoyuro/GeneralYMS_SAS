using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GeneralYMS_SAS.Model;
using System.ComponentModel;
using System.Threading;

namespace GeneralYMS_SAS
{
    /// <summary>
    /// AppHome.xaml 的交互逻辑
    /// </summary>
    public partial class AppHome : Page
    {
        int PageNum;

        int PageCount;

        int NowPage;

        List<Button> btnList;

        public AppHome()
        {
            InitializeComponent();
            Loaded += AppHome_Loaded;
        }

        private void AppHome_Loaded(object sender, RoutedEventArgs e)
        {
            BindPage(NowPage);
        }

        private void BindPage(int nowPage)
        {
            btnList = new List<Button>()
            {
                CreateButton("dd1","/Image/Home/icon_03.png"),
                CreateButton("dd2","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
                CreateButton("ddd3","/Image/Home/icon_03.png"),
            };

            PageNum = GetPageNum();

            PageCount = (int)Math.Ceiling((double)btnList.Count / PageNum);

            var LoadingBar = this.GetLoadingBar();

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                Thread.Sleep(500);
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                wpHome.Children.Clear();
                btnList.Skip(nowPage * PageNum).Take(PageNum).ToList().ForEach(x => wpHome.Children.Add(x));

                LoadingBar.IsBusy = false;
            };

            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private int GetPageNum()
        {
            var svContent = Window.GetWindow(this).FindName("svContent") as ScrollViewer;
            int wpVerticalMargin = 0;        //wpHome外部的上下间距

            int wpHorizentalMargin = 44;     //wpHome外部的左右间距

            int wpVerticalPadding = 32;      //wpHome内部的上下间距

            int wpHoriztentalPadding = 28;   //wpHome内部的左右间距

            int shortcutLeftMargin = 18;     //快捷方式的左边距

            int shortcutTopMargin = 10;      //快捷方式的上边距

            int shortcutRightMargin = 32;    //快捷方式的右边距 

            int shortcutBottomMargin = 0;    //快捷方式的下边距

            int shortcutWidth = 87;           //快捷方式的宽度
            int shortcutHeight = 120;         //快捷方式的高度

            int wpActualWidth = (int)svContent.ActualWidth - wpHorizentalMargin * 2 - wpHoriztentalPadding * 2;

            int wpActualHeight = (int)svContent.ActualHeight - wpVerticalMargin * 2 - wpVerticalPadding * 2;

            int shortcutActualWidth = shortcutWidth + shortcutLeftMargin + shortcutRightMargin;

            int shortcutActualHeight = shortcutHeight + shortcutTopMargin + shortcutBottomMargin;

            int HorizentalNum = wpActualWidth / shortcutActualWidth; //可以容纳多少列
            int VerticalNum = wpActualHeight / shortcutActualHeight; //可以容纳多少行

            return HorizentalNum * VerticalNum; //列*行=图标总数
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;

            switch (btn.Content.ToString())
            {
                case "dd1":
                    this.ShowWindow(new Statistics());
                    break;

                default:
                    this.ShowWindow(new LdInfo());
                    break;
            }
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (PageCount > 1 && (NowPage > 0))
            {
                BindPage(--NowPage);
            }
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            if (PageCount > 1 && (NowPage < PageCount - 1))
            {
                BindPage(++NowPage);
            }
        }

        private Button CreateButton(string name, string imgUrl)
        {
            Button btn = new Button();
            btn.Template = App.Current.Resources["HotBtnStyle"] as ControlTemplate;
            btn.Content = name;
            btn.Tag = imgUrl;
            btn.Click += Button_Click;
            return btn;
        }
    }
}
