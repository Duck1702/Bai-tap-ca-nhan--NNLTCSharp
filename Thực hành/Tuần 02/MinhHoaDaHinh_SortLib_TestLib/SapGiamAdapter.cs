using MinhHoaDaHinh_SortLib;

namespace MinhHoaDaHinh_SortLib_TestLib
{
    public class SapGiamAdapter : SapXepAdapter
    {
        public override int SoSanh(object a, object b)
        {
            int ia = (int)a;
            int ib = (int)b;

            if (ia < ib)
                return 1;

            if (ia == ib)
                return 0;

            return -1;
        }
    }
}