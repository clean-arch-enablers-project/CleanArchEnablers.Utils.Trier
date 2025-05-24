using Cae.Utils.MappedExceptions;

namespace CleanArchEnablers.Utils.Trier.Exceptions.Handlers
{
    public interface IUnexpectedExceptionHandler
    {
        /// <summary>
        /// Transform normal Exceptions in MappedExceptions
        /// </summary>
        /// <param name="exception">Generic Exception</param>
        /// <returns>Mapped Exception</returns>
        MappedException Handle(Exception exception);
    }
}
