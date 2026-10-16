using System.Collections;
using System.Text;
using System.Text.Json;

namespace Bai05b_QuanLySinhVien;

/// <summary>Lớp cơ sở theo sơ đồ lớp tại trang 4 tài liệu 05b.</summary>
public abstract class SinhVien
{
    public string MSSV { get; set; } = ""; // chuỗi để giữ số 0 ở đầu
    public string HoTen { get; set; } = "";
    public DateTime NgaySinh { get; set; } = DateTime.Today;
    public string DiaChi { get; set; } = "";
    public string DienThoai { get; set; } = "";
    public int NienKhoa { get; set; }
    public abstract string LoaiHinh();
    public virtual string ThongTinThem() => "";
}

public sealed class SinhVienDaiHoc : SinhVien
{
    public string ChuyenNganh { get; set; } = "";
    public override string LoaiHinh() => "Đại học";
    public override string ThongTinThem() => ChuyenNganh;
}

public sealed class SinhVienCaoDang : SinhVien
{
    public override string LoaiHinh() => "Cao đẳng";
}

public sealed class SinhVienBangHai : SinhVien
{
    public string Bang1 { get; set; } = "";
    public string DonViCongTac { get; set; } = "";
    public override string LoaiHinh() => "Bằng hai";
    public override string ThongTinThem() => $"{Bang1} / {DonViCongTac}";
}

/// <summary>
/// Bài 05b yêu cầu ArrayList chứa đối tượng SinhVien. Dữ liệu bền vững
/// được ghi ra mỗi dòng JSON trong FILE TEXT UTF-8, không cần database.
/// </summary>
public sealed class KhoSinhVien
{
    public ArrayList StudentList { get; } = new();

    public SinhVien? Tim(string ma) => StudentList.Cast<SinhVien>()
        .FirstOrDefault(s => s.MSSV.Equals(ma, StringComparison.OrdinalIgnoreCase));

    public void Them(SinhVien sv)
    {
        if (Tim(sv.MSSV) != null) throw new InvalidOperationException("MSSV đã tồn tại!");
        StudentList.Add(sv);
    }

    public void XoaNhieu(IEnumerable<string> dsMa)
    {
        foreach (string ma in dsMa.ToArray())
        {
            SinhVien? sv = Tim(ma);
            if (sv != null) StudentList.Remove(sv);
        }
    }

    private sealed record DuLieuFile(
        string Loai, string MSSV, string HoTen, DateTime NgaySinh, string DiaChi,
        string DienThoai, int NienKhoa, string? ChuyenNganh, string? Bang1, string? DonViCongTac);

    private static DuLieuFile ToFile(SinhVien s) => new(
        s.LoaiHinh(), s.MSSV, s.HoTen, s.NgaySinh, s.DiaChi, s.DienThoai, s.NienKhoa,
        (s as SinhVienDaiHoc)?.ChuyenNganh, (s as SinhVienBangHai)?.Bang1,
        (s as SinhVienBangHai)?.DonViCongTac);

    private static SinhVien FromFile(DuLieuFile r)
    {
        SinhVien sv = r.Loai switch
        {
            "Đại học" => new SinhVienDaiHoc { ChuyenNganh = r.ChuyenNganh ?? "" },
            "Cao đẳng" => new SinhVienCaoDang(),
            "Bằng hai" => new SinhVienBangHai
            {
                Bang1 = r.Bang1 ?? "", DonViCongTac = r.DonViCongTac ?? ""
            },
            _ => throw new FormatException("Loại hình sinh viên không hợp lệ: " + r.Loai)
        };
        sv.MSSV = r.MSSV;
        sv.HoTen = r.HoTen;
        sv.NgaySinh = r.NgaySinh;
        sv.DiaChi = r.DiaChi;
        sv.DienThoai = r.DienThoai;
        sv.NienKhoa = r.NienKhoa;
        return sv;
    }

    public void Luu(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        using var writer = new StreamWriter(path, false, new UTF8Encoding(false));
        foreach (SinhVien s in StudentList)
            writer.WriteLine(JsonSerializer.Serialize(ToFile(s)));
    }

    public void Mo(string path)
    {
        var temp = new ArrayList();
        var daCo = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        int soDong = 0;
        foreach (string line in File.ReadLines(path, Encoding.UTF8))
        {
            soDong++;
            if (string.IsNullOrWhiteSpace(line)) continue;
            try
            {
                var dto = JsonSerializer.Deserialize<DuLieuFile>(line)
                    ?? throw new FormatException("Dòng rỗng");
                SinhVien sv = FromFile(dto);
                if (string.IsNullOrWhiteSpace(sv.MSSV) || !daCo.Add(sv.MSSV))
                    throw new FormatException("MSSV trùng hoặc rỗng");
                temp.Add(sv);
            }
            catch (Exception ex) when (ex is JsonException or FormatException)
            {
                throw new FormatException($"Lỗi dữ liệu tại dòng {soDong}: {ex.Message}", ex);
            }
        }
        // Chỉ thay thế dữ liệu hiện tại khi toàn bộ file đọc thành công.
        StudentList.Clear();
        StudentList.AddRange(temp);
    }
}
