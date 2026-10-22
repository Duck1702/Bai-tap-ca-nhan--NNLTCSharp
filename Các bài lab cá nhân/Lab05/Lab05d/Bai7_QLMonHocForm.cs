using Microsoft.Data.SqlClient;
using Lab05d.Database;

namespace Lab05d;

public class Bai7_QLMonHocForm : Form
{
    readonly TextBox ma = new();
    readonly TextBox ten = new();

    public Bai7_QLMonHocForm()
    {
        Text = "Quản lý môn học";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(360, 220);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = Ui.Root();
        root.RowCount = 3;
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(DbUi.Row("Mã môn học:", ma));
        root.Controls.Add(DbUi.Row("Tên môn học:", ten));

        var add = new Button { Text = "Thêm" };
        var del = new Button { Text = "Xóa" };
        var edit = new Button { Text = "Sửa" };
        root.Controls.Add(DbUi.Buttons(add, del, edit));
        Controls.Add(root);

        add.Click += async (_, _) => await Them();
        del.Click += async (_, _) => await Xoa();
        edit.Click += async (_, _) => await Sua();
    }

    async Task Run(string sql, string m, string? t, string msg)
    {
        try
        {
            await using var conn = Db.CreateConnection();
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Ma", m);
            if (t != null) cmd.Parameters.AddWithValue("@Ten", t);
            int n = await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n > 0 ? msg : "Không tìm thấy dữ liệu.");
        }
        catch (Exception ex) { MessageBox.Show("Có lỗi:\n" + ex.Message); }
    }

    async Task Them()
    {
        string m = ma.Text.Trim(), t = ten.Text.Trim();
        if (m == "" || t == "") { MessageBox.Show("Mã môn học và tên môn học không được để trống."); return; }
        await Run("INSERT INTO MonHoc(MaMonHoc,TenMonHoc) VALUES(@Ma,@Ten)", m, t, "Thêm môn học thành công.");
    }

    async Task Xoa()
    {
        string m = ma.Text.Trim();
        if (m == "") { MessageBox.Show("Mã môn học không được để trống."); return; }
        if (ten.Text.Trim() != "") { MessageBox.Show("Khi xóa không nhập tên môn học."); return; }
        await Run("DELETE FROM MonHoc WHERE MaMonHoc=@Ma", m, null, "Xóa môn học thành công.");
    }

    async Task Sua()
    {
        string m = ma.Text.Trim(), t = ten.Text.Trim();
        if (m == "" || t == "") { MessageBox.Show("Mã môn học và tên môn học không được để trống."); return; }
        await Run("UPDATE MonHoc SET TenMonHoc=@Ten WHERE MaMonHoc=@Ma", m, t, "Sửa môn học thành công.");
    }
}
