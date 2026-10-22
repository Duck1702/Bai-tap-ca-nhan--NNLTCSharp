using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Globalization;
using Microsoft.Data.SqlClient;
namespace Bai06c_QuanLySinhVien;

internal enum InputKind { Text, Date, Boolean, KhoaLookup, StudentLookup, SubjectLookup }
internal sealed record ColumnSpec(string Name, string Label, InputKind Kind = InputKind.Text);

internal sealed class TableEditorForm : Form
{
    private readonly string _tableName;
    private readonly ColumnSpec[] _fields;
    private readonly BindingSource _source = new();
    private readonly DataGridView _grid = new();
    private SqlDataAdapter? _adapter;
    private SqlCommandBuilder? _builder;
    private DataTable? _table;

    internal TableEditorForm(string title, string tableName, params ColumnSpec[] fields)
    {
        Text = title;
        _tableName = tableName; // Tên bảng chỉ được truyền từ cấu hình nội bộ (không lấy từ người dùng)
        _fields = fields;
        Width = 1050; Height = 670; MinimumSize = new Size(760, 510);
        StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Segoe UI", 10);

        var actionBar = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 48, Padding = new Padding(8), WrapContents = false };
        AddAction(actionBar, "Thêm", (_, _) => AddNew());
        AddAction(actionBar, "Xóa", (_, _) => DeleteSelected());
        AddAction(actionBar, "Lưu SQL Server", (_, _) => SaveData());
        AddAction(actionBar, "Hủy thay đổi", (_, _) => ReloadSafe());
        AddAction(actionBar, "Tải lại", (_, _) => ReloadSafe());

        var formArea = new TableLayoutPanel {
            Dock = DockStyle.Top, AutoSize = true, AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 4, Padding = new Padding(12), BackColor = Color.FromArgb(244,247,251)
        };
        formArea.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        formArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        formArea.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        formArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (int i = 0; i < _fields.Length; ++i)
        {
            var col = _fields[i]; int row = i / 2; int column = (i % 2) * 2;
            var label = new Label { Text = col.Label, TextAlign = ContentAlignment.MiddleRight,
                                    Dock = DockStyle.Fill, AutoSize = true, Margin = new Padding(2, 5, 8, 5) };
            Control editor = MakeInput(col);
            editor.Dock = DockStyle.Fill; editor.Margin = new Padding(3, 4, 12, 4);
            editor.MinimumSize = new Size(100, 28);
            formArea.Controls.Add(label, column, row);
            formArea.Controls.Add(editor, column + 1, row);
        }
        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true; _grid.AllowUserToAddRows = false;
        _grid.MultiSelect = false; _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.RowHeadersVisible = false; _grid.BackgroundColor = Color.White;
        _grid.DataSource = _source;
        _grid.DataError += (_, e) => { e.ThrowException = false; };
        Controls.Add(_grid);
        Controls.Add(formArea);
        Controls.Add(actionBar);
        Shown += (_, _) => ReloadSafe();
        FormClosed += (_, _) => { _builder?.Dispose(); _adapter?.Dispose(); _table?.Dispose(); _source.Dispose(); };
    }

    private static void AddAction(FlowLayoutPanel bar, string text, EventHandler clicked)
    {
        var button = new Button { Text = text, Width = text.Length > 12 ? 145 : 112, Height = 32 };
        button.Click += clicked; bar.Controls.Add(button);
    }

    private Control MakeInput(ColumnSpec col)
    {
        if (col.Kind == InputKind.Boolean)
        {
            var check = new CheckBox { Text = "Có", AutoSize = true };
            check.DataBindings.Add("Checked", _source, col.Name, true, DataSourceUpdateMode.OnValidation, false);
            return check;
        }
        if (col.Kind is InputKind.KhoaLookup or InputKind.StudentLookup or InputKind.SubjectLookup)
        {
            string sql = col.Kind switch {
                InputKind.KhoaLookup => "SELECT MaKhoa AS Ma, TenKhoa AS Ten FROM dbo.Khoa ORDER BY TenKhoa",
                InputKind.StudentLookup => "SELECT MaSo AS Ma, HoTen AS Ten FROM dbo.SinhVien ORDER BY HoTen",
                _ => "SELECT MaMH AS Ma, TenMH AS Ten FROM dbo.Mon ORDER BY TenMH"
            };
            var combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, FormattingEnabled = true };
            combo.Tag = sql;
            combo.DataBindings.Add("SelectedValue", _source, col.Name, true, DataSourceUpdateMode.OnValidation);
            return combo;
        }
        var text = new TextBox { PlaceholderText = col.Kind == InputKind.Date ? "yyyy-MM-dd" : "" };
        var binding = text.DataBindings.Add("Text", _source, col.Name, true, DataSourceUpdateMode.OnValidation, "");
        if (col.Kind == InputKind.Date)
        {
            binding.Format += (_, e) => {
                if (e.Value is DateTime dt) e.Value = dt.ToString("yyyy-MM-dd");
                else if (e.Value == DBNull.Value) e.Value = "";
            };
            binding.Parse += (_, e) => {
                var input = Convert.ToString(e.Value)?.Trim();
                e.Value = string.IsNullOrEmpty(input) ? DBNull.Value
                    : DateTime.ParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            };
        }
        return text;
    }

    private void ReloadSafe()
    {
        try { Reload(); } catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Error); }
    }
    private void Reload()
    {
        _source.DataSource = null;
        _builder?.Dispose(); _adapter?.Dispose(); _table?.Dispose();
        _table = new DataTable();
        _adapter = new SqlDataAdapter($"SELECT * FROM dbo.[{_tableName}]", SqlDatabase.ConnectionString);
        _adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
        _adapter.Fill(_table);
        _builder = new SqlCommandBuilder(_adapter) { QuotePrefix = "[", QuoteSuffix = "]" };
        _source.DataSource = _table;
        foreach (Control control in GetAllControls(this))
        {
            if (control is ComboBox cb && cb.Tag is string sql)
            {
                var lookup = SqlDatabase.Query(sql);
                cb.DisplayMember = "Ten"; cb.ValueMember = "Ma"; cb.DataSource = lookup;
            }
        }
        foreach (DataGridViewColumn col in _grid.Columns)
        {
            var spec = _fields.FirstOrDefault(f => f.Name == col.DataPropertyName);
            if (spec != null) col.HeaderText = spec.Label;
        }
    }
    private static IEnumerable<Control> GetAllControls(Control parent)
    {
        foreach (Control child in parent.Controls)
        {
            yield return child;
            foreach (var descendant in GetAllControls(child)) yield return descendant;
        }
    }
    private void AddNew()
    {
        try { _source.AddNew(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
    }
    private void DeleteSelected()
    {
        if (_source.Current is null) return;
        if (MessageBox.Show("Xóa bản ghi được chọn?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
        _source.RemoveCurrent();
    }
    private void SaveData()
    {
        try
        {
            ValidateChildren(); _source.EndEdit();
            if (_table is null || _adapter is null) return;
            _adapter.InsertCommand = _builder!.GetInsertCommand();
            _adapter.UpdateCommand = _builder.GetUpdateCommand();
            _adapter.DeleteCommand = _builder.GetDeleteCommand();
            int changed = _adapter.Update(_table);
            MessageBox.Show($"Đã ghi {changed} thay đổi vào SQL Server.");
            Reload();
        }
        catch (Exception ex) { MessageBox.Show(ex.Message, "Không thể lưu", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
