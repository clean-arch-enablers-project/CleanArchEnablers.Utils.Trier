

using Cae.Utils.Trier.Exceptions;

namespace Cae.Utils.Trier;

public class TrierBuilder<T,TO>
{
    private readonly Actions.Action<T, TO> _action;
    private readonly T? _input;

    private List<Exception> ExceptionsToRetry { get; set; } = []; 
    private int MaxAmountOfRetries { get; set; } = 0;

    public TrierBuilder(Actions.Action<T, TO> action, T? input)
    {
        _action = action;
        _input = input;
    }

    public TrierBuilder<T, TO> AutoRetryOn<TE>(int maxAmountOfRetries) where TE : Exception, new()
    {
        MaxAmountOfRetries = maxAmountOfRetries;
        ExceptionsToRetry.Add(new TE());
        
        return this;
    }

    public Trier<T,TO> WithUnexpectedExceptionHandler(IUnexpectedExceptionHandler unexpectedExceptionHandler)
    {
        return new Trier<T, TO>(_action, unexpectedExceptionHandler, _input);
    }
}