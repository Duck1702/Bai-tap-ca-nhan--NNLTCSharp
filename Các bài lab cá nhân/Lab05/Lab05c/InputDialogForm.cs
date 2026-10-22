namespace Lab05c;

public class InputDialogForm : Form
{
    readonly TextBox txt = new();
    public string Value => txt.Text.Trim();

    public InputDialogForm(string value)
    {
        Text = "Sửa họ tên";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(460, 190);
        MinimumSize = new Size(420, 180);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var root = Ui.Root();
        root.Controls.Add(new Label { Text = "Nhập họ tên mới:", AutoSize = true });
        txt.Text = value;
        txt.Dock = DockStyle.Top;
        root.Controls.Add(txt);

        var buttons = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.Right };
        var ok = new Button { Text = "OK", Width = 90, DialogResult = DialogResult.OK };
        var cancel = new Button { Text = "Hủy", Width = 90, DialogResult = DialogResult.Cancel };
        buttons.Controls.AddRange(new Control[] { ok, cancel });
        root.Controls.Add(buttons);
        Controls.Add(root);
        AcceptButton = ok;
        CancelButton = cancel;
    }
}
