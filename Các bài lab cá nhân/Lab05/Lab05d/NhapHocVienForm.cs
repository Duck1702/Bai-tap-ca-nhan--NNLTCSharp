namespace Lab05d;

public class NhapHocVienForm : Form
{
    readonly TextBox ten = new();
    readonly ComboBox lop = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    public string HoTen => ten.Text.Trim(); public string Lop => lop.Text;
    public NhapHocVienForm()
    {
        Text = "Nhập học viên mới"; StartPosition = FormStartPosition.CenterParent; AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(350, 220); MinimumSize = Size; FormBorderStyle = FormBorderStyle.FixedDialog; MaximizeBox = false; MinimizeBox = false;
        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), RowCount = 3 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.Controls.Add(Ui.Row("Họ và Tên", ten, 80)); lop.Items.AddRange(new object[] { "Lớp A", "Lớp B" }); lop.SelectedIndex = 0; root.Controls.Add(Ui.Row("Lớp", lop, 80));
        var a = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None }; a.Controls.Add(Ui.Btn("Cập nhật", (_, _) => { if (HoTen == "") { MessageBox.Show("Phải nhập họ tên."); ten.Focus(); return; } DialogResult = DialogResult.OK; Close(); }, 90)); a.Controls.Add(Ui.Btn("Trở về", (_, _) => { DialogResult = DialogResult.Cancel; Close(); }, 80)); root.Controls.Add(a); Controls.Add(root);
    }
}
