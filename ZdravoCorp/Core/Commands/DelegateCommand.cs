using System;
using System.Windows.Input;

namespace ZdravoCorp.Core.Commands;

public class DelegateCommand : ICommand
{
    private readonly Predicate<object> canExecutePredicate;
    private readonly Action<object> executionAction;

    /// <param name="execute">The delegate to call on execution</param>
    public DelegateCommand(Action<object> execute)
        : this(execute, null)
    {
    }

    /// <param name="execute">The delegate to call on execution</param>
    /// <param name="canExecute">The predicate to determine if command is valid for execution</param>
    public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
    {
        if (execute == null) throw new ArgumentNullException("execute");

        executionAction = execute;
        canExecutePredicate = canExecute;
    }


    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    /// <param name="parameter">parameter to pass to predicate</param>
    public bool CanExecute(object parameter)
    {
        return canExecutePredicate == null ? true : canExecutePredicate(parameter);
    }


    /// <param name="parameter">parameter to pass to delegate</param>
    /// <exception cref="InvalidOperationException">Thrown if CanExecute returns false</exception>
    public void Execute(object parameter)
    {
        if (!CanExecute(parameter))
            throw new InvalidOperationException(
                "The command is not valid for execution, check the CanExecute method before attempting to execute.");

        executionAction(parameter);
    }

    public void RaiseCanExecuteChanged()
    {
    }
}