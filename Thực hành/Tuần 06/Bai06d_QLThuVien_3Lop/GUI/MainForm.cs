using System.Windows.Forms;
using System.Drawing;
using Bai06d_QLThuVien_3Lop.BLL;
namespace Bai06d_QLThuVien_3Lop.GUI;

internal sealed class MainForm : Form
{
    private readonly MenuStrip menu = new();
    private readonly ToolStrip toolbar = new() { GripStyle=ToolStripGripStyle.Hidden };
    private readonly StatusStrip status = new();
    internal MainForm()
    {
        Text="BÀI THỰC HÀNH 06d - QUẢN LÝ THƯ VIỆN - MÔ HÌNH 3 LỚP";
        Width=1280;Height=800;WindowState=FormWindowState.Maximized;IsMdiContainer=true;
        var categories=new ToolStripMenuItem("Danh mục");
        AddMenu(categories,"Bằng cấp",()=>new BangCap());
        AddMenu(categories,"Nhân viên",()=>new NhanVien());
        AddMenu(categories,"Độc giả",()=>new DocGia());
        AddMenu(categories,"Sách",()=>new Sach());
        AddMenu(categories,"Tham số",()=>new ThamSo());
        var transactions=new ToolStripMenuItem("Nghiệp vụ");
        AddMenu(transactions,"Phiếu mượn",()=>new PhieuMuonSach());
        AddMenu(transactions,"Chi tiết phiếu mượn",()=>new ChiTietPhieuMuon());
        AddMenu(transactions,"Phiếu thu tiền",()=>new PhieuThuTien());
        var direct=new ToolStripMenuItem("DataGrid / DataAdapter");
        direct.Click+=(_,_)=>OpenForm("DocGiaGrid",()=>new FrmDocGiaDataGrid());
        var exit=new ToolStripMenuItem("Thoát");exit.Click+=(_,_)=>Close();
        menu.Items.AddRange(new ToolStripItem[]{categories,transactions,direct,exit});
        foreach(var x in new (string title,Func<CatalogService> make)[]{
            ("Nhân viên",()=>new NhanVien()), ("Bằng cấp",()=>new BangCap()),
            ("Độc giả",()=>new DocGia()), ("Sách",()=>new Sach()),
            ("Phiếu mượn",()=>new PhieuMuonSach()), ("Thu tiền",()=>new PhieuThuTien())
        }) {
            var button=new ToolStripButton(x.title);
            button.Click+=(_,_)=>OpenCatalog(x.make());
            toolbar.Items.Add(button);
        }
        var statusLabel=new ToolStripStatusLabel("QLTHUVIEN | ADO.NET | DAL - BLL - GUI")
        { Spring=true,TextAlign=ContentAlignment.MiddleLeft };
        status.Items.Add(statusLabel);
        Controls.Add(status);Controls.Add(toolbar);Controls.Add(menu);MainMenuStrip=menu;
    }
    private void AddMenu(ToolStripMenuItem parent,string name,Func<CatalogService> make)
    {
        var item=new ToolStripMenuItem(name);item.Click+=(_,_)=>OpenCatalog(make());parent.DropDownItems.Add(item);
    }
    private void OpenCatalog(CatalogService service) => OpenForm(service.Spec.Table,()=>new FrmDanhMuc(service));
    private void OpenForm(string key,Func<Form> create)
    {
        var current=MdiChildren.FirstOrDefault(f=>Equals(f.Tag,key));
        if(current!=null) {current.Activate();return;}
        var child=create();child.Tag=key;child.MdiParent=this;child.Show();
    }
}
