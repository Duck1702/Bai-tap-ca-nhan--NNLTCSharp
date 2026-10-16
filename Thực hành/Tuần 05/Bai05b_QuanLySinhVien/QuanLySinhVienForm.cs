using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Bai05b_QuanLySinhVien;

/// <summary>Form 1 theo tài liệu 05b. GUI viết bằng C# để làm hoàn toàn trong VS Code.</summary>
public sealed class QuanLySinhVienForm : Form
{
    private readonly KhoSinhVien kho = new();
    private readonly TextBox txtMSSV = new(), txtHoTen = new(), txtDiaChi = new(),
        txtDienThoai = new(), txtNienKhoa = new(), txtBang1 = new(), txtCongTac = new();
    private readonly DateTimePicker dtpNgaySinh = new() { Format = DateTimePickerFormat.Short, Width = 200 };
    private readonly ComboBox cboChuyenNganh = new()
    {
        DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill
    };
    private readonly RadioButton radDaiHoc = new() { Text = "Đại học", AutoSize = true, Checked = true },
        radCaoDang = new() { Text = "Cao đẳng", AutoSize = true },
        radBangHai = new() { Text = "Bằng hai", AutoSize = true };
    private readonly ListView lvwSinhVien = new()
    {
        Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true,
        GridLines = true, MultiSelect = true, HideSelection = false
    };
    private readonly ErrorProvider errorProvider = new() { BlinkStyle = ErrorBlinkStyle.NeverBlink };
    private readonly Button btnThem = new() { Text = "Thêm" },
        btnSua = new() { Text = "Sửa" }, btnLuuSua = new() { Text = "Lưu sửa" },
        btnXoa = new() { Text = "Xóa đã chọn" }, btnReset = new() { Text = "Reset" },
        btnLuuTep = new() { Text = "Lưu file TXT" }, btnMoTep = new() { Text = "Mở file TXT" },
        btnThoat = new() { Text = "Thoát" };
    private readonly Label lblTrangThai = new() { AutoSize = true, ForeColor = Color.SteelBlue };
    private bool dangSua;
    private string? maDangSua;
    private readonly string duongDanMacDinh = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        "Tuan05_ThucHanh", "SinhVien", "sinhvien.txt");

    public QuanLySinhVienForm()
    {
        Text = "Bài TH Tuần 05b — Quản lý thông tin sinh viên CNTT";
        Font = new Font("Segoe UI", 10);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(1080, 740);
        Size = new Size(1330, 820);
        errorProvider.ContainerControl = this;
        AutoValidate = AutoValidate.EnableAllowFocusChange;
        TaoGiaoDien();
        cboChuyenNganh.Items.AddRange(["CNPM", "HTTT", "Mạng MT"]);
        cboChuyenNganh.SelectedIndex = 0;
        radDaiHoc.CheckedChanged += LoaiHinh_CheckedChanged;
        radCaoDang.CheckedChanged += LoaiHinh_CheckedChanged;
        radBangHai.CheckedChanged += LoaiHinh_CheckedChanged;
        LoaiHinh_CheckedChanged(this, EventArgs.Empty);
        txtMSSV.Validating += (_, e) => GanKiemTra(txtMSSV, txtMSSV.Text.Trim().Length is >= 4 and <= 16,
            "MSSV phải có 4–16 ký tự.", e);
        txtHoTen.Validating += (_, e) => GanKiemTra(txtHoTen, !string.IsNullOrWhiteSpace(txtHoTen.Text),
            "Bạn chưa nhập họ tên.", e);
        txtDiaChi.Validating += (_, e) => GanKiemTra(txtDiaChi, !string.IsNullOrWhiteSpace(txtDiaChi.Text),
            "Bạn chưa nhập địa chỉ.", e);
        txtDienThoai.Validating += (_, e) => GanKiemTra(txtDienThoai,
            Regex.IsMatch(txtDienThoai.Text.Trim(), @"^\d{9,11}$"),
            "Điện thoại phải có 9–11 chữ số.", e);
        txtNienKhoa.Validating += (_, e) => GanKiemTra(txtNienKhoa,
            int.TryParse(txtNienKhoa.Text, out int nk) && nk >= 1990 && nk <= DateTime.Now.Year + 1,
            "Niên khóa phải là năm hợp lệ.", e);
        cboChuyenNganh.Validating += (_, e) => GanKiemTra(cboChuyenNganh,
            !radDaiHoc.Checked || cboChuyenNganh.SelectedIndex >= 0, "Hãy chọn chuyên ngành.", e);
        txtBang1.Validating += (_, e) => GanKiemTra(txtBang1,
            !radBangHai.Checked || !string.IsNullOrWhiteSpace(txtBang1.Text), "Thiếu chuyên ngành bằng 1.", e);
        txtCongTac.Validating += (_, e) => GanKiemTra(txtCongTac,
            !radBangHai.Checked || !string.IsNullOrWhiteSpace(txtCongTac.Text), "Thiếu đơn vị công tác.", e);
        btnLuuSua.Enabled = false;
        GanSuKien();
        txtNienKhoa.Text = DateTime.Now.Year.ToString();
        dtpNgaySinh.Value = DateTime.Today.AddYears(-18);
        CapNhatDanhSach();
    }

    private void TaoGiaoDien()
    {
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), RowCount = 4, ColumnCount = 1 };
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 289));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Absolute, 85));
        Controls.Add(root);
        root.Controls.Add(new Label
        {
            Text = "QUẢN LÝ THÔNG TIN SINH VIÊN — KHOA CÔNG NGHỆ THÔNG TIN",
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter,
            ForeColor = Color.FromArgb(32, 90, 135), Font = new Font("Segoe UI", 15, FontStyle.Bold)
        }, 0, 0);

        var thongTin = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1 };
        thongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));
        thongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));
        root.Controls.Add(thongTin, 0, 1);
        var chung = new GroupBox { Text = "Thông tin chung", Dock = DockStyle.Fill };
        var fields = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 4, Padding = new Padding(10) };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 31));
        for (int i = 0; i < 4; i++) fields.RowStyles.Add(new RowStyle(SizeType.Absolute, 53));
        AddField(fields, 0, 0, "MSSV", txtMSSV);
        AddField(fields, 2, 0, "Họ và tên", txtHoTen);
        AddField(fields, 0, 1, "Ngày sinh", dtpNgaySinh);
        AddField(fields, 2, 1, "Điện thoại", txtDienThoai);
        AddField(fields, 0, 2, "Địa chỉ", txtDiaChi);
        AddField(fields, 2, 2, "Niên khóa", txtNienKhoa);
        fields.Controls.Add(new Label
        {
            Text = "Khi sửa: chỉ thay đổi ngày sinh, địa chỉ, điện thoại.",
            Dock = DockStyle.Fill, AutoSize = true, ForeColor = Color.DimGray
        }, 0, 3);
        fields.SetColumnSpan(fields.GetControlFromPosition(0, 3)!, 4);
        chung.Controls.Add(fields);
        thongTin.Controls.Add(chung, 0, 0);

        var loai = new GroupBox { Text = "Loại hình đào tạo", Dock = DockStyle.Fill };
        var loaiPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 5, Padding = new Padding(8) };
        loaiPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155));
        loaiPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        for (int i = 0; i < 5; i++) loaiPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        var radios = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = false, WrapContents = true };
        radios.Controls.AddRange([radDaiHoc, radCaoDang, radBangHai]);
        loaiPanel.Controls.Add(radios, 0, 0); loaiPanel.SetColumnSpan(radios, 2);
        AddField(loaiPanel, 0, 1, "Chuyên ngành", cboChuyenNganh);
        AddField(loaiPanel, 0, 2, "Chuyên ngành bằng 1", txtBang1);
        AddField(loaiPanel, 0, 3, "Đơn vị công tác", txtCongTac);
        loaiPanel.Controls.Add(new Label
        {
            Text = "Đại học: CNPM / HTTT / Mạng MT. Bằng hai: cần hai thông tin bổ sung.",
            Dock = DockStyle.Fill, ForeColor = Color.DimGray, AutoSize = true
        }, 0, 4);
        loaiPanel.SetColumnSpan(loaiPanel.GetControlFromPosition(0, 4)!, 2);
        loai.Controls.Add(loaiPanel);
        thongTin.Controls.Add(loai, 1, 0);

        var danhSach = new GroupBox { Text = "Danh sách sinh viên (giữ Ctrl hoặc Shift để chọn nhiều dòng và xóa)", Dock = DockStyle.Fill };
        string[] cols = ["MSSV", "Họ tên", "Ngày sinh", "Địa chỉ", "Điện thoại", "Niên khóa", "Loại hình", "Thông tin chuyên biệt"];
        int[] widths = [105, 165, 110, 195, 130, 94, 110, 220];
        for (int i = 0; i < cols.Length; i++) lvwSinhVien.Columns.Add(cols[i], widths[i]);
        danhSach.Controls.Add(lvwSinhVien);
        root.Controls.Add(danhSach, 0, 2);

        var footer = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
        footer.RowStyles.Add(new RowStyle(SizeType.Absolute, 49));
        footer.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true };
        foreach (var btn in new[] { btnThem, btnSua, btnLuuSua, btnXoa, btnReset, btnLuuTep, btnMoTep, btnThoat })
        {
            btn.AutoSize = true;
            btn.Padding = new Padding(8, 2, 8, 2);
            btn.Height = 39;
            btn.Margin = new Padding(4);
            buttons.Controls.Add(btn);
        }
        footer.Controls.Add(buttons, 0, 0);
        footer.Controls.Add(lblTrangThai, 0, 1);
        root.Controls.Add(footer, 0, 3);
    }

    private static void AddField(TableLayoutPanel p, int col, int row, string title, Control ctrl)
    {
        p.Controls.Add(new Label
        {
            Text = title + ":", TextAlign = ContentAlignment.MiddleLeft, Dock = DockStyle.Fill
        }, col, row);
        ctrl.Dock = DockStyle.Fill;
        ctrl.Margin = new Padding(3, 6, 16, 9);
        p.Controls.Add(ctrl, col + 1, row);
    }

    private void GanSuKien()
    {
        btnThem.Click += (_, _) => ThemSinhVien();
        btnSua.Click += (_, _) => BatDauSua();
        btnLuuSua.Click += (_, _) => LuuSua();
        btnXoa.Click += (_, _) => XoaSinhVien();
        btnReset.Click += (_, _) => ResetForm();
        btnLuuTep.Click += (_, _) => LuuTep();
        btnMoTep.Click += (_, _) => MoTep();
        btnThoat.Click += (_, _) => Close();
        lvwSinhVien.SelectedIndexChanged += (_, _) =>
        {
            if (!dangSua && lvwSinhVien.SelectedItems.Count > 0)
                NapLenForm(kho.Tim(lvwSinhVien.SelectedItems[0].Text)!);
        };
    }

    private void GanKiemTra(Control c, bool hopLe, string thongBao, CancelEventArgs e)
    {
        errorProvider.SetError(c, hopLe ? "" : thongBao);
        // Chỉ hiển thị cảnh báo; việc chặn lưu do DuLieuHopLe đảm nhiệm.
        e.Cancel = false;
    }

    private void LoaiHinh_CheckedChanged(object? sender, EventArgs e)
    {
        cboChuyenNganh.Enabled = radDaiHoc.Checked && !dangSua;
        txtBang1.Enabled = radBangHai.Checked && !dangSua;
        txtCongTac.Enabled = radBangHai.Checked && !dangSua;
    }

    private bool DuLieuHopLe(bool sua = false)
    {
        errorProvider.Clear();
        ValidateChildren(ValidationConstraints.Enabled);
        bool ok = true;
        void Check(Control c, bool h, string m)
        {
            errorProvider.SetError(c, h ? "" : m);
            ok &= h;
        }
        if (!sua)
        {
            Check(txtMSSV, txtMSSV.Text.Trim().Length is >= 4 and <= 16, "MSSV phải có 4–16 ký tự.");
            Check(txtHoTen, !string.IsNullOrWhiteSpace(txtHoTen.Text), "Vui lòng nhập họ tên.");
            Check(txtNienKhoa, int.TryParse(txtNienKhoa.Text, out int nk) &&
                nk >= 1990 && nk <= DateTime.Today.Year + 1, "Niên khóa phải là một năm hợp lệ.");
            Check(cboChuyenNganh, !radDaiHoc.Checked || cboChuyenNganh.SelectedIndex >= 0, "Chọn chuyên ngành.");
            Check(txtBang1, !radBangHai.Checked || !string.IsNullOrWhiteSpace(txtBang1.Text), "Nhập chuyên ngành bằng 1.");
            Check(txtCongTac, !radBangHai.Checked || !string.IsNullOrWhiteSpace(txtCongTac.Text), "Nhập đơn vị công tác.");
        }
        Check(txtDiaChi, !string.IsNullOrWhiteSpace(txtDiaChi.Text), "Vui lòng nhập địa chỉ.");
        Check(txtDienThoai, Regex.IsMatch(txtDienThoai.Text.Trim(), @"^\d{9,11}$"), "Điện thoại gồm 9–11 chữ số.");
        Check(dtpNgaySinh, dtpNgaySinh.Value.Date <= DateTime.Today, "Ngày sinh không được ở tương lai.");
        if (!ok) MessageBox.Show("Dữ liệu không hợp lệ. Xem biểu tượng lỗi bên cạnh các ô nhập.",
            "Kiểm tra dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return ok;
    }

    private SinhVien SinhVienTuForm()
    {
        SinhVien sv = radDaiHoc.Checked
            ? new SinhVienDaiHoc { ChuyenNganh = cboChuyenNganh.Text }
            : radCaoDang.Checked
                ? new SinhVienCaoDang()
                : new SinhVienBangHai { Bang1 = txtBang1.Text.Trim(), DonViCongTac = txtCongTac.Text.Trim() };
        sv.MSSV = txtMSSV.Text.Trim();
        sv.HoTen = txtHoTen.Text.Trim();
        sv.NgaySinh = dtpNgaySinh.Value.Date;
        sv.DiaChi = txtDiaChi.Text.Trim();
        sv.DienThoai = txtDienThoai.Text.Trim();
        sv.NienKhoa = int.Parse(txtNienKhoa.Text);
        return sv;
    }

    private void ThemSinhVien()
    {
        if (dangSua || !DuLieuHopLe()) return;
        try
        {
            kho.Them(SinhVienTuForm());
            ResetForm();
            CapNhatDanhSach();
        }
        catch (InvalidOperationException ex) { MessageBox.Show(ex.Message, "Trùng MSSV"); }
    }

    private void NapLenForm(SinhVien sv)
    {
        txtMSSV.Text = sv.MSSV;
        txtHoTen.Text = sv.HoTen;
        dtpNgaySinh.Value = sv.NgaySinh;
        txtDiaChi.Text = sv.DiaChi;
        txtDienThoai.Text = sv.DienThoai;
        txtNienKhoa.Text = sv.NienKhoa.ToString();
        radDaiHoc.Checked = sv is SinhVienDaiHoc;
        radCaoDang.Checked = sv is SinhVienCaoDang;
        radBangHai.Checked = sv is SinhVienBangHai;
        if (sv is SinhVienDaiHoc dh) cboChuyenNganh.SelectedItem = dh.ChuyenNganh;
        if (sv is SinhVienBangHai b2) { txtBang1.Text = b2.Bang1; txtCongTac.Text = b2.DonViCongTac; }
        LoaiHinh_CheckedChanged(this, EventArgs.Empty);
    }

    private void BatDauSua()
    {
        if (lvwSinhVien.SelectedItems.Count != 1)
        {
            MessageBox.Show("Chọn đúng một sinh viên để sửa."); return;
        }
        maDangSua = lvwSinhVien.SelectedItems[0].Text;
        var sv = kho.Tim(maDangSua);
        if (sv == null) return;
        NapLenForm(sv);
        dangSua = true;
        txtMSSV.Enabled = txtHoTen.Enabled = txtNienKhoa.Enabled = false;
        radDaiHoc.Enabled = radCaoDang.Enabled = radBangHai.Enabled = false;
        cboChuyenNganh.Enabled = txtBang1.Enabled = txtCongTac.Enabled = false;
        lvwSinhVien.Enabled = false;
        btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
        btnLuuSua.Enabled = true;
        lblTrangThai.Text = "Chế độ sửa: chỉ ngày sinh, địa chỉ và điện thoại được thay đổi.";
    }

    private void LuuSua()
    {
        if (!dangSua || !DuLieuHopLe(true) || maDangSua == null) return;
        var sv = kho.Tim(maDangSua);
        if (sv == null) return;
        sv.NgaySinh = dtpNgaySinh.Value.Date;
        sv.DiaChi = txtDiaChi.Text.Trim();
        sv.DienThoai = txtDienThoai.Text.Trim();
        ResetForm();
        CapNhatDanhSach();
    }

    private void XoaSinhVien()
    {
        if (dangSua || lvwSinhVien.SelectedItems.Count == 0) { MessageBox.Show("Chọn ít nhất một sinh viên cần xóa."); return; }
        var ids = lvwSinhVien.SelectedItems.Cast<ListViewItem>().Select(x => x.Text).ToArray();
        if (MessageBox.Show($"Xóa {ids.Length} sinh viên đã chọn?", "Xác nhận",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        kho.XoaNhieu(ids);
        ResetForm();
        CapNhatDanhSach();
    }

    private void CapNhatDanhSach()
    {
        lvwSinhVien.BeginUpdate();
        lvwSinhVien.Items.Clear();
        foreach (SinhVien sv in kho.StudentList)
        {
            var item = new ListViewItem(sv.MSSV);
            item.SubItems.AddRange([
                sv.HoTen, sv.NgaySinh.ToString("dd/MM/yyyy"), sv.DiaChi,
                sv.DienThoai, sv.NienKhoa.ToString(), sv.LoaiHinh(), sv.ThongTinThem()
            ]);
            lvwSinhVien.Items.Add(item);
        }
        lvwSinhVien.EndUpdate();
        lblTrangThai.Text = $"Có {kho.StudentList.Count} sinh viên. File dữ liệu mặc định: {duongDanMacDinh}";
    }

    private void ResetForm()
    {
        dangSua = false;
        maDangSua = null;
        foreach (var t in new[] { txtMSSV, txtHoTen, txtDiaChi, txtDienThoai, txtNienKhoa, txtBang1, txtCongTac })
        {
            t.Enabled = true; t.Clear();
        }
        txtNienKhoa.Text = DateTime.Today.Year.ToString();
        dtpNgaySinh.Value = DateTime.Today.AddYears(-18);
        radDaiHoc.Enabled = radCaoDang.Enabled = radBangHai.Enabled = true;
        radDaiHoc.Checked = true;
        cboChuyenNganh.SelectedIndex = 0;
        lvwSinhVien.Enabled = true;
        foreach (ListViewItem item in lvwSinhVien.SelectedItems.Cast<ListViewItem>().ToArray())
            item.Selected = false;
        btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = true;
        btnLuuSua.Enabled = false;
        errorProvider.Clear();
        LoaiHinh_CheckedChanged(this, EventArgs.Empty);
    }

    private void LuuTep()
    {
        using var dlg = new SaveFileDialog { Filter = "Text file (*.txt)|*.txt", FileName = "sinhvien.txt",
            InitialDirectory = Directory.Exists(Path.GetDirectoryName(duongDanMacDinh))
                ? Path.GetDirectoryName(duongDanMacDinh) : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try { kho.Luu(dlg.FileName); MessageBox.Show("Đã lưu danh sách vào file TXT.\n" + dlg.FileName); }
        catch (Exception ex) { MessageBox.Show("Lỗi lưu file: " + ex.Message); }
    }

    private void MoTep()
    {
        using var dlg = new OpenFileDialog { Filter = "Text file (*.txt)|*.txt|Tất cả file|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        try { kho.Mo(dlg.FileName); ResetForm(); CapNhatDanhSach(); }
        catch (Exception ex) { MessageBox.Show("Lỗi mở file: " + ex.Message); }
    }
}
