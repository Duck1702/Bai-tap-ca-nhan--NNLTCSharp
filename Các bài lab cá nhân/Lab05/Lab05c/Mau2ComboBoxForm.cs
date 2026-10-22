namespace Lab05c;

public class Mau2ComboBoxForm : Form
{
    readonly ComboBox cbo = new();
    readonly Label ketQua = new() { AutoSize = true, ForeColor = Color.Red, Font = new Font("Segoe UI", 11, FontStyle.Bold) };

    public Mau2ComboBoxForm()
    {
        Text = "Mẫu 2 - ComboBox dân tộc";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(560, 340);
        MinimumSize = new Size(460, 290);

        var root = Ui.Root();
        root.Controls.Add(Ui.Title("COMBOBOX DÂN TỘC"));

        var btnLoad = Ui.Btn("Load dữ liệu ComboBox", (_, _) => LoadData(), 190);
        btnLoad.Anchor = AnchorStyles.None;
        root.Controls.Add(btnLoad);

        cbo.DropDownStyle = ComboBoxStyle.DropDownList;
        root.Controls.Add(Ui.Row("Dân tộc:", cbo));

        var btnHienThi = Ui.Btn("Hiển thị", (_, _) => HienThi(), 120);
        btnHienThi.Anchor = AnchorStyles.None;
        root.Controls.Add(btnHienThi);

        ketQua.Anchor = AnchorStyles.None;
        root.Controls.Add(ketQua);

        Controls.Add(root);

        cbo.SelectedIndexChanged += (_, _) =>
        {
            if (cbo.SelectedIndex >= 0)
                MessageBox.Show("Dân tộc được chọn: " + cbo.SelectedItem);
        };
    }

    void LoadData()
    {
        cbo.Items.Clear();
        cbo.Items.AddRange(new object[] { "Kinh", "Hoa", "K’Me", "H’Mong", "Khác" });
        if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
    }

    void HienThi()
    {
        ketQua.Text = cbo.SelectedIndex >= 0
            ? "Dân tộc được chọn: " + cbo.SelectedItem
            : "Bạn chưa chọn dân tộc";
    }
}
