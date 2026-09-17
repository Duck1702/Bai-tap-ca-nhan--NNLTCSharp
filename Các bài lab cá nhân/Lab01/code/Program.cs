using System;
using System.Text;
using System.Collections;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.Clear();

            Console.WriteLine("==========================================");
            Console.WriteLine("      THỰC HÀNH NGÔN NGỮ LẬP TRÌNH C#");
            Console.WriteLine("==========================================");
            Console.WriteLine("1. Nhập và xuất họ tên");
            Console.WriteLine("2. Xuất họ tên theo mẫu");
            Console.WriteLine("3. Tính x^y");
            Console.WriteLine("4. Kiểm tra số nguyên");
            Console.WriteLine("5. Menu tính toán");
            Console.WriteLine("6. Tìm số lớn nhất trong 3 số nguyên");
            Console.WriteLine("7. Kiểm tra số nguyên tố");
            Console.WriteLine("8. Hoán vị hai số thực");
            Console.WriteLine("9. Tìm số lớn nhất và nhỏ nhất");
            Console.WriteLine("10. Kiểm tra chuỗi đối xứng");
            Console.WriteLine("11. Lấy ký tự đầu và cuối chuỗi");
            Console.WriteLine("12. Đếm chữ thường, chữ hoa, chữ số");
            Console.WriteLine("13. Nhập xuất thông tin sinh viên");
            Console.WriteLine("14. Tính lương nhân viên");
            Console.WriteLine("15. Các thao tác với mảng");
            Console.WriteLine("16. Sắp xếp mảng tên");
            Console.WriteLine("17. Mảng số ngẫu nhiên");
            Console.WriteLine("0. Thoát");
            Console.WriteLine("==========================================");

            Console.Write("Chọn bài: ");
            string chon = Console.ReadLine() ?? "";

            Console.Clear();

            switch (chon)
            {
                case "1":
                    Bai1();
                    break;

                case "2":
                    Bai2();
                    break;

                case "3":
                    Bai3();
                    break;

                case "4":
                    Bai4();
                    break;

                case "5":
                    Bai5();
                    break;

                case "6":
                    Bai6();
                    break;

                case "7":
                    Bai7();
                    break;

                case "8":
                    Bai8();
                    break;

                case "9":
                    Bai9();
                    break;

                case "10":
                    Bai10();
                    break;

                case "11":
                    Bai11();
                    break;

                case "12":
                    Bai12();
                    break;

                case "13":
                    Bai13();
                    break;

                case "14":
                    Bai14();
                    break;

                case "15":
                    Bai15();
                    break;

                case "16":
                    Bai16();
                    break;

                case "17":
                    Bai17();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Không có bài này.");
                    break;
            }

            Console.WriteLine();
            Console.WriteLine("Nhấn Enter để quay lại menu...");
            Console.ReadLine();
        }
    }


    // =====================================================
    // BÀI 1
    // Nhập họ tên và xuất họ tên ra màn hình
    // =====================================================

    static void Bai1()
    {
        Console.WriteLine("----- BÀI 1 -----");

        Console.Write("Nhập họ tên: ");
        string hoTen = Console.ReadLine() ?? "";

        Console.WriteLine("Họ tên vừa nhập: " + hoTen);
    }


    // =====================================================
    // BÀI 2
    // Nhập họ tên và xuất theo định dạng của đề
    // =====================================================

    static void Bai2()
    {
        Console.WriteLine("----- BÀI 2 -----");

        Console.Write("Nhap ho ten cua ban: ");
        string hoTen = Console.ReadLine() ?? "";

        Console.WriteLine("Chao ban " + hoTen + "!");
    }


    // =====================================================
    // BÀI 3
    // Nhập hai số nguyên x, y và tính x^y
    // =====================================================

    static void Bai3()
    {
        Console.WriteLine("----- BÀI 3 -----");

        int x = NhapSoNguyen("Nhap so nguyen x: ");
        int y = NhapSoNguyen("Nhap so nguyen y: ");

        double ketQua = Math.Pow(x, y);

        Console.WriteLine("Ket qua {0}^{1} la: {2}", x, y, ketQua);
    }


    // =====================================================
    // BÀI 4
    // Làm lại bài 3 nhưng báo lỗi nếu nhập không phải số nguyên
    // =====================================================

    static void Bai4()
    {
        Console.WriteLine("----- BÀI 4 -----");

        int x;
        int y;

        // Nhập x
        while (true)
        {
            Console.Write("Nhap x: ");

            if (int.TryParse(Console.ReadLine(), out x))
                break;

            Console.WriteLine("Loi: x phai la so nguyen.");
        }

        // Nhập y
        while (true)
        {
            Console.Write("Nhap y: ");

            if (int.TryParse(Console.ReadLine(), out y))
                break;

            Console.WriteLine("Loi: y phai la so nguyen.");
        }

        Console.WriteLine("Ket qua {0}^{1} la: {2}",
            x, y, Math.Pow(x, y));
    }


    // =====================================================
    // BÀI 5
    // Menu tính toán với hai số thực x, y
    // =====================================================

    static void Bai5()
    {
        Console.WriteLine("----- BÀI 5 -----");

        double x = NhapSoThuc("Nhap x: ");
        double y = NhapSoThuc("Nhap y: ");

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("MENU");
            Console.WriteLine("1. Tinh x^y");
            Console.WriteLine("2. Tinh can bac hai cua x va y");
            Console.WriteLine("3. Thoat");
            Console.Write("Chon: ");

            string chon = Console.ReadLine() ?? "";

            if (chon == "1")
            {
                Console.WriteLine("x^y = " + Math.Pow(x, y));
            }
            else if (chon == "2")
            {
                if (x < 0 || y < 0)
                {
                    Console.WriteLine(
                        "Khong the tinh can bac hai cua so am.");
                }
                else
                {
                    Console.WriteLine(
                        "Can bac hai cua x = " + Math.Sqrt(x));

                    Console.WriteLine(
                        "Can bac hai cua y = " + Math.Sqrt(y));
                }
            }
            else if (chon == "3")
            {
                break;
            }
            else
            {
                Console.WriteLine("Lua chon khong hop le.");
            }
        }
    }


    // =====================================================
    // BÀI 6
    // Phương thức trả về số lớn nhất trong 3 số nguyên
    // =====================================================

    static int TimMax(int a, int b, int c)
    {
        int max = a;

        if (b > max)
            max = b;

        if (c > max)
            max = c;

        return max;
    }

    static void Bai6()
    {
        Console.WriteLine("----- BÀI 6 -----");

        int a = NhapSoNguyen("Nhap a: ");
        int b = NhapSoNguyen("Nhap b: ");
        int c = NhapSoNguyen("Nhap c: ");

        int max = TimMax(a, b, c);

        Console.WriteLine("So lon nhat la: " + max);
    }


    // =====================================================
    // BÀI 7
    // Kiểm tra một số có phải số nguyên tố hay không
    // =====================================================

    static bool LaSoNguyenTo(int n)
    {
        if (n < 2)
            return false;

        for (int i = 2; i <= Math.Sqrt(n); i++)
        {
            if (n % i == 0)
                return false;
        }

        return true;
    }

    static void Bai7()
    {
        Console.WriteLine("----- BÀI 7 -----");

        int n = NhapSoNguyen("Nhap n: ");

        if (LaSoNguyenTo(n))
            Console.WriteLine(n + " la so nguyen to.");
        else
            Console.WriteLine(n + " khong phai la so nguyen to.");
    }


    // =====================================================
    // BÀI 8
    // Hoán vị hai số thực bằng tham chiếu ref
    // =====================================================

    static void HoanVi(ref double a, ref double b)
    {
        double tam = a;

        a = b;
        b = tam;
    }

    static void Bai8()
    {
        Console.WriteLine("----- BÀI 8 -----");

        double a = NhapSoThuc("Nhap a: ");
        double b = NhapSoThuc("Nhap b: ");

        Console.WriteLine(
            "Truoc khi doi: a = {0}, b = {1}", a, b);

        HoanVi(ref a, ref b);

        Console.WriteLine(
            "Sau khi doi: a = {0}, b = {1}", a, b);
    }


    // =====================================================
    // BÀI 9
    // Tìm giá trị lớn nhất và nhỏ nhất bằng out
    // =====================================================

    static void TimMaxMin(
        double a,
        double b,
        double c,
        out double max,
        out double min)
    {
        max = a;
        min = a;

        if (b > max)
            max = b;

        if (c > max)
            max = c;

        if (b < min)
            min = b;

        if (c < min)
            min = c;
    }

    static void Bai9()
    {
        Console.WriteLine("----- BÀI 9 -----");

        double a = NhapSoThuc("Nhap a: ");
        double b = NhapSoThuc("Nhap b: ");
        double c = NhapSoThuc("Nhap c: ");

        double max;
        double min;

        TimMaxMin(a, b, c, out max, out min);

        Console.WriteLine("Gia tri lon nhat: " + max);
        Console.WriteLine("Gia tri nho nhat: " + min);
    }


    // =====================================================
    // BÀI 10
    // Kiểm tra chuỗi có đối xứng hay không
    // =====================================================

    static bool LaChuoiDoiXung(string s)
    {
        int dau = 0;
        int cuoi = s.Length - 1;

        while (dau < cuoi)
        {
            if (s[dau] != s[cuoi])
                return false;

            dau++;
            cuoi--;
        }

        return true;
    }

    static void Bai10()
    {
        Console.WriteLine("----- BÀI 10 -----");

        Console.Write("Nhap chuoi: ");
        string s = Console.ReadLine() ?? "";

        if (LaChuoiDoiXung(s))
            Console.WriteLine("Chuoi doi xung.");
        else
            Console.WriteLine("Chuoi khong doi xung.");
    }


    // =====================================================
    // BÀI 11
    // In ra ký tự đầu và ký tự cuối của chuỗi
    // =====================================================

    static void Bai11()
    {
        Console.WriteLine("----- BÀI 11 -----");

        Console.Write("Nhap chuoi: ");
        string s = Console.ReadLine() ?? "";

        if (s.Length == 0)
        {
            Console.WriteLine("Chuoi rong.");
            return;
        }

        Console.WriteLine("Ky tu dau: " + s[0]);
        Console.WriteLine(
            "Ky tu cuoi: " + s[s.Length - 1]);
    }


    // =====================================================
    // BÀI 12
    // Đếm chữ thường, chữ hoa và chữ số
    // =====================================================

    static void Bai12()
    {
        Console.WriteLine("----- BÀI 12 -----");

        Console.Write("Nhap chuoi: ");
        string s = Console.ReadLine() ?? "";

        int chuThuong = 0;
        int chuHoa = 0;
        int chuSo = 0;

        foreach (char c in s)
        {
            if (char.IsLower(c))
                chuThuong++;
            else if (char.IsUpper(c))
                chuHoa++;
            else if (char.IsDigit(c))
                chuSo++;
        }

        Console.WriteLine("So chu thuong: " + chuThuong);
        Console.WriteLine("So chu hoa: " + chuHoa);
        Console.WriteLine("So chu so: " + chuSo);
    }


    // =====================================================
    // BÀI 13
    // Xây dựng lớp sinh viên
    // =====================================================

    class SinhVien
    {
        public string MaSinhVien;
        public string HoTen;
        public string DiaChi;
        public int NamThuMay;

        public SinhVien(
            string ma,
            string ten,
            string diaChi,
            int nam)
        {
            MaSinhVien = ma;
            HoTen = ten;
            DiaChi = diaChi;
            NamThuMay = nam;
        }

        public void XuatThongTin()
        {
            Console.WriteLine("Ma sinh vien: " + MaSinhVien);
            Console.WriteLine("Ho ten: " + HoTen);
            Console.WriteLine("Dia chi: " + DiaChi);
            Console.WriteLine(
                "Sinh vien nam thu: " + NamThuMay);
        }
    }

    static void Bai13()
    {
        Console.WriteLine("----- BÀI 13 -----");

        Console.Write("Ma sinh vien: ");
        string ma = Console.ReadLine() ?? "";

        Console.Write("Ho ten: ");
        string ten = Console.ReadLine() ?? "";

        Console.Write("Dia chi: ");
        string diaChi = Console.ReadLine() ?? "";

        int nam = NhapSoNguyen(
            "Sinh vien nam thu may: ");

        SinhVien sv = new SinhVien(
            ma, ten, diaChi, nam);

        Console.WriteLine();
        Console.WriteLine("THONG TIN SINH VIEN");
        Console.WriteLine("-------------------");

        sv.XuatThongTin();
    }


    // =====================================================
    // BÀI 14
    // Tính lương nhân viên
    // =====================================================

    static void Bai14()
    {
        Console.WriteLine("----- BÀI 14 -----");

        Console.Write("Ho ten nhan vien: ");
        string hoTen = Console.ReadLine() ?? "";

        double mucLuong =
            NhapSoThuc("Muc luong: ");

        int soNgay =
            NhapSoNguyen("So ngay cong: ");

        double luong = mucLuong * soNgay;

        // Theo đề: nếu lương < 1.000.000
        // thì được cộng thêm 100.000
        if (luong < 1000000)
        {
            luong = luong + 100000;
        }

        Console.WriteLine();
        Console.WriteLine("Nhan vien: " + hoTen);
        Console.WriteLine(
            "Tien luong: {0:N0} VND", luong);
    }


    // =====================================================
    // BÀI 15
    // Các thao tác với mảng:
    // - Nhập mảng
    // - In mảng
    // - Tìm max, min
    // - Sắp xếp
    // =====================================================

    static void Bai15()
    {
        Console.WriteLine("----- BÀI 15 -----");

        int n;

        do
        {
            n = NhapSoNguyen(
                "Nhap so luong phan tu: ");

            if (n <= 0)
                Console.WriteLine(
                    "So luong phan tu phai lon hon 0.");

        } while (n <= 0);

        int[] a = new int[n];

        // Nhập mảng
        for (int i = 0; i < n; i++)
        {
            a[i] = NhapSoNguyen(
                "a[" + i + "] = ");
        }

        Console.WriteLine();
        Console.WriteLine("Mang vua nhap:");
        XuatMang(a);

        // Tìm phần tử lớn nhất và nhỏ nhất
        int max = a[0];
        int min = a[0];

        for (int i = 1; i < n; i++)
        {
            if (a[i] > max)
                max = a[i];

            if (a[i] < min)
                min = a[i];
        }

        Console.WriteLine(
            "Phan tu lon nhat: " + max);

        Console.WriteLine(
            "Phan tu nho nhat: " + min);

        // Sắp xếp tăng dần
        Array.Sort(a);

        Console.WriteLine(
            "Mang sau khi sap xep tang dan:");

        XuatMang(a);
    }


    // =====================================================
    // BÀI 16
    // Nhập mảng tên và sắp xếp tăng dần
    // =====================================================

    static void Bai16()
    {
        Console.WriteLine("----- BÀI 16 -----");

        int n;

        do
        {
            n = NhapSoNguyen(
                "Nhap so luong ten: ");

            if (n <= 0)
                Console.WriteLine(
                    "So luong phai lon hon 0.");

        } while (n <= 0);

        string[] dsTen = new string[n];

        for (int i = 0; i < n; i++)
        {
            Console.Write(
                "Nhap ten thu " + (i + 1) + ": ");

            dsTen[i] = Console.ReadLine() ?? "";
        }

        // Sắp xếp tên
        Array.Sort(
            dsTen,
            StringComparer.CurrentCultureIgnoreCase);

        Console.WriteLine();
        Console.WriteLine(
            "Danh sach sau khi sap xep:");

        for (int i = 0; i < dsTen.Length; i++)
        {
            Console.WriteLine(
                (i + 1) + ". " + dsTen[i]);
        }
    }


    // =====================================================
    // BÀI 17
    // Sinh mảng số nguyên ngẫu nhiên trong [10, 100]
    // Tách thành mảng số chẵn và số lẻ
    // =====================================================

    static void Bai17()
    {
        Console.WriteLine("----- BÀI 17 -----");

        int n;

        do
        {
            n = NhapSoNguyen("Nhap n: ");

            if (n <= 0)
                Console.WriteLine("n phai lon hon 0.");

        } while (n <= 0);

        int[] a = new int[n];

        Random random = new Random();

        // Sinh số ngẫu nhiên từ 10 đến 100
        for (int i = 0; i < n; i++)
        {
            a[i] = random.Next(10, 101);
        }

        ArrayList soChan = new ArrayList();
        ArrayList soLe = new ArrayList();

        // Chia số chẵn và số lẻ
        for (int i = 0; i < n; i++)
        {
            if (a[i] % 2 == 0)
                soChan.Add(a[i]);
            else
                soLe.Add(a[i]);
        }

        Console.WriteLine();
        Console.WriteLine("Mang vua sinh:");
        XuatMang(a);

        Console.WriteLine();
        Console.WriteLine("Cac so chan:");
        XuatArrayList(soChan);

        Console.WriteLine();
        Console.WriteLine("Cac so le:");
        XuatArrayList(soLe);
    }


    // =====================================================
    // HÀM DÙNG CHUNG
    // Nhập số nguyên
    // =====================================================

    static int NhapSoNguyen(string thongBao)
    {
        int n;

        while (true)
        {
            Console.Write(thongBao);

            if (int.TryParse(
                Console.ReadLine(), out n))
            {
                return n;
            }

            Console.WriteLine(
                "Nhap sai. Vui long nhap so nguyen.");
        }
    }


    // =====================================================
    // HÀM DÙNG CHUNG
    // Nhập số thực
    // =====================================================

    static double NhapSoThuc(string thongBao)
    {
        double n;

        while (true)
        {
            Console.Write(thongBao);

            if (double.TryParse(
                Console.ReadLine(), out n))
            {
                return n;
            }

            Console.WriteLine(
                "Nhap sai. Vui long nhap so.");
        }
    }


    // =====================================================
    // Hàm in mảng số nguyên
    // =====================================================

    static void XuatMang(int[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            Console.Write(a[i]);

            if (i < a.Length - 1)
                Console.Write(" ");
        }

        Console.WriteLine();
    }


    // =====================================================
    // Hàm in ArrayList
    // =====================================================

    static void XuatArrayList(ArrayList ds)
    {
        if (ds.Count == 0)
        {
            Console.WriteLine("(Khong co)");
            return;
        }

        for (int i = 0; i < ds.Count; i++)
        {
            Console.Write(ds[i] + " ");
        }

        Console.WriteLine();
    }
}