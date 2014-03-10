using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeneralYMS_SAS.App_Data;

namespace GeneralYMS_SAS.ViewModel
{
    class StatisticsModel
    {

    }

    public class ChartDataClass
    {
        public string XValue { get; set; }
        public double? YValue { get; set; }
    }

    public class UserAffair
    {
        public PersonBase Person { get; set; }
        public Affairs Affair { get; set; }
        public AffairsInfo AffairInfo { get; set; }
        //public string PersonName { get; set; }
        //public string AffairName { get; set; }
        //public string AffairCode { get; set; }
        //public DateTime? BeginTransTime { get; set; }
    }
}
