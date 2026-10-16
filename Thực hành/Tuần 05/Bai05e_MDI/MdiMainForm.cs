namespace Bai05e_MDI;

/// <summary>
/// Bài 2 PDF 05e: MDI Parent với MenuStrip, ToolStrip, StatusStrip,
/// About, các form con Sinh viên / Khoa / Môn học / Nhập điểm /
/// Xem điểm / Thống kê khoa. Ribbon mô phỏng ở cửa sổ khác.
/// </summary>
public sealed class MdiMainForm : Form
{
    private readonly AcademicData data = new();
    private readonly MenuStrip menu = new();
    private readonly ToolStrip toolbar = new() { GripStyle = ToolStripGripStyle.Hidden, ImageScalingSize = new Size(24, 24) };
    private readonly StatusStrip status = new();
    private readonly ToolStripStatusLabel statusText = new("Sẵn sàng") { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
    private readonly ToolStripStatusLabel clockText = new();
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 1000 };
    private readonly string pathMacDinh = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "Tuan05_ThucHanh", "MDI", "quanly.xml");

    public MdiMainForm()
    {
        Text = "QUẢN LÝ SINH VIÊN — ỨNG DỤNG MDI — BÀI THỰC HÀNH TUẦN 05";
        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;
        MinimumSize = new Size(960, 640);
        Font = new Font("Segoe UI", 10);
        MainMenuStrip = menu;
        TaoMenuVaThanhCongCu();
        timer.Tick += (_, _) => clockText.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        timer.Start();
        FormClosed += (_, _) => timer.Dispose();
    }

    private void TaoMenuVaThanhCongCu()
    {
        var heThong = new ToolStripMenuItem("&Hệ thống");
        heThong.DropDownItems.Add(MenuItem("Mở dữ liệu XML", (_, _) => MoDuLieu()));
        heThong.DropDownItems.Add(MenuItem("Lưu dữ liệu XML", (_, _) => LuuDuLieu()));
        heThong.DropDownItems.Add(new ToolStripSeparator());
        heThong.DropDownItems.Add(MenuItem("Thoát", (_, _) => Close()));

        var chucNang = new ToolStripMenuItem("&Chức năng");
        chucNang.DropDownItems.Add(MenuItem("Sinh viên", (_, _) => MoDanhMuc("SinhVien", "Quản lý sinh viên")));
        chucNang.DropDownItems.Add(MenuItem("Khoa", (_, _) => MoDanhMuc("Khoa", "Quản lý khoa")));
        chucNang.DropDownItems.Add(MenuItem("Môn học", (_, _) => MoDanhMuc("MonHoc", "Quản lý môn học")));
        chucNang.DropDownItems.Add(new ToolStripSeparator());
        chucNang.DropDownItems.Add(MenuItem("Nhập điểm", (_, _) => Mo(new NhapDiemForm(data))));
        chucNang.DropDownItems.Add(MenuItem("Xem điểm", (_, _) => Mo(new XemDiemForm(data))));
        chucNang.DropDownItems.Add(MenuItem("Thống kê khoa", (_, _) => Mo(new ThongKeKhoaForm(data))));

        var hoTro = new ToolStripMenuItem("&Hỗ trợ");
        hoTro.DropDownItems.Add(MenuItem("Giới thiệu", (_, _) => GioiThieu()));
        hoTro.DropDownItems.Add(MenuItem("Ribbon mô phỏng (WinForms thuần)", (_, _) => Mo(new RibbonDemoForm(data))));
        menu.Items.AddRange([heThong, chucNang, hoTro]);

        toolbar.Items.Add(Nut("Sinh viên", (_, _) => MoDanhMuc("SinhVien", "Quản lý sinh viên")));
        toolbar.Items.Add(Nut("Khoa", (_, _) => MoDanhMuc("Khoa", "Quản lý khoa")));
        toolbar.Items.Add(Nut("Môn học", (_, _) => MoDanhMuc("MonHoc", "Quản lý môn học")));
        toolbar.Items.Add(new ToolStripSeparator());
        toolbar.Items.Add(Nut("Nhập điểm", (_, _) => Mo(new NhapDiemForm(data))));
        toolbar.Items.Add(Nut("Xem điểm", (_, _) => Mo(new XemDiemForm(data))));
        toolbar.Items.Add(Nut("Thống kê khoa", (_, _) => Mo(new ThongKeKhoaForm(data))));
        toolbar.Items.Add(new ToolStripSeparator());
        toolbar.Items.Add(Nut("Thoát", (_, _) => Close()));

        status.Items.Add(statusText);
        status.Items.Add(clockText);
        // WinForms Dock sử dụng thứ tự thêm control để xác lập thanh menu/toolbar.
        Controls.Add(toolbar);
        Controls.Add(menu);
        Controls.Add(status);
    }

    private static ToolStripMenuItem MenuItem(string text, EventHandler handler)
    {
        var item = new ToolStripMenuItem(text);
        item.Click += handler;
        return item;
    }

    private static ToolStripButton Nut(string text, EventHandler handler)
    {
        var icon = new Bitmap(23, 23);
        using (var g = Graphics.FromImage(icon))
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);
            using var brush = new SolidBrush(Color.FromArgb(46, 117, 169));
            g.FillEllipse(brush, 1, 1, 21, 21);
            TextRenderer.DrawText(g, text[..1], new Font("Segoe UI", 9, FontStyle.Bold),
                new Rectangle(3, 2, 19, 19), Color.White, TextFormatFlags.HorizontalCenter);
        }
        var item = new ToolStripButton(text) { DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
            Image = icon, ImageScaling = ToolStripItemImageScaling.None,
            Font = new Font("Segoe UI", 10, FontStyle.Bold), Padding = new Padding(7, 6, 7, 6) };
        item.Click += handler;
        return item;
    }

    private void MoDanhMuc(string bang, string title) => Mo(new DanhMucForm(data.Data.Tables[bang]!, title));

    private void Mo(Form child)
    {
        child.MdiParent = this;
        child.Show();
        statusText.Text = "Đang mở: " + child.Text;
    }

    private void LuuDuLieu()
    {
        using var dlg = new SaveFileDialog { Filter = "XML DataSet|*.xml", FileName = "quanly.xml",
            InitialDirectory = Directory.Exists(Path.GetDirectoryName(pathMacDinh))
                ? Path.GetDirectoryName(pathMacDinh) : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try { data.Save(dlg.FileName); statusText.Text = "Đã lưu dữ liệu: " + dlg.FileName; }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi lưu dữ liệu"); }
    }

    private void MoDuLieu()
    {
        using var dlg = new OpenFileDialog { Filter = "XML DataSet|*.xml|Tất cả file|*.*" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            foreach (var form in MdiChildren) form.Close();
            data.Load(dlg.FileName);
            statusText.Text = "Đã mở: " + dlg.FileName;
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi mở dữ liệu"); }
    }

    private void GioiThieu()
    {
        using var dlg = new Form { Text = "About QLSV", StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, Size = new Size(520, 300) };
        dlg.Controls.Add(new Label
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 12),
            Text = "QUẢN LÝ SINH VIÊN (MDI)\nVersion 1.0.0\nBài thực hành Tuần 05 — Windows Forms\n\n" +
                   "Chức năng: Sinh viên, Khoa, Môn học, Nhập / Xem điểm, Thống kê."
        });
        dlg.ShowDialog(this);
    }
}
