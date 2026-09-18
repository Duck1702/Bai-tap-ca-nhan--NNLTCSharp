using MinhHoaDaHinh_SortLib;

namespace MinhHoaDaHinh_SortLib_TestLib
{
    public class SapTangDoubleAdapter : SapXepAdapter
    {
        public override int SoSanh(object a, object b)
        {
            double da = (double)a;
            double db = (double)b;

            if (da < db)
                return -1;

            if (da == db)
                return 0;

            return 1;
        }
    }
}