using System.Text.RegularExpressions;
namespace Lab04c;
public class Bai2DangKyForm : Form
{
    TextBox txtTenDangNhap = new(); TextBox txtEmail = new(); TextBox txtMatKhau = new(); TextBox txtXacNhan = new(); ErrorProvider errorProvider = new();
    public Bai2DangKyForm()
    {
        Text = "Đăng ký tài khoản"; StartPosition = FormStartPosition.CenterParent;
        TableLayoutPanel content = UiLayout.CreateContent(this, new Size(620, 460), new Size(500, 400)); content.ColumnCount = 1; content.RowCount = 4;
        content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.AutoSize)); content.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); content.Controls.Add(UiLayout.Title("ĐĂNG KÝ TÀI KHOẢN"), 0, 0);
        TableLayoutPanel fields = UiLayout.Fields(4, 38); UiLayout.AddField(fields, 0, "Tên đăng nhập (*)", txtTenDangNhap); UiLayout.AddField(fields, 1, "Địa chỉ email (*)", txtEmail); UiLayout.AddField(fields, 2, "Mật khẩu (*)", txtMatKhau); UiLayout.AddField(fields, 3, "Xác nhận mật khẩu", txtXacNhan); content.Controls.Add(fields, 0, 1);
        txtMatKhau.PasswordChar = '*'; txtXacNhan.PasswordChar = '*'; FlowLayoutPanel actions = UiLayout.ButtonBar(); Button btnDangKy = UiLayout.Button("Đăng ký"); btnDangKy.Click += DangKy; actions.Controls.Add(btnDangKy); content.Controls.Add(actions, 0, 2); AcceptButton = btnDangKy;
        txtEmail.Leave += (_, _) => KiemTraEmail(); FormClosing += (_, e) => { if (MessageBox.Show("Bạn có muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) e.Cancel = true; };
    }
    private bool KiemTraEmail() { const string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; if (!Regex.IsMatch(txtEmail.Text.Trim(), pattern)) { errorProvider.SetError(txtEmail, "Email không đúng định dạng."); return false; } errorProvider.SetError(txtEmail, ""); return true; }
    private void DangKy(object? sender, EventArgs e) { errorProvider.Clear(); bool hopLe = true; if (string.IsNullOrWhiteSpace(txtTenDangNhap.Text)) { errorProvider.SetError(txtTenDangNhap, "Không được để trống."); hopLe = false; } if (string.IsNullOrWhiteSpace(txtEmail.Text)) { errorProvider.SetError(txtEmail, "Không được để trống."); hopLe = false; } else if (!KiemTraEmail()) hopLe = false; if (string.IsNullOrWhiteSpace(txtMatKhau.Text)) { errorProvider.SetError(txtMatKhau, "Không được để trống."); hopLe = false; } if (txtMatKhau.Text != txtXacNhan.Text) { errorProvider.SetError(txtXacNhan, "Mật khẩu xác nhận không khớp."); hopLe = false; } if (!hopLe) return; MessageBox.Show($"Tên đăng nhập: {txtTenDangNhap.Text}\nEmail: {txtEmail.Text}\nMật khẩu: {txtMatKhau.Text}\nXác nhận mật khẩu: {txtXacNhan.Text}", "Thông tin đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Information); }
}
