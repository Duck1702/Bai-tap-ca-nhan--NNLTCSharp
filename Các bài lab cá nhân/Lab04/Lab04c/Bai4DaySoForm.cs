namespace Lab04c;

public class Bai4DaySoForm : Form
{
    TextBox txtNhapSo = new(); TextBox txtDaySo = new(); TextBox txtTong = new(); TextBox txtTongChan = new(); TextBox txtTongLe = new(); readonly List<int> daySo = new();
    public Bai4DaySoForm()
    {
        Text = "Dãy số và tính tổng"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(700, 530), new Size(580, 440)); content.ColumnCount = 1; content.RowCount = 5;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); content.Controls.Add(UiLayout.Title("NHẬP DÃY SỐ VÀ TÍNH TỔNG"), 0, 0);
        TableLayoutPanel input = new() { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 3, RowCount = 1 }; input.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25)); input.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55)); input.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20)); input.Controls.Add(UiLayout.Label("Nhập số:"), 0, 0); txtNhapSo.Dock = DockStyle.Fill; input.Controls.Add(txtNhapSo, 1, 0); Button btnNhap = UiLayout.Button("Nhập"); btnNhap.Dock = DockStyle.Fill; input.Controls.Add(btnNhap, 2, 0); content.Controls.Add(input, 0, 1);
        TableLayoutPanel fields = UiLayout.Fields(4, 34); txtDaySo.ReadOnly = true; txtTong.ReadOnly = true; txtTongChan.ReadOnly = true; txtTongLe.ReadOnly = true; UiLayout.AddField(fields, 0, "Dãy vừa nhập:", txtDaySo); UiLayout.AddField(fields, 1, "Tổng các phần tử:", txtTong); UiLayout.AddField(fields, 2, "Tổng chẵn:", txtTongChan); UiLayout.AddField(fields, 3, "Tổng lẻ:", txtTongLe); content.Controls.Add(fields, 0, 2);
        FlowLayoutPanel actions = UiLayout.ButtonBar(); Button btnTiepTuc = UiLayout.Button("Tiếp tục"), btnThoat = UiLayout.Button("Thoát"); actions.Controls.AddRange(new Control[] { btnTiepTuc, btnThoat }); content.Controls.Add(actions, 0, 3);
        btnNhap.Click += (_, _) => ThemSo(); btnTiepTuc.Click += (_, _) => { daySo.Clear(); txtNhapSo.Clear(); txtDaySo.Clear(); txtTong.Clear(); txtTongChan.Clear(); txtTongLe.Clear(); txtNhapSo.Focus(); }; btnThoat.Click += (_, _) => Close(); FormClosing += (_, e) => { if (MessageBox.Show("Bạn có muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No) e.Cancel = true; };
    }
    private void ThemSo() { if (!int.TryParse(txtNhapSo.Text, out int n)) { MessageBox.Show("Vui lòng nhập số nguyên.", "Lỗi"); txtNhapSo.SelectAll(); txtNhapSo.Focus(); return; } daySo.Add(n); txtDaySo.Text = string.Join(" ", daySo); txtTong.Text = daySo.Sum().ToString(); txtTongChan.Text = daySo.Where(x => x % 2 == 0).Sum().ToString(); txtTongLe.Text = daySo.Where(x => x % 2 != 0).Sum().ToString(); txtNhapSo.Clear(); txtNhapSo.Focus(); }
}
