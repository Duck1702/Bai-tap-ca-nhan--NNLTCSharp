namespace Lab05c;

public class Bai1UocSoForm : Form
{
    readonly TextBox txtNhap = new();
    readonly ComboBox cboSo = new() { DropDownStyle = ComboBoxStyle.DropDownList };
    readonly ListBox lstUoc = new();

    public Bai1UocSoForm()
    {
        Text = "Tại lớp 1 - Xử lý ước số";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(570, 440);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 1, RowCount = 5 };
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        var input = new FlowLayoutPanel { AutoSize = true, Margin = new Padding(0, 0, 0, 10) };
        input.Controls.Add(new Label { Text = "Nhập số", AutoSize = true, Margin = new Padding(3, 7, 8, 3) });
        txtNhap.Width = 130;
        input.Controls.Add(txtNhap);
        input.Controls.Add(Ui.Btn("&Cập nhật", (_, _) => CapNhat(), 95));
        root.Controls.Add(input);

        var lists = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Margin = new Padding(0, 0, 0, 10) };
        lists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 42));
        lists.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 58));
        cboSo.Dock = DockStyle.Fill;
        lstUoc.Dock = DockStyle.Fill;
        lists.Controls.Add(cboSo, 0, 0);
        lists.Controls.Add(lstUoc, 1, 0);
        root.Controls.Add(lists);

        var actions = new FlowLayoutPanel { AutoSize = true, Anchor = AnchorStyles.None };
        actions.Controls.Add(Ui.Btn("&Tổng các ước số", (_, _) => MessageBox.Show($"Tổng các ước số = {lstUoc.Items.Cast<int>().Sum()}"), 125));
        actions.Controls.Add(Ui.Btn("Số lượng các ước số &chẵn", (_, _) => MessageBox.Show($"Số lượng ước chẵn = {lstUoc.Items.Cast<int>().Count(x => x % 2 == 0)}"), 165));
        root.Controls.Add(actions);

        var prime = Ui.Btn("Số lượng các ước số &nguyên tố", (_, _) => MessageBox.Show($"Số lượng ước nguyên tố = {lstUoc.Items.Cast<int>().Count(LaNguyenTo)}"), 205);
        prime.Anchor = AnchorStyles.None;
        root.Controls.Add(prime);
        var exit = Ui.Btn("&Thoát", (_, _) => Close(), 85);
        exit.Anchor = AnchorStyles.None;
        root.Controls.Add(exit);
        Controls.Add(root);

        cboSo.SelectedIndexChanged += (_, _) => HienThiUoc();
        FormClosing += (_, e) =>
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                e.Cancel = true;
        };
    }

    void CapNhat()
    {
        if (!int.TryParse(txtNhap.Text, out int n) || n <= 0) { MessageBox.Show("Chỉ nhập số nguyên dương."); txtNhap.SelectAll(); txtNhap.Focus(); return; }
        if (cboSo.Items.Cast<int>().Contains(n)) { MessageBox.Show("Số này đã tồn tại."); txtNhap.SelectAll(); txtNhap.Focus(); return; }
        cboSo.Items.Add(n); cboSo.SelectedItem = n; txtNhap.Clear(); txtNhap.Focus();
    }
    void HienThiUoc() { if (cboSo.SelectedItem is not int n) return; lstUoc.Items.Clear(); for (int i = 1; i <= n; i++) if (n % i == 0) lstUoc.Items.Add(i); }
    static bool LaNguyenTo(int n) { if (n < 2) return false; for (int i = 2; i * i <= n; i++) if (n % i == 0) return false; return true; }
}
