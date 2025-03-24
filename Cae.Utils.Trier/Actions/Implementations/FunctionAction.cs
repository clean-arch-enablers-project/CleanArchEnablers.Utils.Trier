namespace Cae.Utils.Trier.Actions.Implementations;

public class FunctionAction<T, TO> : Action<T, TO>
{
    private readonly Func<T, Task<TO>>? _functionAsync;
    private readonly Func<T, TO>? _function;

    public FunctionAction(Func<T, TO> function) => _function = function;
    public FunctionAction(Func<T, Task<TO>> functionAsync) => _functionAsync = functionAsync;
    
    protected override TO ExecuteInternalAction(T input)
    {
        if (_function == null) throw new Exception();

        return _function(input);
    }

    protected override Task<TO> ExecuteInternalActionAsync(T input)
    {
        if (_functionAsync == null) throw new Exception();

        return _functionAsync(input);
    }
}