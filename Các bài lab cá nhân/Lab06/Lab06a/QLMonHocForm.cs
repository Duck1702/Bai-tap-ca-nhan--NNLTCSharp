using Lab06a.Database;
using Microsoft.Data.SqlClient;

namespace Lab06a;

public class QLMonHocForm : Form
{
    readonly TextBox txtMa = new();
    readonly TextBox txtTen = new();

    public QLMonHocForm()
    {
        Text = "Quản lý môn học";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(390, 240);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(25), RowCount = 3 };
        root.Controls.Add(Ui.Row("Mã mh", txtMa));
        root.Controls.Add(Ui.Row("Tên mh", txtTen));
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
        if (txtMa.Text.Trim()=="" || txtTen.Text.Trim()=="")
        { MessageBox.Show("Mã môn học, Tên môn học không được để trống."); return; }
        await Exec("INSERT INTO MonHoc(MaMonHoc,TenMonHoc) VALUES(@Ma,@Ten)", true);
    }

    async Task Xoa()
    {
        if (txtMa.Text.Trim()=="" || txtTen.Text.Trim()!="")
        { MessageBox.Show("Khi xóa: Mã môn học bắt buộc, Tên môn học để trống."); return; }
        await Exec("DELETE FROM MonHoc WHERE MaMonHoc=@Ma", false);
    }

    async Task Sua()
    {
        if (txtMa.Text.Trim()=="" || txtTen.Text.Trim()=="")
        { MessageBox.Show("Mã môn học, Tên môn học không được để trống."); return; }
        await Exec("UPDATE MonHoc SET TenMonHoc=@Ten WHERE MaMonHoc=@Ma", true);
    }

    async Task Exec(string sql, bool ten)
    {
        try
        {
            await using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Ma", txtMa.Text.Trim());
            if (ten) cmd.Parameters.AddWithValue("@Ten", txtTen.Text.Trim());
            int n = await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n > 0 ? "Thành công" : "Không tìm thấy dữ liệu.");
        }
        catch (Exception ex) { MessageBox.Show("Thất bại:\n" + ex.Message); }
    }
}
