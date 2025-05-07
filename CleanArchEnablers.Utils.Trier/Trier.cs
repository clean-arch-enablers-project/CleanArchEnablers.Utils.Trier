using Cae.Utils.MappedExceptions;
using CleanArchEnablers.Utils.Trier.Actions.Implementations;
using CleanArchEnablers.Utils.Trier.Exceptions;
using CleanArchEnablers.Utils.Trier.Exceptions.Handlers;

namespace CleanArchEnablers.Utils.Trier;

public class Trier<T, TO>
{
    private readonly Actions.Action<T, TO> _action;
    private readonly T _input;
    private readonly IUnexpectedExceptionHandler _unexpectedExceptionHandler;
    private readonly Dictionary<Type, int> _retryLimits;

    #pragma warning disable CS8618, CS8601
    public Trier(Actions.Action<T, TO> action, IUnexpectedExceptionHandler unexpectedExceptionHandler, 
                 T? input = default, Dictionary<Type, int>? retryLimits = null)
    {
        _action = action;

        if (_action is not (RunnableAction or SupplierAction<TO>)
            && EqualityComparer<T>.Default.Equals(input, default))
        {
            throw new InvalidInputForThisActionTypeMappedException();
        }

        _input = input;
        _unexpectedExceptionHandler = unexpectedExceptionHandler;
        _retryLimits = retryLimits ?? new Dictionary<Type, int>();
    }
    #pragma warning restore
    
    /// <summary>
    /// Initialize Trier Builder
    /// </summary>
    /// <param name="action">Your function</param>
    /// <param name="input">The parameter of your function</param>
    /// <returns>Trier Builder</returns>
    public static TrierBuilder<T, TO> CreateInstance(Actions.Action<T, TO> action, T? input)
    {
        return new TrierBuilder<T, TO>(action, input);
    }

    /// <summary>
    /// Execute Non-Async Actions defined in builder
    /// </summary>
    /// <returns>Provided type</returns>
    /// <exception cref="MappedException">Exception defined on your action</exception>
    public TO Execute()
    {
        var attemptByException = new Dictionary<Type, int>();
        
        while (true)
        {
            try
            {
                return _action.Execute(_input);
            }
            catch (MappedException)
            {
                throw;
            }
            catch (Exception e)
            {
                var exceptionType = e.GetType();

                if (!_retryLimits.TryGetValue(exceptionType, out var maxRetries))
                    throw _unexpectedExceptionHandler.Handle(e);

                attemptByException.TryAdd(exceptionType, 0);

                if (attemptByException[exceptionType] >= maxRetries) 
                    throw _unexpectedExceptionHandler.Handle(e);
                
                attemptByException[exceptionType]++;
            }
        }
    }

    /// <summary>
    /// Execute Async Actions defined in builder
    /// </summary>
    /// <returns>Provided type</returns>
    /// <exception cref="MappedException">Exception defined on your action</exception>
    public async Task<TO> ExecuteAsync()
    {
        var attemptByException = new Dictionary<Type, int>();

        while (true)
        {
            try
            {
                return await _action.ExecuteAsync(_input);
            }
            catch (MappedException)
            {
                throw;
            }
            catch (Exception e)
            {
                var exceptionType = e.GetType();

                if (!_retryLimits.TryGetValue(exceptionType, out var maxRetries))
                    throw _unexpectedExceptionHandler.Handle(e);

                attemptByException.TryAdd(exceptionType, 0);

                if (attemptByException[exceptionType] >= maxRetries) 
                    throw _unexpectedExceptionHandler.Handle(e);
                
                attemptByException[exceptionType]++;
            }
        }
    }
}
