using System.Data;
using System.Globalization;
using System.Text;

namespace Bai05a_EOrder;

/// <summary>
/// Bài thực hành 05a: ComboBox, PictureBox, Button có hình,
/// DataTable + DataGridView, quản lý món theo từng bàn.
/// Việc gửi bếp được MINH HỌA bằng lưu đơn hàng vào file .txt,
/// không giả định có sẵn mạng wireless/server thật.
/// </summary>
public sealed class EOrderForm : Form
{
    private readonly ComboBox cboBan = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 175 };
    private readonly DataGridView dgvMon = new()
    {
        Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
        AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        BackgroundColor = Color.White, RowHeadersVisible = false
    };
    private readonly Label lblTong = new()
    {
        AutoSize = true, Font = new Font("Segoe UI", 12, FontStyle.Bold),
        ForeColor = Color.FromArgb(24, 93, 67)
    };
    private readonly Label lblTrangThai = new() { AutoSize = true, ForeColor = Color.DarkSlateGray };
    private readonly Dictionary<string, DataTable> donTheoBan = new();
    private readonly Dictionary<string, decimal> donGia = new(StringComparer.OrdinalIgnoreCase);
    private readonly CultureInfo vi = CultureInfo.GetCultureInfo("vi-VN");
    private DataTable? donHienTai;
    private readonly string thuMucDon = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Tuan05_ThucHanh", "DonHang");

    private static readonly (string Ten, decimal Gia)[] MonAn =
    [
        ("Burger phô mai bò", 45000), ("Burger phô mai gà", 42000),
        ("Burger phô mai tôm", 48000), ("Burger phô mai cá", 44000),
        ("Tôm viên Cola", 36000), ("Gà viên Cola", 34000),
        ("Gà rán phần", 49000), ("Cơm gà Tender", 52000),
        ("Pepsi", 18000), ("Coca", 18000), ("7 Up", 18000),
        ("Lipton", 20000), ("Cafe", 23000), ("Cam", 25000),
        ("Khoai tây chiên", 32000)
    ];

    public EOrderForm()
    {
        Text = "Bài TH Tuần 05a — Fastfood E-Order";
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(950, 670);
        Size = new Size(1130, 770);
        Font = new Font("Segoe UI", 10);
        foreach (var (ten, gia) in MonAn) donGia[ten] = gia;
        TaoGiaoDien();
        for (int i = 1; i <= 12; i++) cboBan.Items.Add($"Bàn {i:00}");
        cboBan.SelectedIndexChanged += (_, _) => ChuyenBan();
        cboBan.SelectedIndex = 0;
    }

    private void TaoGiaoDien()
    {
        var main = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4,
            BackColor = Color.FromArgb(245, 247, 249), Padding = new Padding(14)
        };
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 88));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 52));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 48));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        Controls.Add(main);

        var header = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        var logo = new PictureBox { Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom };
        logo.Image = VeHinhLogo();
        header.Controls.Add(logo, 0, 0);
        header.Controls.Add(new Label
        {
            Text = "FASTFOOD ORDER\nChọn món → chọn bàn → gửi bếp (mô phỏng)",
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 16, FontStyle.Bold), ForeColor = Color.FromArgb(30, 71, 105)
        }, 1, 0);
        main.Controls.Add(header, 0, 0);

        var groupMon = new GroupBox { Text = "DANH MỤC MÓN ĂN / THỨC UỐNG", Dock = DockStyle.Fill, Padding = new Padding(9) };
        var gridMon = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4 };
        for (int i = 0; i < 4; i++) gridMon.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        for (int i = 0; i < 4; i++) gridMon.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
        foreach (var (mon, index) in MonAn.Select((mon, index) => (mon, index)))
        {
            var ten = mon.Ten;
            var btn = new Button
            {
                Text = $"{ten}\n{mon.Gia.ToString("N0", vi)} đ",
                Dock = DockStyle.Fill, Margin = new Padding(4), TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleCenter, ImageAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = index < 8 ? Color.FromArgb(223, 237, 249) : Color.FromArgb(238, 244, 223)
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(191, 203, 213);
            btn.Image = TaoIconMon(index < 8 ? Color.SteelBlue : Color.ForestGreen);
            btn.Click += (_, _) => ThemMon(ten);
            gridMon.Controls.Add(btn, index % 4, index / 4);
        }
        groupMon.Controls.Add(gridMon);
        main.Controls.Add(groupMon, 0, 1);

        var panelDon = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
        panelDon.RowStyles.Add(new RowStyle(SizeType.Absolute, 61));
        panelDon.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var action = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true, Padding = new Padding(3) };
        action.Controls.Add(Nhan("Tên bàn:", new Padding(5, 12, 8, 0)));
        action.Controls.Add(cboBan);
        action.Controls.Add(TaoNut("Xóa món", (_, _) => XoaMon()));
        action.Controls.Add(TaoNut("Xóa cả bàn", (_, _) => XoaBan()));
        action.Controls.Add(TaoNut("Order / Gửi bếp", (_, _) => GuiBep(), Color.FromArgb(43, 139, 99)));
        action.Controls.Add(TaoNut("Xem đơn đã gửi", (_, _) => XemDonDaGui()));
        panelDon.Controls.Add(action, 0, 0);

        var bottom = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
        bottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        bottom.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
        dgvMon.DataBindingComplete += (_, _) => DinhDangBang();
        bottom.Controls.Add(dgvMon, 0, 0);
        bottom.Controls.Add(lblTong, 0, 1);
        panelDon.Controls.Add(bottom, 0, 1);
        main.Controls.Add(panelDon, 0, 2);
        lblTrangThai.Text = "Mỗi lần nhấn món ăn: số lượng tăng 1. Đơn hàng từng bàn được quản lý độc lập.";
        main.Controls.Add(lblTrangThai, 0, 3);
    }

    private static Label Nhan(string text, Padding margin) => new()
    {
        Text = text, AutoSize = true, Margin = margin, Font = new Font("Segoe UI", 10, FontStyle.Bold)
    };

    private static Button TaoNut(string text, EventHandler handler, Color? color = null)
    {
        var b = new Button
        {
            Text = text, Height = 40, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Margin = new Padding(6, 0, 3, 3), Padding = new Padding(7, 0, 7, 0)
        };
        if (color is Color c) { b.BackColor = c; b.ForeColor = Color.White; b.FlatStyle = FlatStyle.Flat; }
        b.Click += handler;
        return b;
    }

    private static Bitmap VeHinhLogo()
    {
        var bmp = new Bitmap(96, 85);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.Clear(Color.Transparent);
        using var p = new Pen(Color.DarkOrange, 5);
        g.DrawEllipse(p, 13, 14, 65, 65);
        using var brush = new SolidBrush(Color.DarkOrange);
        g.FillRectangle(brush, 35, 26, 7, 40);
        g.FillRectangle(brush, 48, 26, 7, 40);
        g.FillRectangle(brush, 30, 29, 30, 7);
        return bmp;
    }

    private static Bitmap TaoIconMon(Color c)
    {
        var bmp = new Bitmap(24, 24);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        using var b = new SolidBrush(c);
        g.FillEllipse(b, 2, 2, 20, 20);
        g.FillEllipse(Brushes.White, 7, 7, 10, 10);
        return bmp;
    }

    private static DataTable TaoDataTable()
    {
        var table = new DataTable("Order");
        table.Columns.Add("FoodName", typeof(string));
        table.Columns.Add("Quantity", typeof(int));
        table.PrimaryKey = [table.Columns["FoodName"]!];
        return table;
    }

    private void ChuyenBan()
    {
        if (cboBan.SelectedItem is not string ban) return;
        if (!donTheoBan.TryGetValue(ban, out var table))
            donTheoBan[ban] = table = TaoDataTable();
        donHienTai = table;
        dgvMon.DataSource = donHienTai;
        CapNhatTong();
    }

    private void ThemMon(string ten)
    {
        if (donHienTai == null) return;
        DataRow? row = donHienTai.Rows.Find(ten);
        if (row == null) donHienTai.Rows.Add(ten, 1);
        else row["Quantity"] = (int)row["Quantity"] + 1;
        CapNhatTong();
        lblTrangThai.Text = $"Đã thêm {ten} vào {cboBan.Text}.";
    }

    private void XoaMon()
    {
        if (donHienTai == null || dgvMon.CurrentRow?.DataBoundItem is not DataRowView row) return;
        donHienTai.Rows.Remove(row.Row);
        CapNhatTong();
    }

    private void XoaBan()
    {
        if (donHienTai == null || donHienTai.Rows.Count == 0) return;
        if (MessageBox.Show($"Xóa toàn bộ món của {cboBan.Text}?", "Xác nhận",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        donHienTai.Clear();
        CapNhatTong();
    }

    private decimal LayTong()
    {
        if (donHienTai == null) return 0;
        return donHienTai.AsEnumerable().Sum(r => donGia[r.Field<string>("FoodName")!] * r.Field<int>("Quantity"));
    }

    private void CapNhatTong()
    {
        lblTong.Text = $"TỔNG: {LayTong().ToString("N0", vi)} đ — {cboBan.Text}";
    }

    private void DinhDangBang()
    {
        if (dgvMon.Columns.Contains("FoodName")) dgvMon.Columns["FoodName"]!.HeaderText = "Món ăn";
        if (dgvMon.Columns.Contains("Quantity"))
        {
            dgvMon.Columns["Quantity"]!.HeaderText = "Số lượng";
            dgvMon.Columns["Quantity"]!.FillWeight = 36;
        }
    }

    private void GuiBep()
    {
        if (donHienTai == null || donHienTai.Rows.Count == 0)
        {
            MessageBox.Show("Chưa có món để gửi bếp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var sb = new StringBuilder();
        sb.AppendLine($"ĐƠN HÀNG — {cboBan.Text} — {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
        foreach (DataRow r in donHienTai.Rows)
        {
            string ten = (string)r["FoodName"];
            int sl = (int)r["Quantity"];
            sb.AppendLine($"  {ten} x {sl}: {(donGia[ten] * sl).ToString("N0", vi)} đ");
        }
        sb.AppendLine($"TỔNG: {LayTong().ToString("N0", vi)} đ");
        sb.AppendLine(new string('-', 52));
        try
        {
            Directory.CreateDirectory(thuMucDon);
            string filename = Path.Combine(thuMucDon, "orders.txt");
            File.AppendAllText(filename, sb.ToString() + Environment.NewLine, Encoding.UTF8);
            donHienTai.Clear();
            CapNhatTong();
            lblTrangThai.Text = "Đã ghi yêu cầu bếp vào file orders.txt (mô phỏng).";
            MessageBox.Show($"Đã gửi bếp (mô phỏng bằng lưu file).\n{filename}", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể lưu đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void XemDonDaGui()
    {
        string filename = Path.Combine(thuMucDon, "orders.txt");
        if (!File.Exists(filename))
        {
            MessageBox.Show("Chưa có đơn hàng nào được gửi.");
            return;
        }
        using var f = new Form { Text = "Đơn đã gửi bếp", Size = new Size(740, 530), StartPosition = FormStartPosition.CenterParent };
        var text = new TextBox { Dock = DockStyle.Fill, Multiline = true, ScrollBars = ScrollBars.Both,
            ReadOnly = true, WordWrap = false, Font = new Font("Consolas", 10) };
        try { text.Text = File.ReadAllText(filename, Encoding.UTF8); }
        catch (Exception ex) { MessageBox.Show(ex.Message); return; }
        f.Controls.Add(text);
        f.ShowDialog(this);
    }
}
