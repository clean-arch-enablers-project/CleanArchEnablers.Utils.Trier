using Cae.Utils.MappedExceptions;

namespace CleanArchEnablers.Utils.Trier.Exceptions.Handlers
{
    public interface IUnexpectedExceptionHandler
    {
        // TODO: Implement MappedException on return instead of a Generic Exception
        MappedException Handle(Exception exception);
    }
}
