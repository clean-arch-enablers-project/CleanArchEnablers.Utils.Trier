using Types_VoidReturn = CleanArchEnablers.Utils.Trier.Types.VoidReturn;
using VoidReturn = CleanArchEnablers.Utils.Trier.Types.VoidReturn;

namespace CleanArchEnablers.Utils.Trier.Actions.Implementations;

public class SupplierAction<TO> : Action<Types_VoidReturn?, TO>
{
    private readonly Func<Task<TO>>? _supplierAsync;
    private readonly Func<TO>? _supplier;

    public SupplierAction(Func<Task<TO>> supplierAsync) => _supplierAsync = supplierAsync;
    public SupplierAction(Func<TO> supplier) => _supplier = supplier;
    
    protected override TO ExecuteInternalAction(Types_VoidReturn? input)
    {
        if (_supplier == null) throw new Exception();

        return _supplier();
    }

    protected override Task<TO> ExecuteInternalActionAsync(Types_VoidReturn? input)
    {
        if (_supplierAsync == null) throw new Exception();

        return _supplierAsync();
    }
}