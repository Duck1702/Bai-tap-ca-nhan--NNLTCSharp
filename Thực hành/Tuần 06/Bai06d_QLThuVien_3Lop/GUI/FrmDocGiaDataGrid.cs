using System.Windows.Forms;
using System.Drawing;
using System.Data;
using Microsoft.Data.SqlClient;
using Bai06d_QLThuVien_3Lop.DAL;
namespace Bai06d_QLThuVien_3Lop.GUI;

// Bài tập 5: cập nhật trực tiếp DataGridView với SqlDataAdapter và SqlCommandBuilder.
internal sealed class FrmDocGiaDataGrid : Form
{
    private readonly Database database=new();
    private readonly DataGridView grid=new() { Dock=DockStyle.Fill,BackgroundColor=Color.White,
        AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,RowHeadersVisible=false };
    private DataTable? table;
    internal FrmDocGiaDataGrid()
    {
        Text="Nhập danh mục độc giả bằng DataGridView / DataAdapter";Width=1100;Height=620;
        var commands=new FlowLayoutPanel { Dock=DockStyle.Top,Height=48,Padding=new Padding(7) };
        var save=new Button { Text="Lưu xuống SQL Server",Width=180,Height=32 };
        var reload=new Button { Text="Tải lại",Width=115,Height=32 };
        var exit=new Button { Text="Thoát",Width=110,Height=32 };
        save.Click+=(_,_)=>SaveData();reload.Click+=(_,_)=>Reload();exit.Click+=(_,_)=>Close();
        commands.Controls.Add(save);commands.Controls.Add(reload);commands.Controls.Add(exit);
        grid.DataError+=(_,e)=>{e.ThrowException=false;MessageBox.Show(e.Exception?.Message??"Dữ liệu không hợp lệ");};
        Controls.Add(grid);Controls.Add(commands);Shown+=(_,_)=>Reload();
    }
    private void Reload()
    {
        try {
            using var adapter=new SqlDataAdapter("SELECT * FROM dbo.DOCGIA",database.ConnectionString);
            adapter.MissingSchemaAction=MissingSchemaAction.AddWithKey;
            table=new DataTable();adapter.Fill(table);
            grid.DataSource=table;
            if(grid.Columns.Contains("MaDocGia"))grid.Columns["MaDocGia"]!.ReadOnly=true;
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Lỗi tải dữ liệu"); }
    }
    private void SaveData()
    {
        if(table is null)return;
        try {
            grid.EndEdit();
            if(grid.DataSource is DataTable dt)
            {
                int change=database.Update("DOCGIA",dt);
                MessageBox.Show($"Đã cập nhật {change} bản ghi bằng SqlDataAdapter.");
                Reload();return;
            }
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Không thể ghi SQL Server"); }
    }
}
