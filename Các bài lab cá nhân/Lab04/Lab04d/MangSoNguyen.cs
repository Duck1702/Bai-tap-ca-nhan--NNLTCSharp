namespace Lab04d;

public class MangSoNguyen
{
    public List<int> Mang { get; private set; }

    public MangSoNguyen(IEnumerable<int> mang)
    {
        Mang = mang.ToList();
    }

    public void SapXepTang()
    {
        Mang.Sort();
    }

    public void SapXepGiam()
    {
        Mang.Sort();
        Mang.Reverse();
    }

    public int TimViTri(int giaTri)
    {
        return Mang.IndexOf(giaTri);
    }

    public void Them(int viTri, int giaTri)
    {
        Mang.Insert(viTri, giaTri);
    }

    public bool XoaGiaTri(int giaTri)
    {
        return Mang.Remove(giaTri);
    }

    public void XoaViTri(int viTri)
    {
        Mang.RemoveAt(viTri);
    }

    public int Tong()
    {
        return Mang.Sum();
    }

    public int TongChan()
    {
        return Mang.Where(x => x % 2 == 0).Sum();
    }

    public int TongLe()
    {
        return Mang.Where(x => x % 2 != 0).Sum();
    }

    public int Max()
    {
        return Mang.Max();
    }

    public int Min()
    {
        return Mang.Min();
    }

    public void ThayTheTheoGiaTri(int giaTriCu, int giaTriMoi)
    {
        for (int i = 0; i < Mang.Count; i++)
        {
            if (Mang[i] == giaTriCu)
                Mang[i] = giaTriMoi;
        }
    }

    public void ThayTheTheoViTri(int viTri, int giaTriMoi)
    {
        Mang[viTri] = giaTriMoi;
    }

    public override string ToString()
    {
        return string.Join(" ", Mang);
    }
}