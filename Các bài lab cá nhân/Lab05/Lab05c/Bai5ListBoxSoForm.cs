namespace Lab05c;

public class Bai5ListBoxSoForm : Form
{
    readonly TextBox txt = new();
    readonly ListBox lst = new() { SelectionMode = SelectionMode.MultiExtended };

    public Bai5ListBoxSoForm()
    {
        Text = "BTVN 2 - Xử lý ListBox số";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(480, 520);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 3 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(new Label { Text = "LISTBOX", ForeColor = Color.DarkOrange, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Anchor = AnchorStyles.None, Margin = new Padding(3, 0, 3, 8) });

        var main = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48)); main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
        var left = new GroupBox { Text = "Listbox", Dock = DockStyle.Fill, Padding = new Padding(10) };
        var leftLayout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3 };
        leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize)); leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        txt.Dock = DockStyle.Fill; leftLayout.Controls.Add(txt);
        var input = Ui.Btn("Nhập", (_, _) => Nhap(), 75); input.Anchor = AnchorStyles.None; leftLayout.Controls.Add(input);
        lst.Dock = DockStyle.Fill; leftLayout.Controls.Add(lst); left.Controls.Add(leftLayout); main.Controls.Add(left, 0, 0);
        var right = new GroupBox { Text = "Xử lý Listbox", Dock = DockStyle.Fill, Padding = new Padding(8) };
        var a = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false };
        void Add(string text, EventHandler action) { var b = Ui.Btn(text, action, 205); b.Height = 31; b.Margin = new Padding(2, 1, 2, 4); a.Controls.Add(b); }
        Add("Tổng các phần tử trong List", (_, _) => MessageBox.Show($"Tổng = {lst.Items.Cast<int>().Sum()}"));
        Add("Xóa Phần tử đầu và cuối", (_, _) => XoaDauCuoi());
        Add("Xóa Phần tử đang chọn", (_, _) => XoaChon());
        Add("Tăng mỗi phần tử lên 2", (_, _) => BienDoi(x => x + 2));
        Add("Thay bằng bình phương", (_, _) => BienDoi(x => x * x));
        Add("Chọn số chẵn", (_, _) => Chon(x => x % 2 == 0));
        Add("Chọn số lẻ", (_, _) => Chon(x => x % 2 != 0));
        right.Controls.Add(a); main.Controls.Add(right, 1, 0); root.Controls.Add(main);
        var end = Ui.Btn("KẾT THÚC", (_, _) => Close(), 160); end.Anchor = AnchorStyles.None; root.Controls.Add(end);
        Controls.Add(root);
    }
    void Nhap() { if (!int.TryParse(txt.Text, out int n) || n < 0) { MessageBox.Show("Nhập số tự nhiên."); return; } lst.Items.Add(n); txt.Clear(); txt.Focus(); }
    void XoaDauCuoi() { if (lst.Items.Count == 0) return; if (lst.Items.Count == 1) { lst.Items.RemoveAt(0); return; } lst.Items.RemoveAt(lst.Items.Count - 1); lst.Items.RemoveAt(0); }
    void XoaChon() { foreach (int i in lst.SelectedIndices.Cast<int>().OrderByDescending(x => x).ToArray()) lst.Items.RemoveAt(i); }
    void BienDoi(Func<int, int> f) { for (int i = 0; i < lst.Items.Count; i++) lst.Items[i] = f((int)lst.Items[i]!); }
    void Chon(Func<int, bool> f) { lst.ClearSelected(); for (int i = 0; i < lst.Items.Count; i++) if (f((int)lst.Items[i]!)) lst.SetSelected(i, true); }
}
