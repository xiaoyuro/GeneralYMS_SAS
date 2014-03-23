using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using GeneralYMS_SAS.App_Data;
using GeneralYMS_SAS.Model;
using GeneralYMS_SAS.ViewModel;
using MahApps.Metro.Controls;
using Telerik.Windows.Controls;

namespace GeneralYMS_SAS
{
    /// <summary>
    ///     Statistics.xaml 的交互逻辑
    /// </summary>
    public partial class LdInfo : MetroWindow
    {
        private readonly YmsDb _db;

        private readonly UserList _loginUser;

        private readonly List<AffairsInfo> _myAffairInfo;

        private readonly List<AffairLine> _myAffairLine;

        private readonly List<Affairs> _myAffairs;

        public LdInfo()
        {
            InitializeComponent();

            _db = new YmsDb();

            _loginUser = Application.Current.Resources["MyUser"] as UserList;

            _myAffairs = _db.Affairs.ToList();

            var firstDay = DateTime.Now.AddDays(-DateTime.Now.Day + 1);

            //DateTime LastDay = DateTime.Now.AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);

            //MyAffairInfo = Db.AffairsInfo.Where(x => x.OperatorId == LoginUser.UserId && x.LastTransTime >= FirstDay).AsEnumerable().Distinct(new AffairInfoCodeComparer()).ToList();

            _myAffairInfo = _db.AffairsInfo.Where(x => x.OperatorId == _loginUser.UserId)
                                           .AsEnumerable().Distinct(new AffairInfoCodeComparer()).ToList();

            _myAffairLine = _db.AffairLine.Where(x => x.AreaId == 0).ToList();

            Loaded += Statistics_Loaded;
        }

        private void Statistics_Loaded(object sender, RoutedEventArgs e)
        {
            InitArea();
            InitPage();
        }

        private void InitPage()
        {
            var barData = new List<ChartDataClass>();

            var worker = new BackgroundWorker();
            worker.DoWork += (o, a) => { barData = BindBar(); };
            worker.RunWorkerCompleted += (o, a) =>
            {
                //CartesianChart1.Series[0].ItemsSource = BarData;
                RadChart.ItemsSource = barData;
                LoadingBar.IsBusy = false;
            };

            LoadingBar.IsBusy = true;
            worker.RunWorkerAsync();
        }

        private List<ChartDataClass> BindBar()
        {
            //AffairId Group
            var query = from p in _myAffairInfo
                        join q in _myAffairs
                            on p.AffairId equals q.AffairId
                        group p by new { p.AffairId, q.WindowId }
                            into g
                            select new
                            {
                                g.Key.AffairId,
                                g.Key.WindowId,
                                Count = g == null ? 0 : g.Count()
                            };

            //WindowId Group
            var query2 = from p in query
                         join q in _db.Window
                             on p.WindowId equals q.WindowId
                         group p by new { p.WindowId, q.AffairLineId }
                             into g
                             select new
                             {
                                 g.Key.WindowId,
                                 g.Key.AffairLineId,
                                 Count = g == null ? 0 : g.Sum(x => x.Count)
                             };

            IEnumerable<ChartDataClass> result = from p in _myAffairLine
                                                 select new ChartDataClass
                                                 {
                                                     XValue = p.AffairLineName,
                                                     YValue =
                                                         query2.Any(x => x.AffairLineId == p.AffairLineId)
                                                             ? (double?)query2.Where(x => x.AffairLineId == p.AffairLineId).Sum(x => x.Count)
                                                             : null
                                                 };

            return result.ToList();
        }

        #region RadCombobox按钮事件

        private void CbProvince_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbCity.ItemsSource = CbProvince.GetChildArea();
            CbCity.SelectedIndex = 0;
        }

        private void CbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbArea.ItemsSource = CbCity.GetChildArea();
            CbArea.SelectedIndex = 0;
        }

        private void CbArea_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CbStreet.ItemsSource = CbArea.GetChildArea();
            CbStreet.SelectedIndex = 0;
        }

        private void CbStreet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbStreet.SelectedValue != null)
            {
                Application.Current.Resources["MyAreaId"] = (int)CbStreet.SelectedValue;
                //BindGridViewSource();
            }
        }

        #endregion

        #region 获取Area数据源

        /// <summary>
        ///     获取省市区街四个下拉框列表的数据源，并做相关配置
        /// </summary>
        private void InitArea()
        {
            //Bind省
            CbProvince.ItemsSource = _db.Area.Where(p => p.ParentId == 0).ToList();

            //如果用户区域是"全国"，立刻返回
            if (_loginUser.UserAreaId == 0)
            {
                return;
            }

            //根据用户的AreaId获取所属省市区街
            //初始化一个省市区街下拉框的列表
            var comboBoxList = new List<RadComboBox> { CbProvince, CbCity, CbArea, CbStreet };

            //获取该Area的父节点并保存到List,获取List的个体数目并绑定到每个RadCombobox并把RadCombobox设为不可用
            var loginUserArea = _db.Area.FirstOrDefault(x => x.AreaId == _loginUser.UserAreaId);
            var areaList = GetAreaData(loginUserArea);
            var areaListCount = areaList.Count;

            for (int i = 0; i < areaListCount; i++)
            {
                comboBoxList[i].SelectedValue = areaList[i].AreaId;
                comboBoxList[i].IsEnabled = false;
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
        private void GetHighLevelArea(IList<Area> areaList, Area area)
        {
            //如果父节点是0，代表此Area为省，添加列表并返回
            if (area.ParentId == 0)
            {
                areaList.Insert(0, area);
                return;
            }

            //递归添加父节点到列表，直至父节点是0
            var data = _db.Area.Where(x => x.AreaId == area.ParentId);
            if (data.Any())
            {
                areaList.Insert(0, area);
                GetHighLevelArea(areaList, data.FirstOrDefault());
            }
        }

        #endregion
    }
}