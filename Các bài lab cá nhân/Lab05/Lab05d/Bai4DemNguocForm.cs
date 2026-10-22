namespace Lab05d;

public class Bai4DemNguocForm : Form
{
    readonly Label time = new(); readonly Button start = new(); readonly System.Windows.Forms.Timer timer = new() { Interval = 1000 };
    int remain = 30 * 60;
    public Bai4DemNguocForm()
    {
        Text = "Đồng hồ"; StartPosition = FormStartPosition.CenterParent; AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(240, 180); MinimumSize = Size; FormBorderStyle = FormBorderStyle.FixedSingle; MaximizeBox = false;
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(10), RowCount = 2 };
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 62)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 38));
        time.Text = "30:00"; time.Font = new Font("Segoe UI", 24, FontStyle.Bold); time.AutoSize = true; time.Anchor = AnchorStyles.None; root.Controls.Add(time);
        start.Text = "Bắt đầu"; start.Width = 90; start.Height = 30; start.Anchor = AnchorStyles.None; root.Controls.Add(start); Controls.Add(root);
        timer.Tick += (_, _) => { if (remain <= 0) { timer.Stop(); start.Text = "Bắt đầu"; MessageBox.Show("Hết thời gian!"); return; } remain--; time.Text = $"{remain / 60:00}:{remain % 60:00}"; };
        start.Click += (_, _) => { if (timer.Enabled) { timer.Stop(); start.Text = "Tiếp tục"; } else { if (remain == 0) remain = 30 * 60; timer.Start(); start.Text = "Tạm dừng"; } };
    }
}
