namespace Lab05c;

public class Bai3XuLyChuoiForm : Form
{
    readonly ListBox lst = new() { SelectionMode = SelectionMode.MultiExtended };
    readonly Random rd = new();
    readonly string[] HO = { "Lê", "Nguyễn", "Lý", "Trần", "Lâm", "Hồ", "Lai", "Huỳnh", "La" };
    readonly string[] LOT = { "Quang", "Thành", "Ngọc", "Anh", "Xuân", "Bảo", "Cẩm", "Thị", "Kim", "Thái", "Hồng" };
    readonly string[] TEN = { "Hà", "Danh", "Sơn", "Mai", "Thắng", "Kỳ", "Thành", "Lâm", "Tâm", "Phụng", "Thắm" };

    public Bai3XuLyChuoiForm()
    {
        Text = "Nâng cao - Xử lý chuỗi";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(540, 510);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        var left = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2 };
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var random = Ui.Btn("Nhập tên ngẫu nhiên", (_, _) => Tao50Ten(), 165);
        random.Margin = new Padding(0, 0, 0, 7);
        left.Controls.Add(random);
        lst.Dock = DockStyle.Fill;
        left.Controls.Add(lst);
        root.Controls.Add(left, 0, 0);

        var acts = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, Padding = new Padding(10, 0, 0, 0) };
        void Add(string text, EventHandler action) { var b = Ui.Btn(text, action, 290); b.Height = 33; b.Margin = new Padding(0, 0, 0, 6); acts.Controls.Add(b); }
        Add("Xóa Phần tử đang chọn", (_, _) => XoaDangChon());
        Add("Xóa phần tử có tên là Sơn", (_, _) => XoaTheoTen("Sơn"));
        Add("Xóa phần tử có họ là Lê", (_, _) => XoaTheoHo("Lê"));
        Add("Chuyển PT đang chọn thành chữ HOA", (_, _) => Doi(x => x.ToUpper()));
        Add("Chuyển PT đang chọn thành chữ thường", (_, _) => Doi(x => x.ToLower()));
        Add("Chuyển PT đang chọn thành viết Hoa đầu mỗi từ", (_, _) => Doi(VietHoa));
        Add("Xóa tất cả các Phần tử", (_, _) => lst.Items.Clear());
        root.Controls.Add(acts, 1, 0);
        Controls.Add(root);
        lst.DoubleClick += (_, _) => SuaTen();
    }

    void Tao50Ten() { lst.Items.Clear(); for (int i = 0; i < 50; i++) lst.Items.Add($"{HO[rd.Next(HO.Length)]} {LOT[rd.Next(LOT.Length)]} {TEN[rd.Next(TEN.Length)]}"); }
    void XoaDangChon() { foreach (object x in lst.SelectedItems.Cast<object>().ToList()) lst.Items.Remove(x); }
    void XoaTheoTen(string ten) { for (int i = lst.Items.Count - 1; i >= 0; i--) if (lst.Items[i]!.ToString()!.Split(' ').Last().Equals(ten, StringComparison.OrdinalIgnoreCase)) lst.Items.RemoveAt(i); }
    void XoaTheoHo(string ho) { for (int i = lst.Items.Count - 1; i >= 0; i--) if (lst.Items[i]!.ToString()!.Split(' ').First().Equals(ho, StringComparison.OrdinalIgnoreCase)) lst.Items.RemoveAt(i); }
    void Doi(Func<string, string> f) { foreach (int i in lst.SelectedIndices.Cast<int>().ToArray()) lst.Items[i] = f(lst.Items[i]!.ToString()!); }
    static string VietHoa(string s) => string.Join(" ", s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => char.ToUpper(x[0]) + x[1..].ToLower()));
    void SuaTen() { if (lst.SelectedIndex < 0) return; using var f = new InputDialogForm(lst.SelectedItem!.ToString()!); if (f.ShowDialog(this) == DialogResult.OK && f.Value != "") lst.Items[lst.SelectedIndex] = f.Value; }
}
