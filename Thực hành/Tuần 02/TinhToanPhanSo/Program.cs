using System.Text;
using TinhToanPhanSo;

Console.InputEncoding = Encoding.UTF8;
Console.OutputEncoding = Encoding.UTF8;

PhanSo p1 = new PhanSo();
PhanSo p2 = new PhanSo(5);
PhanSo p3 = new PhanSo(1, 2);
PhanSo p4 = new PhanSo(p3);

Console.WriteLine("=== KIỂM TRA CONSTRUCTOR ===");
Console.WriteLine($"p1 = {p1}");
Console.WriteLine($"p2 = {p2}");
Console.WriteLine($"p3 = {p3}");
Console.WriteLine($"p4 = {p4}");

Console.WriteLine();

Console.WriteLine("=== KIỂM TRA PHÉP CỘNG ===");

PhanSo a = new PhanSo(1, 4);
PhanSo b = new PhanSo(2, 6);

PhanSo kq1 = a.TinhCong(b);
PhanSo kq2 = PhanSo.TinhCong(a, b);
PhanSo kq3 = a + b;

Console.WriteLine($"Cách 1: {kq1}");
Console.WriteLine($"Cách 2: {kq2}");
Console.WriteLine($"Cách 3: {kq3}");

Console.WriteLine();

Console.WriteLine("=== KIỂM TRA BIỂU THỨC ===");

BieuThuc bt = new BieuThuc(
    p1,
    p2,
    p3,
    p4
);

Console.WriteLine($"Biểu thức: {bt}");
Console.WriteLine($"Kết quả: {bt.TinhToan()}");

Console.WriteLine(
    $"Phân số ở vị trí 2: {bt[2]}"
);

Console.WriteLine();

Console.WriteLine("Các phân số trong biểu thức:");

foreach (PhanSo p in bt)
{
    Console.WriteLine(p);
}