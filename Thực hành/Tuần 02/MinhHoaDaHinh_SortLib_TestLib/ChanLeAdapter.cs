using MinhHoaDaHinh_SortLib;

namespace MinhHoaDaHinh_SortLib_TestLib
{
    public class ChanLeAdapter : SapXepAdapter
    {
        public override int SoSanh(object a, object b)
        {
            int ia = (int)a;
            int ib = (int)b;

            // a chẵn, b lẻ
            // => a đứng trước b
            if (ia % 2 == 0 && ib % 2 != 0)
                return -1;

            // a lẻ, b chẵn
            // => a đứng sau b
            if (ia % 2 != 0 && ib % 2 == 0)
                return 1;

            return 0;
        }
    }
}