namespace Cae.Utils.Trier.Actions;

public abstract class Action<T, TO>
{
    public TO Execute(T? input = default)
    {
        return ExecuteInternalAction(input);
    }

    public Task<TO> ExecuteAsync(T? input = default)
    {
        return ExecuteInternalActionAsync(input);
    }

    protected abstract TO ExecuteInternalAction(T input);
    protected abstract Task<TO> ExecuteInternalActionAsync(T input);
}