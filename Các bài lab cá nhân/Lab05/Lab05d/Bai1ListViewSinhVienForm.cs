namespace Lab05d;

public class Bai1ListViewSinhVienForm : Form
{
    readonly TextBox hoTen = new(); readonly TextBox ma = new();
    readonly RadioButton nam = new() { Text = "Nam", Checked = true, AutoSize = true };
    readonly RadioButton nu = new() { Text = "Nữ", AutoSize = true };
    readonly CheckBox anh = new() { Text = "Anh", AutoSize = true };
    readonly CheckBox phap = new() { Text = "Pháp", AutoSize = true };
    readonly CheckBox hoa = new() { Text = "Hoa", AutoSize = true };
    readonly ComboBox danToc = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly ListView list = new();

    public Bai1ListViewSinhVienForm()
    {
        Text = "Tại lớp 1 - ListView sinh viên";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(540, 550);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), RowCount = 7 };
        for (int i = 0; i < 5; i++) root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(Ui.Row("Họ và Tên", hoTen, 110));
        root.Controls.Add(Ui.Row("Mã sinh viên", ma, 110));
        var gt = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(4, 4, 4, 4) };
        gt.Controls.Add(new Label { Text = "Giới tính", Width = 110, Margin = new Padding(3, 7, 3, 3) }); gt.Controls.Add(nam); gt.Controls.Add(nu); root.Controls.Add(gt);
        var nn = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(4, 4, 4, 4) };
        nn.Controls.Add(new Label { Text = "Ngoại ngữ", Width = 110, Margin = new Padding(3, 7, 3, 3) }); nn.Controls.Add(anh); nn.Controls.Add(phap); nn.Controls.Add(hoa); root.Controls.Add(nn);
        root.Controls.Add(Ui.Row("Dân tộc", danToc, 110));

        list.Dock = DockStyle.Fill; list.View = View.Details; list.FullRowSelect = true; list.MultiSelect = true; list.GridLines = true;
        list.Columns.Add("Họ tên", 110); list.Columns.Add("Mã sinh viên", 95); list.Columns.Add("Giới tính", 70); list.Columns.Add("Ngoại ngữ", 110); list.Columns.Add("Dân tộc", 80);
        var ctx = new ContextMenuStrip(); var xoaCtx = new ToolStripMenuItem("Xóa"); ctx.Items.Add(xoaCtx); list.ContextMenuStrip = ctx;
        root.Controls.Add(list);
        var actions = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        actions.Controls.Add(Ui.Btn("Thêm", (_, _) => Them(), 80)); actions.Controls.Add(Ui.Btn("Xóa", (_, _) => Xoa(), 80)); actions.Controls.Add(Ui.Btn("Sửa", (_, _) => Sua(), 80)); root.Controls.Add(actions);
        Controls.Add(root);
        Load += (_, _) => { danToc.Items.AddRange(new object[] { "Kinh", "Hoa", "K’Me", "H’Mong", "Khác" }); danToc.SelectedIndex = 0; };
        xoaCtx.Click += (_, _) => Xoa(); list.SelectedIndexChanged += (_, _) => HienThi();
        FormClosing += (_, e) => { if (MessageBox.Show("Bạn có chắc muốn đóng Form?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Cancel = true; };
    }
    string NgoaiNgu() => string.Join(", ", new[] { anh.Checked ? "Anh" : "", phap.Checked ? "Pháp" : "", hoa.Checked ? "Hoa" : "" }.Where(x => x != ""));
    void Them()
    {
        string m = ma.Text.Trim(), t = hoTen.Text.Trim(); if (m == "" || t == "") { MessageBox.Show("Họ tên và mã sinh viên không được trống."); return; }
        if (list.Items.Cast<ListViewItem>().Any(i => i.SubItems[1].Text.Equals(m, StringComparison.OrdinalIgnoreCase))) { MessageBox.Show("Mã sinh viên đã tồn tại."); return; }
        list.Items.Add(new ListViewItem(new[] { t, m, nam.Checked ? "Nam" : "Nữ", NgoaiNgu(), danToc.Text })); ResetInput();
    }
    void Xoa() { if (list.SelectedItems.Count == 0) { MessageBox.Show("Chọn sinh viên cần xóa."); return; } foreach (ListViewItem i in list.SelectedItems.Cast<ListViewItem>().ToList()) i.Remove(); ResetInput(); }
    void Sua() { if (list.SelectedItems.Count != 1) { MessageBox.Show("Chọn đúng một sinh viên cần sửa."); return; } var i = list.SelectedItems[0]; i.SubItems[0].Text = hoTen.Text.Trim(); i.SubItems[2].Text = nam.Checked ? "Nam" : "Nữ"; i.SubItems[3].Text = NgoaiNgu(); i.SubItems[4].Text = danToc.Text; }
    void HienThi()
    {
        if (list.SelectedItems.Count != 1) { ma.ReadOnly = false; return; }
        var i = list.SelectedItems[0]; hoTen.Text = i.SubItems[0].Text; ma.Text = i.SubItems[1].Text; ma.ReadOnly = true; nam.Checked = i.SubItems[2].Text == "Nam"; nu.Checked = !nam.Checked;
        string s = i.SubItems[3].Text; anh.Checked = s.Contains("Anh"); phap.Checked = s.Contains("Pháp"); hoa.Checked = s.Contains("Hoa"); danToc.Text = i.SubItems[4].Text;
    }
    void ResetInput() { hoTen.Clear(); ma.Clear(); ma.ReadOnly = false; nam.Checked = true; anh.Checked = phap.Checked = hoa.Checked = false; if (danToc.Items.Count > 0) danToc.SelectedIndex = 0; }
}
