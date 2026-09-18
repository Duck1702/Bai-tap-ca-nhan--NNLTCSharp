using System.Collections;

namespace MinhHoaDaHinh_SortLib
{
    public static class SortLib
    {
        // Cách 1: hard-code sắp xếp tăng dần int[]
        public static void SapTang(int[] ds)
        {
            for (int i = 0; i < ds.Length - 1; i++)
            {
                for (int j = i + 1; j < ds.Length; j++)
                {
                    if (ds[i] > ds[j])
                    {
                        int temp = ds[i];
                        ds[i] = ds[j];
                        ds[j] = temp;
                    }
                }
            }
        }

        // Cách 2: dùng Adapter để quyết định khi nào hoán vị
        public static void SapXep(
            int[] ds,
            SapXepAdapter adapter)
        {
            for (int i = 0; i < ds.Length - 1; i++)
            {
                for (int j = i + 1; j < ds.Length; j++)
                {
                    if (adapter.SoSanh(ds[i], ds[j]) > 0)
                    {
                        int temp = ds[i];
                        ds[i] = ds[j];
                        ds[j] = temp;
                    }
                }
            }
        }

        // Tổng quát hơn: nhận IList
        public static void SapXep(
            IList ds,
            SapXepAdapter adapter)
        {
            for (int i = 0; i < ds.Count - 1; i++)
            {
                for (int j = i + 1; j < ds.Count; j++)
                {
                    if (adapter.SoSanh(ds[i]!, ds[j]!) > 0)
                    {
                        object temp = ds[i]!;
                        ds[i] = ds[j];
                        ds[j] = temp;
                    }
                }
            }
        }
    }
}