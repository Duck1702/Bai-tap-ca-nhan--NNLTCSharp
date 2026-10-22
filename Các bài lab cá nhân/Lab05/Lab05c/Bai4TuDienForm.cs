namespace Lab05c;

public class Bai4TuDienForm : Form
{
    readonly Dictionary<string, string> av = new(StringComparer.OrdinalIgnoreCase)
    {
        ["student"] = "Sinh viên, học sinh", ["teacher"] = "Giáo viên", ["worker"] = "Công nhân", ["hat"] = "Cái mũ", ["head"] = "Đầu", ["mouse"] = "Con chuột", ["cat"] = "Con mèo", ["dog"] = "Con chó", ["snake"] = "Con rắn", ["lamb"] = "Cừu non", ["loan"] = "Khoản vay", ["frog"] = "Con ếch", ["sheep"] = "Con cừu"
    };
    readonly TabControl tabs = new() { Dock = DockStyle.Fill };
    readonly ComboBox cboAnh = new() { AutoCompleteMode = AutoCompleteMode.SuggestAppend, AutoCompleteSource = AutoCompleteSource.ListItems };
    readonly ListBox lstAnh = new();
    readonly TextBox nghiaViet = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };
    readonly ComboBox cboViet = new() { AutoCompleteMode = AutoCompleteMode.SuggestAppend, AutoCompleteSource = AutoCompleteSource.ListItems };
    readonly ListBox lstViet = new();
    readonly TextBox nghiaAnh = new() { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical };

    public Bai4TuDienForm()
    {
        Text = "Từ điển Anh - Việt / Việt - Anh";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(430, 510);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tabs.TabPages.Add(CreatePage("ANH - VIỆT", "Tiếng Anh", cboAnh, lstAnh, "Tiếng Việt", nghiaViet));
        tabs.TabPages.Add(CreatePage("VIỆT - ANH", "Tiếng Việt", cboViet, lstViet, "Tiếng Anh", nghiaAnh));
        root.Controls.Add(tabs);
        var exit = Ui.Btn("Thoát", (_, _) => Close(), 80);
        exit.Anchor = AnchorStyles.Right;
        root.Controls.Add(exit);
        Controls.Add(root);

        Load += (_, _) => { Nap(cboAnh, lstAnh, nghiaViet, av); Nap(cboViet, lstViet, nghiaAnh, VietAnh()); };
        Wire(cboAnh, lstAnh, nghiaViet, () => av);
        Wire(cboViet, lstViet, nghiaAnh, VietAnh);
    }

    static TabPage CreatePage(string tabText, string leftLabel, ComboBox cbo, ListBox lst, string rightLabel, TextBox meaning)
    {
        var page = new TabPage(tabText);
        var main = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), ColumnCount = 2, RowCount = 2 };
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
        main.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        var left = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3 };
        left.RowStyles.Add(new RowStyle(SizeType.AutoSize)); left.RowStyles.Add(new RowStyle(SizeType.AutoSize)); left.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        left.Controls.Add(new Label { Text = leftLabel, AutoSize = true, Margin = new Padding(0, 0, 0, 4) });
        cbo.Dock = DockStyle.Fill; left.Controls.Add(cbo);
        lst.Dock = DockStyle.Fill; left.Controls.Add(lst);
        main.Controls.Add(left, 0, 0); main.SetRowSpan(left, 2);
        main.Controls.Add(new Label { Text = rightLabel, AutoSize = true, Margin = new Padding(8, 0, 0, 4) }, 1, 0);
        meaning.Dock = DockStyle.Fill; meaning.Margin = new Padding(8, 0, 0, 0); main.Controls.Add(meaning, 1, 1);
        page.Controls.Add(main);
        return page;
    }

    Dictionary<string, string> VietAnh() => av.ToDictionary(x => x.Value, x => x.Key, StringComparer.OrdinalIgnoreCase);
    static void Nap(ComboBox cbo, ListBox lst, TextBox nghia, Dictionary<string, string> dict)
    {
        cbo.Items.Clear(); lst.Items.Clear(); nghia.Clear(); cbo.Text = "";
        foreach (string k in dict.Keys.OrderBy(x => x)) { cbo.Items.Add(k); lst.Items.Add(k); }
    }
    static void TimGanNhat(ComboBox cbo, ListBox lst)
    {
        string q = cbo.Text.Trim(); if (q == "") return;
        for (int i = 0; i < lst.Items.Count; i++) { string s = lst.Items[i]!.ToString()!; if (s.StartsWith(q, StringComparison.OrdinalIgnoreCase)) { lst.SelectedIndex = i; lst.TopIndex = i; break; } }
    }
    static void Tra(ComboBox cbo, TextBox nghia, Dictionary<string, string> dict)
    {
        string q = cbo.Text.Trim(); string? k = dict.Keys.FirstOrDefault(x => x.Equals(q, StringComparison.OrdinalIgnoreCase)) ?? dict.Keys.FirstOrDefault(x => x.StartsWith(q, StringComparison.OrdinalIgnoreCase));
        nghia.Text = k == null ? "Không tìm thấy từ." : dict[k];
    }
    static void Wire(ComboBox cbo, ListBox lst, TextBox nghia, Func<Dictionary<string, string>> dict)
    {
        cbo.TextChanged += (_, _) => TimGanNhat(cbo, lst);
        cbo.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { Tra(cbo, nghia, dict()); e.SuppressKeyPress = true; } };
        lst.DoubleClick += (_, _) => { if (lst.SelectedItem != null) { cbo.Text = lst.SelectedItem.ToString(); Tra(cbo, nghia, dict()); } };
    }
}
