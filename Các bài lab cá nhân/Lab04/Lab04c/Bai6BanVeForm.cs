namespace Lab04c;

public class Bai6BanVeForm : Form
{
    readonly List<Button> ghe = new(); TextBox txtThanhTien = new();
    public Bai6BanVeForm()
    {
        Text = "Bán vé rạp chiếu phim"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(680, 650), new Size(560, 570)); content.ColumnCount = 1; content.RowCount = 6;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        content.Controls.Add(UiLayout.Title("MÀN ẢNH"), 0, 0);
        TableLayoutPanel seats = new() { Anchor = AnchorStyles.None, AutoSize = true, ColumnCount = 5, RowCount = 3, Margin = new Padding(0, 8, 0, 12) };
        for (int i = 0; i < 5; i++) seats.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); for (int i = 0; i < 3; i++) seats.RowStyles.Add(new RowStyle(SizeType.AutoSize)); TaoGhe(seats); content.Controls.Add(seats, 0, 1);
        TableLayoutPanel total = UiLayout.Fields(1, 35); txtThanhTien.ReadOnly = true; txtThanhTien.Text = "0"; UiLayout.AddField(total, 0, "Thành tiền:", txtThanhTien); content.Controls.Add(total, 0, 2);
        FlowLayoutPanel actions = UiLayout.ButtonBar(); Button btnChon = UiLayout.Button("Chọn"), btnHuy = UiLayout.Button("Hủy bỏ"), btnKetThuc = UiLayout.Button("Kết thúc"); actions.Controls.AddRange(new Control[] { btnChon, btnHuy, btnKetThuc }); content.Controls.Add(actions, 0, 3);
        btnChon.Click += (_, _) => ThanhToan(); btnHuy.Click += (_, _) => { foreach (Button btn in ghe) if (btn.BackColor == Color.Blue) btn.BackColor = Color.White; txtThanhTien.Text = "0"; }; btnKetThuc.Click += (_, _) => Close();
    }
    private void TaoGhe(TableLayoutPanel seats)
    {
        int so = 1;
        for (int dong = 0; dong < 3; dong++) for (int cot = 0; cot < 5; cot++) { Button btn = new() { Text = so.ToString(), Size = new Size(58, 52), Margin = new Padding(5), BackColor = Color.White, Tag = so }; btn.Click += ChonGhe; ghe.Add(btn); seats.Controls.Add(btn, cot, dong); so++; }
    }
    private void ChonGhe(object? sender, EventArgs e) { Button btn = (Button)sender!; if (btn.BackColor == Color.Gold) { MessageBox.Show("Ghế này đã được bán."); return; } btn.BackColor = btn.BackColor == Color.Blue ? Color.White : Color.Blue; }
    private void ThanhToan() { int tongTien = 0; foreach (Button btn in ghe) { if (btn.BackColor != Color.Blue) continue; int soGhe = (int)btn.Tag!; if (soGhe <= 5) tongTien += 1000; else if (soGhe <= 10) tongTien += 1500; else tongTien += 2000; btn.BackColor = Color.Gold; } txtThanhTien.Text = tongTien.ToString("N0") + " đ"; }
}
