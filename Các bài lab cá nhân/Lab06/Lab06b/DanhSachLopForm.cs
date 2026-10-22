using Lab06b.Database;
using Microsoft.Data.SqlClient;

namespace Lab06b;

public class DanhSachLopForm : Form
{
    readonly ListView list=new();

    public DanhSachLopForm()
    {
        Text="Danh sách Lớp";
        StartPosition=FormStartPosition.CenterParent;
        Size=new Size(620,460);

        var root=new TableLayoutPanel{Dock=DockStyle.Fill,Padding=new Padding(20),RowCount=3};
        var show=new Button{Text="Hiển thị dữ liệu",Width=120,Height=32,Anchor=AnchorStyles.Left};
        root.Controls.Add(show);

        list.Dock=DockStyle.Fill;list.View=View.Details;list.FullRowSelect=true;list.GridLines=true;
        list.Columns.Add("STT",50);list.Columns.Add("Mã lớp",120);list.Columns.Add("Tên lớp",260);list.Columns.Add("Mã khoa",100);
        root.Controls.Add(list);

        var exit=new Button{Text="Thoát",Width=100,Height=34,Anchor=AnchorStyles.Right};
        root.Controls.Add(exit);Controls.Add(root);

        show.Click+=async(_,_)=>await LoadData();
        exit.Click+=(_,_)=>Close();
    }

    async Task LoadData()
    {
        list.Items.Clear();
        await using var conn=Db.SinhVienConn();await conn.OpenAsync();
        await using var cmd=new SqlCommand("SELECT MaLop,TenLop,MaKhoa FROM Lop ORDER BY MaLop",conn);
        await using var rd=await cmd.ExecuteReaderAsync();
        int stt=1;
        while(await rd.ReadAsync())
        {
            list.Items.Add(new ListViewItem(new[]{
                (stt++).ToString(),
                rd["MaLop"].ToString()!,
                rd["TenLop"].ToString()!,
                rd["MaKhoa"].ToString()!
            }));
        }
    }
}
