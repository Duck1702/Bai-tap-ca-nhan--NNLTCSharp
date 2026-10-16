using System.Data;

namespace Bai05e_MDI;

/// <summary>
/// Dữ liệu demo MDI dùng DataSet (không yêu cầu SQL Server),
/// các form con dùng chung một bộ bảng Khoa, SinhVien, MonHoc, Diem.
/// </summary>
public sealed class AcademicData
{
    public DataSet Data { get; private set; } = new("QuanLySinhVienMDI");
    public DataTable Khoa => Data.Tables["Khoa"]!;
    public DataTable SinhVien => Data.Tables["SinhVien"]!;
    public DataTable MonHoc => Data.Tables["MonHoc"]!;
    public DataTable Diem => Data.Tables["Diem"]!;

    public AcademicData()
    {
        var khoa = TaoBang("Khoa", ("MaKhoa", typeof(string)), ("TenKhoa", typeof(string)));
        khoa.PrimaryKey = [khoa.Columns["MaKhoa"]!];
        var sv = TaoBang("SinhVien", ("MaSV", typeof(string)), ("HoTen", typeof(string)),
            ("Lop", typeof(string)), ("MaKhoa", typeof(string)));
        sv.PrimaryKey = [sv.Columns["MaSV"]!];
        var mon = TaoBang("MonHoc", ("MaMon", typeof(string)), ("TenMon", typeof(string)),
            ("SoTinChi", typeof(int)));
        mon.PrimaryKey = [mon.Columns["MaMon"]!];
        var diem = TaoBang("Diem", ("MaSV", typeof(string)), ("MaMon", typeof(string)),
            ("DiemSo", typeof(double)));
        diem.PrimaryKey = [diem.Columns["MaSV"]!, diem.Columns["MaMon"]!];
        khoa.Rows.Add("CNTT", "Công nghệ thông tin");
        khoa.Rows.Add("QTKD", "Quản trị kinh doanh");
        khoa.Rows.Add("NN", "Ngôn ngữ Anh");
        sv.Rows.Add("SV001", "Nguyễn Văn An", "DCT1241", "CNTT");
        sv.Rows.Add("SV002", "Trần Thị Bình", "DCT1242", "CNTT");
        sv.Rows.Add("SV003", "Lê Văn Cường", "DQK1241", "QTKD");
        mon.Rows.Add("CS01", "C# căn bản", 3);
        mon.Rows.Add("DB01", "Cơ sở dữ liệu", 3);
        mon.Rows.Add("ENG1", "Tiếng Anh", 2);
        diem.Rows.Add("SV001", "CS01", 8.0);
        diem.Rows.Add("SV002", "DB01", 8.5);
        diem.Rows.Add("SV003", "ENG1", 7.0);
    }

    private DataTable TaoBang(string ten, params (string Ten, Type Kieu)[] columns)
    {
        var t = new DataTable(ten);
        foreach (var (col, type) in columns) t.Columns.Add(col, type);
        Data.Tables.Add(t);
        return t;
    }

    public void Save(string path)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        Data.WriteXml(path, XmlWriteMode.WriteSchema);
    }

    public void Load(string path)
    {
        var set = new DataSet();
        set.ReadXml(path, XmlReadMode.ReadSchema);
        if (!(new[] { "Khoa", "SinhVien", "MonHoc", "Diem" }).All(set.Tables.Contains))
            throw new InvalidDataException("File thiếu bảng Khoa, SinhVien, MonHoc hoặc Diem.");
        Data = set;
    }

    public string TenKhoa(string ma) => Khoa.Rows.Find(ma)?["TenKhoa"]?.ToString() ?? ma;
    public string TenMon(string ma) => MonHoc.Rows.Find(ma)?["TenMon"]?.ToString() ?? ma;
    public string TenSinhVien(string ma) => SinhVien.Rows.Find(ma)?["HoTen"]?.ToString() ?? ma;
}
