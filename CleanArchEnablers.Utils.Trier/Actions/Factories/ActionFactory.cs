using CleanArchEnablers.Utils.Trier.Actions.Implementations;
using Types_VoidReturn = CleanArchEnablers.Utils.Trier.Types.VoidReturn;
using VoidReturn = CleanArchEnablers.Utils.Trier.Types.VoidReturn;

namespace CleanArchEnablers.Utils.Trier.Actions.Factories;

public static class ActionFactory
{
    #region FunctionActionFactory

    public static Action<T, TO> CreateInstance<T, TO>(Func<T, TO> action)
    {
        return new FunctionAction<T, TO>(action);
    }

    public static Action<T, TO> CreateInstance<T, TO>(Func<T, Task<TO>> action)
    {
        return new FunctionAction<T, TO>(action);
    }

    #endregion

    #region ConsumerActionFactory

    public static Action<T, Types_VoidReturn?> CreateInstance<T>(Action<T> consumer)
    {
        return new ConsumerAction<T>(consumer);
    }

    public static Action<T, Types_VoidReturn?> CreateInstance<T>(Func<T, Task> consumerAsync)
    {
        return new ConsumerAction<T>(consumerAsync);
    }

    #endregion

    #region RunnableActionFactory

    public static Action<Types_VoidReturn?, Types_VoidReturn?> CreateInstance(Action action)
    {
        return new RunnableAction(action);
    }
    
    public static Action<Types_VoidReturn?, Types_VoidReturn?> CreateInstance(Func<Task> action)
    {
        return new RunnableAction(action);
    }

    #endregion

    #region SupplierActionFactory

    public static Action<Types_VoidReturn?, TO> CreateInstance<TO>(Func<TO> action)
    {
        return new SupplierAction<TO>(action);
    }

    public static Action<Types_VoidReturn?, TO> CreateInstance<TO>(Func<Task<TO>> action)
    {
        return new SupplierAction<TO>(action);
    }

    #endregion
}