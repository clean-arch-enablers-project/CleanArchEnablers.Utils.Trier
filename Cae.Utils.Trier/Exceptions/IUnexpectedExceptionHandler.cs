using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cae.Utils.Trier.Exceptions
{
    public interface IUnexpectedExceptionHandler
    {
        // TODO: Implement MappedException on return instead of a Generic Exception
        Exception Handle(Exception exception);
    }
}
