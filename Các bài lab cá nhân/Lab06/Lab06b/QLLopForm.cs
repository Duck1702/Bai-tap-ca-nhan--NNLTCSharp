using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class QLLopForm : Form
{
    readonly ComboBox cboKhoa=new(){DropDownStyle=ComboBoxStyle.DropDownList};
    readonly TextBox txtMaLop=new();
    readonly TextBox txtTenLop=new();

    public QLLopForm()
    {
        Text="Quản lý Lớp";
        StartPosition=FormStartPosition.CenterParent;
        Size=new Size(430,300);
        FormBorderStyle=FormBorderStyle.FixedDialog;MaximizeBox=false;

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(25),RowCount=4};
        root.Controls.Add(Ui.Row("Khoa",cboKhoa));
        root.Controls.Add(Ui.Row("Mã lớp",txtMaLop));
        root.Controls.Add(Ui.Row("Tên lớp",txtTenLop));
        var them=new Button{Text="Thêm"};var xoa=new Button{Text="Xóa"};var sua=new Button{Text="Sửa"};
        root.Controls.Add(Ui.Buttons(them,xoa,sua));Controls.Add(root);

        Load+=async(_,_)=>await LoadKhoa();
        them.Click+=async(_,_)=>await Them();
        xoa.Click+=async(_,_)=>await Xoa();
        sua.Click+=async(_,_)=>await Sua();
    }

    async Task LoadKhoa()
    {
        cboKhoa.Items.Clear();
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaKhoa FROM Khoa ORDER BY MaKhoa",conn);
        await using var rd=await cmd.ExecuteReaderAsync();
        while(await rd.ReadAsync()) cboKhoa.Items.Add(rd["MaKhoa"].ToString());
        if(cboKhoa.Items.Count>0)cboKhoa.SelectedIndex=0;
    }

    async Task<bool> MaLopChuaTonTai(string ma)
    {
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaLop FROM Lop WHERE MaLop=@Ma",conn);
        cmd.Parameters.AddWithValue("@Ma",ma);
        await using var rd=await cmd.ExecuteReaderAsync();
        return !rd.HasRows;
    }

    async Task<bool> KhoaTonTai(string maKhoa)
    {
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaKhoa FROM Khoa WHERE MaKhoa=@Ma",conn);
        cmd.Parameters.AddWithValue("@Ma",maKhoa);
        await using var rd=await cmd.ExecuteReaderAsync();
        return rd.HasRows;
    }

    async Task Them()
    {
        if(cboKhoa.SelectedItem==null||txtMaLop.Text.Trim()==""||txtTenLop.Text.Trim()=="")
        {MessageBox.Show("Tất cả thông tin phải được nhập.");return;}

        string khoa=cboKhoa.SelectedItem.ToString()!;
        if(!await MaLopChuaTonTai(txtMaLop.Text.Trim())){MessageBox.Show("Trùng mã lớp.");return;}
        if(!await KhoaTonTai(khoa)){MessageBox.Show("Mã khoa không tồn tại.");return;}

        await Exec("INSERT INTO Lop(MaLop,TenLop,MaKhoa) VALUES(@Lop,@Ten,@Khoa)",true,true);
    }

    async Task Xoa()
    {
        if(txtMaLop.Text.Trim()==""){MessageBox.Show("Bạn phải nhập Mã lớp.");return;}
        await Exec("DELETE FROM Lop WHERE MaLop=@Lop",false,false);
    }

    async Task Sua()
    {
        if(cboKhoa.SelectedItem==null||txtMaLop.Text.Trim()==""||txtTenLop.Text.Trim()=="")
        {MessageBox.Show("Tất cả thông tin phải được nhập.");return;}
        string khoa=cboKhoa.SelectedItem.ToString()!;
        if(!await KhoaTonTai(khoa)){MessageBox.Show("Mã khoa không tồn tại.");return;}
        await Exec("UPDATE Lop SET TenLop=@Ten,MaKhoa=@Khoa WHERE MaLop=@Lop",true,true);
    }

    async Task Exec(string sql,bool ten,bool khoa)
    {
        try{
            await using var conn=Db.SinhVienConn();await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Lop",txtMaLop.Text.Trim());
            if(ten)cmd.Parameters.AddWithValue("@Ten",txtTenLop.Text.Trim());
            if(khoa)cmd.Parameters.AddWithValue("@Khoa",cboKhoa.SelectedItem!.ToString()!);
            int n=await cmd.ExecuteNonQueryAsync();MessageBox.Show(n>0?"Thành công":"Không tìm thấy dữ liệu.");
        }catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
