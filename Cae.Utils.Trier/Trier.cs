using Cae.Utils.MappedExceptions;
using Cae.Utils.Trier.Actions.Implementations;
using Cae.Utils.Trier.Exceptions;

namespace Cae.Utils.Trier;

public class Trier<T, TO>
{
    private readonly Actions.Action<T,TO> _action;
    private readonly T _input;
    private readonly IUnexpectedExceptionHandler _unexpectedExceptionHandler;
    private readonly int _maxRetries;
    private readonly List<Type> _exceptionsToRetry;

#pragma warning disable CS8618, CS8601
    public Trier(Actions.Action<T, TO> action, IUnexpectedExceptionHandler unexpectedExceptionHandler, 
                 T? input = default, int maxRetries = 0, List<Type>? exceptionsToRetry = null)
    {
        _action = action;

        if (_action is not (RunnableAction or SupplierAction<TO>)
            && EqualityComparer<T>.Default.Equals(input, default))
        {
            throw new Exception("Input cannot be null for this action type.");
        }

        _input = input;
        _unexpectedExceptionHandler = unexpectedExceptionHandler;
        _maxRetries = maxRetries;
        _exceptionsToRetry = exceptionsToRetry ?? [];
    }
#pragma warning restore
    
    public static TrierBuilder<T, TO> CreateInstance(Actions.Action<T, TO> action, T? input)
    {
        return new TrierBuilder<T, TO>(action, input);
    }

    public TO Execute()
    {
        var attempt = 0;

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
                if (attempt >= _maxRetries || !_exceptionsToRetry.Contains(e.GetType()))
                    
                    throw _unexpectedExceptionHandler.Handle(e);
                attempt++;
            }
        }
    }

    public async Task<TO> ExecuteAsync()
    {
        var attempt = 0;

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
                if (attempt >= _maxRetries || !_exceptionsToRetry.Contains(e.GetType()))
                    throw _unexpectedExceptionHandler.Handle(e);
                
                attempt++;
            }
        }
    }
}