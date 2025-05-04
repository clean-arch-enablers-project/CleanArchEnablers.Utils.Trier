using Cae.Utils.MappedExceptions;
using CleanArchEnablers.Utils.Trier.Actions.Implementations;
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
            throw new MappedException("Input Is Null.", "Input cannot be null for this action type.");
        }

        _input = input;
        _unexpectedExceptionHandler = unexpectedExceptionHandler;
        _retryLimits = retryLimits ?? new Dictionary<Type, int>();
    }
    #pragma warning restore
    
    public static TrierBuilder<T, TO> CreateInstance(Actions.Action<T, TO> action, T? input)
    {
        return new TrierBuilder<T, TO>(action, input);
    }

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
