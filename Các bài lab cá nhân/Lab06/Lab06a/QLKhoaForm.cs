using Lab06a.Database;
using Microsoft.Data.SqlClient;

namespace Lab06a;

public class QLKhoaForm : Form
{
    readonly TextBox txtMaKhoa = new();
    readonly TextBox txtTenKhoa = new();

    public QLKhoaForm()
    {
        Text = "Quản lý Khoa";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(390, 240);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(25), RowCount = 3 };
        root.Controls.Add(Ui.Row("Mã khoa", txtMaKhoa));
        root.Controls.Add(Ui.Row("Tên khoa", txtTenKhoa));

        var them = new Button { Text = "Thêm" };
        var xoa = new Button { Text = "Xóa" };
        var sua = new Button { Text = "Sửa" };
        root.Controls.Add(Ui.Buttons(them, xoa, sua));
        Controls.Add(root);

        them.Click += async (_, _) => await Them();
        xoa.Click += async (_, _) => await Xoa();
        sua.Click += async (_, _) => await Sua();
    }

    async Task Them()
    {
        if (txtMaKhoa.Text.Trim() == "" || txtTenKhoa.Text.Trim() == "")
        {
            MessageBox.Show("Mã khoa và Tên khoa không được để trống.");
            return;
        }

        const string sql = "INSERT INTO Khoa(MaKhoa,TenKhoa) VALUES(@Ma,@Ten)";
        await Execute(sql, true);
    }

    async Task Xoa()
    {
        if (txtMaKhoa.Text.Trim() == "" || txtTenKhoa.Text.Trim() != "")
        {
            MessageBox.Show("Khi xóa chỉ nhập Mã khoa.");
            return;
        }
        await Execute("DELETE FROM Khoa WHERE MaKhoa=@Ma", false);
    }

    async Task Sua()
    {
        if (txtMaKhoa.Text.Trim() == "" || txtTenKhoa.Text.Trim() == "")
        {
            MessageBox.Show("Mã khoa và Tên khoa không được để trống.");
            return;
        }
        await Execute("UPDATE Khoa SET TenKhoa=@Ten WHERE MaKhoa=@Ma", true);
    }

    async Task Execute(string sql, bool useTen)
    {
        try
        {
            await using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Ma", txtMaKhoa.Text.Trim());
            if (useTen) cmd.Parameters.AddWithValue("@Ten", txtTenKhoa.Text.Trim());

            int n = await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n > 0 ? "Thành công" : "Không tìm thấy dữ liệu.");
        }
        catch (Exception ex) { MessageBox.Show("Thất bại:\n" + ex.Message); }
    }
}
