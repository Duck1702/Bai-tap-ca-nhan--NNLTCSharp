using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class CuaHangMainForm : Form
{
    public CuaHangMainForm()
    {
        Text="QL Cửa hàng";
        StartPosition=FormStartPosition.CenterParent;
        Size=new Size(650,430);
        IsMdiContainer=true;

        var menu=new MenuStrip();
        var ton=new ToolStripMenuItem("Tạo tồn kho");
        var sp=new ToolStripMenuItem("Thêm sản phẩm mới");
        var ban=new ToolStripMenuItem("Bán hàng");
        var thoat=new ToolStripMenuItem("Thoát");
        menu.Items.AddRange(new ToolStripItem[]{ton,sp,ban,thoat});
        MainMenuStrip=menu;Controls.Add(menu);

        ton.Click+=async(_,_)=>await TaoTonKhoHomNay();
        sp.Click+=(_,_)=>Mo(new ThemSanPhamForm());
        ban.Click+=(_,_)=>Mo(new BanHangForm());
        thoat.Click+=(_,_)=>Close();
    }

    void Mo(Form f)
    {
        f.MdiParent=this;f.Show();
    }

    async Task TaoTonKhoHomNay()
    {
        try
        {
            await using var conn=Db.CuaHangConn();await conn.OpenAsync();

            // Không tạo lại nếu hôm nay đã có tồn kho.
            await using(var check=new SqlCommand("SELECT COUNT(*) FROM TONKHO WHERE NgayTon=@Ngay",conn))
            {
                check.Parameters.AddWithValue("@Ngay",DateTime.Today);
                if(Convert.ToInt32(await check.ExecuteScalarAsync())>0)
                {MessageBox.Show("Tồn kho hôm nay đã được tạo.");return;}
            }

            // Tạo tồn hôm nay từ tồn ngày trước gần nhất của từng sản phẩm.
            const string sql = """
                INSERT INTO TONKHO(MaSP,NgayTon,SLDau,SLNhap,SLBan)
                SELECT s.MaSP, @Ngay,
                       ISNULL((
                           SELECT TOP 1 tk.SLDau + tk.SLNhap - tk.SLBan
                           FROM TONKHO tk
                           WHERE tk.MaSP=s.MaSP AND tk.NgayTon < @Ngay
                           ORDER BY tk.NgayTon DESC
                       ),0),
                       0,0
                FROM SANPHAM s
                """;
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Ngay",DateTime.Today);
            int n=await cmd.ExecuteNonQueryAsync();
            MessageBox.Show($"Đã tạo tồn kho hôm nay cho {n} sản phẩm.");
        }
        catch(Exception ex){MessageBox.Show("Không thể tạo tồn kho:\n"+ex.Message);}
    }
}
