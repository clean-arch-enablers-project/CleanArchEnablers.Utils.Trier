

namespace Cae.Utils.Trier;

public class TrierBuilder<T,TO>
{
    private readonly Actions.Action<T, TO> _action;
    private readonly T? _input;

    public TrierBuilder(Actions.Action<T, TO> action,  T? input)
    {
        _action = action;
        _input = input;
    }

    public Trier<T,TO> WithUnexpectedExceptionHandler()
    {
        return new Trier<T, TO>(_action, _input);
    }
}