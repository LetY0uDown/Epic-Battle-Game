using System;
using System.Windows.Input;

namespace WPF_Client_Library;

public class Command : ICommand
{
    protected readonly Action<object> _execute;
    protected readonly Func<object, bool> _canExecute;

    /// <summary>
    /// Constructor for a commands with no execution conditions
    /// </summary>
    /// <param name="execute">Defines the method to be called when the command is invoked</param>
    public Command(Action<object> execute)
    {
        _execute = execute;
    }

    /// <summary>
    /// Constructor for a commands with an execution conditions
    /// </summary>
    /// <param name="execute">Defines the method to be called when the command is invoked</param>
    /// <param name="canExecute">Defines the method that determines whether the command can execute in its current state</param>
    public Command(Action<object> execute, Func<object, bool> canExecute) : this(execute)
    {
        _canExecute = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public virtual bool CanExecute(object parameter)
    {
        return _canExecute is null || _canExecute(parameter);
    }

    public virtual void Execute(object parameter)
    {
        _execute(parameter);
    }
}
