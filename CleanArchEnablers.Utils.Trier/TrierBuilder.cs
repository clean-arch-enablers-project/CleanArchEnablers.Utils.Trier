using CleanArchEnablers.Utils.Trier;
using CleanArchEnablers.Utils.Trier.Exceptions.Handlers;
using Actions = CleanArchEnablers.Utils.Trier.Actions;

namespace CleanArchEnablers.Utils.Trier;

public class TrierBuilder<T, TO>
{
    private readonly Actions.Action<T, TO> _action;
    private readonly T? _input;
    private readonly Dictionary<Type, int> _retryLimits = new(); 

    public TrierBuilder(Actions.Action<T, TO> action, T? input)
    {
        _action = action;
        _input = input;
    }

    /// <summary>
    /// Defines when "Trier" will AutoRetry
    /// </summary>
    /// <param name="maxAmountOfRetries">Limit of Retries</param>
    /// <typeparam name="TE">Exception Type</typeparam>
    /// <returns>Builder for Trier</returns>
    public TrierBuilder<T, TO> AutoRetryOn<TE>(int maxAmountOfRetries) where TE : Exception
    {
        _retryLimits[typeof(TE)] = maxAmountOfRetries;
        return this;
    }

    /// <summary>
    /// Defines the handler of Exceptions -> MappedExceptions
    /// </summary>
    /// <param name="unexpectedExceptionHandler">Implementation of IUnexpectedExceptionHandler</param>
    /// <returns>Implementation of Trier</returns>
    public Trier<T, TO> WithUnexpectedExceptionHandler(IUnexpectedExceptionHandler unexpectedExceptionHandler)
    {
        return new Trier<T, TO>(_action, unexpectedExceptionHandler, _input, _retryLimits);
    }
}
