namespace Lab05d;

public class Mau3FileForm : Form
{
    readonly TextBox path = new() { ReadOnly = true };
    readonly RichTextBox editor = new();

    public Mau3FileForm()
    {
        Text = "Mẫu 3 - Mở/Lưu file văn bản";
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(800, 600);
        MinimumSize = new Size(640, 460);

        var root = Ui.Root();
        var actions = new FlowLayoutPanel { AutoSize = true };
        actions.Controls.Add(Ui.Btn("Mở File", (_, _) => Mo()));
        actions.Controls.Add(Ui.Btn("Lưu File", (_, _) => Luu()));
        root.Controls.Add(actions);
        root.Controls.Add(Ui.Row("Đường dẫn:", path));
        editor.Dock = DockStyle.Fill;
        root.Controls.Add(editor);

        Controls.Add(root);
    }

    void Mo()
    {
        using var dlg = new OpenFileDialog
        {
            Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        path.Text = dlg.FileName;
        editor.Text = File.ReadAllText(dlg.FileName);
    }

    void Luu()
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Text files (*.txt)|*.txt",
            DefaultExt = "txt"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        File.WriteAllText(dlg.FileName, editor.Text);
        path.Text = dlg.FileName;
        MessageBox.Show("Lưu file thành công.");
    }
}
