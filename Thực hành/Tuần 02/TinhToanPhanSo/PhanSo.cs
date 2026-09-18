namespace TinhToanPhanSo
{
    public class PhanSo
    {
        private int tuSo;
        private int mauSo;

        public int TuSo
        {
            get => tuSo;
            set => tuSo = value;
        }

        public int MauSo
        {
            get => mauSo;
            set
            {
                if (value == 0)
                    throw new ArgumentException("Mẫu số không được bằng 0.");

                mauSo = value;
            }
        }

        // Constructor mặc định: 0/1
        public PhanSo()
        {
            tuSo = 0;
            mauSo = 1;
        }

        // Constructor từ số nguyên: n/1
        public PhanSo(int n)
        {
            tuSo = n;
            mauSo = 1;
        }

        // Constructor tử/mẫu
        public PhanSo(int tu, int mau)
        {
            TuSo = tu;
            MauSo = mau;
            RutGon();
        }

        // Copy constructor
        public PhanSo(PhanSo p)
        {
            tuSo = p.tuSo;
            mauSo = p.mauSo;
        }

        public override string ToString()
        {
            if (mauSo == 1)
                return $"[{tuSo}]";

            return $"[{tuSo}/{mauSo}]";
        }

        // Cộng theo kiểu phương thức thành viên
        public PhanSo TinhCong(PhanSo b)
        {
            int tuMoi = tuSo * b.mauSo
                      + mauSo * b.tuSo;

            int mauMoi = mauSo * b.mauSo;

            return new PhanSo(tuMoi, mauMoi);
        }

        // Cộng theo kiểu static
        public static PhanSo TinhCong(
            PhanSo a,
            PhanSo b)
        {
            return a.TinhCong(b);
        }

        // Overload toán tử +
        public static PhanSo operator +(
            PhanSo a,
            PhanSo b)
        {
            return TinhCong(a, b);
        }

        public void RutGon()
        {
            int ucln = Ucln(tuSo, mauSo);

            tuSo /= ucln;
            mauSo /= ucln;

            // Luôn để dấu âm ở tử
            if (mauSo < 0)
            {
                tuSo = -tuSo;
                mauSo = -mauSo;
            }
        }

        private static int Ucln(int a, int b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);

            while (b != 0)
            {
                int r = a % b;
                a = b;
                b = r;
            }

            return a == 0 ? 1 : a;
        }
    }
}