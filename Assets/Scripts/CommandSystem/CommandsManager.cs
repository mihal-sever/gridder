using System.Collections.Generic;

namespace Sever.Gridder.CommandSystem
{
    public static class CommandsManager
    {
        private static readonly Stack<ICommand> _commands = new();
        
        
        public static void Add(ICommand command)
        {
            _commands.Push(command);
            command.Execute();
        }

        public static void Undo()
        {
            if (_commands.TryPop(out var lastCommand))
            {
                lastCommand.Undo();
            }
        }

        public static void ClearHistory()
        {
            _commands.Clear();
        }
    }
}