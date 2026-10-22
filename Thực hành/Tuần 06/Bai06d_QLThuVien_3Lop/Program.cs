using System.Windows.Forms;
namespace Bai06d_QLThuVien_3Lop;
internal static class Program
{
    [STAThread]
    static void Main() { ApplicationConfiguration.Initialize(); Application.Run(new GUI.MainForm()); }
}
