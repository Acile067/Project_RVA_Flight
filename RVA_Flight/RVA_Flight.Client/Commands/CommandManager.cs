using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RVA_Flight.Client.Commands
{
    public class CommandManager
    {
       private Stack<IFlightCommand> undoStack = new Stack<IFlightCommand>();
       private Stack<IFlightCommand> redoStack = new Stack<IFlightCommand>();

        public void ExecuteCommand(IFlightCommand command)
        {
            command.Execute();
            undoStack.Push(command);
        }

        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                IFlightCommand command = undoStack.Pop();
                command.Undo();
                redoStack.Push(command);
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                IFlightCommand command = redoStack.Pop();
                command.Execute();
                undoStack.Push(command);
            }
        }

    }
}
