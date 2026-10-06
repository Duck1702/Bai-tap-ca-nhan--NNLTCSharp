namespace Lab04c;

public class Bai1PhepTinhForm : Form
{
    TextBox txtA = new(); TextBox txtB = new(); TextBox txtKetQua = new(); ErrorProvider errorProvider = new();
    public Bai1PhepTinhForm()
    {
        Text = "Cộng trừ nhân chia"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(600, 400), new Size(500, 350));
        content.ColumnCount = 1; content.RowCount = 4;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        content.Controls.Add(UiLayout.Title("CỘNG TRỪ NHÂN CHIA"), 0, 0);
        TableLayoutPanel inputs = new() { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2, RowCount = 1 };
        inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50)); inputs.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        TableLayoutPanel first = UiLayout.Fields(1, 20); UiLayout.AddField(first, 0, "a =", txtA);
        TableLayoutPanel second = UiLayout.Fields(1, 20); UiLayout.AddField(second, 0, "b =", txtB);
        inputs.Controls.Add(first, 0, 0); inputs.Controls.Add(second, 1, 0); content.Controls.Add(inputs, 0, 1);
        TableLayoutPanel result = UiLayout.Fields(1, 20); txtKetQua.ReadOnly = true; UiLayout.AddField(result, 0, "Kết quả:", txtKetQua); content.Controls.Add(result, 0, 2);
        FlowLayoutPanel buttons = UiLayout.ButtonBar(); buttons.Controls.Add(CreateButton("+", (a, b) => a + b)); buttons.Controls.Add(CreateButton("-", (a, b) => a - b)); buttons.Controls.Add(CreateButton("×", (a, b) => a * b));
        Button btnChia = UiLayout.Button("/"); btnChia.Click += (_, _) => { if (!LayDuLieu(out double a, out double b)) return; if (b == 0) { MessageBox.Show("Không thể chia cho 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; } txtKetQua.Text = (a / b).ToString(); }; buttons.Controls.Add(btnChia); content.Controls.Add(buttons, 0, 3);
        txtA.KeyPress += ChiChoNhapSo; txtB.KeyPress += ChiChoNhapSo; FormClosing += XacNhanThoat;
    }
    private Button CreateButton(string text, Func<double, double, double> phepTinh) { Button btn = UiLayout.Button(text); btn.Click += (_, _) => { if (LayDuLieu(out double a, out double b)) txtKetQua.Text = phepTinh(a, b).ToString(); }; return btn; }
    private bool LayDuLieu(out double a, out double b) { errorProvider.Clear(); bool okA = double.TryParse(txtA.Text, out a); bool okB = double.TryParse(txtB.Text, out b); if (!okA) errorProvider.SetError(txtA, "a phải là số."); if (!okB) errorProvider.SetError(txtB, "b phải là số."); if (!okA || !okB) { MessageBox.Show("Vui lòng nhập đúng dữ liệu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning); return false; } return true; }
    private void ChiChoNhapSo(object? sender, KeyPressEventArgs e) { TextBox txt = (TextBox)sender!; if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.') e.Handled = true; if (e.KeyChar == '-' && txt.SelectionStart != 0) e.Handled = true; if (e.KeyChar == '.' && txt.Text.Contains('.')) e.Handled = true; }
    private void XacNhanThoat(object? sender, FormClosingEventArgs e) { if (MessageBox.Show("Bạn có muốn thoát không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Cancel = true; }
}
