namespace Lab05c;

public class Mau3TreeViewForm : Form
{
    readonly TreeView tree = new();
    readonly ComboBox cboPhongBan = new();
    readonly TextBox txtPhongBan = new();
    readonly TextBox txtMaSo = new();
    readonly TextBox txtHoTen = new();
    readonly TextBox txtDiaChi = new();

    public Mau3TreeViewForm()
    {
        Text = "Mẫu 3 - Phòng ban - Nhân viên";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(920, 600);
        MinimumSize = new Size(760, 500);

        var root = Ui.Root(2);
        root.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 38);
        root.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 62);

        var left = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 4 };
        left.Controls.Add(new Label { Text = "Phòng ban", Font = new Font("Segoe UI", 11, FontStyle.Bold), AutoSize = true });
        tree.Dock = DockStyle.Fill;
        left.Controls.Add(tree);
        left.Controls.Add(Ui.Row("Phòng ban:", txtPhongBan, 90));

        var pbButtons = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        pbButtons.Controls.Add(Ui.Btn("Thêm phòng ban", (_, _) => ThemPhongBan(), 130));
        pbButtons.Controls.Add(Ui.Btn("Xóa phòng ban", (_, _) => XoaPhongBan(), 130));
        left.Controls.Add(pbButtons);

        root.Controls.Add(left, 0, 0);

        var right = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(20), RowCount = 6 };
        right.Controls.Add(Ui.Title("HỒ SƠ NHÂN VIÊN"));
        right.Controls.Add(Ui.Row("Mã số:", txtMaSo));
        right.Controls.Add(Ui.Row("Họ tên:", txtHoTen));
        right.Controls.Add(Ui.Row("Địa chỉ:", txtDiaChi));
        cboPhongBan.DropDownStyle = ComboBoxStyle.DropDownList;
        right.Controls.Add(Ui.Row("Phòng ban:", cboPhongBan));

        var actions = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        actions.Controls.Add(Ui.Btn("Thêm", (_, _) => ThemNhanVien()));
        actions.Controls.Add(Ui.Btn("Thoát", (_, _) => Close()));
        right.Controls.Add(actions);

        root.Controls.Add(right, 1, 0);
        Controls.Add(root);

        Load += (_, _) => KhoiTao();
        FormClosing += (_, e) =>
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        };
    }

    void KhoiTao()
    {
        string[] ds = { "Giám đốc", "Tổ chức hành chính", "Kế hoạch", "Kế Toán" };
        tree.Nodes.Clear();
        cboPhongBan.Items.Clear();
        foreach (string s in ds)
        {
            tree.Nodes.Add(s);
            cboPhongBan.Items.Add(s);
        }
        cboPhongBan.SelectedIndex = 0;
    }

    bool PhongBanChuaTonTai(string s)
        => !tree.Nodes.Cast<TreeNode>().Any(n => string.Equals(n.Text, s, StringComparison.OrdinalIgnoreCase));

    void ThemPhongBan()
    {
        string s = txtPhongBan.Text.Trim();
        if (s == "") { MessageBox.Show("Tên phòng ban không được trống."); return; }
        if (!PhongBanChuaTonTai(s)) { MessageBox.Show("Phòng ban đã tồn tại!"); return; }

        tree.Nodes.Add(s);
        cboPhongBan.Items.Add(s);
        cboPhongBan.SelectedItem = s;
        txtPhongBan.Clear();
        txtPhongBan.Focus();
    }

    void XoaPhongBan()
    {
        TreeNode? n = tree.SelectedNode;
        if (n == null || n.Parent != null) { MessageBox.Show("Vui lòng chọn node phòng ban."); return; }

        if (MessageBox.Show("Bạn có chắc muốn xóa?", "Thông báo",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

        cboPhongBan.Items.Remove(n.Text);
        tree.Nodes.Remove(n);
        if (cboPhongBan.Items.Count > 0 && cboPhongBan.SelectedIndex < 0) cboPhongBan.SelectedIndex = 0;
    }

    void ThemNhanVien()
    {
        if (txtMaSo.Text.Trim() == "" || txtHoTen.Text.Trim() == "" || txtDiaChi.Text.Trim() == "")
        {
            MessageBox.Show("Vui lòng nhập đủ mã số, họ tên và địa chỉ.");
            return;
        }

        TreeNode? pb = tree.Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == cboPhongBan.Text);
        if (pb == null) return;

        var nv = pb.Nodes.Add($"{txtHoTen.Text.Trim()} ({txtMaSo.Text.Trim()})");
        nv.Nodes.Add(txtDiaChi.Text.Trim());
        tree.ExpandAll();

        txtMaSo.Clear();
        txtHoTen.Clear();
        txtDiaChi.Clear();
        txtMaSo.Focus();
    }
}
