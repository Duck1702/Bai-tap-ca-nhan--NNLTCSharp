namespace Bai05e_MediaPlayer;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MediaPlayerForm());
    }
}
