using Microsoft.Data.SqlClient;
using Lab05d.Database;

namespace Lab05d;

public class Bai7_QLLopForm : Form
{
    readonly TextBox maKhoa = new();
    readonly TextBox maLop = new();
    readonly TextBox tenLop = new();

    public Bai7_QLLopForm()
    {
        Text = "Quản lý Lớp";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(360, 250);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = Ui.Root();
        root.RowCount = 4;
        for (int i = 0; i < 4; i++) root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(DbUi.Row("Mã khoa:", maKhoa));
        root.Controls.Add(DbUi.Row("Mã lớp:", maLop));
        root.Controls.Add(DbUi.Row("Tên lớp:", tenLop));

        var add = new Button { Text = "Thêm" };
        var del = new Button { Text = "Xóa" };
        var edit = new Button { Text = "Sửa" };
        root.Controls.Add(DbUi.Buttons(add, del, edit));
        Controls.Add(root);

        add.Click += async (_, _) => await Them();
        del.Click += async (_, _) => await Xoa();
        edit.Click += async (_, _) => await Sua();
    }

    async Task Them()
    {
        string mk = maKhoa.Text.Trim(), ml = maLop.Text.Trim(), tl = tenLop.Text.Trim();
        if (mk == "" || ml == "" || tl == "") { MessageBox.Show("Nhập đủ Mã khoa, Mã lớp, Tên lớp."); return; }

        try
        {
            await using var conn = Db.CreateConnection(); await conn.OpenAsync();
            await using var cmd = new SqlCommand("INSERT INTO Lop(MaLop,TenLop,MaKhoa) VALUES(@Lop,@Ten,@Khoa)", conn);
            cmd.Parameters.AddWithValue("@Lop", ml); cmd.Parameters.AddWithValue("@Ten", tl); cmd.Parameters.AddWithValue("@Khoa", mk);
            await cmd.ExecuteNonQueryAsync();
            MessageBox.Show("Thêm lớp thành công.");
        }
        catch (Exception ex) { MessageBox.Show("Có lỗi:\n" + ex.Message); }
    }

    async Task Xoa()
    {
        string ml = maLop.Text.Trim();
        if (ml == "") { MessageBox.Show("Mã lớp không được để trống."); return; }
        if (maKhoa.Text.Trim() != "" || tenLop.Text.Trim() != "") { MessageBox.Show("Khi xóa chỉ nhập Mã lớp."); return; }

        await using var conn = Db.CreateConnection(); await conn.OpenAsync();
        await using var cmd = new SqlCommand("DELETE FROM Lop WHERE MaLop=@Lop", conn);
        cmd.Parameters.AddWithValue("@Lop", ml);
        int n = await cmd.ExecuteNonQueryAsync();
        MessageBox.Show(n > 0 ? "Xóa lớp thành công." : "Không tìm thấy lớp.");
    }

    async Task Sua()
    {
        string ml = maLop.Text.Trim(), mk = maKhoa.Text.Trim(), tl = tenLop.Text.Trim();
        if (ml == "") { MessageBox.Show("Mã lớp không được để trống."); return; }
        if (mk == "" && tl == "") { MessageBox.Show("Phải nhập Mã khoa hoặc Tên lớp cần sửa."); return; }

        var sets = new List<string>();
        await using var conn = Db.CreateConnection();
        await using var cmd = new SqlCommand { Connection = conn };

        if (mk != "") { sets.Add("MaKhoa=@Khoa"); cmd.Parameters.AddWithValue("@Khoa", mk); }
        if (tl != "") { sets.Add("TenLop=@Ten"); cmd.Parameters.AddWithValue("@Ten", tl); }

        cmd.CommandText = $"UPDATE Lop SET {string.Join(",", sets)} WHERE MaLop=@Lop";
        cmd.Parameters.AddWithValue("@Lop", ml);

        try
        {
            await conn.OpenAsync();
            int n = await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n > 0 ? "Sửa lớp thành công." : "Không tìm thấy lớp.");
        }
        catch (Exception ex) { MessageBox.Show("Có lỗi:\n" + ex.Message); }
    }
}
