using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class QLDiemForm : Form
{
    readonly ComboBox cboSV=new(){DropDownStyle=ComboBoxStyle.DropDownList};
    readonly ComboBox cboMon=new(){DropDownStyle=ComboBoxStyle.DropDownList};
    readonly TextBox txtDiem=new();

    public QLDiemForm()
    {
        Text="Cập nhật dữ liệu Điểm";
        StartPosition=FormStartPosition.CenterParent;
        Size=new Size(420,280);
        FormBorderStyle=FormBorderStyle.FixedDialog;MaximizeBox=false;

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(25),RowCount=4};
        root.Controls.Add(Ui.Row("Sinh viên",cboSV));
        root.Controls.Add(Ui.Row("Môn học",cboMon));
        root.Controls.Add(Ui.Row("Điểm",txtDiem));
        var them=new Button{Text="Thêm"};var xoa=new Button{Text="Xóa"};var sua=new Button{Text="Sửa"};
        root.Controls.Add(Ui.Buttons(them,xoa,sua));Controls.Add(root);

        Load+=async(_,_)=>await LoadCombos();
        them.Click+=async(_,_)=>await Them();
        xoa.Click+=async(_,_)=>await Xoa();
        sua.Click+=async(_,_)=>await Sua();
    }

    async Task LoadCombos()
    {
        cboSV.Items.Clear();cboMon.Items.Clear();
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();

        await using(var cmd=new SqlCommand("SELECT MaSinhVien FROM SinhVien ORDER BY MaSinhVien",conn))
        await using(var rd=await cmd.ExecuteReaderAsync())
            while(await rd.ReadAsync())cboSV.Items.Add(rd["MaSinhVien"].ToString());

        await using(var cmd=new SqlCommand("SELECT MaMonHoc FROM MonHoc ORDER BY MaMonHoc",conn))
        await using(var rd=await cmd.ExecuteReaderAsync())
            while(await rd.ReadAsync())cboMon.Items.Add(rd["MaMonHoc"].ToString());

        if(cboSV.Items.Count>0)cboSV.SelectedIndex=0;
        if(cboMon.Items.Count>0)cboMon.SelectedIndex=0;
    }

    bool TryScore(out double d)
    {
        if(!double.TryParse(txtDiem.Text,out d)||d<=0||d>=10)
        {MessageBox.Show("Điểm phải nằm trong khoảng (0-10).");return false;}
        return true;
    }

    async Task<bool> ChuaTonTai(string sv,string mh)
    {
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaSinhVien FROM Diem WHERE MaSinhVien=@SV AND MaMonHoc=@MH",conn);
        cmd.Parameters.AddWithValue("@SV",sv);cmd.Parameters.AddWithValue("@MH",mh);
        await using var rd=await cmd.ExecuteReaderAsync();
        return !rd.HasRows;
    }

    async Task Them()
    {
        if(cboSV.SelectedItem==null||cboMon.SelectedItem==null||!TryScore(out var d))
        {if(cboSV.SelectedItem==null||cboMon.SelectedItem==null)MessageBox.Show("Tất cả thông tin phải được nhập.");return;}

        string sv=cboSV.SelectedItem.ToString()!,mh=cboMon.SelectedItem.ToString()!;
        if(!await ChuaTonTai(sv,mh)){MessageBox.Show("Trùng khóa chính trong bảng Điểm.");return;}
        await Exec("INSERT INTO Diem(MaSinhVien,MaMonHoc,Diem) VALUES(@SV,@MH,@Diem)",d);
    }

    async Task Xoa()
    {
        if(cboSV.SelectedItem==null||cboMon.SelectedItem==null){MessageBox.Show("Phải chọn Sinh viên và Môn học.");return;}
        await Exec("DELETE FROM Diem WHERE MaSinhVien=@SV AND MaMonHoc=@MH",null);
    }

    async Task Sua()
    {
        if(cboSV.SelectedItem==null||cboMon.SelectedItem==null||!TryScore(out var d))return;
        await Exec("UPDATE Diem SET Diem=@Diem WHERE MaSinhVien=@SV AND MaMonHoc=@MH",d);
    }

    async Task Exec(string sql,double? d)
    {
        try{
            await using var conn=Db.SinhVienConn();await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@SV",cboSV.SelectedItem!.ToString()!);
            cmd.Parameters.AddWithValue("@MH",cboMon.SelectedItem!.ToString()!);
            if(d.HasValue)cmd.Parameters.AddWithValue("@Diem",d.Value);
            int n=await cmd.ExecuteNonQueryAsync();MessageBox.Show(n>0?"Thành công":"Không tìm thấy dữ liệu.");
        }catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
