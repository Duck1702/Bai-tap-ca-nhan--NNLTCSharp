namespace Lab05c;

public class Bai2QuanLySinhVienForm : Form
{
    readonly TreeView tree = new();
    readonly ComboBox cboLop = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly TextBox txtMa = new(); readonly TextBox txtTen = new(); readonly TextBox txtDiaChi = new();
    readonly CheckBox chkThemLop = new() { Text = "Thêm lớp", AutoSize = true };
    readonly GroupBox grpThemLop = new() { Text = "Thông tin lớp" };
    readonly TextBox txtTenLop = new();
    record SinhVien(string Ma, string Ten, string DiaChi);

    public Bai2QuanLySinhVienForm()
    {
        Text = "Tại lớp 2 - Quản lý sinh viên";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(620, 480);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        tree.Dock = DockStyle.Fill;
        tree.HideSelection = false;
        root.Controls.Add(tree, 0, 0);

        var right = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(10, 0, 0, 0) };
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        right.Controls.Add(Ui.Row("Chọn Lớp", cboLop, 80));

        var info = new GroupBox { Text = "Thông tin sinh viên", Dock = DockStyle.Top, Padding = new Padding(10) };
        var infoLayout = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1, RowCount = 4 };
        infoLayout.Controls.Add(Ui.Row("Mã SV:", txtMa, 70));
        infoLayout.Controls.Add(Ui.Row("Họ Tên:", txtTen, 70));
        infoLayout.Controls.Add(Ui.Row("Địa Chỉ:", txtDiaChi, 70));
        var svButtons = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        svButtons.Controls.Add(Ui.Btn("Cập nhật", (_, _) => ThemSV(), 90));
        svButtons.Controls.Add(Ui.Btn("Xóa", (_, _) => XoaSV(), 75));
        infoLayout.Controls.Add(svButtons);
        info.Controls.Add(infoLayout);
        right.Controls.Add(info);
        right.Controls.Add(chkThemLop);

        grpThemLop.Dock = DockStyle.Top;
        grpThemLop.Padding = new Padding(10);
        var lopPanel = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2 };
        lopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        lopPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        lopPanel.Controls.Add(new Label { Text = "Tên lớp", AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 7, 8, 3) }, 0, 0);
        txtTenLop.Dock = DockStyle.Fill;
        lopPanel.Controls.Add(txtTenLop, 1, 0);
        var addLop = Ui.Btn("Thêm lớp", (_, _) => ThemLop(), 90);
        addLop.Anchor = AnchorStyles.Right;
        lopPanel.Controls.Add(addLop, 1, 1);
        grpThemLop.Controls.Add(lopPanel);
        right.Controls.Add(grpThemLop);
        root.Controls.Add(right, 1, 0);
        Controls.Add(root);

        Load += (_, _) => KhoiTao();
        chkThemLop.CheckedChanged += (_, _) => grpThemLop.Visible = chkThemLop.Checked;
        tree.AfterSelect += (_, e) => { if (e.Node != null) HienThi(e.Node); };
    }

    void KhoiTao()
    {
        tree.Nodes.Clear(); cboLop.Items.Clear(); var root = tree.Nodes.Add("Danh sách lớp");
        foreach (string lop in new[] { "05DHTH1", "05DHTH2", "05DHTH3", "05DHTH4" }) { root.Nodes.Add(lop); cboLop.Items.Add(lop); }
        root.Expand(); cboLop.SelectedIndex = 0; grpThemLop.Visible = false;
    }
    void ThemLop()
    {
        string lop = txtTenLop.Text.Trim(); if (lop == "") { MessageBox.Show("Tên lớp không được trống."); return; }
        var root = tree.Nodes[0]; if (root.Nodes.Cast<TreeNode>().Any(n => n.Text.Equals(lop, StringComparison.OrdinalIgnoreCase))) { MessageBox.Show("Tên lớp đã tồn tại."); return; }
        root.Nodes.Add(lop); cboLop.Items.Add(lop); cboLop.SelectedItem = lop; txtTenLop.Clear(); root.Expand();
    }
    bool MaTonTai(string ma) { foreach (TreeNode lop in tree.Nodes[0].Nodes) foreach (TreeNode sv in lop.Nodes) if (sv.Tag is SinhVien s && s.Ma.Equals(ma, StringComparison.OrdinalIgnoreCase)) return true; return false; }
    void ThemSV()
    {
        string ma = txtMa.Text.Trim(), ten = txtTen.Text.Trim(), dc = txtDiaChi.Text.Trim(); if (ma == "" || ten == "" || dc == "") { MessageBox.Show("Không được để trống thông tin sinh viên."); return; }
        if (MaTonTai(ma)) { MessageBox.Show("Mã sinh viên đã tồn tại."); return; }
        TreeNode? lop = tree.Nodes[0].Nodes.Cast<TreeNode>().FirstOrDefault(n => n.Text == cboLop.Text); if (lop == null) return;
        var sv = new TreeNode($"{ma}, {ten}") { Tag = new SinhVien(ma, ten, dc) }; sv.Nodes.Add("|_ " + dc); lop.Nodes.Add(sv); lop.Expand(); txtMa.Clear(); txtTen.Clear(); txtDiaChi.Clear(); txtMa.Focus();
    }
    void XoaSV() { if (tree.SelectedNode?.Tag is not SinhVien) { MessageBox.Show("Chỉ được xóa node sinh viên."); return; } if (MessageBox.Show("Xóa sinh viên đang chọn?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) tree.SelectedNode.Remove(); }
    void HienThi(TreeNode node) { if (node.Tag is not SinhVien s) return; txtMa.Text = s.Ma; txtTen.Text = s.Ten; txtDiaChi.Text = s.DiaChi; if (node.Parent != null) cboLop.Text = node.Parent.Text; }
}
