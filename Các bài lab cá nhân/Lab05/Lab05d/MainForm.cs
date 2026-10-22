namespace Lab05d;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "LAB 05D - WINFORMS NÂNG CAO 2 + ADO.NET";
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Dpi;
        MinimumSize = new Size(900, 600);
        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;

        var menu = new MenuStrip();

        var mau = new ToolStripMenuItem("Bài mẫu");
        var mau1 = new ToolStripMenuItem("Mẫu 1 - Timer");
        var mau2 = new ToolStripMenuItem("Mẫu 2 - ListView");
        var mau3 = new ToolStripMenuItem("Mẫu 3 - Mở/Lưu file");
        mau.DropDownItems.AddRange(new ToolStripItem[] { mau1, mau2, mau3 });

        var win = new ToolStripMenuItem("Bài tập WinForms");
        var bai1 = new ToolStripMenuItem("Tại lớp 1 - ListView sinh viên");
        var bai2 = new ToolStripMenuItem("Tại lớp 2 - Quản lý tài khoản");
        var nc = new ToolStripMenuItem("Nâng cao - Quản lý học viên");
        var countdown = new ToolStripMenuItem("BTVN 1 - Đếm ngược");
        win.DropDownItems.AddRange(new ToolStripItem[] { bai1, bai2, nc, countdown });

        var ado = new ToolStripMenuItem("Bài 7 - ADO.NET");
        var khoa = new ToolStripMenuItem("Mẫu - Quản lý Khoa");
        var mon = new ToolStripMenuItem("Tại lớp - Quản lý Môn học");
        var sv = new ToolStripMenuItem("Nâng cao 1 - Quản lý Sinh viên");
        var diem = new ToolStripMenuItem("Nâng cao 2 - Quản lý Điểm");
        var lop = new ToolStripMenuItem("BTVN - Quản lý Lớp");
        ado.DropDownItems.AddRange(new ToolStripItem[] { khoa, mon, sv, diem, lop });

        var exit = new ToolStripMenuItem("Kết thúc");

        menu.Items.AddRange(new ToolStripItem[] { mau, win, ado, exit });
        MainMenuStrip = menu;
        Controls.Add(menu);

        mau1.Click += (_, _) => Open(new Mau1TimerForm());
        mau2.Click += (_, _) => Open(new Mau2ListViewForm());
        mau3.Click += (_, _) => Open(new Mau3FileForm());

        bai1.Click += (_, _) => Open(new Bai1ListViewSinhVienForm());
        bai2.Click += (_, _) => Open(new Bai2TaiKhoanForm());
        nc.Click += (_, _) => Open(new Bai3QuanLyHocVienForm());
        countdown.Click += (_, _) => Open(new Bai4DemNguocForm());

        khoa.Click += (_, _) => Open(new Bai7_QLKhoaForm());
        mon.Click += (_, _) => Open(new Bai7_QLMonHocForm());
        sv.Click += (_, _) => Open(new Bai7_QLSinhVienForm());
        diem.Click += (_, _) => Open(new Bai7_QLDiemForm());
        lop.Click += (_, _) => Open(new Bai7_QLLopForm());

        exit.Click += (_, _) =>
        {
            if (MessageBox.Show("Bạn có chắc muốn kết thúc?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Close();
        };
    }

    void Open(Form f)
    {
        foreach (Form child in MdiChildren)
            if (child.GetType() == f.GetType())
            {
                child.Activate();
                f.Dispose();
                return;
            }

        f.MdiParent = this;
        f.Show();
    }
}
