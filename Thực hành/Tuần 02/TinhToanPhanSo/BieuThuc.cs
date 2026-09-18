using System.Collections;

namespace TinhToanPhanSo
{
    public class BieuThuc : IEnumerable<PhanSo>
    {
        private List<PhanSo> dsPhanSo;

        public BieuThuc()
        {
            dsPhanSo = new List<PhanSo>();
        }

        public BieuThuc(params PhanSo[] ds)
        {
            dsPhanSo = new List<PhanSo>();

            foreach (PhanSo p in ds)
            {
                dsPhanSo.Add(new PhanSo(p));
            }
        }

        public void ThemPhanSo(PhanSo p)
        {
            dsPhanSo.Add(new PhanSo(p));
        }

        public PhanSo TinhToan()
        {
            PhanSo ketQua = new PhanSo();

            foreach (PhanSo p in dsPhanSo)
            {
                ketQua = ketQua + p;
            }

            return ketQua;
        }

        // Indexer
        public PhanSo this[int viTri]
        {
            get
            {
                if (viTri < 0 || viTri >= dsPhanSo.Count)
                    throw new IndexOutOfRangeException(
                        "Vị trí không tồn tại."
                    );

                return dsPhanSo[viTri];
            }
        }

        public override string ToString()
        {
            return string.Join(
                " + ",
                dsPhanSo
            );
        }

        public IEnumerator<PhanSo> GetEnumerator()
        {
            return dsPhanSo.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}