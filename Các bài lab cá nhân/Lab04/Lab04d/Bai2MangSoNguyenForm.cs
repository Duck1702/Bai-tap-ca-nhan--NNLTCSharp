namespace Lab04d;

public class Bai2MangSoNguyenForm : Form
{
    TextBox txtMang = new(); TextBox txtKetQua = new(); TextBox txtGiaTri = new(); TextBox txtViTri = new(); MangSoNguyen? mang;
    public Bai2MangSoNguyenForm()
    {
        Text = "Mảng số nguyên"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(920, 820), new Size(700, 650)); content.ColumnCount = 1; content.RowCount = 7;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); for (int i = 0; i < 5; i++) content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Label title = UiLayout.Title("MẢNG SỐ NGUYÊN"); title.ForeColor = Color.Red; content.Controls.Add(title, 0, 0);

        Button btnNhap = UiLayout.Button("Nhập"); GroupBox nhap = Group("Nhập mảng", InputRow("Nhập mảng:", txtMang, btnNhap)); content.Controls.Add(nhap, 0, 1);
        txtKetQua.ReadOnly = true; GroupBox ketQua = Group("Kết quả mảng", OneField("Kết quả:", txtKetQua)); content.Controls.Add(ketQua, 0, 2);

        Button btnTang = UiLayout.Button("Sắp xếp tăng"), btnGiam = UiLayout.Button("Sắp xếp giảm"), btnTong = UiLayout.Button("Tính tổng"), btnMaxMin = UiLayout.Button("Max - Min"); FlowLayoutPanel sortButtons = UiLayout.ButtonBar(); sortButtons.Controls.AddRange(new Control[] { btnTang, btnGiam, btnTong, btnMaxMin }); GroupBox sort = Group("Sắp xếp / Tổng / Max - Min", sortButtons); content.Controls.Add(sort, 0, 3);

        TableLayoutPanel values = UiLayout.Fields(1, 20); UiLayout.AddField(values, 0, "Giá trị:", txtGiaTri); UiLayout.AddField(values, 0, "Vị trí:", txtViTri);
        // The two input pairs need independent columns so both remain visible on resize.
        values.Controls.Clear(); values.ColumnCount = 4; values.ColumnStyles.Clear(); values.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); values.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); values.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); values.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); values.Controls.Add(UiLayout.Label("Giá trị:"), 0, 0); txtGiaTri.Dock = DockStyle.Fill; values.Controls.Add(txtGiaTri, 1, 0); values.Controls.Add(UiLayout.Label("Vị trí:"), 2, 0); txtViTri.Dock = DockStyle.Fill; values.Controls.Add(txtViTri, 3, 0);
        Button btnTim = UiLayout.Button("Tìm giá trị"), btnThem = UiLayout.Button("Thêm"), btnXoaGiaTri = UiLayout.Button("Xóa giá trị"), btnXoaViTri = UiLayout.Button("Xóa vị trí"), btnThayGiaTri = UiLayout.Button("Thay theo giá trị"), btnThayViTri = UiLayout.Button("Thay theo vị trí");
        TableLayoutPanel operations = new() { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2, RowCount = 3 }; operations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); operations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); operations.Controls.Add(Group("Tìm kiếm", Bar(btnTim)), 0, 0); operations.Controls.Add(Group("Thêm", Bar(btnThem)), 1, 0); operations.Controls.Add(Group("Xóa", Bar(btnXoaGiaTri, btnXoaViTri)), 0, 1); operations.Controls.Add(Group("Thay thế", Bar(btnThayGiaTri, btnThayViTri)), 1, 1); operations.SetColumnSpan(values, 2); operations.Controls.Add(values, 0, 2); content.Controls.Add(operations, 0, 4);
        Button btnReset = UiLayout.Button("Reset"), btnThoat = UiLayout.Button("Thoát"); GroupBox reset = Group("Reset / Thoát", Bar(btnReset, btnThoat)); content.Controls.Add(reset, 0, 5);
        Label help = new() { Text = "Cách nhập mảng: 5 6 4 7 8 9 10.  Thêm: nhập Giá trị + Vị trí.  Thay theo giá trị: Giá trị = số cũ, Vị trí = số mới.  Thay theo vị trí: Vị trí = vị trí cần thay, Giá trị = số mới.", Dock = DockStyle.Fill, AutoSize = true, MaximumSize = new Size(0, 52), Margin = new Padding(4, 10, 4, 0) }; content.Controls.Add(help, 0, 6);

        btnNhap.Click += (_, _) => NhapMang(); btnTang.Click += (_, _) => { if (!CoMang()) return; mang!.SapXepTang(); HienThi(); }; btnGiam.Click += (_, _) => { if (!CoMang()) return; mang!.SapXepGiam(); HienThi(); }; btnTong.Click += (_, _) => { if (CoMang()) MessageBox.Show($"Tổng mảng: {mang!.Tong()}\nTổng chẵn: {mang.TongChan()}\nTổng lẻ: {mang.TongLe()}"); }; btnMaxMin.Click += (_, _) => { if (CoMang()) MessageBox.Show($"Max: {mang!.Max()}\nMin: {mang.Min()}"); };
        btnTim.Click += (_, _) => { if (!CoMang() || !LayGiaTri(out int value)) return; int viTri = mang!.TimViTri(value); if (viTri == -1) MessageBox.Show("Không tìm thấy."); else MessageBox.Show($"Tìm thấy ở vị trí {viTri}."); };
        btnThem.Click += (_, _) => { if (!CoMang() || !LayGiaTri(out int value) || !LayViTri(out int position)) return; if (position < 0 || position > mang!.Mang.Count) { MessageBox.Show("Vị trí không hợp lệ."); return; } mang.Them(position, value); HienThi(); };
        btnXoaGiaTri.Click += (_, _) => { if (!CoMang() || !LayGiaTri(out int value)) return; if (!mang!.XoaGiaTri(value)) MessageBox.Show("Không tìm thấy giá trị."); HienThi(); };
        btnXoaViTri.Click += (_, _) => { if (!CoMang() || !LayViTri(out int position) || !ViTriHopLe(position)) return; mang!.XoaViTri(position); HienThi(); };
        btnThayGiaTri.Click += (_, _) => { if (!CoMang() || !LayGiaTri(out int cu) || !LayViTri(out int moi)) return; mang!.ThayTheTheoGiaTri(cu, moi); HienThi(); };
        btnThayViTri.Click += (_, _) => { if (!CoMang() || !LayViTri(out int position) || !LayGiaTri(out int value) || !ViTriHopLe(position)) return; mang!.ThayTheTheoViTri(position, value); HienThi(); };
        btnReset.Click += (_, _) => { mang = null; txtMang.Clear(); txtKetQua.Clear(); txtGiaTri.Clear(); txtViTri.Clear(); txtMang.Focus(); }; btnThoat.Click += (_, _) => Close(); FormClosing += (_, e) => { if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Cancel = true; };
    }
    private static GroupBox Group(string text, Control child) { GroupBox box = new() { Text = text, Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(10), Margin = new Padding(3) }; child.Dock = DockStyle.Fill; box.Controls.Add(child); return box; }
    private static FlowLayoutPanel Bar(params Control[] controls) { FlowLayoutPanel bar = UiLayout.ButtonBar(); bar.Margin = Padding.Empty; bar.Controls.AddRange(controls); return bar; }
    private static TableLayoutPanel OneField(string label, TextBox field) { TableLayoutPanel table = UiLayout.Fields(1, 16); UiLayout.AddField(table, 0, label, field); return table; }
    private static TableLayoutPanel InputRow(string label, TextBox field, Button button) { TableLayoutPanel table = new() { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 3, RowCount = 1 }; table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); table.Controls.Add(UiLayout.Label(label), 0, 0); field.Dock = DockStyle.Fill; table.Controls.Add(field, 1, 0); table.Controls.Add(button, 2, 0); return table; }
    private void NhapMang() { try { int[] values = txtMang.Text.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray(); if (values.Length == 0) { MessageBox.Show("Mảng không được rỗng."); return; } mang = new MangSoNguyen(values); HienThi(); } catch { MessageBox.Show("Mảng phải chứa các số nguyên."); } }
    private bool CoMang() { if (mang == null || mang.Mang.Count == 0) { MessageBox.Show("Bạn chưa nhập mảng."); return false; } return true; }
    private bool LayGiaTri(out int value) { if (!int.TryParse(txtGiaTri.Text, out value)) { MessageBox.Show("Giá trị phải là số nguyên."); return false; } return true; }
    private bool LayViTri(out int value) { if (!int.TryParse(txtViTri.Text, out value)) { MessageBox.Show("Vị trí phải là số nguyên."); return false; } return true; }
    private bool ViTriHopLe(int position) { if (position < 0 || position >= mang!.Mang.Count) { MessageBox.Show($"Vị trí phải từ 0 đến {mang!.Mang.Count - 1}."); return false; } return true; }
    private void HienThi() => txtKetQua.Text = mang?.ToString();
}
