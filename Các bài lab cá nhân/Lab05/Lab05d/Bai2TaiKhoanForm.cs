namespace Lab05d;

public class Bai2TaiKhoanForm : Form
{
    readonly TextBox soTK = new(); readonly TextBox ten = new(); readonly TextBox diaChi = new(); readonly TextBox soTien = new();
    readonly ListView list = new();
    readonly Label tong = new() { AutoSize = true, BorderStyle = BorderStyle.Fixed3D, MinimumSize = new Size(100, 22) };
    readonly Button them = new() { Text = "Thêm" }; readonly Button luu = new() { Text = "Lưu" }; readonly Button xoa = new() { Text = "Xóa" };
    public Bai2TaiKhoanForm()
    {
        Text = "Tại lớp 2 - Quản lý thông tin tài khoản";
        StartPosition = FormStartPosition.CenterParent; AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(540, 480); MinimumSize = Size; FormBorderStyle = FormBorderStyle.FixedSingle; MaximizeBox = false;
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), RowCount = 8 };
        for (int i = 0; i < 6; i++) root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(new Label { Text = "QUẢN LÝ THÔNG TIN TÀI KHOẢN", ForeColor = Color.Blue, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Anchor = AnchorStyles.None, Margin = new Padding(3, 0, 3, 8) });
        root.Controls.Add(Ui.Row("Số tài khoản", soTK, 160)); root.Controls.Add(Ui.Row("Tên khách hàng", ten, 160)); root.Controls.Add(Ui.Row("Địa chỉ khách hàng", diaChi, 160)); root.Controls.Add(Ui.Row("Số tiền trong tài khoản", soTien, 160));
        var actions = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        foreach (Button b in new[] { them, luu, xoa }) { b.Width = 75; b.Height = 30; }
        actions.Controls.AddRange(new Control[] { them, luu, xoa, Ui.Btn("Thoát", (_, _) => Close(), 75) }); root.Controls.Add(actions);
        list.View = View.Details; list.FullRowSelect = true; list.GridLines = true; list.MultiSelect = false; list.Dock = DockStyle.Fill;
        list.Columns.Add("STT", 40); list.Columns.Add("Số tài khoản", 90); list.Columns.Add("Tên khách hàng", 115); list.Columns.Add("Địa chỉ", 115); list.Columns.Add("Số tiền", 85); root.Controls.Add(list);
        var total = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.Right }; total.Controls.Add(new Label { Text = "Tổng tiền", AutoSize = true, Margin = new Padding(3, 5, 5, 3) }); total.Controls.Add(tong); root.Controls.Add(total);
        Controls.Add(root); luu.Enabled = false; xoa.Enabled = false;
        them.Click += (_, _) => BatDauThem(); luu.Click += (_, _) => Luu(); xoa.Click += (_, _) => Xoa(); list.SelectedIndexChanged += (_, _) => Chon();
    }
    void BatDauThem() { if (them.Text == "Thêm") { them.Text = "Hủy"; luu.Enabled = true; list.Enabled = false; Clear(); soTK.Focus(); } else { them.Text = "Thêm"; luu.Enabled = false; list.Enabled = true; Clear(); } }
    void Luu()
    {
        if (soTK.Text.Trim() == "" || ten.Text.Trim() == "" || diaChi.Text.Trim() == "" || !decimal.TryParse(soTien.Text, out decimal money) || money < 0) { MessageBox.Show("Thông tin tài khoản không hợp lệ."); return; }
        if (list.Items.Cast<ListViewItem>().Any(i => i.SubItems[1].Text == soTK.Text.Trim())) { MessageBox.Show("Số tài khoản đã tồn tại."); return; }
        list.Items.Add(new ListViewItem(new[] { "", soTK.Text.Trim(), ten.Text.Trim(), diaChi.Text.Trim(), money.ToString("N0") })); DanhSTT(); TinhTong(); them.Text = "Thêm"; luu.Enabled = false; list.Enabled = true; Clear();
    }
    void Chon() { xoa.Enabled = list.SelectedItems.Count > 0; if (list.SelectedItems.Count != 1) return; var i = list.SelectedItems[0]; soTK.Text = i.SubItems[1].Text; ten.Text = i.SubItems[2].Text; diaChi.Text = i.SubItems[3].Text; soTien.Text = i.SubItems[4].Text.Replace(",", ""); }
    void Xoa() { if (list.SelectedItems.Count == 0) return; list.SelectedItems[0].Remove(); DanhSTT(); TinhTong(); Clear(); xoa.Enabled = false; }
    void DanhSTT() { for (int i = 0; i < list.Items.Count; i++) list.Items[i].SubItems[0].Text = (i + 1).ToString(); }
    void TinhTong() { decimal sum = 0; foreach (ListViewItem i in list.Items) if (decimal.TryParse(i.SubItems[4].Text.Replace(",", ""), out decimal n)) sum += n; tong.Text = sum.ToString("N0"); }
    void Clear() { soTK.Clear(); ten.Clear(); diaChi.Clear(); soTien.Clear(); }
}
