using Microsoft.Data.SqlClient;
using Lab05d.Database;

namespace Lab05d;

public class Bai7_QLKhoaForm : Form
{
    readonly TextBox ma = new();
    readonly TextBox ten = new();

    public Bai7_QLKhoaForm()
    {
        Text = "Quản lý Khoa";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(400, 230);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = Ui.Root();
        root.RowCount = 3;
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        var exit = new Button { Text = "Thoát" };
        root.Controls.Add(DbUi.Row("Mã khoa:", ma));
        root.Controls.Add(DbUi.Row("Tên khoa:", ten));

        var add = new Button { Text = "Thêm" };
        var del = new Button { Text = "Xóa" };
        var edit = new Button { Text = "Sửa" };
        root.Controls.Add(DbUi.Buttons(add, del, edit, exit));
        Controls.Add(root);

        add.Click += async (_, _) => await Them();
        del.Click += async (_, _) => await Xoa();
        edit.Click += async (_, _) => await Sua();
        exit.Click += (_, _) => Close();
    }

    async Task Run(string sql, string m, string? t, string ok)
    {
        try
        {
            await using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Ma", m);
            if (t != null) cmd.Parameters.AddWithValue("@Ten", t);
            int n = await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n > 0 ? ok : "Không tìm thấy dữ liệu.");
        }
        catch (Exception ex) { MessageBox.Show("Có lỗi:\n" + ex.Message); }
    }

    async Task Them()
    {
        string m = ma.Text.Trim(), t = ten.Text.Trim();
        if (m == "" || t == "") { MessageBox.Show("Mã khoa và tên khoa không được trống."); return; }
        await Run("INSERT INTO Khoa(MaKhoa,TenKhoa) VALUES(@Ma,@Ten)", m, t, "Thêm thành công.");
    }

    async Task Xoa()
    {
        string m = ma.Text.Trim();
        if (m == "") { MessageBox.Show("Nhập mã khoa."); return; }
        await Run("DELETE FROM Khoa WHERE MaKhoa=@Ma", m, null, "Xóa thành công.");
    }

    async Task Sua()
    {
        string m = ma.Text.Trim(), t = ten.Text.Trim();
        if (m == "" || t == "") { MessageBox.Show("Nhập mã khoa và tên khoa."); return; }
        await Run("UPDATE Khoa SET TenKhoa=@Ten WHERE MaKhoa=@Ma", m, t, "Sửa thành công.");
    }
}
