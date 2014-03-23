using GeneralYMS_SAS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace GeneralYMS_SAS
{
    /// <summary>
    ///     AppHome.xaml 的交互逻辑
    /// </summary>
    public partial class AppHome : Page
    {
        private int _nowPage;
        private int _pageCount;
        private int _pageNum;

        private List<Button> _btnList;

        public AppHome()
        {
            InitializeComponent();
            Loaded += AppHome_Loaded;
        }

        private void AppHome_Loaded(object sender, RoutedEventArgs e)
        {
            BindPage(_nowPage);
        }

        private void BindPage(int nowPage)
        {
            _btnList = new List<Button>
            {
                CreateButton("dd1", "/Image/Home/icon_03.png"),
                CreateButton("dd2", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
                CreateButton("ddd3", "/Image/Home/icon_03.png"),
            };

            _pageNum = GetPageNum();

            _pageCount = (int) Math.Ceiling((double) _btnList.Count/_pageNum);

            var loadingBar = this.GetLoadingBar();

            var worker = new BackgroundWorker();
            worker.DoWork += (o, a) => Thread.Sleep(500);
            worker.RunWorkerCompleted += (o, a) =>
            {
                WpHome.Children.Clear();
                _btnList.Skip(nowPage*_pageNum).Take(_pageNum).ToList().ForEach(x => WpHome.Children.Add(x));

                loadingBar.IsBusy = false;
            };

            loadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private int GetPageNum()
        {
            var svContent = Window.GetWindow(this).FindName("SvContent") as ScrollViewer;
            const int wpVerticalMargin = 0; //wpHome外部的上下间距

            const int wpHorizentalMargin = 44; //wpHome外部的左右间距

            const int wpVerticalPadding = 32; //wpHome内部的上下间距

            const int wpHoriztentalPadding = 28; //wpHome内部的左右间距

            const int shortcutLeftMargin = 18; //快捷方式的左边距

            const int shortcutTopMargin = 10; //快捷方式的上边距

            const int shortcutRightMargin = 32; //快捷方式的右边距 

            const int shortcutBottomMargin = 0; //快捷方式的下边距

            const int shortcutWidth = 87; //快捷方式的宽度
            const int shortcutHeight = 120; //快捷方式的高度

            int wpActualWidth = (int) svContent.ActualWidth - wpHorizentalMargin*2 - wpHoriztentalPadding*2;

            int wpActualHeight = (int) svContent.ActualHeight - wpVerticalMargin*2 - wpVerticalPadding*2;

            int shortcutActualWidth = shortcutWidth + shortcutLeftMargin + shortcutRightMargin;

            int shortcutActualHeight = shortcutHeight + shortcutTopMargin + shortcutBottomMargin;

            int HorizentalNum = wpActualWidth/shortcutActualWidth; //可以容纳多少列
            int VerticalNum = wpActualHeight/shortcutActualHeight; //可以容纳多少行

            return HorizentalNum*VerticalNum; //列*行=图标总数
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

        private void BtnLeft_Click(object sender, RoutedEventArgs e)
        {
            if (_pageCount > 1 && (_nowPage > 0))
            {
                BindPage(--_nowPage);
            }
        }

        private void BtnRight_Click(object sender, RoutedEventArgs e)
        {
            if (_pageCount > 1 && (_nowPage < _pageCount - 1))
            {
                BindPage(++_nowPage);
            }
        }

        private Button CreateButton(string name, string imgUrl)
        {
            var btn = new Button
            {
                Template = Application.Current.Resources["HotBtnStyle"] as ControlTemplate,
                Content = name,
                Tag = imgUrl
            };
            btn.Click += Button_Click;
            return btn;
        }
    }
}