namespace Lab05d;

public class Bai3QuanLyHocVienForm : Form
{
    readonly ListBox a = new() { SelectionMode = SelectionMode.MultiExtended };
    readonly ListBox b = new() { SelectionMode = SelectionMode.MultiExtended };
    public Bai3QuanLyHocVienForm()
    {
        Text = "Nâng cao - Quản lý học viên";
        StartPosition = FormStartPosition.CenterParent; AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(460, 340); MinimumSize = Size; FormBorderStyle = FormBorderStyle.FixedSingle; MaximizeBox = false;
        var menu = new MenuStrip(); var update = new ToolStripMenuItem("Cập nhật"); var exit = new ToolStripMenuItem("Kết thúc");
        var add = new ToolStripMenuItem("Nhập học viên mới"); var toB = new ToolStripMenuItem("Chuyển sang lớp B"); var toA = new ToolStripMenuItem("Chuyển sang lớp A"); var del = new ToolStripMenuItem("Xóa học viên");
        update.DropDownItems.AddRange(new ToolStripItem[] { add, toB, toA, del }); menu.Items.AddRange(new ToolStripItem[] { update, exit }); Controls.Add(menu);
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        var ga = new GroupBox { Text = "Lớp A", Dock = DockStyle.Fill, Padding = new Padding(8) }; var gb = new GroupBox { Text = "Lớp B", Dock = DockStyle.Fill, Padding = new Padding(8) };
        a.Dock = b.Dock = DockStyle.Fill; ga.Controls.Add(a); gb.Controls.Add(b); root.Controls.Add(ga, 0, 0); root.Controls.Add(gb, 1, 0); Controls.Add(root);
        a.Items.AddRange(new object[] { "Trần Hùng", "Nguyễn Hùng", "Lâm Tử" }); b.Items.AddRange(new object[] { "Tuấn Hùng", "Hồng Ngọc", "Anh Minh", "Bảo Lâm" });
        toB.Click += (_, _) => Chuyen(a, b); toA.Click += (_, _) => Chuyen(b, a); del.Click += (_, _) => Xoa();
        add.Click += (_, _) => { using var f = new NhapHocVienForm(); if (f.ShowDialog(this) == DialogResult.OK) (f.Lop == "Lớp A" ? a : b).Items.Add(f.HoTen); };
        exit.Click += (_, _) => { if (MessageBox.Show("Bạn có chắc muốn kết thúc?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes) Close(); };
    }
    static void Chuyen(ListBox from, ListBox to) { if (from.SelectedItems.Count == 0) { MessageBox.Show("Phải chọn học viên trước."); return; } foreach (object x in from.SelectedItems.Cast<object>().ToList()) { to.Items.Add(x); from.Items.Remove(x); } }
    void Xoa() { if (a.SelectedItems.Count == 0 && b.SelectedItems.Count == 0) { MessageBox.Show("Phải chọn học viên cần xóa."); return; } foreach (object x in a.SelectedItems.Cast<object>().ToList()) a.Items.Remove(x); foreach (object x in b.SelectedItems.Cast<object>().ToList()) b.Items.Remove(x); }
}
