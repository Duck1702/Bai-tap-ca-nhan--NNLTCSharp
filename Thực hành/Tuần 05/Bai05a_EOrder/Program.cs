namespace Bai05a_EOrder;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new EOrderForm());
    }
}
