using Microsoft.Data.SqlClient;
using Lab05d.Database;
using System.Globalization;

namespace Lab05d;

public class Bai7_QLSinhVienForm : Form
{
    readonly TextBox maLop = new();
    readonly TextBox maSV = new();
    readonly TextBox hoTen = new();
    readonly MaskedTextBox ngaySinh = new() { Mask = "00/00/0000" };

    public Bai7_QLSinhVienForm()
    {
        Text = "Quản lý sinh viên";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(380, 290);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = Ui.Root();
        root.RowCount = 5;
        for (int i = 0; i < 5; i++) root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(DbUi.Row("Mã lớp:", maLop));
        root.Controls.Add(DbUi.Row("Mã sinh viên:", maSV));
        root.Controls.Add(DbUi.Row("Họ tên:", hoTen));
        root.Controls.Add(DbUi.Row("Ngày sinh:", ngaySinh));

        var add = new Button { Text = "Thêm" };
        var del = new Button { Text = "Xóa" };
        var edit = new Button { Text = "Sửa" };
        root.Controls.Add(DbUi.Buttons(add, del, edit));
        Controls.Add(root);

        add.Click += async (_, _) => await Them();
        del.Click += async (_, _) => await Xoa();
        edit.Click += async (_, _) => await Sua();
    }

    bool TryDate(out DateTime d)
        => DateTime.TryParseExact(ngaySinh.Text.Trim(), "dd/MM/yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

    async Task Them()
    {
        string ml = maLop.Text.Trim(), ms = maSV.Text.Trim(), ht = hoTen.Text.Trim();
        if (ml == "" || ms == "" || ht == "" || !TryDate(out DateTime ns))
        { MessageBox.Show("Nhập đủ Mã lớp, Mã SV, Họ tên, Ngày sinh dd/MM/yyyy."); return; }

        const string sql = "INSERT INTO SinhVien(MaSinhVien,HoTen,NgaySinh,MaLop) VALUES(@MaSV,@HoTen,@NgaySinh,@MaLop)";
        try
        {
            await using var conn = Db.CreateConnection(); await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaSV", ms);
            cmd.Parameters.AddWithValue("@HoTen", ht);
            cmd.Parameters.AddWithValue("@NgaySinh", ns.Date);
            cmd.Parameters.AddWithValue("@MaLop", ml);
            await cmd.ExecuteNonQueryAsync();
            MessageBox.Show("Thêm sinh viên thành công.");
        }
        catch (Exception ex) { MessageBox.Show("Có lỗi:\n" + ex.Message); }
    }

    async Task Xoa()
    {
        string ms = maSV.Text.Trim(), ht = hoTen.Text.Trim();
        if (ms == "" || ht == "") { MessageBox.Show("Khi xóa phải nhập Mã SV và Họ tên."); return; }
        if (maLop.Text.Trim() != "" || ngaySinh.Text.Trim() != "")
        { MessageBox.Show("Khi xóa không nhập Mã lớp và Ngày sinh."); return; }

        const string sql = "DELETE FROM SinhVien WHERE MaSinhVien=@MaSV AND HoTen=@HoTen";
        await using var conn = Db.CreateConnection(); await conn.OpenAsync();
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@MaSV", ms);
        cmd.Parameters.AddWithValue("@HoTen", ht);
        int n = await cmd.ExecuteNonQueryAsync();
        MessageBox.Show(n > 0 ? "Xóa thành công." : "Không tìm thấy sinh viên.");
    }

    async Task Sua()
    {
        string ms = maSV.Text.Trim(), ml = maLop.Text.Trim(), ht = hoTen.Text.Trim(), nsText = ngaySinh.Text.Trim();
        if (ms == "") { MessageBox.Show("Mã sinh viên không được để trống."); return; }

        var sets = new List<string>();
        await using var conn = Db.CreateConnection();
        await using var cmd = new SqlCommand { Connection = conn };

        if (ml != "") { sets.Add("MaLop=@MaLop"); cmd.Parameters.AddWithValue("@MaLop", ml); }
        if (ht != "") { sets.Add("HoTen=@HoTen"); cmd.Parameters.AddWithValue("@HoTen", ht); }
        if (nsText != "")
        {
            if (!TryDate(out DateTime ns)) { MessageBox.Show("Ngày sinh phải dạng dd/MM/yyyy."); return; }
            sets.Add("NgaySinh=@NgaySinh"); cmd.Parameters.AddWithValue("@NgaySinh", ns.Date);
        }

        if (sets.Count == 0) { MessageBox.Show("Phải có ít nhất một trường cần sửa."); return; }

        cmd.CommandText = $"UPDATE SinhVien SET {string.Join(",", sets)} WHERE MaSinhVien=@MaSV";
        cmd.Parameters.AddWithValue("@MaSV", ms);
        await conn.OpenAsync();
        int n = await cmd.ExecuteNonQueryAsync();
        MessageBox.Show(n > 0 ? "Sửa thành công." : "Không tìm thấy sinh viên.");
    }
}
