namespace Lab06b;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "LAB 06B - TƯƠNG TÁC CƠ SỞ DỮ LIỆU (2)";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(640, 620);

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(30), RowCount = 12 };
        root.Controls.Add(new Label
        {
            Text = "THỰC HÀNH 06B",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Anchor = AnchorStyles.None,
            Margin = new Padding(0,0,0,12)
        });

        Add(root,"Bài mẫu 1 - Quản lý Khoa (kiểm tra trùng)",()=>new QLKhoaForm().ShowDialog(this));
        Add(root,"Bài mẫu 2 - Quản lý Lớp (ComboBox Khoa)",()=>new QLLopForm().ShowDialog(this));
        Add(root,"Tại lớp 1 - Quản lý Sinh viên (ComboBox Lớp)",()=>new QLSinhVienForm().ShowDialog(this));
        Add(root,"Tại lớp 2 - Cập nhật Điểm (ComboBox)",()=>new QLDiemForm().ShowDialog(this));
        Add(root,"Nâng cao 1 - Danh sách Lớp",()=>new DanhSachLopForm().ShowDialog(this));
        Add(root,"Nâng cao 2 - Danh sách Điểm theo môn",()=>new DanhSachDiemForm().ShowDialog(this));
        Add(root,"BTVN - Quản lý cửa hàng",()=>new CuaHangMainForm().ShowDialog(this));

        var exit=new Button{Text="Thoát",Width=120,Height=36,Anchor=AnchorStyles.None};
        exit.Click+=(_,_)=>Close();
        root.Controls.Add(exit);
        Controls.Add(root);
    }

    static void Add(TableLayoutPanel root,string text,Action action)
    {
        var b=new Button{Text=text,Width=410,Height=38,Anchor=AnchorStyles.None};
        b.Click+=(_,_)=>action();
        root.Controls.Add(b);
    }
}
