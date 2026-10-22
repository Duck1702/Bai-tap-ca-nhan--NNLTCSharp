using System.Windows.Forms;
using System.Drawing;
namespace Bai06c_QuanLySinhVien;
internal sealed class MainForm : Form
{
    private readonly MenuStrip _menu = new();
    private readonly ToolStrip _tool = new();
    private readonly StatusStrip _status = new();

    internal MainForm()
    {
        Text = "BÀI THỰC HÀNH 06c - QUẢN LÝ SINH VIÊN / STUDENTDB";
        Width = 1260; Height = 800; WindowState = FormWindowState.Maximized;
        IsMdiContainer = true;
        var statusText = new ToolStripStatusLabel("StudentDB - SQL Server - ADO.NET") { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
        _status.Items.Add(statusText);
        var menuNhap = new ToolStripMenuItem("Nhập liệu");
        AddItem(menuNhap, "Sinh viên", "SinhVien", MakeStudent);
        AddItem(menuNhap, "Khoa", "Khoa", MakeKhoa);
        AddItem(menuNhap, "Môn học", "Mon", MakeMon);
        AddItem(menuNhap, "Nhập điểm", "KetQua", MakeKetQua);
        var menuBaoCao = new ToolStripMenuItem("Tra cứu / Thống kê");
        AddItem(menuBaoCao, "Xem điểm sinh viên", "XemDiem", () => new StudentScoreForm());
        AddItem(menuBaoCao, "Sinh viên theo khoa", "TheoKhoa", () => new StudentsByFacultyForm());
        _menu.Items.Add(menuNhap); _menu.Items.Add(menuBaoCao);
        var exit = new ToolStripMenuItem("Thoát"); exit.Click += (_, _) => Close(); _menu.Items.Add(exit);
        _tool.GripStyle = ToolStripGripStyle.Hidden;
        foreach (var item in new (string name,string key, Func<Form> maker)[] {
            ("Sinh viên","SinhVien",MakeStudent),("Khoa","Khoa",MakeKhoa),
            ("Môn học","Mon",MakeMon),("Nhập điểm","KetQua",MakeKetQua),
            ("Xem điểm","XemDiem",()=>new StudentScoreForm()),
            ("SV theo khoa","TheoKhoa",()=>new StudentsByFacultyForm()) })
        {
            var button = new ToolStripButton(item.name);
            button.Click += (_, _) => OpenChild(item.key, item.maker);
            _tool.Items.Add(button);
        }
        Controls.Add(_status); Controls.Add(_tool); Controls.Add(_menu); MainMenuStrip = _menu;
    }
    private void AddItem(ToolStripMenuItem parent,string name,string key, Func<Form> factory)
    {
        var item = new ToolStripMenuItem(name);
        item.Click += (_, _) => OpenChild(key, factory);
        parent.DropDownItems.Add(item);
    }
    private void OpenChild(string key, Func<Form> factory)
    {
        var existing = MdiChildren.FirstOrDefault(f => Equals(f.Tag, key));
        if (existing != null) { existing.Activate(); return; }
        Form child = factory(); child.Tag = key; child.MdiParent = this; child.Show();
    }
    private static Form MakeStudent() => new TableEditorForm("Nhập thông tin sinh viên", "SinhVien",
        new("MaSo","Mã số"), new("HoTen","Họ tên"), new("NgaySinh","Ngày sinh",InputKind.Date),
        new("GioiTinh","Giới tính (Nam)",InputKind.Boolean), new("DiaChi","Địa chỉ"),
        new("DienThoai","Điện thoại"), new("Khoa","Khoa",InputKind.KhoaLookup));
    private static Form MakeKhoa() => new TableEditorForm("Danh mục khoa","Khoa",
        new("MaKhoa","Mã khoa"),new("TenKhoa","Tên khoa"));
    private static Form MakeMon() => new TableEditorForm("Danh mục môn học","Mon",
        new("MaMH","Mã môn học"),new("TenMH","Tên môn học"),new("SoTiet","Số tiết"));
    private static Form MakeKetQua() => new TableEditorForm("Nhập điểm sinh viên","KetQua",
        new("MaSo","Sinh viên",InputKind.StudentLookup),
        new("MaMH","Môn học",InputKind.SubjectLookup),new("Diem","Điểm (0-10)"));
}
