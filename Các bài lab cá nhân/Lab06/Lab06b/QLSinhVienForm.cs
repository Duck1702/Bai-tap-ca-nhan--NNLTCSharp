using Lab06b.Database;
using Microsoft.Data.SqlClient;
using System.Globalization;

namespace Lab06b;

public class QLSinhVienForm : Form
{
    readonly ComboBox cboLop=new(){DropDownStyle=ComboBoxStyle.DropDownList};
    readonly TextBox txtMaSV=new();
    readonly TextBox txtHoTen=new();
    readonly MaskedTextBox txtNgaySinh=new("00/00/0000");

    public QLSinhVienForm()
    {
        Text="Quản lý Sinh viên";
        StartPosition=FormStartPosition.CenterParent;
        Size=new Size(440,330);
        FormBorderStyle=FormBorderStyle.FixedDialog;MaximizeBox=false;

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(25),RowCount=5};
        root.Controls.Add(Ui.Row("Lớp",cboLop));
        root.Controls.Add(Ui.Row("Mã sinh viên",txtMaSV));
        root.Controls.Add(Ui.Row("Họ tên",txtHoTen));
        root.Controls.Add(Ui.Row("Ngày sinh",txtNgaySinh));
        var them=new Button{Text="Thêm"};var xoa=new Button{Text="Xóa"};var sua=new Button{Text="Sửa"};
        root.Controls.Add(Ui.Buttons(them,xoa,sua));Controls.Add(root);

        Load+=async(_,_)=>await LoadLop();
        them.Click+=async(_,_)=>await Them();
        xoa.Click+=async(_,_)=>await Xoa();
        sua.Click+=async(_,_)=>await Sua();
    }

    async Task LoadLop()
    {
        cboLop.Items.Clear();
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaLop FROM Lop ORDER BY MaLop",conn);
        await using var rd=await cmd.ExecuteReaderAsync();
        while(await rd.ReadAsync())cboLop.Items.Add(rd["MaLop"].ToString());
        if(cboLop.Items.Count>0)cboLop.SelectedIndex=0;
    }

    bool TryDate(out DateTime d)=>DateTime.TryParseExact(txtNgaySinh.Text,"dd/MM/yyyy",
        CultureInfo.InvariantCulture,DateTimeStyles.None,out d);

    async Task<bool> SinhVienChuaTonTai(string ma)
    {
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaSinhVien FROM SinhVien WHERE MaSinhVien=@Ma",conn);
        cmd.Parameters.AddWithValue("@Ma",ma);
        await using var rd=await cmd.ExecuteReaderAsync();
        return !rd.HasRows;
    }

    async Task Them()
    {
        if(cboLop.SelectedItem==null||txtMaSV.Text.Trim()==""||txtHoTen.Text.Trim()==""||!TryDate(out var d))
        {MessageBox.Show("Tất cả thông tin phải được nhập.");return;}

        int age=DateTime.Today.Year-d.Year;
        if(age<17){MessageBox.Show("Sinh viên phải từ 17 tuổi trở lên (tính theo năm sinh).");return;}
        if(!await SinhVienChuaTonTai(txtMaSV.Text.Trim())){MessageBox.Show("Trùng mã sinh viên.");return;}

        const string sql="INSERT INTO SinhVien(MaSinhVien,HoTen,NgaySinh,MaLop) VALUES(@SV,@Ten,@Ngay,@Lop)";
        try{
            await using var conn=Db.SinhVienConn();await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@SV",txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@Ten",txtHoTen.Text.Trim());
            cmd.Parameters.AddWithValue("@Ngay",d.Date);
            cmd.Parameters.AddWithValue("@Lop",cboLop.SelectedItem.ToString()!);
            await cmd.ExecuteNonQueryAsync();MessageBox.Show("Thành công");
        }catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }

    async Task Xoa()
    {
        if(txtMaSV.Text.Trim()==""){MessageBox.Show("Mã sinh viên không được để trống.");return;}
        await ExecSimple("DELETE FROM SinhVien WHERE MaSinhVien=@SV");
    }

    async Task Sua()
    {
        if(cboLop.SelectedItem==null||txtMaSV.Text.Trim()==""||txtHoTen.Text.Trim()==""||!TryDate(out var d))
        {MessageBox.Show("Tất cả thông tin phải được nhập.");return;}

        const string sql="UPDATE SinhVien SET HoTen=@Ten,NgaySinh=@Ngay,MaLop=@Lop WHERE MaSinhVien=@SV";
        try{
            await using var conn=Db.SinhVienConn();await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@SV",txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@Ten",txtHoTen.Text.Trim());
            cmd.Parameters.AddWithValue("@Ngay",d.Date);
            cmd.Parameters.AddWithValue("@Lop",cboLop.SelectedItem.ToString()!);
            int n=await cmd.ExecuteNonQueryAsync();MessageBox.Show(n>0?"Thành công":"Không tìm thấy sinh viên.");
        }catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }

    async Task ExecSimple(string sql)
    {
        try{
            await using var conn=Db.SinhVienConn();await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@SV",txtMaSV.Text.Trim());
            int n=await cmd.ExecuteNonQueryAsync();MessageBox.Show(n>0?"Thành công":"Không tìm thấy sinh viên.");
        }catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
