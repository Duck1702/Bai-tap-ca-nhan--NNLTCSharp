namespace Lab06a;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "LAB 06A - TƯƠNG TÁC CƠ SỞ DỮ LIỆU (1)";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(560, 470);

        var root = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(35),
            RowCount = 7
        };

        root.Controls.Add(new Label
        {
            Text = "THỰC HÀNH 06A",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0, 0, 0, 20)
        });

        Add(root, "Bài mẫu - Quản lý Khoa", () => new QLKhoaForm().ShowDialog(this));
        Add(root, "Bài tại lớp - Quản lý Môn học", () => new QLMonHocForm().ShowDialog(this));
        Add(root, "Nâng cao - Quản lý Sinh viên", () => new QLSinhVienForm().ShowDialog(this));
        Add(root, "Nâng cao - Quản lý Điểm", () => new QLDiemForm().ShowDialog(this));
        Add(root, "BTVN - Quản lý Lớp", () => new QLLopForm().ShowDialog(this));

        var exit = new Button { Text = "Thoát", Width = 120, Height = 36, Anchor = AnchorStyles.None };
        exit.Click += (_, _) => Close();
        root.Controls.Add(exit);

        Controls.Add(root);
    }

    private static void Add(TableLayoutPanel root, string text, Action action)
    {
        var b = new Button { Text = text, Width = 330, Height = 38, Anchor = AnchorStyles.None };
        b.Click += (_, _) => action();
        root.Controls.Add(b);
    }
}
