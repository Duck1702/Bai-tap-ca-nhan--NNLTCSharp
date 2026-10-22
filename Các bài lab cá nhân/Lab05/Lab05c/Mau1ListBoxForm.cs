namespace Lab05c;

public class Mau1ListBoxForm : Form
{
    readonly ListBox trai = new() { SelectionMode = SelectionMode.MultiExtended };
    readonly ListBox phai = new() { SelectionMode = SelectionMode.MultiExtended };

    public Mau1ListBoxForm()
    {
        Text = "Mẫu 1 - Sử dụng ListBox";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(720, 520);
        MinimumSize = new Size(620, 430);

        trai.Items.AddRange(new object[] { "Cóc", "Ổi", "Xoài", "Me", "Bưởi", "Cam" });

        var root = Ui.Root(3);
        trai.Dock = DockStyle.Fill;
        phai.Dock = DockStyle.Fill;
        root.Controls.Add(trai, 0, 0);

        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, Anchor = AnchorStyles.None, AutoSize = true };
        buttons.Controls.Add(Ui.Btn(">", (_, _) => ChuyenDaChon(trai, phai), 60));
        buttons.Controls.Add(Ui.Btn(">>", (_, _) => ChuyenTatCa(trai, phai), 60));
        buttons.Controls.Add(Ui.Btn("<", (_, _) => ChuyenDaChon(phai, trai), 60));
        buttons.Controls.Add(Ui.Btn("<<", (_, _) => ChuyenTatCa(phai, trai), 60));
        buttons.Controls.Add(Ui.Btn("Chuyển tùy ý", (_, _) => ChuyenDaChon(trai, phai), 120));
        root.Controls.Add(buttons, 1, 0);
        root.Controls.Add(phai, 2, 0);
        Controls.Add(root);

        FormClosing += (_, e) =>
        {
            if (MessageBox.Show("Bạn có chắc muốn đóng Form?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        };
    }

    static void ChuyenDaChon(ListBox from, ListBox to)
    {
        if (from.SelectedItems.Count == 0)
        {
            MessageBox.Show("Vui lòng chọn phần tử.");
            return;
        }

        foreach (object item in from.SelectedItems.Cast<object>().ToList())
        {
            to.Items.Add(item);
            from.Items.Remove(item);
        }
    }

    static void ChuyenTatCa(ListBox from, ListBox to)
    {
        foreach (object item in from.Items.Cast<object>().ToList())
            to.Items.Add(item);
        from.Items.Clear();
    }
}
