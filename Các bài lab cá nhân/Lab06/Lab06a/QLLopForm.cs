using Lab06a.Database;
using Microsoft.Data.SqlClient;

namespace Lab06a;

public class QLLopForm : Form
{
    readonly TextBox txtMaKhoa = new();
    readonly TextBox txtMaLop = new();
    readonly TextBox txtTenLop = new();

    public QLLopForm()
    {
        Text = "Quản lý Lớp";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(410, 280);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(25), RowCount = 4 };
        root.Controls.Add(Ui.Row("Mã khoa",txtMaKhoa));
        root.Controls.Add(Ui.Row("Mã lớp",txtMaLop));
        root.Controls.Add(Ui.Row("Tên lớp",txtTenLop));

        var them=new Button{Text="Thêm"}; var xoa=new Button{Text="Xóa"}; var sua=new Button{Text="Sửa"};
        root.Controls.Add(Ui.Buttons(them,xoa,sua));
        Controls.Add(root);

        them.Click += async (_,_)=>await Them();
        xoa.Click += async (_,_)=>await Xoa();
        sua.Click += async (_,_)=>await Sua();
    }

    async Task Them()
    {
        if(txtMaKhoa.Text.Trim()==""||txtMaLop.Text.Trim()==""||txtTenLop.Text.Trim()=="")
        {MessageBox.Show("Phải nhập đầy đủ Mã khoa, Mã lớp, Tên lớp.");return;}
        await Exec("INSERT INTO Lop(MaLop,TenLop,MaKhoa) VALUES(@Lop,@Ten,@Khoa)",true,true);
    }
    async Task Xoa()
    {
        if(txtMaLop.Text.Trim()==""||txtMaKhoa.Text.Trim()!=""||txtTenLop.Text.Trim()!="")
        {MessageBox.Show("Khi xóa chỉ nhập Mã lớp.");return;}
        await Exec("DELETE FROM Lop WHERE MaLop=@Lop",false,false);
    }
    async Task Sua()
    {
        if(txtMaLop.Text.Trim()==""){MessageBox.Show("Mã lớp không được trống.");return;}
        if(txtMaKhoa.Text.Trim()==""&&txtTenLop.Text.Trim()==""){MessageBox.Show("Phải nhập Mã khoa hoặc Tên lớp cần sửa.");return;}

        var sets=new List<string>();
        await using var conn=Db.CreateConnection();
        await using var cmd=new SqlCommand{Connection=conn};
        if(txtMaKhoa.Text.Trim()!=""){sets.Add("MaKhoa=@Khoa");cmd.Parameters.AddWithValue("@Khoa",txtMaKhoa.Text.Trim());}
        if(txtTenLop.Text.Trim()!=""){sets.Add("TenLop=@Ten");cmd.Parameters.AddWithValue("@Ten",txtTenLop.Text.Trim());}
        cmd.CommandText=$"UPDATE Lop SET {string.Join(",",sets)} WHERE MaLop=@Lop";
        cmd.Parameters.AddWithValue("@Lop",txtMaLop.Text.Trim());
        try{await conn.OpenAsync();int n=await cmd.ExecuteNonQueryAsync();MessageBox.Show(n>0?"Thành công":"Không tìm thấy lớp.");}
        catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }

    async Task Exec(string sql,bool ten,bool khoa)
    {
        try
        {
            await using var conn=Db.CreateConnection();await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Lop",txtMaLop.Text.Trim());
            if(ten)cmd.Parameters.AddWithValue("@Ten",txtTenLop.Text.Trim());
            if(khoa)cmd.Parameters.AddWithValue("@Khoa",txtMaKhoa.Text.Trim());
            int n=await cmd.ExecuteNonQueryAsync();MessageBox.Show(n>0?"Thành công":"Không tìm thấy dữ liệu.");
        }
        catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
