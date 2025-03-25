namespace Cae.Utils.Trier.Actions;

public abstract class Action<T,TO>
{
    public TO Execute(T? input = default)
    {
        try
        {
            return ExecuteInternalAction(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<TO> ExecuteAsync(T? input = default)
    {
        try
        {
            return ExecuteInternalActionAsync(input);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    protected abstract TO ExecuteInternalAction(T input);
    protected abstract Task<TO> ExecuteInternalActionAsync(T input);
}