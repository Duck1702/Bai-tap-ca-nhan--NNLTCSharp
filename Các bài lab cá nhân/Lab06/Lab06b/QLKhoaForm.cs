using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class QLKhoaForm : Form
{
    readonly TextBox txtMa = new();
    readonly TextBox txtTen = new();

    public QLKhoaForm()
    {
        Text="Quản lý Khoa";
        StartPosition=FormStartPosition.CenterParent;
        Size=new Size(390,240);
        FormBorderStyle=FormBorderStyle.FixedDialog; MaximizeBox=false;

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(25),RowCount=3};
        root.Controls.Add(Ui.Row("Mã khoa",txtMa));
        root.Controls.Add(Ui.Row("Tên khoa",txtTen));
        var them=new Button{Text="Thêm"};var xoa=new Button{Text="Xóa"};var sua=new Button{Text="Sửa"};
        root.Controls.Add(Ui.Buttons(them,xoa,sua)); Controls.Add(root);

        them.Click+=async(_,_)=>await Them();
        xoa.Click+=async(_,_)=>await Xoa();
        sua.Click+=async(_,_)=>await Sua();
    }

    async Task<bool> MaKhoaChuaTonTai(string ma)
    {
        await using var conn=Db.SinhVienConn(); await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT COUNT(*) FROM Khoa WHERE MaKhoa=@Ma",conn);
        cmd.Parameters.AddWithValue("@Ma",ma);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync())==0;
    }

    async Task Them()
    {
        if(txtMa.Text.Trim()==""||txtTen.Text.Trim()==""){MessageBox.Show("Phải nhập Mã khoa và Tên khoa.");return;}
        if(!await MaKhoaChuaTonTai(txtMa.Text.Trim())){MessageBox.Show("Trùng mã khoa.");return;}
        await Exec("INSERT INTO Khoa(MaKhoa,TenKhoa) VALUES(@Ma,@Ten)",true);
    }

    async Task Xoa()
    {
        if(txtMa.Text.Trim()==""){MessageBox.Show("Phải nhập Mã khoa.");return;}
        await Exec("DELETE FROM Khoa WHERE MaKhoa=@Ma",false);
    }

    async Task Sua()
    {
        if(txtMa.Text.Trim()==""||txtTen.Text.Trim()==""){MessageBox.Show("Phải nhập Mã khoa và Tên khoa.");return;}
        await Exec("UPDATE Khoa SET TenKhoa=@Ten WHERE MaKhoa=@Ma",true);
    }

    async Task Exec(string sql,bool useTen)
    {
        try{
            await using var conn=Db.SinhVienConn();await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Ma",txtMa.Text.Trim());
            if(useTen)cmd.Parameters.AddWithValue("@Ten",txtTen.Text.Trim());
            int n=await cmd.ExecuteNonQueryAsync();MessageBox.Show(n>0?"Thành công":"Không tìm thấy dữ liệu.");
        }catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
