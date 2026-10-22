using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class ThemSanPhamForm : Form
{
    readonly TextBox txtTen=new();

    public ThemSanPhamForm()
    {
        Text="Thêm sản phẩm";
        Size=new Size(420,220);

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(25),RowCount=2};
        root.Controls.Add(Ui.Row("Tên sản phẩm",txtTen));
        var them=new Button{Text="Thêm sản phẩm",Width=130,Height=36,Anchor=AnchorStyles.None};
        root.Controls.Add(them);Controls.Add(root);
        them.Click+=async(_,_)=>await Them();
    }

    async Task Them()
    {
        if(txtTen.Text.Trim()==""){MessageBox.Show("Tên sản phẩm không được để trống.");return;}

        await using var conn=Db.CuaHangConn();await conn.OpenAsync();
        await using var tran=await conn.BeginTransactionAsync();

        try
        {
            const string sqlSP="INSERT INTO SANPHAM(TenSP) VALUES(@Ten); SELECT CAST(SCOPE_IDENTITY() AS int);";
            await using var cmd=new SqlCommand(sqlSP,conn,(SqlTransaction)tran);
            cmd.Parameters.AddWithValue("@Ten",txtTen.Text.Trim());
            int ma=Convert.ToInt32(await cmd.ExecuteScalarAsync());

            const string sqlTon="INSERT INTO TONKHO(MaSP,NgayTon,SLDau,SLNhap,SLBan) VALUES(@Ma,@Ngay,0,0,0)";
            await using var cmd2=new SqlCommand(sqlTon,conn,(SqlTransaction)tran);
            cmd2.Parameters.AddWithValue("@Ma",ma);cmd2.Parameters.AddWithValue("@Ngay",DateTime.Today);
            await cmd2.ExecuteNonQueryAsync();

            await tran.CommitAsync();
            MessageBox.Show($"Thêm sản phẩm thành công. Mã SP = {ma}");
            txtTen.Clear();txtTen.Focus();
        }
        catch(Exception ex){await tran.RollbackAsync();MessageBox.Show("Thất bại:\n"+ex.Message);}
    }
}
