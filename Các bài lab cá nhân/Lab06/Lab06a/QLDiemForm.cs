using Lab06a.Database;
using Microsoft.Data.SqlClient;

namespace Lab06a;

public class QLDiemForm : Form
{
    readonly TextBox txtMaSV = new();
    readonly TextBox txtMaMon = new();
    readonly TextBox txtDiem = new();

    public QLDiemForm()
    {
        Text = "Quản lý Điểm";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(410, 280);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(25), RowCount = 4 };
        root.Controls.Add(Ui.Row("Mã sinh viên", txtMaSV));
        root.Controls.Add(Ui.Row("Mã môn học", txtMaMon));
        root.Controls.Add(Ui.Row("Điểm", txtDiem));

        var them = new Button { Text = "Thêm" };
        var xoa = new Button { Text = "Xóa" };
        var sua = new Button { Text = "Sửa" };
        root.Controls.Add(Ui.Buttons(them,xoa,sua));
        Controls.Add(root);

        them.Click += async (_,_) => await Them();
        xoa.Click += async (_,_) => await Xoa();
        sua.Click += async (_,_) => await Sua();
    }

    bool Valid(bool needScore, out double d)
    {
        d=0;
        if(txtMaSV.Text.Trim()==""||txtMaMon.Text.Trim()==""){MessageBox.Show("Mã sinh viên và Mã môn học không được trống.");return false;}
        if(needScore && !double.TryParse(txtDiem.Text,out d)){MessageBox.Show("Điểm phải là số.");return false;}
        return true;
    }

    async Task Them()
    {
        if(!Valid(true,out var d)) return;
        await Exec("INSERT INTO Diem(MaSinhVien,MaMonHoc,Diem) VALUES(@SV,@MH,@Diem)",d);
    }
    async Task Xoa()
    {
        if(!Valid(false,out _)) return;
        if(txtDiem.Text.Trim()!=""){MessageBox.Show("Khi xóa, Điểm phải để trống.");return;}
        await Exec("DELETE FROM Diem WHERE MaSinhVien=@SV AND MaMonHoc=@MH",null);
    }
    async Task Sua()
    {
        if(!Valid(true,out var d)) return;
        await Exec("UPDATE Diem SET Diem=@Diem WHERE MaSinhVien=@SV AND MaMonHoc=@MH",d);
    }

    async Task Exec(string sql,double? diem)
    {
        try
        {
            await using var conn=Db.CreateConnection(); await conn.OpenAsync();
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@SV",txtMaSV.Text.Trim());
            cmd.Parameters.AddWithValue("@MH",txtMaMon.Text.Trim());
            if(diem.HasValue) cmd.Parameters.AddWithValue("@Diem",diem.Value);
            int n=await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n>0?"Thành công":"Không tìm thấy dữ liệu.");
        }
        catch(Exception ex){MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
