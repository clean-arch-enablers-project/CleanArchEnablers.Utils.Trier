using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cae.Utils.MappedExceptions;
using Cae.Utils.MappedExceptions.Specifics;
using CleanArchEnablers.Utils.Trier.Exceptions.Handlers;

namespace CleanArchEnablers.Utils.Trier.Tests.Mocks
{
    internal class UnexpectedExceptionHandlerMock : IUnexpectedExceptionHandler
    {
        public MappedException Handle(Exception exception)
        {
            return new InternalMappedException("One error ocurred.", "See more details: " + exception.Message, exception);
        }
    }
}
