

using Cae.Utils.Trier.Exceptions;

namespace Cae.Utils.Trier;

public class TrierBuilder<T,TO>
{
    private readonly Actions.Action<T, TO> _action;
    private readonly T? _input;
    private readonly IUnexpectedExceptionHandler _unexpectedExceptionHandler;

    public TrierBuilder(Actions.Action<T, TO> action, T? input, IUnexpectedExceptionHandler unexpectedExceptionHandler)
    {
        _action = action;
        _input = input;
        _unexpectedExceptionHandler = unexpectedExceptionHandler;
    }

    public Trier<T,TO> WithUnexpectedExceptionHandler()
    {
        return new Trier<T, TO>(_action, _input, _unexpectedExceptionHandler);
    }
}