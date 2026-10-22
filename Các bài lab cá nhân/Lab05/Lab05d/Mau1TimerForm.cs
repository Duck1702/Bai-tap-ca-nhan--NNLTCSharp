namespace Lab05d;

public class Mau1TimerForm : Form
{
    readonly Label clock = new();
    readonly System.Windows.Forms.Timer timer = new();

    public Mau1TimerForm()
    {
        Text = "Mẫu 1 - Timer";
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(480, 280);
        MinimumSize = new Size(400, 230);

        clock.Font = new Font("Segoe UI", 22, FontStyle.Bold);
        clock.ForeColor = Color.Red;
        clock.AutoSize = true;
        clock.Anchor = AnchorStyles.None;

        var root = Ui.Root();
        root.Controls.Add(clock);
        Controls.Add(root);

        timer.Interval = 1000;
        timer.Tick += (_, _) => clock.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
        Load += (_, _) =>
        {
            clock.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
            timer.Start();
        };
        FormClosed += (_, _) => timer.Stop();
    }
}
