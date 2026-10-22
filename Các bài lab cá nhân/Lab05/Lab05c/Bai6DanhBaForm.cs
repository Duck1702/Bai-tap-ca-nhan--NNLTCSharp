namespace Lab05c;

public class Bai6DanhBaForm : Form
{
    readonly TreeView tree = new();
    readonly TextBox first = new(); readonly TextBox last = new();
    public Bai6DanhBaForm()
    {
        Text = "BTVN 3 - Danh bạ";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(560, 450);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), ColumnCount = 2 };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48)); root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52));
        tree.Dock = DockStyle.Fill; root.Controls.Add(tree, 0, 0);
        var right = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, Padding = new Padding(10, 0, 0, 0) };
        right.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); right.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var info = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(10) };
        var form = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 1 };
        form.Controls.Add(Ui.Row("First Name", first, 80)); form.Controls.Add(Ui.Row("Last Name", last, 80));
        var add = Ui.Btn("Add", (_, _) => Add(), 75); add.Anchor = AnchorStyles.Right; form.Controls.Add(add); info.Controls.Add(form); right.Controls.Add(info);
        var exit = Ui.Btn("Exit", (_, _) => Close(), 75); exit.Anchor = AnchorStyles.Right; right.Controls.Add(exit);
        root.Controls.Add(right, 1, 0); Controls.Add(root);
        Load += (_, _) => { tree.Nodes.Clear(); for (char c = 'A'; c <= 'Z'; c++) tree.Nodes.Add(c.ToString()); };
    }
    void Add()
    {
        string f = first.Text.Trim(), l = last.Text.Trim(); if (f == "" || l == "") { MessageBox.Show("Nhập đầy đủ First Name và Last Name."); return; }
        char c = char.ToUpper(f[0]); if (c < 'A' || c > 'Z') { MessageBox.Show("First Name phải bắt đầu bằng A-Z."); return; }
        int i = c - 'A'; tree.Nodes[i].Nodes.Add($"{f}, {l}"); tree.Nodes[i].Expand(); first.Clear(); last.Clear(); first.Focus();
    }
}
