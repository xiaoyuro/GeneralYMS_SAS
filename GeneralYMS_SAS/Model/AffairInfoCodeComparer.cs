using System.Collections.Generic;
using GeneralYMS_SAS.App_Data;

namespace GeneralYMS_SAS.Model
{
    public class AffairInfoCodeComparer : IEqualityComparer<AffairsInfo>
    {
        public bool Equals(AffairsInfo x, AffairsInfo y)
        {
            return x.AffairInfoCode == y.AffairInfoCode;
        }

        public int GetHashCode(AffairsInfo obj)
        {
            return obj.AffairInfoCode.ToLower().GetHashCode();
        }
    }
}
