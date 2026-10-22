using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Globalization;
using Bai06d_QLThuVien_3Lop.BLL;
namespace Bai06d_QLThuVien_3Lop.GUI;

// GUI: chỉ tạo và xử lý control, toàn bộ kiểm tra nghiệp vụ tại BLL và SQL tại DAL.
internal sealed class FrmDanhMuc : Form
{
    private readonly CatalogService service;
    private readonly DataGridView grid=new() {
        Dock=DockStyle.Fill,ReadOnly=true,AllowUserToAddRows=false,
        SelectionMode=DataGridViewSelectionMode.FullRowSelect,MultiSelect=false,
        BackgroundColor=Color.White,AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill,
        RowHeadersVisible=false
    };
    private readonly Dictionary<string,Control> inputs=new();
    private Dictionary<string,object>? originalKeys;
    private readonly Label stateLabel=new() { AutoSize=true,Padding=new Padding(12,11,0,0),Text="Thêm bản ghi mới" };

    internal FrmDanhMuc(CatalogService business)
    {
        service=business;Text=service.Spec.Caption;
        Width=1060;Height=680;MinimumSize=new Size(850,520);
        Font=new Font("Segoe UI",10);
        var title=new Label { Dock=DockStyle.Top,Height=45,Text=service.Spec.Caption.ToUpperInvariant(),
            TextAlign=ContentAlignment.MiddleCenter,
            Font=new Font("Segoe UI",15,FontStyle.Bold),ForeColor=Color.MidnightBlue };
        int rows=(service.Spec.Fields.Length+1)/2;
        var details=new TableLayoutPanel { Dock=DockStyle.Top,
            Height=rows*43+16,Padding=new Padding(8),ColumnCount=4,BackColor=Color.FromArgb(244,247,251)
        };
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,140));
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute,140));
        details.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
        for(int i=0;i<service.Spec.Fields.Length;++i)
        {
            FieldSpec col=service.Spec.Fields[i];int row=i/2,intCol=i%2*2;
            details.Controls.Add(new Label { Text=col.Label,TextAlign=ContentAlignment.MiddleRight,
                Dock=DockStyle.Fill,AutoSize=true,Margin=new Padding(2,9,7,3) },intCol,row);
            Control ctrl=col.Type==FieldType.Lookup ? new ComboBox { DropDownStyle=ComboBoxStyle.DropDownList } :
                new TextBox { PlaceholderText=col.Type==FieldType.Date ? "yyyy-MM-dd" : "" };
            if(ctrl is TextBox input && col.IsIdentity)input.ReadOnly=true;
            ctrl.Dock=DockStyle.Fill;ctrl.Margin=new Padding(3,6,12,5);
            ctrl.MinimumSize=new Size(80,28);inputs[col.Name]=ctrl;
            details.Controls.Add(ctrl,intCol+1,row);
        }
        var controls=new FlowLayoutPanel { Dock=DockStyle.Top,Height=51,Padding=new Padding(8) };
        AddButton(controls,"Thêm mới",(_,_)=>ClearFields());
        AddButton(controls,"Lưu",(_,_)=>Save());
        AddButton(controls,"Xóa",(_,_)=>Delete());
        AddButton(controls,"Tải lại",(_,_)=>ReloadSafe());
        AddButton(controls,"Thoát",(_,_)=>Close());
        controls.Controls.Add(stateLabel);
        grid.SelectionChanged+=(_,_)=>ReadSelected();
        grid.DataError+=(_,e)=>{e.ThrowException=false;};
        Controls.Add(grid);Controls.Add(controls);Controls.Add(details);Controls.Add(title);
        Shown+=(_,_)=>ReloadSafe();
    }
    private static void AddButton(FlowLayoutPanel bar,string name,EventHandler handler)
    {
        var b=new Button { Text=name,Width=110,Height=33 };
        b.Click+=handler;bar.Controls.Add(b);
    }
    private void ReloadSafe()
    {
        try {
            // Giữ dữ liệu các ComboBox bằng DataSource riêng để không tự đồng bộ vị trí chọn.
            foreach(FieldSpec f in service.Spec.Fields.Where(f=>f.Type==FieldType.Lookup))
            {
                var cbo=(ComboBox)inputs[f.Name];
                cbo.DisplayMember="Caption";cbo.ValueMember="Id";
                cbo.DataSource=service.LayDanhMucLienKet(f);
            }
            grid.DataSource=service.LayDanhSach();
            foreach(DataGridViewColumn column in grid.Columns)
            {
                FieldSpec? f=service.Spec.Fields.FirstOrDefault(x=>x.Name==column.DataPropertyName);
                if(f!=null)column.HeaderText=f.Label;
            }
            if(grid.Rows.Count==0)ClearFields(); else ReadSelected();
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Không đọc được SQL Server",MessageBoxButtons.OK,MessageBoxIcon.Error); }
    }
    private void ReadSelected()
    {
        if(grid.CurrentRow?.DataBoundItem is not DataRowView row)return;
        originalKeys=service.Spec.Fields.Where(x=>x.IsKey)
            .ToDictionary(x=>x.Name,x=>row[x.Name]);
        foreach(FieldSpec field in service.Spec.Fields)
        {
            Control ctrl=inputs[field.Name];object obj=row[field.Name];
            if(ctrl is ComboBox cbo)
            {
                if(obj==DBNull.Value)cbo.SelectedIndex=-1;
                else cbo.SelectedValue=Convert.ToInt32(obj);
                cbo.Enabled=!field.IsKey;
            }
            else if(ctrl is TextBox text)
            {
                text.Text=obj==DBNull.Value ? "" :
                    field.Type==FieldType.Date ? Convert.ToDateTime(obj).ToString("yyyy-MM-dd") :
                    field.Type==FieldType.Number ? Convert.ToDouble(obj).ToString(CultureInfo.InvariantCulture) :
                    Convert.ToString(obj);
                text.ReadOnly=field.IsKey;
            }
        }
        stateLabel.Text="Đang sửa bản ghi";
    }
    private void ClearFields()
    {
        originalKeys=null;grid.ClearSelection();
        foreach(FieldSpec field in service.Spec.Fields)
        {
            Control ctrl=inputs[field.Name];
            if(ctrl is ComboBox combo){combo.Enabled=true;combo.SelectedIndex=combo.Items.Count>0?0:-1;}
            else if(ctrl is TextBox txt){txt.Clear();txt.ReadOnly=field.IsIdentity;}
        }
        stateLabel.Text="Thêm bản ghi mới";
    }
    private Dictionary<string,object?> GetValues()
    {
        var data=new Dictionary<string,object?>();
        foreach(FieldSpec f in service.Spec.Fields)
        {
            Control ctrl=inputs[f.Name];
            data[f.Name]=ctrl is ComboBox cbo ? cbo.SelectedValue : ((TextBox)ctrl).Text;
        }
        return data;
    }
    private void Save()
    {
        try {
            var values=GetValues();
            if(originalKeys==null) {service.Them(values);MessageBox.Show("Đã thêm dữ liệu.");}
            else {service.Sua(values,originalKeys);MessageBox.Show("Đã cập nhật dữ liệu.");}
            ReloadSafe();
        } catch(Exception ex) { MessageBox.Show(ex.Message,"Lỗi lưu dữ liệu",MessageBoxButtons.OK,MessageBoxIcon.Warning); }
    }
    private void Delete()
    {
        if(originalKeys==null){MessageBox.Show("Chọn bản ghi cần xóa.");return;}
        if(MessageBox.Show("Xóa bản ghi đang chọn?","Xác nhận",MessageBoxButtons.YesNo)!=DialogResult.Yes)return;
        try {service.Xoa(originalKeys);ReloadSafe();MessageBox.Show("Đã xóa.");}
        catch(Exception ex){MessageBox.Show(ex.Message,"Không thể xóa (có thể do khóa ngoại)");}
    }
}
