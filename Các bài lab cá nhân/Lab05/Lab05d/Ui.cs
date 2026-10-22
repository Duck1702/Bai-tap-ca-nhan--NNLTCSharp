namespace Lab05d;

public static class Ui
{
    public static TableLayoutPanel Root(int columns = 1)
    {
        var p = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(16), ColumnCount = columns, AutoScroll = true, GrowStyle = TableLayoutPanelGrowStyle.AddRows };
        for (int i = 0; i < columns; i++) p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / columns));
        return p;
    }
    public static Control Row(string label, Control input, int labelWidth = 150)
    {
        var p = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Margin = new Padding(4, 5, 4, 5) };
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, labelWidth)); p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        p.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(3, 7, 6, 3) }); input.Dock = DockStyle.Fill; p.Controls.Add(input); return p;
    }
    public static Button Btn(string text, EventHandler click, int width = 110)
    {
        var b = new Button { Text = text, Width = width, Height = 38, Margin = new Padding(5) }; b.Click += click; return b;
    }
    public static Label Title(string text) => new() { Text = text, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Anchor = AnchorStyles.None, Margin = new Padding(5, 5, 5, 14) };
}
