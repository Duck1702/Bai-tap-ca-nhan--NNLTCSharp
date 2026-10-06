namespace Lab04c;

public class Bai3UclnBcnnForm : Form
{
    TextBox txtA = new(); TextBox txtB = new(); TextBox txtUcln = new(); TextBox txtBcnn = new();
    public Bai3UclnBcnnForm()
    {
        Text = "Ước số - Bội số"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(640, 480), new Size(520, 410)); content.ColumnCount = 1; content.RowCount = 4;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        content.Controls.Add(UiLayout.Title("ƯỚC SỐ CHUNG - BỘI SỐ CHUNG"), 0, 0);
        TableLayoutPanel fields = UiLayout.Fields(4, 48); UiLayout.AddField(fields, 0, "Nhập số a:", txtA); UiLayout.AddField(fields, 1, "Nhập số b:", txtB); txtUcln.ReadOnly = true; txtBcnn.ReadOnly = true; UiLayout.AddField(fields, 2, "Ước số chung lớn nhất:", txtUcln); UiLayout.AddField(fields, 3, "Bội số chung nhỏ nhất:", txtBcnn); content.Controls.Add(fields, 0, 1);
        FlowLayoutPanel actions = UiLayout.ButtonBar(); Button btnThucHien = UiLayout.Button("Thực hiện"), btnTiepTuc = UiLayout.Button("Tiếp tục"), btnThoat = UiLayout.Button("Thoát"); actions.Controls.AddRange(new Control[] { btnThucHien, btnTiepTuc, btnThoat }); content.Controls.Add(actions, 0, 2);
        btnThucHien.Click += (_, _) => ThucHien(); btnTiepTuc.Click += (_, _) => { txtA.Clear(); txtB.Clear(); txtUcln.Clear(); txtBcnn.Clear(); txtA.Focus(); }; btnThoat.Click += (_, _) => Close(); FormClosing += XacNhanThoat;
    }
    private void ThucHien() { if (!int.TryParse(txtA.Text, out int a) || !int.TryParse(txtB.Text, out int b)) { MessageBox.Show("a và b phải là số nguyên."); return; } if (a == 0 && b == 0) { MessageBox.Show("Không xác định UCLN khi cả hai số bằng 0."); return; } int ucln = UCLN(a, b); long bcnn = a == 0 || b == 0 ? 0 : Math.Abs((long)a * b) / ucln; txtUcln.Text = ucln.ToString(); txtBcnn.Text = bcnn.ToString(); }
    private int UCLN(int a, int b) { a = Math.Abs(a); b = Math.Abs(b); while (b != 0) { int r = a % b; a = b; b = r; } return a; }
    private void XacNhanThoat(object? sender, FormClosingEventArgs e) { if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Cancel = true; }
}
