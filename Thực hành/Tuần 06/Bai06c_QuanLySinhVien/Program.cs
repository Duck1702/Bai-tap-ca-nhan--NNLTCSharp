using System;
using System.Windows.Forms;
namespace Bai06c_QuanLySinhVien;
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
