namespace Lab05d.Database;

public static class DbUi
{
    public static Control Row(string label, Control input)
    {
        var p = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 2, Margin = new Padding(4, 5, 4, 5) };
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        p.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left });
        input.Dock = DockStyle.Fill;
        p.Controls.Add(input);
        return p;
    }

    public static FlowLayoutPanel Buttons(params Button[] buttons)
    {
        var p = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        foreach (var b in buttons) { b.Width = 75; b.Height = 30; }
        p.Controls.AddRange(buttons);
        return p;
    }
}
