namespace Lab04c;

public class Bai7MayTinhForm : Form
{
    TextBox txtManHinh = new(); double soThuNhat; string phepTinh = ""; bool batDauSoMoi = true;
    public Bai7MayTinhForm()
    {
        Text = "Máy tính bỏ túi"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(520, 620), new Size(420, 540)); content.ColumnCount = 1; content.RowCount = 4; content.RowStyles.Add(new RowStyle(SizeType.Percent, 22)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 78));
        txtManHinh.Dock = DockStyle.Fill; txtManHinh.MinimumSize = new Size(0, 56); txtManHinh.Font = new Font("Segoe UI", 20); txtManHinh.TextAlign = HorizontalAlignment.Right; txtManHinh.ReadOnly = true; txtManHinh.Text = "0"; content.Controls.Add(txtManHinh, 0, 1);
        string[,] nut = { { "7", "8", "9", "/" }, { "4", "5", "6", "*" }, { "1", "2", "3", "-" }, { "0", "C", "=", "+" } };
        TableLayoutPanel keys = new() { Dock = DockStyle.Fill, Margin = new Padding(0, 16, 0, 0), ColumnCount = 4, RowCount = 4 };
        for (int i = 0; i < 4; i++) { keys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25)); keys.RowStyles.Add(new RowStyle(SizeType.Percent, 25)); }
        for (int r = 0; r < 4; r++) for (int c = 0; c < 4; c++) { Button btn = new() { Text = nut[r, c], Dock = DockStyle.Fill, Margin = new Padding(4), Font = new Font("Segoe UI", 14) }; btn.Click += XuLyNut; keys.Controls.Add(btn, c, r); }
        content.Controls.Add(keys, 0, 2);
    }
    private void XuLyNut(object? sender, EventArgs e)
    {
        string nut = ((Button)sender!).Text;
        if (char.IsDigit(nut[0])) { if (batDauSoMoi || txtManHinh.Text == "0") { txtManHinh.Text = nut; batDauSoMoi = false; } else txtManHinh.Text += nut; return; }
        if (nut == "C") { txtManHinh.Text = "0"; soThuNhat = 0; phepTinh = ""; batDauSoMoi = true; return; }
        if (nut == "=") { if (phepTinh == "") return; double soThuHai = double.Parse(txtManHinh.Text); double ketQua; switch (phepTinh) { case "+": ketQua = soThuNhat + soThuHai; break; case "-": ketQua = soThuNhat - soThuHai; break; case "*": ketQua = soThuNhat * soThuHai; break; case "/": if (soThuHai == 0) { MessageBox.Show("Không thể chia cho 0."); return; } ketQua = soThuNhat / soThuHai; break; default: return; } txtManHinh.Text = ketQua.ToString(); phepTinh = ""; batDauSoMoi = true; return; }
        soThuNhat = double.Parse(txtManHinh.Text); phepTinh = nut; batDauSoMoi = true;
    }
}
