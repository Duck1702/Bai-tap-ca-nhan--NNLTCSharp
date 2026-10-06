namespace Lab04c;

internal static class UiLayout
{
    public static TableLayoutPanel CreateContent(Form form, Size clientSize, Size minimumSize)
    {
        form.AutoScaleMode = AutoScaleMode.Dpi;
        form.ClientSize = clientSize;
        form.MinimumSize = minimumSize;

        TableLayoutPanel shell = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(12),
            ColumnCount = 3,
            RowCount = 3
        };
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72));
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 6));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 88));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 6));

        TableLayoutPanel content = new()
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            Padding = new Padding(12),
            ColumnCount = 1,
            RowCount = 1
        };
        content.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        shell.Controls.Add(content, 1, 1);
        form.Controls.Add(shell);
        return content;
    }

    public static Label Title(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Dock = DockStyle.None,
        TextAlign = ContentAlignment.MiddleCenter,
        Font = new Font("Segoe UI", 18, FontStyle.Bold),
        Margin = new Padding(0, 0, 0, 16)
    };

    public static Label Label(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Anchor = AnchorStyles.Left,
        Margin = new Padding(0, 5, 10, 5)
    };

    public static TextBox TextBox(bool readOnly = false, bool multiline = false) => new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = readOnly,
        Multiline = multiline,
        Margin = new Padding(0, 3, 0, 3),
        MinimumSize = new Size(0, 28)
    };

    public static Button Button(string text) => new()
    {
        Text = text,
        AutoSize = false,
        Size = new Size(112, 38),
        MinimumSize = new Size(100, 36),
        Margin = new Padding(6)
    };

    public static FlowLayoutPanel ButtonBar() => new()
    {
        Dock = DockStyle.Fill,
        AutoSize = true,
        WrapContents = true,
        FlowDirection = FlowDirection.LeftToRight,
        Anchor = AnchorStyles.None,
        Margin = new Padding(0, 12, 0, 0)
    };

    public static TableLayoutPanel Fields(int rows, int labelPercent = 38) => new TableLayoutPanel()
    {
        Dock = DockStyle.Fill,
        AutoSize = true,
        ColumnCount = 2,
        RowCount = rows,
        Margin = Padding.Empty
    }.WithColumns(labelPercent);

    private static TableLayoutPanel WithColumns(this TableLayoutPanel table, int labelPercent)
    {
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, labelPercent));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100 - labelPercent));
        for (int i = 0; i < table.RowCount; i++)
            table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        return table;
    }

    public static void AddField(TableLayoutPanel table, int row, string label, Control field)
    {
        table.Controls.Add(Label(label), 0, row);
        field.Dock = DockStyle.Fill;
        table.Controls.Add(field, 1, row);
    }
}
