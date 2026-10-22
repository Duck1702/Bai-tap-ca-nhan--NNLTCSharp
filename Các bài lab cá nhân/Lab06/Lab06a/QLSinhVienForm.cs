using Lab06a.Database;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace Lab06a;

public class QLSinhVienForm : Form
{
    readonly TextBox txtMaLop = new();
    readonly TextBox txtMaSV = new();
    readonly TextBox txtHoTen = new();
    readonly MaskedTextBox txtNgaySinh = new("00/00/0000");

    public QLSinhVienForm()
    {
        Text = "Quản lý sinh viên";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(420, 320);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(25), RowCount = 5 };
        root.Controls.Add(Ui.Row("Mã lớp", txtMaLop));
        root.Controls.Add(Ui.Row("Mã sv", txtMaSV));
        root.Controls.Add(Ui.Row("Họ tên", txtHoTen));
        root.Controls.Add(Ui.Row("Ngày sinh", txtNgaySinh));

        var them = new Button { Text = "Thêm" };
        var xoa = new Button { Text = "Xóa" };
        var sua = new Button { Text = "Sửa" };
        root.Controls.Add(Ui.Buttons(them, xoa, sua));
        Controls.Add(root);

        them.Click += async (_, _) => await Them();
        xoa.Click += async (_, _) => await Xoa();
        sua.Click += async (_, _) => await Sua();
    }

    bool TryDate(out DateTime d) =>
        DateTime.TryParseExact(txtNgaySinh.Text, "dd/MM/yyyy",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

    async Task Them()
    {
        if (txtMaLop.Text.Trim()=="" || txtMaSV.Text.Trim()=="" || txtHoTen.Text.Trim()=="" || !TryDate(out var d))
        { MessageBox.Show("Phải nhập đầy đủ Mã lớp, Mã sv, Họ tên, Ngày sinh."); return; }

        const string sql = """
            INSERT INTO SinhVien(MaSinhVien,HoTen,NgaySinh,MaLop)
            VALUES(@MaSV,@HoTen,@NgaySinh,@MaLop)
            """;
        try
        {
            await using var conn = Db.CreateConnection(); await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
            cmd.Parameters.AddWithValue("@NgaySinh", d.Date);
            cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
            await cmd.ExecuteNonQueryAsync();
            MessageBox.Show("Thành công");
        }
        catch (Exception ex) { MessageBox.Show("Thất bại:\n"+ex.Message); }
    }

    async Task Xoa()
    {
        if (txtMaSV.Text.Trim()=="" || txtHoTen.Text.Trim()=="" ||
            txtMaLop.Text.Trim()!="" || txtNgaySinh.MaskCompleted)
        {
            MessageBox.Show("Khi xóa: nhập Mã sv + Họ tên; Mã lớp và Ngày sinh để trống.");
            return;
        }

        const string sql = "DELETE FROM SinhVien WHERE MaSinhVien=@MaSV AND HoTen=@HoTen";
        try
        {
            await using var conn = Db.CreateConnection(); await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@HoTen", txtHoTen.Text.Trim());
            int n=await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n>0?"Thành công":"Không tìm thấy sinh viên.");
        }
        catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }

    async Task Sua()
    {
        string maSV=txtMaSV.Text.Trim();
        if (maSV==""){MessageBox.Show("Mã sinh viên không được để trống."); return;}

        var sets=new List<string>();
        await using var conn=Db.CreateConnection();
        await using var cmd=new SqlCommand{Connection=conn};

        if(txtMaLop.Text.Trim()!="")
        { sets.Add("MaLop=@MaLop"); cmd.Parameters.AddWithValue("@MaLop",txtMaLop.Text.Trim()); }
        if(txtHoTen.Text.Trim()!="")
        { sets.Add("HoTen=@HoTen"); cmd.Parameters.AddWithValue("@HoTen",txtHoTen.Text.Trim()); }
        if(txtNgaySinh.MaskCompleted)
        {
            if(!TryDate(out var d)){MessageBox.Show("Ngày sinh không hợp lệ.");return;}
            sets.Add("NgaySinh=@NgaySinh"); cmd.Parameters.AddWithValue("@NgaySinh",d.Date);
        }

        if(sets.Count==0){MessageBox.Show("Phải có ít nhất một trường cần sửa.");return;}

        cmd.CommandText=$"UPDATE SinhVien SET {string.Join(",",sets)} WHERE MaSinhVien=@MaSV";
        cmd.Parameters.AddWithValue("@MaSV",maSV);
        try{await conn.OpenAsync(); int n=await cmd.ExecuteNonQueryAsync(); MessageBox.Show(n>0?"Thành công":"Không tìm thấy sinh viên.");}
        catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
