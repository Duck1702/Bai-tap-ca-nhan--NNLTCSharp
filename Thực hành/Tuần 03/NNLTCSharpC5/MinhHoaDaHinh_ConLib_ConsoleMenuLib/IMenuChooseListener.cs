namespace NNLTCS.UTILS.ConsoleLib
{
    public interface IMenuChooseListener
    {
        void MenuChoose(
            object sender,
            MenuChooseEventArgs args
        );
    }
}