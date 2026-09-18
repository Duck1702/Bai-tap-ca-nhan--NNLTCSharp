namespace MinhHoaDaHinh_SortLib
{
    public class SapXepAdapter
    {
        public virtual int SoSanh(object a, object b)
        {
            int ia = (int)a;
            int ib = (int)b;

            if (ia < ib)
                return -1;

            if (ia == ib)
                return 0;

            return 1;
        }
    }
}