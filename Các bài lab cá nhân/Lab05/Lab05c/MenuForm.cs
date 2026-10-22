namespace Lab05c;

public class MenuForm : Form
{
    public MenuForm()
    {
        Text = "LAB 05C - WINFORMS NÂNG CAO 1";
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(720, 780);
        MinimumSize = new Size(680, 720);

        var root = Ui.Root();
        root.Controls.Add(Ui.Title("THỰC HÀNH 05C - ĐẦY ĐỦ"));

        Add(root, "Mẫu 1 - ListBox chuyển phần tử", () => new Mau1ListBoxForm().ShowDialog());
        Add(root, "Mẫu 2 - ComboBox dân tộc", () => new Mau2ComboBoxForm().ShowDialog());
        Add(root, "Mẫu 3 - TreeView phòng ban - nhân viên", () => new Mau3TreeViewForm().ShowDialog());
        Add(root, "Tại lớp 1 - Xử lý ước số", () => new Bai1UocSoForm().ShowDialog());
        Add(root, "Tại lớp 2 - Quản lý sinh viên", () => new Bai2QuanLySinhVienForm().ShowDialog());
        Add(root, "Nâng cao - Xử lý chuỗi", () => new Bai3XuLyChuoiForm().ShowDialog());
        Add(root, "BTVN 1 - Từ điển Anh - Việt", () => new Bai4TuDienForm().ShowDialog());
        Add(root, "BTVN 2 - Xử lý ListBox số", () => new Bai5ListBoxSoForm().ShowDialog());
        Add(root, "BTVN 3 - Danh bạ TreeView", () => new Bai6DanhBaForm().ShowDialog());

        Controls.Add(root);
    }

    private static void Add(TableLayoutPanel p, string text, Action open)
    {
        var b = new Button { Text = text, Width = 420, Height = 42, Anchor = AnchorStyles.None, Margin = new Padding(6) };
        b.Click += (_, _) => open();
        p.Controls.Add(b);
    }
}
