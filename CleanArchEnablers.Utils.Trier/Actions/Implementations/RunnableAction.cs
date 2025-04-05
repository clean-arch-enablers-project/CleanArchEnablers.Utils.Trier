using Types_VoidReturn = CleanArchEnablers.Utils.Trier.Types.VoidReturn;
using VoidReturn = CleanArchEnablers.Utils.Trier.Types.VoidReturn;

namespace CleanArchEnablers.Utils.Trier.Actions.Implementations;

public class RunnableAction : Action<Types_VoidReturn?, Types_VoidReturn?>
{
    private readonly Action? _action;
    private readonly Func<Task>? _actionAsync;

    public RunnableAction(Action action) => _action = action;
    public RunnableAction(Func<Task> actionAsync) => _actionAsync = actionAsync;

    protected override Types_VoidReturn? ExecuteInternalAction(Types_VoidReturn? input = null)
    {
        if (_action == null) throw new Exception();
        
        _action.Invoke();
        return null;
    }

    protected override async Task<Types_VoidReturn?> ExecuteInternalActionAsync(Types_VoidReturn? input = null)
    {
        if (_actionAsync == null) throw new Exception();
        
        await _actionAsync();
        return null;
    }
}