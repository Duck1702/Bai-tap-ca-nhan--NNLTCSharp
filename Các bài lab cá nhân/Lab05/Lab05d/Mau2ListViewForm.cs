namespace Lab05d;

public class Mau2ListViewForm : Form
{
    readonly TextBox ma = new();
    readonly TextBox ten = new();
    readonly ListView list = new();

    public Mau2ListViewForm()
    {
        Text = "Mẫu 2 - ListView";
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(700, 520);
        MinimumSize = new Size(580, 420);

        var root = Ui.Root();
        var actions = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        actions.Controls.Add(Ui.Btn("Thêm", (_, _) => Them()));
        actions.Controls.Add(Ui.Btn("Xóa", (_, _) => Xoa()));
        actions.Controls.Add(Ui.Btn("Sửa", (_, _) => Sua()));
        root.Controls.Add(actions);
        root.Controls.Add(Ui.Row("Mã SV:", ma));
        root.Controls.Add(Ui.Row("Họ tên:", ten));

        list.Dock = DockStyle.Fill;
        list.View = View.Details;
        list.FullRowSelect = true;
        list.GridLines = true;
        list.Columns.Add("MÃ SỐ SV", 180);
        list.Columns.Add("HỌ VÀ TÊN", 300);
        root.Controls.Add(list);

        Controls.Add(root);
        list.SelectedIndexChanged += (_, _) =>
        {
            if (list.SelectedItems.Count != 1) return;
            ma.Text = list.SelectedItems[0].SubItems[0].Text;
            ten.Text = list.SelectedItems[0].SubItems[1].Text;
        };
    }

    void Them()
    {
        if (ma.Text.Trim() == "" || ten.Text.Trim() == "")
        { MessageBox.Show("Không được để trống."); return; }

        if (list.Items.Cast<ListViewItem>().Any(i => i.Text.Equals(ma.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
        { MessageBox.Show("Mã sinh viên đã tồn tại."); return; }

        list.Items.Add(new ListViewItem(new[] { ma.Text.Trim(), ten.Text.Trim() }));
        ma.Clear(); ten.Clear(); ma.Focus();
    }

    void Xoa()
    {
        foreach (ListViewItem i in list.SelectedItems.Cast<ListViewItem>().ToList()) i.Remove();
    }

    void Sua()
    {
        if (list.SelectedItems.Count != 1) { MessageBox.Show("Chọn một dòng cần sửa."); return; }
        list.SelectedItems[0].SubItems[0].Text = ma.Text.Trim();
        list.SelectedItems[0].SubItems[1].Text = ten.Text.Trim();
    }
}
