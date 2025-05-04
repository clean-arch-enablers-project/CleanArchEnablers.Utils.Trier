using CleanArchEnablers.Utils.Trier;
using CleanArchEnablers.Utils.Trier.Exceptions.Handlers;
using Actions = CleanArchEnablers.Utils.Trier.Actions;

namespace CleanArchEnablers.Utils.Trier;

public class TrierBuilder<T, TO>
{
    private readonly Actions.Action<T, TO> _action;
    private readonly T? _input;
    private readonly Dictionary<Type, int> _retryLimits = new(); // Armazena os limites de retry

    public TrierBuilder(Actions.Action<T, TO> action, T? input)
    {
        _action = action;
        _input = input;
    }

    public TrierBuilder<T, TO> AutoRetryOn<TE>(int maxAmountOfRetries) where TE : Exception
    {
        _retryLimits[typeof(TE)] = maxAmountOfRetries;
        return this;
    }

    public Trier<T, TO> WithUnexpectedExceptionHandler(IUnexpectedExceptionHandler unexpectedExceptionHandler)
    {
        return new Trier<T, TO>(_action, unexpectedExceptionHandler, _input, _retryLimits);
    }
}
