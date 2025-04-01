using Cae.Utils.MappedExceptions;
using Cae.Utils.MappedExceptions.Specifics;
using Cae.Utils.Trier.Actions.Factories;
using Cae.Utils.Trier.Exceptions;
using Cae.Utils.Trier.Tests.Mocks;
using VoidReturn = Cae.Utils.Trier.Types.VoidReturn;

namespace Cae.Utils.Trier.Tests;

public class TrierTests
{
    private readonly IUnexpectedExceptionHandler _unexpectedExceptionHandler = new UnexpectedExceptionHandlerMock();

    [Fact]
    public void ShouldExecuteFunctionActionSuccessfully()
    {
        var action = ActionFactory.CreateInstance((int number) => number.ToString());
        var result = Trier<int, string>.CreateInstance(action, 1)
            .WithUnexpectedExceptionHandler(_unexpectedExceptionHandler)
            .Execute();
        
        Assert.Equal("1", result);
    }
    
    [Fact]
    public async Task ShouldExecuteFunctionActionSuccessfullyAsync()
    {
        var action = ActionFactory.CreateInstance((int number) => Task.FromResult(number.ToString()));
        var trier = Trier<int, string>.CreateInstance(action, 1)
            .WithUnexpectedExceptionHandler(_unexpectedExceptionHandler);
        var result = await trier.ExecuteAsync();
        
        Assert.Equal("1", result);
    }

    [Fact]
    public void ShouldExecuteFunctionActionWithErrors()
    {
        var action = ActionFactory.CreateInstance((int number) =>
        {
            if (number == 1) throw new Exception();
            return number.ToString();
        });
        
        var trier = Trier<int, string>.CreateInstance(action, 1)
            .WithUnexpectedExceptionHandler(_unexpectedExceptionHandler);

        Assert.Throws<InternalMappedException>(trier.Execute);
    }
    
    [Fact]
    public void ShouldExecuteFunctionActionWithErrorsAsync()
    {
        var action = ActionFactory.CreateInstance((int number) =>
        {
            if (number == 1) throw new Exception();
            return Task.FromResult(number.ToString());
        });
        
        var trier = Trier<int, string>.CreateInstance(action, 1)
            .WithUnexpectedExceptionHandler(_unexpectedExceptionHandler);

        Assert.ThrowsAsync<Exception>(trier.ExecuteAsync);
    }

    [Fact]
    public void ShouldBlockCreateTrierWithNullInputAndFunctionAction()
    {
        var action = ActionFactory.CreateInstance((int number) => number.ToString());
        var trier = Trier<int, string>.CreateInstance(action, 0);

        Assert.Throws<MappedException>(() => trier.WithUnexpectedExceptionHandler(_unexpectedExceptionHandler));
    }
    
    [Fact]
    public void ShouldBlockCreateTrierWithNullInputAndConsumerAction()
    {
        var action = ActionFactory.CreateInstance((int number) => {});
        var trier = Trier<int, VoidReturn?>.CreateInstance(action, 0);

        Assert.Throws<MappedException>(() => trier.WithUnexpectedExceptionHandler(_unexpectedExceptionHandler));
    }

    [Fact]
    public void ShouldExecuteSupplierActionSuccessfully()
    {
        var action = ActionFactory.CreateInstance(() => "hi");
        var result = Trier<VoidReturn?, string>.CreateInstance(action, null)
            .WithUnexpectedExceptionHandler(_unexpectedExceptionHandler)
            .Execute();
        
        Assert.Equal("hi", result);
    }

    [Fact]
    public void ShouldAutoRetryActionSuccesfully()
    {
        var attempt = 0;
        var action = ActionFactory.CreateInstance((int _) =>
        {
            attempt++;

            if (attempt <= 4) throw new NotCoolException("Cool Exception");
            if (attempt is <= 6 and > 4) throw new CoolException("Not Cool Exception");

            return "hi";
        });
        
        var result = Trier<int, string>.CreateInstance(action, 1)
            .AutoRetryOn<NotCoolException>(4)
            .AutoRetryOn<CoolException>(2)
            .WithUnexpectedExceptionHandler(_unexpectedExceptionHandler)
            .Execute();
        
        Assert.Equal(7, attempt);
        Assert.Equal("hi", result);
    }
    
    [Fact]
    public void ShouldAutoRetryConsumerActionSuccessfully()
    {
        var attempt = 0;
    
        var action = ActionFactory.CreateInstance((int _) =>
        {
            attempt++; 
        
            switch (attempt)
            {
                case >= 1 and < 3:
                    throw new Exception("ex");
                case >= 3 and <= 6:
                    throw new MappedException("ex");
            }
        });

        var trier = Trier<int, VoidReturn?>
            .CreateInstance(action, 1)
            .AutoRetryOn<Exception>(3)
            .WithUnexpectedExceptionHandler(_unexpectedExceptionHandler);

        MappedException? caughtException = null;
    
        try
        {
            trier.Execute();
        }
        catch (MappedException ex)
        {
            caughtException = ex;
        }

        Assert.NotNull(caughtException);
        Assert.IsType<MappedException>(caughtException); 
        Assert.Equal(3, attempt); 
    }
}