namespace Lab04c;

public class MenuForm : Form
{
    public MenuForm()
    {
        Text = "LAB 04C - WINDOWS FORM BASIC 1";
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(620, 650);
        MinimumSize = new Size(520, 630);

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
            RowCount = 9,
            Margin = Padding.Empty
        };
        menuBlock.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        menuBlock.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        menuBlock.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        for (int i = 0; i < 7; i++)
            menuBlock.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        Label title = UiLayout.Title("THỰC HÀNH 04C");
        title.Anchor = AnchorStyles.None;
        title.Margin = Padding.Empty;
        menuBlock.Controls.Add(title, 0, 0);
        menuBlock.Controls.Add(MenuButton("Bài 1 - Cộng trừ nhân chia", () => new Bai1PhepTinhForm().ShowDialog(this)), 0, 2);
        menuBlock.Controls.Add(MenuButton("Bài 2 - Đăng ký tài khoản", () => new Bai2DangKyForm().ShowDialog(this)), 0, 3);
        menuBlock.Controls.Add(MenuButton("Bài 3 - UCLN và BCNN", () => new Bai3UclnBcnnForm().ShowDialog(this)), 0, 4);
        menuBlock.Controls.Add(MenuButton("Bài 4 - Dãy số và tính tổng", () => new Bai4DaySoForm().ShowDialog(this)), 0, 5);
        menuBlock.Controls.Add(MenuButton("Bài 5 - Đọc số thành chữ", () => new Bai5DocSoForm().ShowDialog(this)), 0, 6);
        menuBlock.Controls.Add(MenuButton("Nâng cao - Bán vé rạp chiếu phim", () => new Bai6BanVeForm().ShowDialog(this)), 0, 7);
        menuBlock.Controls.Add(MenuButton("Bài tập về nhà - Máy tính", () => new Bai7MayTinhForm().ShowDialog(this)), 0, 8);

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
