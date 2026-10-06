namespace Lab04d;

public class MenuForm : Form
{
    public MenuForm()
    {
        Text = "LAB 04D - WINDOWS FORM BASIC 2";
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(600, 500);
        MinimumSize = new Size(500, 440);

        TableLayoutPanel root = new()
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(16),
            ColumnCount = 1,
            RowCount = 3
        };
        root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        TableLayoutPanel menuBlock = new()
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Anchor = AnchorStyles.None,
            ColumnCount = 1,
            RowCount = 6,
            Margin = Padding.Empty
        };
        menuBlock.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        menuBlock.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        menuBlock.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        for (int i = 0; i < 4; i++)
            menuBlock.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Label title = UiLayout.Title("THỰC HÀNH 04D");
        title.Dock = DockStyle.None;
        title.Anchor = AnchorStyles.None;
        title.Margin = Padding.Empty;
        menuBlock.Controls.Add(title, 0, 0);
        menuBlock.Controls.Add(MenuButton("Bài 1 - Giải phương trình", () => new Bai1PhuongTrinhForm().ShowDialog(this)), 0, 2);
        menuBlock.Controls.Add(MenuButton("Bài 2 - Mảng số nguyên", () => new Bai2MangSoNguyenForm().ShowDialog(this)), 0, 3);
        menuBlock.Controls.Add(MenuButton("Bài nâng cao - Cafe Sinh Viên", () => new Bai3CafeForm().ShowDialog(this)), 0, 4);
        menuBlock.Controls.Add(MenuButton("Bài về nhà - Khách sạn", () => new Bai4KhachSanForm().ShowDialog(this)), 0, 5);

        root.Controls.Add(menuBlock, 0, 1);
        Controls.Add(root);
    }

    private static Button MenuButton(string text, Action action)
    {
        Button button = UiLayout.Button(text);
        button.Size = new Size(350, 42);
        button.Anchor = AnchorStyles.None;
        button.Click += (_, _) => action();
        return button;
    }
}
