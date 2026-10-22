using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class BanHangForm : Form
{
    readonly TextBox txtMaHD=new(){ReadOnly=true};
    readonly ComboBox cboSP=new(){DropDownStyle=ComboBoxStyle.DropDownList};
    readonly NumericUpDown nudSL=new(){Minimum=1,Maximum=100000,Value=1};
    readonly Button btnTao=new(){Text="Tạo hóa đơn mới",Width=130};
    readonly Button btnThem=new(){Text="Thêm",Width=90};
    readonly Button btnHuy=new(){Text="Hủy",Width=90};
    readonly Button btnThanhToan=new(){Text="Thanh toán",Width=110};

    public BanHangForm()
    {
        Text="Bán hàng";
        Size=new Size(520,360);

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(25),RowCount=6};
        btnTao.Anchor=AnchorStyles.Right;root.Controls.Add(btnTao);
        root.Controls.Add(Ui.Row("Mã hóa đơn",txtMaHD));
        root.Controls.Add(Ui.Row("Mã sản phẩm",cboSP));
        root.Controls.Add(Ui.Row("Số lượng",nudSL));
        root.Controls.Add(Ui.Buttons(btnThem,btnHuy));
        btnThanhToan.Anchor=AnchorStyles.Right;root.Controls.Add(btnThanhToan);
        Controls.Add(root);

        SetEditing(false);
        btnTao.Click+=async(_,_)=>await TaoHoaDon();
        btnThem.Click+=async(_,_)=>await ThemChiTiet();
        btnHuy.Click+=async(_,_)=>await HuyHoaDon();
        btnThanhToan.Click+=async(_,_)=>await ThanhToan();
    }

    void SetEditing(bool on)
    {
        cboSP.Enabled=on;nudSL.Enabled=on;btnThem.Enabled=on;btnHuy.Enabled=on;btnThanhToan.Enabled=on;
        btnTao.Enabled=!on;
    }

    async Task LoadSP()
    {
        cboSP.Items.Clear();
        await using var conn=Db.CuaHangConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaSP,TenSP FROM SANPHAM ORDER BY MaSP",conn);
        await using var rd=await cmd.ExecuteReaderAsync();
        while(await rd.ReadAsync())cboSP.Items.Add(new SpItem(Convert.ToInt32(rd["MaSP"]),rd["TenSP"].ToString()!));
        cboSP.DisplayMember=nameof(SpItem.Text);
        if(cboSP.Items.Count>0)cboSP.SelectedIndex=0;
    }

    async Task TaoHoaDon()
    {
        try
        {
            await using var conn=Db.CuaHangConn();await conn.OpenAsync();
            const string sql="""
                INSERT INTO HOADON(NgayTao,TinhTrangThanhToan)
                VALUES(@Ngay,0);
                SELECT CAST(SCOPE_IDENTITY() AS int);
                """;
            await using var cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("@Ngay",DateTime.Now);
            int ma=Convert.ToInt32(await cmd.ExecuteScalarAsync());
            txtMaHD.Text=ma.ToString();
            await LoadSP();
            SetEditing(true);
        }
        catch(Exception ex){MessageBox.Show("Không tạo được hóa đơn:\n"+ex.Message);}
    }

    async Task<int> TonHienTai(SqlConnection conn, SqlTransaction tran, int maSP)
    {
        const string sql="""
            SELECT TOP 1 SLDau + SLNhap - SLBan
            FROM TONKHO
            WHERE MaSP=@Ma AND NgayTon<=@Ngay
            ORDER BY NgayTon DESC
            """;
        await using var cmd=new SqlCommand(sql,conn,tran);
        cmd.Parameters.AddWithValue("@Ma",maSP);cmd.Parameters.AddWithValue("@Ngay",DateTime.Today);
        var x=await cmd.ExecuteScalarAsync();
        return x==null||x==DBNull.Value?0:Convert.ToInt32(x);
    }

    async Task ThemChiTiet()
    {
        if(!int.TryParse(txtMaHD.Text,out int maHD)||cboSP.SelectedItem is not SpItem sp)return;
        int sl=(int)nudSL.Value;

        await using var conn=Db.CuaHangConn();await conn.OpenAsync();
        await using var tran=(SqlTransaction)await conn.BeginTransactionAsync();
        try
        {
            int ton=await TonHienTai(conn,tran,sp.Ma);
            if(sl<=0||sl>=ton)
            {
                MessageBox.Show($"Số lượng bán phải > 0 và nhỏ hơn số lượng tồn ({ton}).");
                await tran.RollbackAsync();return;
            }

            await using(var check=new SqlCommand("SELECT COUNT(*) FROM CHITIET_HOADON WHERE MaHD=@HD AND MaSP=@SP",conn,tran))
            {
                check.Parameters.AddWithValue("@HD",maHD);check.Parameters.AddWithValue("@SP",sp.Ma);
                if(Convert.ToInt32(await check.ExecuteScalarAsync())>0)
                {MessageBox.Show("Sản phẩm này đã có trong hóa đơn.");await tran.RollbackAsync();return;}
            }

            await using(var cmd=new SqlCommand("INSERT INTO CHITIET_HOADON(MaHD,MaSP,SoLuong) VALUES(@HD,@SP,@SL)",conn,tran))
            {
                cmd.Parameters.AddWithValue("@HD",maHD);cmd.Parameters.AddWithValue("@SP",sp.Ma);cmd.Parameters.AddWithValue("@SL",sl);
                await cmd.ExecuteNonQueryAsync();
            }

            // Suy luận theo công thức tồn = SLDau + SLNhap - SLBan:
            // khi bán thì tổng SLBan phải TĂNG thêm số lượng bán.
            await using(var cmd=new SqlCommand("""
                UPDATE TONKHO SET SLBan=SLBan+@SL
                WHERE MaSP=@SP AND NgayTon=@Ngay
                """,conn,tran))
            {
                cmd.Parameters.AddWithValue("@SL",sl);cmd.Parameters.AddWithValue("@SP",sp.Ma);cmd.Parameters.AddWithValue("@Ngay",DateTime.Today);
                int n=await cmd.ExecuteNonQueryAsync();
                if(n==0)throw new Exception("Chưa có dữ liệu tồn kho hôm nay. Hãy chạy Tạo tồn kho trước.");
            }

            await tran.CommitAsync();
            MessageBox.Show("Đã thêm chi tiết hóa đơn.");
        }
        catch(Exception ex){await tran.RollbackAsync();MessageBox.Show("Thất bại:\n"+ex.Message);}
    }

    async Task HuyHoaDon()
    {
        if(!int.TryParse(txtMaHD.Text,out int maHD))return;
        if(MessageBox.Show("Hủy hóa đơn hiện tại?","Xác nhận",MessageBoxButtons.YesNo)!=DialogResult.Yes)return;

        await using var conn=Db.CuaHangConn();await conn.OpenAsync();
        await using var tran=(SqlTransaction)await conn.BeginTransactionAsync();
        try
        {
            // Hoàn lại SLBan trước khi xóa chi tiết.
            const string restore="""
                UPDATE tk SET tk.SLBan=tk.SLBan-ct.SoLuong
                FROM TONKHO tk
                JOIN CHITIET_HOADON ct ON tk.MaSP=ct.MaSP
                WHERE ct.MaHD=@HD AND tk.NgayTon=@Ngay
                """;
            await using(var cmd=new SqlCommand(restore,conn,tran))
            {cmd.Parameters.AddWithValue("@HD",maHD);cmd.Parameters.AddWithValue("@Ngay",DateTime.Today);await cmd.ExecuteNonQueryAsync();}

            await using(var cmd=new SqlCommand("DELETE FROM CHITIET_HOADON WHERE MaHD=@HD",conn,tran))
            {cmd.Parameters.AddWithValue("@HD",maHD);await cmd.ExecuteNonQueryAsync();}

            await using(var cmd=new SqlCommand("DELETE FROM HOADON WHERE MaHD=@HD AND TinhTrangThanhToan=0",conn,tran))
            {cmd.Parameters.AddWithValue("@HD",maHD);await cmd.ExecuteNonQueryAsync();}

            await tran.CommitAsync();ResetUI();
        }
        catch(Exception ex){await tran.RollbackAsync();MessageBox.Show("Không hủy được:\n"+ex.Message);}
    }

    async Task ThanhToan()
    {
        if(!int.TryParse(txtMaHD.Text,out int maHD))return;
        await using var conn=Db.CuaHangConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("UPDATE HOADON SET TinhTrangThanhToan=1 WHERE MaHD=@HD",conn);
        cmd.Parameters.AddWithValue("@HD",maHD);
        int n=await cmd.ExecuteNonQueryAsync();
        if(n>0){MessageBox.Show("Thanh toán thành công.");ResetUI();}
    }

    void ResetUI()
    {
        txtMaHD.Clear();cboSP.Items.Clear();nudSL.Value=1;SetEditing(false);
    }

    record SpItem(int Ma,string Ten)
    {
        public string Text => $"{Ma} - {Ten}";
        public override string ToString()=>Text;
    }
}
