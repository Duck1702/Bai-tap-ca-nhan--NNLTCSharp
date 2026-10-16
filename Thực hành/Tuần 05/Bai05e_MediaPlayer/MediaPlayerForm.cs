namespace Bai05e_MediaPlayer;

/// <summary>
/// Bài 1 tài liệu 05e: thực hành nhúng Windows Media Player bằng COM ActiveX.
/// Máy Windows phải bật Windows Media Player / Media Features.
/// Không cài thư viện ngoài; khi ActiveX không có thì báo lỗi và có lựa chọn
/// mở file bằng ứng dụng media mặc định của Windows (chế độ dự phòng).
/// </summary>
public sealed class MediaPlayerForm : Form
{
    private readonly MenuStrip menu = new() { Dock = DockStyle.Top };
    private readonly ToolStrip thanhCongCu = new() { Dock = DockStyle.Top, GripStyle = ToolStripGripStyle.Hidden };
    private readonly StatusStrip status = new();
    private readonly ToolStripStatusLabel lblNgayGio = new() { Spring = true, TextAlign = ContentAlignment.MiddleLeft };
    private readonly ToolStripStatusLabel lblFile = new() { Text = "Chưa mở tệp" };
    private readonly System.Windows.Forms.Timer timer = new() { Interval = 1000 };
    private readonly Panel khuVucMedia = new() { Dock = DockStyle.Fill, BackColor = Color.Black };
    private readonly WindowsMediaPlayerHost wmp = new() { Dock = DockStyle.Fill };
    private bool coPlayer;
    private string? fileDangMo;
    private string chuChay = "       CHÀO MỪNG ĐẾN VỚI CHƯƠNG TRÌNH WINDOWS MEDIA — BÀI THỰC HÀNH TUẦN 05       ";

    public MediaPlayerForm()
    {
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(1020, 720);
        MinimumSize = new Size(760, 500);
        Text = chuChay;
        MainMenuStrip = menu;
        TaoGiaoDien();
        Shown += (_, _) => KhoiTaoActiveX();
        timer.Tick += (_, _) =>
        {
            lblNgayGio.Text = "Hôm nay là ngày " + DateTime.Now.ToString("dd/MM/yyyy") +
                " — Bây giờ là " + DateTime.Now.ToString("HH:mm:ss");
            chuChay = chuChay[1..] + chuChay[0];
            Text = chuChay;
        };
        timer.Start();
        FormClosed += (_, _) => timer.Dispose();
    }

    private void TaoGiaoDien()
    {
        var fileMenu = new ToolStripMenuItem("&File");
        var mo = new ToolStripMenuItem("&Open...");
        mo.ShortcutKeys = Keys.Control | Keys.O;
        mo.Click += (_, _) => MoFile();
        var thoat = new ToolStripMenuItem("E&xit");
        thoat.Click += (_, _) => Close();
        fileMenu.DropDownItems.Add(mo);
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add(thoat);
        menu.Items.Add(fileMenu);
        thanhCongCu.Items.Add(Nut("Open", (_, _) => MoFile()));
        thanhCongCu.Items.Add(new ToolStripSeparator());
        thanhCongCu.Items.Add(Nut("▶ Play", (_, _) => DieuKhien("play")));
        thanhCongCu.Items.Add(Nut("Ⅱ Pause", (_, _) => DieuKhien("pause")));
        thanhCongCu.Items.Add(Nut("■ Stop", (_, _) => DieuKhien("stop")));
        status.Items.Add(lblNgayGio);
        status.Items.Add(lblFile);
        Controls.Add(khuVucMedia);
        Controls.Add(thanhCongCu);
        Controls.Add(menu);
        Controls.Add(status);
    }

    private static ToolStripButton Nut(string text, EventHandler click)
    {
        var b = new ToolStripButton(text) { DisplayStyle = ToolStripItemDisplayStyle.Text,
            Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        b.Click += click;
        return b;
    }

    private void KhoiTaoActiveX()
    {
        try
        {
            khuVucMedia.Controls.Add(wmp);
            wmp.CreateControl();
            // Kiểm tra giao diện COM thực sự có thể truy cập.
            wmp.KiemTra();
            coPlayer = true;
            lblFile.Text = "Windows Media Player ActiveX đã sẵn sàng";
        }
        catch (Exception ex)
        {
            coPlayer = false;
            khuVucMedia.Controls.Clear();
            khuVucMedia.Controls.Add(new Label
            {
                Dock = DockStyle.Fill, ForeColor = Color.White, BackColor = Color.FromArgb(25, 32, 42),
                TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 13),
                Text = "Máy chưa khởi tạo được Windows Media Player ActiveX.\n" +
                       "Hãy bật Media Features / Windows Media Player trong Windows.\n" +
                       "Vẫn có thể chọn Open để mở file bằng trình phát mặc định.\n\n" + ex.Message
            });
            lblFile.Text = "Chế độ dự phòng: mở bằng ứng dụng hệ thống";
        }
    }

    private void MoFile()
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Chọn file video / audio",
            Filter = "Tất cả media|*.mpeg;*.mpg;*.avi;*.wav;*.mid;*.midi;*.mp4;*.mp3;*.wmv;*.wma|" +
                     "MPEG|*.mpeg;*.mpg|AVI|*.avi|WAV|*.wav|MIDI|*.mid;*.midi|MP4|*.mp4|" +
                     "MP3|*.mp3|Tất cả file|*.*"
        };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        fileDangMo = dlg.FileName;
        lblFile.Text = Path.GetFileName(fileDangMo);
        try
        {
            if (coPlayer) wmp.Mo(fileDangMo);
            else MoNgoai(fileDangMo);
        }
        catch (Exception ex)
        {
            var ok = MessageBox.Show(
                "Không mở được media bằng ActiveX: " + ex.Message +
                "\nBạn có muốn mở bằng trình phát mặc định của Windows không?",
                "Media Player", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (ok == DialogResult.Yes) MoNgoai(fileDangMo);
        }
    }

    private static void MoNgoai(string file)
    {
        try
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(file)
            {
                UseShellExecute = true
            });
        }
        catch (Exception ex) { MessageBox.Show("Không mở được media: " + ex.Message); }
    }

    private void DieuKhien(string action)
    {
        if (!coPlayer)
        {
            MessageBox.Show("Các nút Play/Pause/Stop chỉ hoạt động với Windows Media Player ActiveX.");
            return;
        }
        if (fileDangMo == null) { MessageBox.Show("Bạn chưa mở file media."); return; }
        try { wmp.DieuKhien(action); }
        catch (Exception ex) { MessageBox.Show("Không thể điều khiển media: " + ex.Message); }
    }
}

/// <summary>
/// Wrapper COM không cần Interop assemblies do Designer tạo, phù hợp viết trong VS Code.
/// CLSID: Windows Media Player ActiveX. Chỉ hoạt động trên hệ điều hành Windows
/// có WMP control. Sử dụng dynamic cho IDispatch, qua GetOcx() của AxHost.
/// </summary>
public sealed class WindowsMediaPlayerHost : AxHost
{
    public WindowsMediaPlayerHost() : base("{6BF52A52-394A-11d3-B153-00C04F79FAA6}") { }

    public void KiemTra()
    {
        if (GetOcx() == null) throw new InvalidOperationException("Không tìm thấy COM object WMP.");
    }

    public void Mo(string file)
    {
        dynamic player = GetOcx() ?? throw new InvalidOperationException("WMP chưa khởi tạo.");
        player.URL = file;
        player.controls.play();
    }

    public void DieuKhien(string command)
    {
        dynamic player = GetOcx() ?? throw new InvalidOperationException("WMP chưa khởi tạo.");
        switch (command)
        {
            case "play": player.controls.play(); break;
            case "pause": player.controls.pause(); break;
            case "stop": player.controls.stop(); break;
        }
    }
}
