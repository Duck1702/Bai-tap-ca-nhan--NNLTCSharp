using System.Data;

namespace Bai05e_MDI;

/// <summary>Form danh mục tổng quát: thêm/xóa/sửa các dòng ngay trên DataGridView.</summary>
public sealed class DanhMucForm : Form
{
    private readonly DataTable table;
    private readonly DataGridView grid = new()
    {
        Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = true,
        BackgroundColor = Color.White, AllowUserToAddRows = false
    };
    private readonly Label note = new() { AutoSize = true, ForeColor = Color.DimGray };

    public DanhMucForm(DataTable table, string title)
    {
        this.table = table;
        Text = title;
        Font = new Font("Segoe UI", 10);
        Size = new Size(850, 540);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 3 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
        var tools = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true };
        tools.Controls.Add(Button("Thêm mới", (_, _) => Them()));
        tools.Controls.Add(Button("Xóa đã chọn", (_, _) => Xoa()));
        tools.Controls.Add(Button("Chấp nhận sửa", (_, _) => LuuSua()));
        tools.Controls.Add(Button("Đóng", (_, _) => Close()));
        grid.DataSource = table;
        grid.DataError += (_, e) =>
        {
            note.Text = "Dữ liệu sai kiểu: " + e.Exception?.Message;
            e.ThrowException = false;
        };
        grid.CellValidating += (_, e) =>
        {
            var col = table.Columns[e.ColumnIndex];
            if (col?.DataType != typeof(int)) return;
            if (e.FormattedValue?.ToString() is string s && !int.TryParse(s, out int v) ||
                e.FormattedValue?.ToString() is string text && int.TryParse(text, out int parsed) && parsed <= 0)
            {
                grid.Rows[e.RowIndex].ErrorText = "Số tín chỉ phải là số nguyên dương.";
                e.Cancel = true;
            }
            else grid.Rows[e.RowIndex].ErrorText = "";
        };
        note.Text = "Có thể sửa trực tiếp trên bảng, sau đó chọn 'Chấp nhận sửa'. Lưu ra XML từ menu Hệ thống.";
        layout.Controls.Add(tools, 0, 0);
        layout.Controls.Add(grid, 0, 1);
        layout.Controls.Add(note, 0, 2);
        Controls.Add(layout);
    }

    public static Button Button(string text, EventHandler click)
    {
        var b = new Button { Text = text, AutoSize = true, Padding = new Padding(10, 2, 10, 2), Height = 38, Margin = new Padding(4) };
        b.Click += click;
        return b;
    }

    private void Them()
    {
        try
        {
            DataRow r = table.NewRow();
            string keyColumn = table.Columns[0].ColumnName;
            string prefix = table.TableName == "SinhVien" ? "SV" : table.TableName == "MonHoc" ? "MH" : "K";
            int number = 1;
            string key;
            do { key = $"{prefix}{number++:000}"; } while (table.Rows.Find(key) != null);
            r[keyColumn] = key;
            for (int i = 1; i < table.Columns.Count; i++)
            {
                var c = table.Columns[i];
                r[i] = c.DataType == typeof(string) ? "(nhập dữ liệu)" : 1;
            }
            table.Rows.Add(r);
            note.Text = "Đã thêm dòng tạm. Hãy sửa thông tin và nhấn Chấp nhận sửa.";
        }
        catch (Exception ex) { MessageBox.Show("Không thể thêm: " + ex.Message); }
    }

    private void Xoa()
    {
        if (grid.SelectedRows.Count == 0) { MessageBox.Show("Chọn dòng cần xóa."); return; }
        if (MessageBox.Show("Xóa những dòng đã chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        foreach (var row in grid.SelectedRows.Cast<DataGridViewRow>().ToArray())
            if (row.DataBoundItem is DataRowView d) table.Rows.Remove(d.Row);
        note.Text = "Đã xóa các dòng đã chọn.";
    }

    private void LuuSua()
    {
        try
        {
            grid.EndEdit();
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (row[col] == DBNull.Value ||
                        col.DataType == typeof(string) && string.IsNullOrWhiteSpace((string)row[col]))
                        throw new InvalidOperationException($"Không được để trống {col.ColumnName}.");
                }
            }
            table.AcceptChanges();
            note.Text = "Đã áp dụng thay đổi trong bộ nhớ. Hãy chọn Hệ thống > Lưu dữ liệu XML để ghi file.";
        }
        catch (Exception ex) { MessageBox.Show("Lỗi dữ liệu: " + ex.Message); }
    }
}

/// <summary>Form nhập điểm: chọn sinh viên, chọn môn, đặt điểm 0..10.</summary>
public sealed class NhapDiemForm : Form
{
    private readonly AcademicData db;
    private readonly ComboBox cboSV = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 290 },
        cboMon = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 290 };
    private readonly NumericUpDown numDiem = new() { DecimalPlaces = 1, Increment = 0.5M,
        Minimum = 0, Maximum = 10, Width = 100 };
    private readonly DataGridView grid = new() { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
        BackgroundColor = Color.White, ReadOnly = true, AllowUserToAddRows = false };

    public NhapDiemForm(AcademicData db)
    {
        this.db = db;
        Text = "Nhập điểm sinh viên";
        Font = new Font("Segoe UI", 10);
        Size = new Size(840, 560);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 2 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 116));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = true, AutoScroll = true };
        actions.Controls.Add(Ten("Sinh viên:"));
        actions.Controls.Add(cboSV);
        actions.Controls.Add(Ten("Môn học:"));
        actions.Controls.Add(cboMon);
        actions.Controls.Add(Ten("Điểm:"));
        actions.Controls.Add(numDiem);
        actions.Controls.Add(DanhMucForm.Button("Lưu điểm", (_, _) => LuuDiem()));
        actions.Controls.Add(DanhMucForm.Button("Xóa điểm đã chọn", (_, _) => XoaDiem()));
        layout.Controls.Add(actions, 0, 0);
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.MultiSelect = false;
        grid.DataSource = db.Diem;
        layout.Controls.Add(grid, 0, 1);
        Controls.Add(layout);
        GanCombo(cboSV, db.SinhVien, "HoTen", "MaSV");
        GanCombo(cboMon, db.MonHoc, "TenMon", "MaMon");
    }

    private static Label Ten(string text) => new()
    { Text = text, AutoSize = true, Padding = new Padding(0, 11, 0, 0) };

    private static void GanCombo(ComboBox c, DataTable t, string display, string value)
    {
        c.DisplayMember = display;
        c.ValueMember = value;
        c.DataSource = t;
    }

    private void LuuDiem()
    {
        if (cboSV.SelectedValue is not string maSV || cboMon.SelectedValue is not string maMon)
        { MessageBox.Show("Bạn cần chọn sinh viên và môn học."); return; }
        try
        {
            DataRow? r = db.Diem.Rows.Find([maSV, maMon]);
            if (r == null) db.Diem.Rows.Add(maSV, maMon, (double)numDiem.Value);
            else r["DiemSo"] = (double)numDiem.Value;
            MessageBox.Show("Đã thêm/cập nhật điểm vào dữ liệu trong bộ nhớ.");
        }
        catch (Exception ex) { MessageBox.Show("Lỗi nhập điểm: " + ex.Message); }
    }

    private void XoaDiem()
    {
        if (grid.CurrentRow?.DataBoundItem is not DataRowView r) return;
        if (MessageBox.Show("Xóa kết quả điểm này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            db.Diem.Rows.Remove(r.Row);
    }
}

/// <summary>Form xem điểm: liên kết bảng SinhVien + MonHoc + Diem.</summary>
public sealed class XemDiemForm : Form
{
    public XemDiemForm(AcademicData db)
    {
        Text = "Xem điểm sinh viên";
        Size = new Size(900, 580);
        Font = new Font("Segoe UI", 10);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, Padding = new Padding(10) };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 49));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, BackgroundColor = Color.White };
        void RefreshData()
        {
            grid.DataSource = db.Diem.AsEnumerable().Select(r => new
            {
                MSSV = r.Field<string>("MaSV"),
                HoTen = db.TenSinhVien(r.Field<string>("MaSV")!),
                MaMon = r.Field<string>("MaMon"),
                TenMon = db.TenMon(r.Field<string>("MaMon")!),
                Diem = r.Field<double>("DiemSo")
            }).ToList();
        }
        var refresh = DanhMucForm.Button("Làm mới kết quả", (_, _) => RefreshData());
        layout.Controls.Add(refresh, 0, 0);
        layout.Controls.Add(grid, 0, 1);
        Controls.Add(layout);
        RefreshData();
    }
}

/// <summary>Thống kê số sinh viên và điểm trung bình theo khoa.</summary>
public sealed class ThongKeKhoaForm : Form
{
    public ThongKeKhoaForm(AcademicData db)
    {
        Text = "Thống kê khoa";
        Size = new Size(900, 540);
        Font = new Font("Segoe UI", 10);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, Padding = new Padding(10) };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 55));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var grid = new DataGridView { Dock = DockStyle.Fill, BackgroundColor = Color.White,
            ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        void RefreshData()
        {
            grid.DataSource = db.Khoa.AsEnumerable().Select(khoa =>
            {
                string ma = khoa.Field<string>("MaKhoa")!;
                var sinhVien = db.SinhVien.AsEnumerable().Where(s => s.Field<string>("MaKhoa") == ma).ToArray();
                var maSv = sinhVien.Select(s => s.Field<string>("MaSV")!).ToHashSet();
                var diem = db.Diem.AsEnumerable().Where(d => maSv.Contains(d.Field<string>("MaSV")!))
                    .Select(d => d.Field<double>("DiemSo")).ToArray();
                return new
                {
                    MaKhoa = ma, TenKhoa = khoa.Field<string>("TenKhoa"),
                    SoSinhVien = sinhVien.Length, SoLuotDiem = diem.Length,
                    DiemTrungBinh = diem.Length == 0 ? (double?)null : Math.Round(diem.Average(), 2)
                };
            }).ToList();
        }
        layout.Controls.Add(DanhMucForm.Button("Làm mới thống kê", (_, _) => RefreshData()), 0, 0);
        layout.Controls.Add(grid, 0, 1);
        Controls.Add(layout);
        RefreshData();
    }
}

/// <summary>
/// Bài 3 tài liệu 05e chỉ giới thiệu DevExpress, chưa mô tả một GUI bắt buộc.
/// Minh họa bố cục ribbon THUẦN WinForms (KHÔNG phải DevExpress).
/// </summary>
public sealed class RibbonDemoForm : Form
{
    private readonly AcademicData db;
    private readonly TabControl tabs = new() { Dock = DockStyle.Top, Height = 155 };
    public RibbonDemoForm(AcademicData data)
    {
        db = data;
        Text = "Minh họa kiểu giao diện Ribbon (WinForms thuần, không phải DevExpress)";
        Size = new Size(860, 440);
        Font = new Font("Segoe UI", 10);
        var heThong = Tab("Hệ thống", 
            Nut("Lưu dữ liệu", (_, _) => LuuData()),
            Nut("Giới thiệu", (_, _) => MessageBox.Show("Giao diện mô phỏng Ribbon bằng TabControl.\nKhông sử dụng DevExpress.")));
        var chucNang = Tab("Chức năng",
            Nut("Số sinh viên", (_, _) => MessageBox.Show("Có " + db.SinhVien.Rows.Count + " sinh viên.")),
            Nut("Số môn học", (_, _) => MessageBox.Show("Có " + db.MonHoc.Rows.Count + " môn học.")),
            Nut("Số khoa", (_, _) => MessageBox.Show("Có " + db.Khoa.Rows.Count + " khoa.")));
        tabs.TabPages.Add(heThong);
        tabs.TabPages.Add(chucNang);
        Controls.Add(new Label
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
            Font = new Font("Segoe UI", 14),
            Text = "RIBBON UI — MINH HỌA CẤU TRÚC NHÓM LỆNH\n" +
                   "Nếu giảng viên yêu cầu dùng RibbonControl thực của DevExpress,\n" +
                   "cần cài bộ DevExpress có bản quyền và triển khai bằng thư viện đó."
        });
        Controls.Add(tabs);
    }

    private static TabPage Tab(string title, params Button[] buttons)
    {
        var page = new TabPage(title);
        var panel = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12) };
        panel.Controls.AddRange(buttons);
        page.Controls.Add(panel);
        return page;
    }

    private static Button Nut(string text, EventHandler click) => DanhMucForm.Button(text, click);

    private void LuuData()
    {
        using var dialog = new SaveFileDialog { Filter = "XML DataSet|*.xml", FileName = "quanly.xml" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try { db.Save(dialog.FileName); MessageBox.Show("Đã lưu file XML."); }
        catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
}
