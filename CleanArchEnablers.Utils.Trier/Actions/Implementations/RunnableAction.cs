using CleanArchEnablers.Utils.Trier.Types;

namespace CleanArchEnablers.Utils.Trier.Actions.Implementations;

public class RunnableAction : Action<VoidType?, VoidType?>
{
    private readonly Action? _action;
    private readonly Func<Task>? _actionAsync;

    public RunnableAction(Action action) => _action = action;
    public RunnableAction(Func<Task> actionAsync) => _actionAsync = actionAsync;

    protected override VoidType? ExecuteInternalAction(VoidType? input = null)
    {
        if (_action == null) throw new Exception();
        
        _action.Invoke();
        return null;
    }

    protected override async Task<VoidType?> ExecuteInternalActionAsync(VoidType? input = null)
    {
        if (_actionAsync == null) throw new Exception();
        
        await _actionAsync();
        return null;
    }
}