using Cae.Utils.Trier;
using Cae.Utils.Trier.Exceptions;
using Actions = Cae.Utils.Trier.Actions;

public class TrierBuilder<T, TO>
{
    private readonly Actions.Action<T, TO> _action;
    private readonly T? _input;

    private List<Type> ExceptionsToRetry { get; set; } = [];
    private int MaxAmountOfRetries { get; set; } = 0;

    public TrierBuilder(Actions.Action<T, TO> action, T? input)
    {
        _action = action;
        _input = input;
    }

    public TrierBuilder<T, TO> AutoRetryOn<TE>(int maxAmountOfRetries) where TE : Exception
    {
        MaxAmountOfRetries = maxAmountOfRetries;
        ExceptionsToRetry.Add(typeof(TE));

        return this;
    }

    public Trier<T, TO> WithUnexpectedExceptionHandler(IUnexpectedExceptionHandler unexpectedExceptionHandler)
    {
        return new Trier<T, TO>(_action, unexpectedExceptionHandler, _input, MaxAmountOfRetries, ExceptionsToRetry);
    }
}