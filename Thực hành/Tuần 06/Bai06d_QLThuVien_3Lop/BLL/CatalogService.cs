using System.Windows.Forms;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Bai06d_QLThuVien_3Lop.DAL;
namespace Bai06d_QLThuVien_3Lop.BLL;

internal enum FieldType { Text, Integer, Number, Date, Lookup }
internal sealed record FieldSpec(string Name,string Label,FieldType Type=FieldType.Text,
   bool IsKey=false,bool IsIdentity=false,bool Required=false,
   string? LookupTable=null,string? LookupId=null,string? LookupDisplay=null);
internal sealed record TableSpec(string Table,string Caption,FieldSpec[] Fields);

// BLL - Business Logic Layer: kiểm tra dữ liệu và phối hợp CRUD, không chứa control UI.
internal class CatalogService
{
    internal readonly Database Database=new();
    internal TableSpec Spec { get; }
    internal CatalogService(TableSpec spec) => Spec=spec;
    internal DataTable LayDanhSach() => Database.Execute($"SELECT * FROM dbo.[{Spec.Table}] ORDER BY 1");
    internal DataTable LayDanhMucLienKet(FieldSpec field)
    {
        if(field.LookupTable is null || field.LookupId is null || field.LookupDisplay is null)
            throw new InvalidOperationException("Không khai báo danh mục liên kết.");
        return Database.Execute($"SELECT [{field.LookupId}] AS Id,[{field.LookupDisplay}] AS Caption " +
                                $"FROM dbo.[{field.LookupTable}] ORDER BY [{field.LookupDisplay}]");
    }
    // Giá trị nhận từ Form là chuỗi (hoặc SelectedValue); chuẩn hóa về đúng kiểu SQL.
    private object Parse(FieldSpec field,object? value)
    {
        if(value is null || value == DBNull.Value || string.IsNullOrWhiteSpace(Convert.ToString(value)))
        {
            if(field.Required)throw new ArgumentException($"Bạn phải nhập {field.Label}.");
            return DBNull.Value;
        }
        string text=Convert.ToString(value)!.Trim();
        return field.Type switch {
            FieldType.Integer or FieldType.Lookup => int.TryParse(text,out int intValue) ? intValue : throw new ArgumentException($"{field.Label} phải là số nguyên."),
            FieldType.Number => double.TryParse(text,NumberStyles.Float,CultureInfo.InvariantCulture,out double doubleValue) && doubleValue>=0
                                    ? doubleValue : throw new ArgumentException($"{field.Label} phải là số không âm (dùng dấu chấm thập phân)."),
            FieldType.Date => DateTime.TryParseExact(text,"yyyy-MM-dd",CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime dt)
                                    ? dt : throw new ArgumentException($"{field.Label} phải theo định dạng yyyy-MM-dd."),
            _ => text
        };
    }
    private SqlParameter Param(string name, object value) => new(name,value==DBNull.Value?DBNull.Value:value);
    internal void Them(Dictionary<string,object?> values)
    {
        var fields=Spec.Fields.Where(x=>!x.IsIdentity).ToArray();
        var typed=fields.Select(x=>Parse(x,values.GetValueOrDefault(x.Name))).ToArray();
        ValidateBusiness(fields,typed);
        string cols=string.Join(",",fields.Select(x=>$"[{x.Name}]"));
        string args=string.Join(",",fields.Select((x,i)=>$"@v{i}"));
        var parameters=typed.Select((v,i)=>Param($"@v{i}",v)).ToArray();
        Database.ExecuteNonQuery($"INSERT INTO dbo.[{Spec.Table}] ({cols}) VALUES ({args})",parameters);
    }
    internal void Sua(Dictionary<string,object?> values,Dictionary<string,object> originalKeys)
    {
        var fields=Spec.Fields.Where(x=>!x.IsIdentity && !x.IsKey).ToArray();
        if(fields.Length==0)throw new InvalidOperationException("Bảng chi tiết chỉ gồm khóa: hãy xóa dòng cũ rồi thêm dòng mới.");
        var typed=fields.Select(x=>Parse(x,values.GetValueOrDefault(x.Name))).ToArray();
        ValidateBusiness(fields,typed);
        string set=string.Join(",",fields.Select((x,i)=>$"[{x.Name}]=@v{i}"));
        var parameters=typed.Select((v,i)=>Param($"@v{i}",v)).ToList();
        string where=BuildKeyWhere(originalKeys,parameters);
        Database.ExecuteNonQuery($"UPDATE dbo.[{Spec.Table}] SET {set} WHERE {where}",parameters.ToArray());
    }
    internal void Xoa(Dictionary<string,object> originalKeys)
    {
        var parameters=new List<SqlParameter>();
        string where=BuildKeyWhere(originalKeys,parameters);
        Database.ExecuteNonQuery($"DELETE FROM dbo.[{Spec.Table}] WHERE {where}",parameters.ToArray());
    }
    private static void ValidateBusiness(FieldSpec[] fields,object[] values)
    {
        foreach(var pair in fields.Zip(values))
        {
            var field=pair.First;var val=pair.Second;
            if(field.Type==FieldType.Integer && val!=DBNull.Value && Convert.ToInt32(val)<0)
                throw new ArgumentException($"{field.Label} không được âm.");
            if(field.Type==FieldType.Text && val!=DBNull.Value && Convert.ToString(val)!.Length>200)
                throw new ArgumentException($"{field.Label} quá dài.");
        }
    }
    private string BuildKeyWhere(Dictionary<string,object> originalKeys,List<SqlParameter> parameters)
    {
        var keyFields=Spec.Fields.Where(x=>x.IsKey).ToArray();
        if(keyFields.Length==0)throw new InvalidOperationException("Thiếu khóa chính.");
        var predicates=new List<string>();
        foreach(var f in keyFields)
        {
            if(!originalKeys.TryGetValue(f.Name,out object? value))throw new InvalidOperationException($"Thiếu khóa {f.Name}");
            string name="@key"+parameters.Count;
            predicates.Add($"[{f.Name}]={name}");
            parameters.Add(Param(name,value));
        }
        return string.Join(" AND ",predicates);
    }
}

// Các lớp nghiệp vụ danh mục theo bài thực hành 06d.
internal sealed class BangCap : CatalogService { internal BangCap() : base(Catalogs.BangCap){} }
internal sealed class NhanVien : CatalogService { internal NhanVien() : base(Catalogs.NhanVien){} }
internal sealed class DocGia : CatalogService { internal DocGia() : base(Catalogs.DocGia){} }
internal sealed class Sach : CatalogService { internal Sach() : base(Catalogs.Sach){} }
internal sealed class ThamSo : CatalogService { internal ThamSo() : base(Catalogs.ThamSo){} }
internal sealed class PhieuMuonSach : CatalogService { internal PhieuMuonSach() : base(Catalogs.PhieuMuonSach){} }
internal sealed class ChiTietPhieuMuon : CatalogService { internal ChiTietPhieuMuon() : base(Catalogs.ChiTietPhieuMuon){} }
internal sealed class PhieuThuTien : CatalogService { internal PhieuThuTien() : base(Catalogs.PhieuThuTien){} }

internal static class Catalogs
{
    private static FieldSpec F(string name,string caption,FieldType type=FieldType.Text,bool key=false,bool identity=false,bool required=false) =>
        new(name,caption,type,key,identity,required);
    private static FieldSpec L(string name,string caption,string tbl,string id,string display,bool key=false) =>
        new(name,caption,FieldType.Lookup,key,false,true,tbl,id,display);
    internal static readonly TableSpec BangCap=new("BANGCAP","Danh mục bằng cấp",new[]{
        F("MaBangCap","Mã bằng cấp",FieldType.Integer,true,true),F("TenBangCap","Tên bằng cấp",required:true)
    });
    internal static readonly TableSpec NhanVien=new("NHANVIEN","Danh mục nhân viên",new[]{
        F("MaNhanVien","Mã nhân viên",FieldType.Integer,true,true),
        F("HoTenNhanVien","Họ tên",required:true),F("NgaySinh","Ngày sinh",FieldType.Date),
        F("DiaChi","Địa chỉ"),F("DienThoai","Điện thoại"),
        L("MaBangCap","Bằng cấp","BANGCAP","MaBangCap","TenBangCap")
    });
    internal static readonly TableSpec DocGia=new("DOCGIA","Lập thẻ độc giả",new[]{
        F("MaDocGia","Mã độc giả",FieldType.Integer,true,true),F("HoTenDocGia","Họ tên độc giả",required:true),
        F("NgaySinh","Ngày sinh",FieldType.Date),F("DiaChi","Địa chỉ"),F("Email","Email"),
        F("NgayLapThe","Ngày lập thẻ",FieldType.Date),F("NgayHetHan","Ngày hết hạn",FieldType.Date),
        F("TienNo","Tiền nợ",FieldType.Number)
    });
    internal static readonly TableSpec Sach=new("SACH","Tiếp nhận sách",new[]{
        F("MaSach","Mã sách",FieldType.Integer,true,true),F("TenSach","Tên sách",required:true),
        F("TacGia","Tác giả"),F("NamXuatBan","Năm xuất bản",FieldType.Integer),
        F("NhaXuatBan","Nhà xuất bản"),F("TriGia","Trị giá",FieldType.Number),
        F("NgayNhap","Ngày nhập",FieldType.Date)
    });
    internal static readonly TableSpec ThamSo=new("THAMSO","Tham số thư viện",new[]{
        F("TenThamSo","Tên tham số",key:true,required:true),F("GiaTri","Giá trị",FieldType.Integer)
    });
    internal static readonly TableSpec PhieuMuonSach=new("PHIEUMUONSACH","Phiếu mượn sách",new[]{
        F("MaPhieuMuon","Mã phiếu mượn",FieldType.Integer,true,true),
        F("NgayMuon","Ngày mượn",FieldType.Date,required:true),
        L("MaDocGia","Độc giả","DOCGIA","MaDocGia","HoTenDocGia")
    });
    internal static readonly TableSpec ChiTietPhieuMuon=new("CHITIETPHIEUMUON","Chi tiết phiếu mượn",new[]{
        L("MaSach","Sách","SACH","MaSach","TenSach",true),
        L("MaPhieuMuon","Phiếu mượn","PHIEUMUONSACH","MaPhieuMuon","MaPhieuMuon",true)
    });
    internal static readonly TableSpec PhieuThuTien=new("PHIEUTHUTIEN","Phiếu thu tiền",new[]{
        F("MaPhieuThuTien","Mã phiếu thu",FieldType.Integer,true,true),
        F("SoTienNo","Số tiền nợ",FieldType.Number),F("SoTienThu","Số tiền thu",FieldType.Number),
        L("MaDocGia","Độc giả","DOCGIA","MaDocGia","HoTenDocGia"),
        L("MaNhanVien","Nhân viên","NHANVIEN","MaNhanVien","HoTenNhanVien")
    });
}
