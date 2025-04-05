using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.MappedExceptions;

namespace CleanArchEnablers.Utils.Trier.Exceptions
{
    public interface IUnexpectedExceptionHandler
    {
        // TODO: Implement MappedException on return instead of a Generic Exception
        MappedException Handle(Exception exception);
    }
}
