using System.Windows.Forms;
namespace Bai06d_QLThuVien_1Lop;
internal static class Program
{
    [STAThread]
    static void Main() { ApplicationConfiguration.Initialize(); Application.Run(new FrmNhanVien()); }
}
