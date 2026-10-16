namespace Bai05e_MDI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MdiMainForm());
    }
}
