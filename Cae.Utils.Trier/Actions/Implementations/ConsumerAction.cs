using VoidReturn = Cae.Utils.Trier.Types.VoidReturn;

namespace Cae.Utils.Trier.Actions.Implementations;

public class ConsumerAction<T> : Action<T, VoidReturn?>
{
    private readonly Action<T>? _consumer;
    private readonly Func<T, Task>? _consumerAsync; 

    public ConsumerAction(Action<T> consumer) => _consumer = consumer;
    public ConsumerAction(Func<T, Task> consumerAsync) => _consumerAsync = consumerAsync; 

    protected override VoidReturn? ExecuteInternalAction(T input)
    {
        if (_consumer == null) throw new Exception("Consumer is not set");

        _consumer(input);
        return null;
    }

    protected override async Task<VoidReturn?> ExecuteInternalActionAsync(T input)
    {
        if (_consumerAsync == null) throw new Exception("Async Consumer is not set");

        await _consumerAsync(input); 
        return null;
    }
}