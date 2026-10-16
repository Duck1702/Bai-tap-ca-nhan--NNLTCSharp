namespace Bai05b_QuanLySinhVien;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new QuanLySinhVienForm());
    }
}
