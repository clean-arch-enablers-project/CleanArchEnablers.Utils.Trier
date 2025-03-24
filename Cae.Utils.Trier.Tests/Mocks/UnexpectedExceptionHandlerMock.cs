using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.Trier.Exceptions;

namespace Cae.Utils.Trier.Tests.Mocks
{
    internal class UnexpectedExceptionHandlerMock : IUnexpectedExceptionHandler
    {
        public Exception Handle(Exception exception)
        {
            return exception;
        }
    }
}
