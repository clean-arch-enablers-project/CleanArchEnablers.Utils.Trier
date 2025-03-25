using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.MappedExceptions;
using Cae.Utils.MappedExceptions.Specifics;
using Cae.Utils.Trier.Exceptions;

namespace Cae.Utils.Trier.Tests.Mocks
{
    internal class UnexpectedExceptionHandlerMock : IUnexpectedExceptionHandler
    {
        public MappedException Handle(Exception exception)
        {
            return new InternalMappedException("One error ocurred.", "See more details: " + exception.Message, exception);
        }
    }
}
