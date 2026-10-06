namespace Lab04d;

public class Bai1PhuongTrinhForm : Form
{
    RadioButton rdoBac1 = new(); RadioButton rdoBac2 = new(); TextBox txtA = new(); TextBox txtB = new(); TextBox txtC = new(); TextBox txtKetQua = new(); Button btnGiai = new();
    public Bai1PhuongTrinhForm()
    {
        Text = "Giải phương trình bậc 1 - 2"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(700, 620), new Size(560, 520)); content.ColumnCount = 1; content.RowCount = 5;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Label title = UiLayout.Title("GIẢI PHƯƠNG TRÌNH"); title.ForeColor = Color.Red; content.Controls.Add(title, 0, 0);
        GroupBox group = new() { Text = "Bạn vui lòng chọn", Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(16), Margin = new Padding(0, 0, 0, 10) };
        FlowLayoutPanel choices = new() { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown }; rdoBac1.Text = "Phương trình bậc nhất"; rdoBac1.AutoSize = true; rdoBac2.Text = "Phương trình bậc hai"; rdoBac2.AutoSize = true; choices.Controls.AddRange(new Control[] { rdoBac1, rdoBac2 }); group.Controls.Add(choices); content.Controls.Add(group, 0, 1);
        TableLayoutPanel work = new() { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2, RowCount = 1 }; work.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75)); work.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
        TableLayoutPanel fields = UiLayout.Fields(4, 25); UiLayout.AddField(fields, 0, "Nhập a:", txtA); UiLayout.AddField(fields, 1, "Nhập b:", txtB); UiLayout.AddField(fields, 2, "Nhập c:", txtC); txtKetQua.ReadOnly = true; txtKetQua.Multiline = true; txtKetQua.MinimumSize = new Size(0, 62); UiLayout.AddField(fields, 3, "Kết quả:", txtKetQua); work.Controls.Add(fields, 0, 0);
        FlowLayoutPanel actions = new() { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown, Anchor = AnchorStyles.Top }; btnGiai = UiLayout.Button("Giải"); Button btnThoat = UiLayout.Button("Thoát"); actions.Controls.AddRange(new Control[] { btnGiai, btnThoat }); work.Controls.Add(actions, 1, 0); content.Controls.Add(work, 0, 2);
        btnGiai.Enabled = false; txtC.Enabled = false;
        rdoBac1.CheckedChanged += (_, _) => { if (rdoBac1.Checked) { txtC.Enabled = false; txtC.Clear(); CapNhatNutGiai(); } };
        rdoBac2.CheckedChanged += (_, _) => { if (rdoBac2.Checked) { txtC.Enabled = true; CapNhatNutGiai(); } };
        txtA.TextChanged += (_, _) => CapNhatNutGiai(); txtB.TextChanged += (_, _) => CapNhatNutGiai(); txtC.TextChanged += (_, _) => CapNhatNutGiai(); btnGiai.Click += (_, _) => Giai(); btnThoat.Click += (_, _) => Close();
        FormClosing += (_, e) => { if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Cancel = true; };
    }
    private void CapNhatNutGiai() { if (!rdoBac1.Checked && !rdoBac2.Checked) { btnGiai.Enabled = false; return; } if (string.IsNullOrWhiteSpace(txtA.Text) || string.IsNullOrWhiteSpace(txtB.Text)) { btnGiai.Enabled = false; return; } if (rdoBac2.Checked && string.IsNullOrWhiteSpace(txtC.Text)) { btnGiai.Enabled = false; return; } btnGiai.Enabled = true; }
    private void Giai() { if (!double.TryParse(txtA.Text, out double a) || !double.TryParse(txtB.Text, out double b)) { MessageBox.Show("a và b phải là số."); return; } if (rdoBac1.Checked) { PhuongTrinhBacHai pt = new(a, b); txtKetQua.Text = pt.GiaiBacNhat(); btnGiai.Enabled = false; return; } if (!double.TryParse(txtC.Text, out double c)) { MessageBox.Show("c phải là số."); return; } PhuongTrinhBacHai pt2 = new(a, b, c); txtKetQua.Text = pt2.GiaiBacHai(); btnGiai.Enabled = false; }
}
