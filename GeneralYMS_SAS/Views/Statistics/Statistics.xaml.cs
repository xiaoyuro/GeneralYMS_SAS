using GeneralYMS_SAS.App_Data;
using GeneralYMS_SAS.Model;
using GeneralYMS_SAS.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace GeneralYMS_SAS
{
    /// <summary>
    /// Statistics.xaml 的交互逻辑
    /// </summary>
    public partial class Statistics : MetroWindow
    {
        YmsDb Db;

        UserList LoginUser;

        List<Affairs> MyAffairs;

        List<AffairsInfo> MyAffairInfo;

        List<AffairLine> MyAffairLine;

        string MyWorkGroup;

        List<ChartDataClass> BarData = new List<ChartDataClass>();

        List<ChartDataClass> SplineData = new List<ChartDataClass>();

        List<ChartDataClass> PieData = new List<ChartDataClass>();

        public Statistics()
        {
            InitializeComponent();

            Db = new YmsDb();

            LoginUser = App.Current.Resources["MyUser"] as UserList;

            MyAffairs = Db.Affairs.ToList();

            MyWorkGroup = "1|2|3";

            DateTime FirstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);

            //DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

            //MyAffairInfo = Db.AffairsInfo.Where(x => x.OperatorId == LoginUser.UserId && x.LastTransTime >= FirstDay).AsEnumerable().Distinct(new AffairInfoCodeComparer()).ToList();
            MyAffairInfo = Db.AffairsInfo.Where(x => x.OperatorId == LoginUser.UserId).AsEnumerable().Distinct(new AffairInfoCodeComparer()).ToList();

            MyAffairLine = Db.AffairLine.Where(x => x.AreaId == 0).ToList();

            Loaded += Statistics_Loaded;
        }

        private void Statistics_Loaded(object sender, RoutedEventArgs e)
        {
            InitArea();
            InitPage();
        }

        private void InitPage()
        {
            var selectarea = cbStreet.SelectedValue ?? cbArea.SelectedValue ?? cbCity.SelectedValue ?? cbProvince.SelectedValue;

            var areaName = Db.Area.Where(p => p.AreaId == (int)selectarea).FirstOrDefault().AreaName;

            var year = calendar.SelectedDate != null ? calendar.SelectedDate.Value.Year : DateTime.Today.Year;
            titleAll.TitleText = string.Format("{0}{1}年度受理量统计", areaName, year);
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                BarData = BindBar();
                SplineData = BindSpline();
                PieData = BindPie();
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                radChart1.ItemsSource = BarData;
                PieChart1.ItemsSource = PieData;
                lineChart1.ItemsSource = SplineData;

                LoadingBar.IsBusy = false;
            };

            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private List<ChartDataClass> BindBar()
        {
            //AffairId Group
            var query = from p in MyAffairInfo
                        join q in MyAffairs
                        on p.AffairId equals q.AffairId
                        group p by new { p.AffairId, q.WindowId } into g
                        select new
                        {
                            AffairId = g.Key.AffairId,
                            WindowId = g.Key.WindowId,
                            Count = g == null ? 0 : g.Count()
                        };

            //WindowId Group
            var query2 = from p in query
                         join q in Db.Window
                         on p.WindowId equals q.WindowId
                         group p by new { p.WindowId, q.AffairLineId } into g
                         select new
                         {
                             WindowId = g.Key.WindowId,
                             AffairLineId = g.Key.AffairLineId,
                             Count = g == null ? 0 : g.Sum(x => x.Count)
                         };

            var result = from p in MyAffairLine
                         select new ChartDataClass
                         {
                             XValue = p.AffairLineName,
                             YValue = query2.Where(x => x.AffairLineId == p.AffairLineId).Any() ? (double?)query2.Where(x => x.AffairLineId == p.AffairLineId).Sum(x => x.Count) : null
                         };

            return result.ToList();
        }

        private List<ChartDataClass> BindSpline()
        {
            var result = new List<ChartDataClass>();

            for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
            {
                ChartDataClass data = new ChartDataClass();
                data.XValue = DateTime.Now.Month + "-" + i;
                data.YValue = MyAffairInfo.Where(x => x.LastTransTime.Value.Day == i).Any() ? (double?)MyAffairInfo.Where(x => x.LastTransTime.Value.Day == i).Count() : null;
                result.Add(data);
            }
            return result;
        }

        private List<ChartDataClass> BindPie()
        {
            //AffairId Group
            var query = from p in MyAffairInfo
                        join q in MyAffairs
                        on p.AffairId equals q.AffairId
                        group p by new { p.AffairId, q.WindowId } into g
                        select new
                        {
                            AffairId = g.Key.AffairId,
                            WindowId = g.Key.WindowId,
                            Count = g == null ? 0 : g.Count()
                        };

            //WindowId Group
            var query2 = from p in query
                         join q in Db.Window
                         on p.WindowId equals q.WindowId
                         group p by new { p.WindowId, q.AffairLineId } into g
                         select new
                         {
                             WindowId = g.Key.WindowId,
                             AffairLineId = g.Key.AffairLineId,
                             Count = g == null ? 0 : g.Sum(x => x.Count)
                         };

            var result = from p in query2
                         join q in Db.AffairLine
                         on p.AffairLineId equals q.AffairLineId
                         group p by new { p.AffairLineId, q.AffairLineName } into g
                         select new ChartDataClass
                         {
                             XValue = g.Key.AffairLineName,
                             YValue = g == null ? 0 : g.Sum(x => x.Count)
                         };
            return result.ToList();
        }

        #region RadCombobox按钮事件
        private void cbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbCity.ItemsSource = cbProvince.GetChildArea();
            //cbCity.SelectedIndex = 0;
        }

        private void cbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbArea.ItemsSource = cbCity.GetChildArea();
            //cbArea.SelectedIndex = 0;
        }

        private void cbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbStreet.ItemsSource = cbArea.GetChildArea();
            //cbStreet.SelectedIndex = 0;
        }

        private void cbStreet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbStreet.SelectedValue != null)
            {
                App.Current.Resources["MyAreaId"] = (int)cbStreet.SelectedValue;
                //BindGridViewSource();
            }
        }
        #endregion

        #region 获取Area数据源
        /// <summary>
        /// 获取省市区街四个下拉框列表的数据源，并做相关配置
        /// </summary>
        private void InitArea()
        {
            //Bind省
            cbProvince.ItemsSource = Db.Area.Where(p => p.ParentId == 0).ToList();

            //如果用户区域是"全国"，立刻返回
            if (LoginUser.UserAreaId == 0)
            {
                return;
            }

            //根据用户的AreaId获取所属省市区街
            //初始化一个省市区街下拉框的列表
            List<RadComboBox> ComboBoxList = new List<RadComboBox>() { cbProvince, cbCity, cbArea, cbStreet };

            //获取该Area的父节点并保存到List,获取List的个体数目并绑定到每个RadCombobox并把RadCombobox设为不可用
            var LoginUserArea = Db.Area.Where(x => x.AreaId == LoginUser.UserAreaId).FirstOrDefault();
            List<Area> areaList = GetAreaData(LoginUserArea);
            int areaListCount = areaList.Count;

            for (int i = 0; i < areaListCount; i++)
            {
                ComboBoxList[i].SelectedValue = areaList[i].AreaId;
                ComboBoxList[i].IsEnabled = false;
            }
        }

        //获取父节点
        private List<Area> GetAreaData(Area area)
        {
            var result = new List<Area>();
            GetHighLevelArea(result, area);
            return result;
        }

        //获取父节点的具体实现方法
        private void GetHighLevelArea(List<Area> areaList, Area area)
        {
            //如果父节点是0，代表此Area为省，添加列表并返回
            if (area.ParentId == 0)
            {
                areaList.Insert(0, area);
                return;
            }

            //递归添加父节点到列表，直至父节点是0
            var data = Db.Area.Where(x => x.AreaId == area.ParentId);
            if (data.Any())
            {
                areaList.Insert(0, area);
                GetHighLevelArea(areaList, data.FirstOrDefault());
            }
        }
        #endregion

        private void BindGridViewSource()
        {

        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            Storyboard sbShow = this.Resources["sbShowCalendar"] as Storyboard;

            Storyboard sbHide = this.Resources["sbHideCalendar"] as Storyboard;


            if (calendar.Opacity == 0)
            {
                sbHide.Stop();
                sbShow.Begin();

                btn.Tag = "/Image/Statistics/icon_hide.png";
            }

            else
            {
                sbShow.Stop();
                sbHide.Begin();

                btn.Tag = "/Image/Statistics/icon_show.png";
            }
        }

        private void calendar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitPage();
        }

        private void ChartArea_ItemClick(object sender, Telerik.Windows.Controls.Charting.ChartItemClickEventArgs e)
        {
            var td = e.DataPoint.DataItem as ChartDataClass;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                PieData = BindPie();
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                PieChart1.ItemsSource = PieData;

                LoadingBar.IsBusy = false;
            };

            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private void pieChart_ItemClick(object sender, Telerik.Windows.Controls.Charting.ChartItemClickEventArgs e)
        {
            var td = e.DataPoint.DataItem as ChartDataClass;

            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (o, a) =>
            {
                SplineData = BindSpline();
            };
            worker.RunWorkerCompleted += (o, a) =>
            {
                lineChart1.ItemsSource = SplineData;

                LoadingBar.IsBusy = false;
            };

            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }
    }
}
