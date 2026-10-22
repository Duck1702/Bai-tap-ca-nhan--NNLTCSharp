using Microsoft.Data.SqlClient;
using Lab05d.Database;

namespace Lab05d;

public class Bai7_QLDiemForm : Form
{
    readonly TextBox maSV = new();
    readonly TextBox maMon = new();
    readonly TextBox diem = new();

    public Bai7_QLDiemForm()
    {
        Text = "Quản lý Điểm";
        StartPosition = FormStartPosition.CenterParent;
        AutoScaleMode = AutoScaleMode.Dpi;
        Size = new Size(360, 250);
        MinimumSize = Size;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var root = Ui.Root();
        root.RowCount = 4;
        for (int i = 0; i < 4; i++) root.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        root.Controls.Add(DbUi.Row("Mã sinh viên:", maSV));
        root.Controls.Add(DbUi.Row("Mã môn học:", maMon));
        root.Controls.Add(DbUi.Row("Điểm:", diem));

        var add = new Button { Text = "Thêm" };
        var del = new Button { Text = "Xóa" };
        var edit = new Button { Text = "Sửa" };
        root.Controls.Add(DbUi.Buttons(add, del, edit));
        Controls.Add(root);

        add.Click += async (_, _) => await Them();
        del.Click += async (_, _) => await Xoa();
        edit.Click += async (_, _) => await Sua();
    }

    bool Input(bool needScore, out string sv, out string mon, out double score)
    {
        sv = maSV.Text.Trim(); mon = maMon.Text.Trim(); score = 0;
        if (sv == "" || mon == "") { MessageBox.Show("Mã SV và Mã môn học không được trống."); return false; }
        if (needScore && (!double.TryParse(diem.Text, out score) || score < 0 || score > 10))
        { MessageBox.Show("Điểm phải là số từ 0 đến 10."); return false; }
        return true;
    }

    async Task Run(string sql, string sv, string mon, double? score, string msg)
    {
        try
        {
            await using var conn = Db.CreateConnection(); await conn.OpenAsync();
            await using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@SV", sv);
            cmd.Parameters.AddWithValue("@Mon", mon);
            if (score.HasValue) cmd.Parameters.AddWithValue("@Diem", score.Value);
            int n = await cmd.ExecuteNonQueryAsync();
            MessageBox.Show(n > 0 ? msg : "Không tìm thấy dữ liệu.");
        }
        catch (Exception ex) { MessageBox.Show("Có lỗi:\n" + ex.Message); }
    }

    async Task Them()
    {
        if (!Input(true, out string sv, out string mon, out double sc)) return;
        await Run("INSERT INTO Diem(MaSinhVien,MaMonHoc,Diem) VALUES(@SV,@Mon,@Diem)", sv, mon, sc, "Thêm điểm thành công.");
    }

    async Task Xoa()
    {
        if (!Input(false, out string sv, out string mon, out _)) return;
        if (diem.Text.Trim() != "") { MessageBox.Show("Khi xóa không nhập Điểm."); return; }
        await Run("DELETE FROM Diem WHERE MaSinhVien=@SV AND MaMonHoc=@Mon", sv, mon, null, "Xóa điểm thành công.");
    }

    async Task Sua()
    {
        if (!Input(true, out string sv, out string mon, out double sc)) return;
        await Run("UPDATE Diem SET Diem=@Diem WHERE MaSinhVien=@SV AND MaMonHoc=@Mon", sv, mon, sc, "Sửa điểm thành công.");
    }
}
