using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class DanhSachDiemForm : Form
{
    readonly ComboBox cboMon=new(){DropDownStyle=ComboBoxStyle.DropDownList};
    readonly ListView list=new();
    readonly TextBox txtTong=new(){ReadOnly=true};

    public DanhSachDiemForm()
    {
        Text="Danh sách sinh viên";
        StartPosition=FormStartPosition.CenterParent;
        Size=new Size(760,520);

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(20),RowCount=3};
        var filter=new FlowLayoutPanel{AutoSize=true,Dock=DockStyle.Top};
        filter.Controls.Add(new Label{Text="Môn học",AutoSize=true,Margin=new Padding(0,8,8,0)});
        cboMon.Width=180;filter.Controls.Add(cboMon);
        var btn=new Button{Text="Lọc dữ liệu",Width=110};filter.Controls.Add(btn);
        root.Controls.Add(filter);

        list.Dock=DockStyle.Fill;list.View=View.Details;list.FullRowSelect=true;list.GridLines=true;
        list.Columns.Add("STT",50);list.Columns.Add("Tên SV",180);list.Columns.Add("Ngày sinh",130);
        list.Columns.Add("Tên môn học",220);list.Columns.Add("Điểm",80);
        root.Controls.Add(list);

        var total=new FlowLayoutPanel{AutoSize=true,Anchor=AnchorStyles.Right};
        total.Controls.Add(new Label{Text="Tổng điểm",AutoSize=true,Margin=new Padding(0,8,8,0)});
        txtTong.Width=100;total.Controls.Add(txtTong);root.Controls.Add(total);
        Controls.Add(root);

        Load+=async(_,_)=>{await LoadMon();await LoadData(null);};
        btn.Click+=async(_,_)=>await LoadData(cboMon.SelectedItem?.ToString());
    }

    async Task LoadMon()
    {
        cboMon.Items.Clear();cboMon.Items.Add("");
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaMonHoc FROM MonHoc ORDER BY MaMonHoc",conn);
        await using var rd=await cmd.ExecuteReaderAsync();
        while(await rd.ReadAsync())cboMon.Items.Add(rd["MaMonHoc"].ToString());
        cboMon.SelectedIndex=0;
    }

    async Task LoadData(string? maMon)
    {
        list.Items.Clear();
        const string allSql = """
            SELECT sv.HoTen, sv.NgaySinh, mh.TenMonHoc, d.Diem
            FROM Diem d
            JOIN SinhVien sv ON d.MaSinhVien=sv.MaSinhVien
            JOIN MonHoc mh ON d.MaMonHoc=mh.MaMonHoc
            ORDER BY sv.HoTen, mh.TenMonHoc
            """;
        const string filterSql = """
            SELECT sv.HoTen, sv.NgaySinh, mh.TenMonHoc, d.Diem
            FROM Diem d
            JOIN SinhVien sv ON d.MaSinhVien=sv.MaSinhVien
            JOIN MonHoc mh ON d.MaMonHoc=mh.MaMonHoc
            WHERE d.MaMonHoc=@MaMon
            ORDER BY sv.HoTen
            """;

        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand(string.IsNullOrWhiteSpace(maMon)?allSql:filterSql,conn);
        if(!string.IsNullOrWhiteSpace(maMon))cmd.Parameters.AddWithValue("@MaMon",maMon);
        await using var rd=await cmd.ExecuteReaderAsync();

        int stt=1;double tong=0;
        while(await rd.ReadAsync())
        {
            double d=Convert.ToDouble(rd["Diem"]);tong+=d;
            list.Items.Add(new ListViewItem(new[]{
                (stt++).ToString(),
                rd["HoTen"].ToString()!,
                Convert.ToDateTime(rd["NgaySinh"]).ToString("dd/MM/yyyy"),
                rd["TenMonHoc"].ToString()!,
                d.ToString("0.##")
            }));
        }
        txtTong.Text=tong.ToString("0.##");
    }
}
