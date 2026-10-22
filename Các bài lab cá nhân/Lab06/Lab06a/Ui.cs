namespace Lab06a;

public static class Ui
{
    public static Control Row(string label, Control input)
    {
        var p = new TableLayoutPanel
        {
            Dock = DockStyle.Top,
            AutoSize = true,
            ColumnCount = 2,
            Margin = new Padding(0, 5, 0, 5)
        };
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130));
        p.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        p.Controls.Add(new Label
        {
            Text = label,
            AutoSize = true,
            Anchor = AnchorStyles.Left
        }, 0, 0);

        input.Dock = DockStyle.Fill;
        p.Controls.Add(input, 1, 0);
        return p;
    }

    public static FlowLayoutPanel Buttons(params Button[] buttons)
    {
        var p = new FlowLayoutPanel
        {
            AutoSize = true,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 10, 0, 0)
        };
        foreach (var b in buttons)
        {
            b.Width = 90;
            b.Height = 34;
        }
        p.Controls.AddRange(buttons);
        return p;
    }
}
