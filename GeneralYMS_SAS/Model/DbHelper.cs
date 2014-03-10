using GeneralYMS_SAS.App_Data;
using GeneralYMS_SAS.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Controls;

namespace GeneralYMS_SAS.Model
{
    public static class DbHelper
    {
        public static List<Area> GetChildArea(this RadComboBox radComboBox)
        {
            if (radComboBox.SelectedItem == null)
            {
                return null;
            }

            YmsDb Db = new YmsDb();

            int areaId = (radComboBox.SelectedItem as Area).AreaId;

            return Db.Area.Where(p => p.ParentId == areaId).ToList();
        }

        //检查是否有后台权限
        public static bool HasPower(this string mypowerString, string kind)
        {
            YmsDb Db = new YmsDb();
            int BackUserPowerId = Db.WorkGroup.Where(x => x.WorkGroupName == kind).FirstOrDefault().WorkGroupId;
            return mypowerString.IsContain(BackUserPowerId);
        }

        public static bool IsContain(this string orginString, object searchstring)
        {
            return orginString.ToFomatString().Contains(searchstring.ToFomatString());
        }

        public static List<UserAffair> FormatData(List<AffairsInfo> theAffairs, List<Affairs> myAffairs)
        {
            YmsDb Db = new YmsDb();
            var result = from p in theAffairs
                         select new UserAffair
                         {
                             Person = Db.PersonBase.Where(x => x.PersonId == p.PersonId).FirstOrDefault(),
                             Affair = myAffairs.Where(x => x.AffairId == p.AffairId).FirstOrDefault(),
                             AffairInfo = p
                         };
            return result.ToList();
        }
    }
}
