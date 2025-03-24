using Cae.Utils.Trier.Actions.Factories;
using Cae.Utils.Trier.Exceptions;
using Cae.Utils.Trier.Tests.Mocks;
using VoidReturn = Cae.Utils.Trier.Types.VoidReturn;

namespace Cae.Utils.Trier.Tests;

public class TrierTests
{
    private readonly IUnexpectedExceptionHandler _unexpectedExceptionHandler = new UnexpectedExceptionHandlerMock();

    [Fact]
    public void ShouldExecuteFunctionActionSuccesfuly()
    {
        var action = ActionFactory.CreateInstance((int number) => number.ToString());
        var result = Trier<int, string>.CreateInstance(action, 1, _unexpectedExceptionHandler)
            .WithUnexpectedExceptionHandler()
            .Execute();
        
        Assert.Equal("1", result);
    }
    
    [Fact]
    public async Task ShouldExecuteFunctionActionSuccesfulyAsync()
    {
        var action = ActionFactory.CreateInstance((int number) => Task.FromResult(number.ToString()));
        var trier = Trier<int, string>.CreateInstance(action, 1, _unexpectedExceptionHandler).WithUnexpectedExceptionHandler();
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
        
        var trier = Trier<int, string>.CreateInstance(action, 1, _unexpectedExceptionHandler).WithUnexpectedExceptionHandler();

        Assert.Throws<Exception>(trier.Execute);
    }
    
    [Fact]
    public void ShouldExecuteFunctionActionWithErrorsAsync()
    {
        var action = ActionFactory.CreateInstance((int number) =>
        {
            if (number == 1) throw new Exception();
            return Task.FromResult(number.ToString());
        });
        
        var trier = Trier<int, string>.CreateInstance(action, 1, _unexpectedExceptionHandler).WithUnexpectedExceptionHandler();

        Assert.ThrowsAsync<Exception>(trier.ExecuteAsync);
    }

    [Fact]
    public void ShouldBlockCreateTrierWithNullInputAndFunctionAction()
    {
        var action = ActionFactory.CreateInstance((int number) => number.ToString());
        var trier = Trier<int, string>.CreateInstance(action, 0, _unexpectedExceptionHandler);

        Assert.Throws<Exception>(trier.WithUnexpectedExceptionHandler);
    }
    
    [Fact]
    public void ShouldBlockCreateTrierWithNullInputAndConsumerAction()
    {
        var action = ActionFactory.CreateInstance((int number) => {});
        var trier = Trier<int, VoidReturn?>.CreateInstance(action, 0, _unexpectedExceptionHandler);

        Assert.Throws<Exception>(trier.WithUnexpectedExceptionHandler);
    }
}