using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;
namespace Bai06d_QLThuVien_1Lop;

// Mô hình 1 lớp: giao diện, xử lý nghiệp vụ và truy cập SQL đều đặt trong cùng Form.
internal sealed class FrmNhanVien : Form
{
    private readonly TextBox txtHoTen = new() { Width = 230 };
    private readonly TextBox txtDiaChi = new() { Width = 230 };
    private readonly TextBox txtDienThoai = new() { Width = 230 };
    private readonly DateTimePicker dtpNgaySinh = new() { Width=230, Format=DateTimePickerFormat.Custom, CustomFormat="dd/MM/yyyy" };
    private readonly ComboBox cboBangCap = new() { Width=230, DropDownStyle=ComboBoxStyle.DropDownList };
    private readonly ListView lsvNhanVien = new() {
        Dock=DockStyle.Fill,View=View.Details,FullRowSelect=true,GridLines=true,MultiSelect=false
    };
    private int? selectedId;
    private string ConnectionString
    {
        get {
            string? setting = Environment.GetEnvironmentVariable("TUAN06_THUVIENDB");
            if(!string.IsNullOrWhiteSpace(setting))return setting;
            using var doc=JsonDocument.Parse(File.ReadAllText(Path.Combine(AppContext.BaseDirectory,"dbsettings.json")));
            return doc.RootElement.GetProperty("ConnectionString").GetString()!;
        }
    }
    internal FrmNhanVien()
    {
        Text="Bài thực hành 06d - 1 lớp - Danh mục nhân viên";
        Width=1060;Height=670;MinimumSize=new Size(800,530);
        StartPosition=FormStartPosition.CenterScreen;
        Font=new Font("Segoe UI",10);
        var details=new TableLayoutPanel { Dock=DockStyle.Top, Height=180,ColumnCount=4,
            BackColor=Color.FromArgb(240,246,253),Padding=new Padding(16) };
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,120));
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,120));
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        AddDetail(details,"Họ tên",txtHoTen,0,0);
        AddDetail(details,"Ngày sinh",dtpNgaySinh,2,0);
        AddDetail(details,"Địa chỉ",txtDiaChi,0,1);
        AddDetail(details,"Điện thoại",txtDienThoai,2,1);
        AddDetail(details,"Bằng cấp",cboBangCap,0,2);
        var title=new Label { Text="DANH MỤC NHÂN VIÊN",Dock=DockStyle.Top,Height=48,
            TextAlign=ContentAlignment.MiddleCenter,Font=new Font("Segoe UI",16,FontStyle.Bold),ForeColor=Color.MidnightBlue };
        var actions=new FlowLayoutPanel { Dock=DockStyle.Top, Height=55, Padding=new Padding(12) };
        AddButton(actions,"Thêm",(_,_)=>Insert());
        AddButton(actions,"Sửa",(_,_)=>Update());
        AddButton(actions,"Xóa",(_,_)=>Delete());
        AddButton(actions,"Làm mới",(_,_)=>ResetInputs());
        AddButton(actions,"Tải lại",(_,_)=>LoadAllSafe());
        AddButton(actions,"Thoát",(_,_)=>Close());
        foreach (var (text,width) in new[] {
            ("Mã NV",90),("Họ tên",215),("Ngày sinh",115),("Địa chỉ",250),("Điện thoại",140),("Bằng cấp",145)
        }) lsvNhanVien.Columns.Add(text,width);
        lsvNhanVien.SelectedIndexChanged+=(_,_)=>SelectRow();
        Controls.Add(lsvNhanVien); Controls.Add(actions); Controls.Add(details); Controls.Add(title);
        Shown+=(_,_)=>LoadAllSafe();
    }
    private static void AddDetail(TableLayoutPanel layout,string title,Control control,int col,int row)
    {
        layout.Controls.Add(new Label { Text=title, Dock=DockStyle.Fill,
            TextAlign=ContentAlignment.MiddleRight },col,row);
        control.Dock=DockStyle.Fill;control.Margin=new Padding(5);
        layout.Controls.Add(control,col+1,row);
    }
    private static void AddButton(FlowLayoutPanel panel,string text,EventHandler action)
    {
        var b=new Button { Text=text,Width=115,Height=34 };b.Click+=action;panel.Controls.Add(b);
    }
    // ADO.NET trực tiếp tại Form (không tách DAL/BLL), đúng mục tiêu bài tập mô hình một lớp.
    private DataTable Query(string sql,params SqlParameter[] args)
    {
        using var con=new SqlConnection(ConnectionString);
        using var cmd=new SqlCommand(sql,con);cmd.Parameters.AddRange(args);
        using var da=new SqlDataAdapter(cmd);
        var table=new DataTable();da.Fill(table);return table;
    }
    private int Execute(string sql,params SqlParameter[] args)
    {
        using var con=new SqlConnection(ConnectionString);
        using var cmd=new SqlCommand(sql,con);cmd.Parameters.AddRange(args);
        con.Open();return cmd.ExecuteNonQuery();
    }
    private void LoadAllSafe()
    {
        try {
            DataTable degrees=Query("SELECT MaBangCap,TenBangCap FROM dbo.BANGCAP ORDER BY TenBangCap");
            cboBangCap.DataSource=degrees;
            cboBangCap.DisplayMember="TenBangCap";cboBangCap.ValueMember="MaBangCap";
            var dt=Query("SELECT n.MaNhanVien,n.HoTenNhanVien,n.NgaySinh,n.DiaChi,n.DienThoai,"+
                        "n.MaBangCap,b.TenBangCap FROM dbo.NHANVIEN n LEFT JOIN dbo.BANGCAP b ON b.MaBangCap=n.MaBangCap ORDER BY n.MaNhanVien");
            lsvNhanVien.BeginUpdate();lsvNhanVien.Items.Clear();
            foreach(DataRow r in dt.Rows)
            {
                var item=new ListViewItem(r["MaNhanVien"].ToString());
                item.SubItems.Add(r["HoTenNhanVien"].ToString());
                item.SubItems.Add(r["NgaySinh"]==DBNull.Value ? "" : Convert.ToDateTime(r["NgaySinh"]).ToString("dd/MM/yyyy"));
                item.SubItems.Add(r["DiaChi"].ToString());
                item.SubItems.Add(r["DienThoai"].ToString());
                item.SubItems.Add(r["TenBangCap"].ToString());
                item.Tag=r;
                lsvNhanVien.Items.Add(item);
            }
            lsvNhanVien.EndUpdate();ResetInputs();
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Không thể tải dữ liệu",MessageBoxButtons.OK,MessageBoxIcon.Error); }
    }
    private void SelectRow()
    {
        if(lsvNhanVien.SelectedItems.Count==0) return;
        var row=(DataRow)lsvNhanVien.SelectedItems[0].Tag!;
        selectedId=Convert.ToInt32(row["MaNhanVien"]);
        txtHoTen.Text=row["HoTenNhanVien"].ToString();
        txtDiaChi.Text=row["DiaChi"].ToString();
        txtDienThoai.Text=row["DienThoai"].ToString();
        if(row["NgaySinh"]!=DBNull.Value)dtpNgaySinh.Value=Convert.ToDateTime(row["NgaySinh"]);
        if(row["MaBangCap"]!=DBNull.Value)cboBangCap.SelectedValue=Convert.ToInt32(row["MaBangCap"]);
    }
    private SqlParameter[] InputParameters() => new[] {
        new SqlParameter("@ho",SqlDbType.NVarChar,50) {Value=txtHoTen.Text.Trim()},
        new SqlParameter("@ngaysinh",SqlDbType.DateTime) {Value=dtpNgaySinh.Value.Date},
        new SqlParameter("@diachi",SqlDbType.NVarChar,50) {Value=txtDiaChi.Text.Trim()},
        new SqlParameter("@dienthoai",SqlDbType.NVarChar,15) {Value=txtDienThoai.Text.Trim()},
        new SqlParameter("@bangcap",SqlDbType.Int) {Value=cboBangCap.SelectedValue ?? DBNull.Value}
    };
    private bool IsValid()
    {
        if(string.IsNullOrWhiteSpace(txtHoTen.Text)) { MessageBox.Show("Vui lòng nhập họ tên.");txtHoTen.Focus();return false; }
        if(txtHoTen.Text.Length>50 || txtDiaChi.Text.Length>50 || txtDienThoai.Text.Length>15)
        { MessageBox.Show("Dữ liệu vượt quá độ dài quy định của CSDL.");return false; }
        if(cboBangCap.SelectedValue is not int) { MessageBox.Show("Vui lòng chọn bằng cấp.");return false; }
        return true;
    }
    private void Insert()
    {
        if(!IsValid())return;
        try {
            Execute("INSERT INTO dbo.NHANVIEN(HoTenNhanVien,NgaySinh,DiaChi,DienThoai,MaBangCap) "+
                    "VALUES(@ho,@ngaysinh,@diachi,@dienthoai,@bangcap)",InputParameters());
            LoadAllSafe();MessageBox.Show("Đã thêm nhân viên.");
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Lỗi thêm"); }
    }
    private void Update()
    {
        if(selectedId is not int id || !IsValid()){if(selectedId is null)MessageBox.Show("Chọn nhân viên cần sửa.");return;}
        try {
            var args=InputParameters().Concat(new[]{new SqlParameter("@id",id)}).ToArray();
            Execute("UPDATE dbo.NHANVIEN SET HoTenNhanVien=@ho,NgaySinh=@ngaysinh,DiaChi=@diachi,"+
                    "DienThoai=@dienthoai,MaBangCap=@bangcap WHERE MaNhanVien=@id",args);
            LoadAllSafe();MessageBox.Show("Đã cập nhật.");
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Lỗi sửa"); }
    }
    private void Delete()
    {
        if(selectedId is not int id){MessageBox.Show("Chọn nhân viên cần xóa.");return;}
        if(MessageBox.Show("Bạn có chắc muốn xóa nhân viên này?","Xác nhận",MessageBoxButtons.YesNo)!=DialogResult.Yes)return;
        try {
            Execute("DELETE FROM dbo.NHANVIEN WHERE MaNhanVien=@id",new SqlParameter("@id",id));
            LoadAllSafe();MessageBox.Show("Đã xóa.");
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Không thể xóa (kiểm tra khóa ngoại)"); }
    }
    private void ResetInputs()
    {
        selectedId=null;foreach(var selected in lsvNhanVien.SelectedItems.Cast<ListViewItem>().ToArray())selected.Selected=false;
        txtHoTen.Clear();txtDiaChi.Clear();txtDienThoai.Clear();dtpNgaySinh.Value=DateTime.Today;
        if(cboBangCap.Items.Count>0)cboBangCap.SelectedIndex=0;
    }
}
