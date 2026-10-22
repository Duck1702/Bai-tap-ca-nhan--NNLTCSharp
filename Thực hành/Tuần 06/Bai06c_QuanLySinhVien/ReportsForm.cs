using System.Windows.Forms;
using System.Drawing;
using System.Data;
using Microsoft.Data.SqlClient;
namespace Bai06c_QuanLySinhVien;
internal sealed class StudentScoreForm : Form
{
    private readonly ComboBox _code = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160 };
    private readonly ComboBox _name = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 225 };
    private readonly Label _khoa = new() { AutoSize = true, Padding = new Padding(8,10,0,0) };
    private readonly DataGridView _grid = ReportGrid.Make();
    private DataTable? _students;
    private bool _sync;
    internal StudentScoreForm()
    {
        Text = "Tra cứu điểm theo sinh viên"; Width = 900; Height = 600;
        var top = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 55, Padding = new Padding(10) };
        top.Controls.Add(new Label { Text = "Mã số", AutoSize = true, Padding = new Padding(0,10,0,0) }); top.Controls.Add(_code);
        top.Controls.Add(new Label { Text = "Họ tên", AutoSize = true, Padding = new Padding(10,10,0,0) }); top.Controls.Add(_name);
        top.Controls.Add(_khoa);
        var see = new Button { Text = "Xem điểm", Width = 105, Height = 29 };
        see.Click += (_, _) => ShowResult(); top.Controls.Add(see);
        Controls.Add(_grid); Controls.Add(top);
        _code.SelectedIndexChanged += (_, _) => SyncSelection(_code,_name);
        _name.SelectedIndexChanged += (_, _) => SyncSelection(_name,_code);
        Shown += (_, _) => {
            try {
                _students = SqlDatabase.Query("SELECT s.MaSo,s.HoTen,k.TenKhoa FROM dbo.SinhVien s JOIN dbo.Khoa k ON k.MaKhoa=s.Khoa ORDER BY s.MaSo");
                _code.DataSource = _students.Copy(); _code.DisplayMember="MaSo"; _code.ValueMember="MaSo";
                _name.DataSource = _students.Copy(); _name.DisplayMember="HoTen"; _name.ValueMember="MaSo";
                ShowResult();
            } catch(Exception ex) { MessageBox.Show(ex.Message); }
        };
    }
    private void SyncSelection(ComboBox source, ComboBox target)
    {
        if (_sync || source.SelectedValue is not string code) return;
        _sync=true; target.SelectedValue=code; _sync=false; ShowResult();
    }
    private void ShowResult()
    {
        if (_code.SelectedValue is not string code || _students == null) return;
        try {
            var row = _students.AsEnumerable().FirstOrDefault(r=>r.Field<string>("MaSo")==code);
            _khoa.Text = "Khoa: " + (row?["TenKhoa"]?.ToString() ?? "");
            _grid.DataSource = SqlDatabase.Query(
                "SELECT m.MaMH AS [Mã MH], m.TenMH AS [Môn học], k.Diem AS [Điểm] " +
                "FROM dbo.KetQua k JOIN dbo.Mon m ON m.MaMH=k.MaMH WHERE k.MaSo=@ma ORDER BY m.TenMH",
                new SqlParameter("@ma", code));
        } catch(Exception ex) { MessageBox.Show(ex.Message); }
    }
}
internal sealed class StudentsByFacultyForm : Form
{
    private readonly ComboBox _faculty = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 300 };
    private readonly DataGridView _grid = ReportGrid.Make();
    internal StudentsByFacultyForm()
    {
        Text="Danh sách sinh viên theo khoa"; Width=950; Height=620;
        var top=new FlowLayoutPanel { Dock=DockStyle.Top, Height=52, Padding=new Padding(10) };
        top.Controls.Add(new Label { Text="Khoa", AutoSize=true,Padding=new Padding(0,9,0,0) }); top.Controls.Add(_faculty);
        var button=new Button { Text="Xem", Width=110,Height=29 }; button.Click+=(_,_)=>ShowResult();top.Controls.Add(button);
        Controls.Add(_grid); Controls.Add(top);
        _faculty.SelectedIndexChanged+=(_,_)=>ShowResult();
        Shown+=(_,_)=>{
            try {
                _faculty.DisplayMember="TenKhoa";_faculty.ValueMember="MaKhoa";
                _faculty.DataSource=SqlDatabase.Query("SELECT MaKhoa,TenKhoa FROM dbo.Khoa ORDER BY TenKhoa");
                ShowResult();
            } catch(Exception ex) { MessageBox.Show(ex.Message); }
        };
    }
    private void ShowResult()
    {
        if(_faculty.SelectedValue is not string khoa)return;
        try {
            _grid.DataSource=SqlDatabase.Query(
                "SELECT MaSo AS [Mã số],HoTen AS [Họ tên],NgaySinh AS [Ngày sinh],"+
                "CASE WHEN GioiTinh=1 THEN N'Nam' ELSE N'Nữ' END AS [Giới tính],"+
                "DiaChi AS [Địa chỉ],DienThoai AS [Điện thoại] FROM dbo.SinhVien "+
                "WHERE Khoa=@khoa ORDER BY HoTen",new SqlParameter("@khoa",khoa));
        } catch(Exception ex) { MessageBox.Show(ex.Message); }
    }
}
internal static class ReportGrid
{
    internal static DataGridView Make() => new() {
        Dock=DockStyle.Fill,BackgroundColor=Color.White,ReadOnly=true,
        AllowUserToAddRows=false,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,
        SelectionMode=DataGridViewSelectionMode.FullRowSelect,RowHeadersVisible=false
    };
}
