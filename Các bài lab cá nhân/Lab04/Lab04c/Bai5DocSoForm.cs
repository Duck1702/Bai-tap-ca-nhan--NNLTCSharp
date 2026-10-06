namespace Lab04c;

public class Bai5DocSoForm : Form
{
    TextBox txtSo = new(); Label lblKetQua = new();
    static readonly string[] ChuSo = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
    public Bai5DocSoForm()
    {
        Text = "Đọc số thành chữ"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(660, 440), new Size(520, 360)); content.ColumnCount = 1; content.RowCount = 5;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); content.Controls.Add(UiLayout.Title("ĐỌC SỐ THÀNH CHỮ"), 0, 0);
        TableLayoutPanel field = UiLayout.Fields(1, 35); UiLayout.AddField(field, 0, "Nhập số (1 - 999):", txtSo); content.Controls.Add(field, 0, 1);
        FlowLayoutPanel actions = UiLayout.ButtonBar(); Button btnThucHien = UiLayout.Button("Thực hiện"), btnXoa = UiLayout.Button("Xóa"), btnThoat = UiLayout.Button("Thoát"); actions.Controls.AddRange(new Control[] { btnThucHien, btnXoa, btnThoat }); content.Controls.Add(actions, 0, 2);
        lblKetQua.AutoSize = false; lblKetQua.Dock = DockStyle.Fill; lblKetQua.MinimumSize = new Size(0, 56); lblKetQua.Font = new Font("Segoe UI", 11, FontStyle.Bold); lblKetQua.TextAlign = ContentAlignment.MiddleLeft; lblKetQua.Margin = new Padding(0, 14, 0, 0); content.Controls.Add(lblKetQua, 0, 3);
        btnThucHien.Click += (_, _) => { if (!int.TryParse(txtSo.Text, out int n) || n < 1 || n > 999) { MessageBox.Show("Chỉ nhập số nguyên từ 1 đến 999."); return; } lblKetQua.Text = "Kết quả: " + DocSo(n); };
        btnXoa.Click += (_, _) => { txtSo.Clear(); lblKetQua.Text = ""; txtSo.Focus(); }; btnThoat.Click += (_, _) => Close();
    }
    private string DocSo(int n) { if (n < 10) return ChuSo[n]; if (n < 100) return DocHaiChuSo(n); int tram = n / 100, conLai = n % 100; string kq = ChuSo[tram] + " trăm"; if (conLai == 0) return kq; if (conLai < 10) return kq + " lẻ " + ChuSo[conLai]; return kq + " " + DocHaiChuSo(conLai); }
    private string DocHaiChuSo(int n) { int chuc = n / 10, donVi = n % 10; if (chuc == 1) { string kq = "mười"; if (donVi == 0) return kq; if (donVi == 5) return kq + " lăm"; return kq + " " + ChuSo[donVi]; } string ketQua = ChuSo[chuc] + " mươi"; if (donVi == 0) return ketQua; if (donVi == 1) return ketQua + " mốt"; if (donVi == 5) return ketQua + " lăm"; return ketQua + " " + ChuSo[donVi]; }
}
