namespace Sever.Gridder.CommandSystem
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}