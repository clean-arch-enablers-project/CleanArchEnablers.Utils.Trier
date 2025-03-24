using Cae.Utils.Trier.Actions.Factories;
using Void = Cae.Utils.Trier.Types.Void;

namespace Cae.Utils.Trier.Tests;

public class TrierTests
{
    [Fact]
    public void ShouldExecuteFunctionActionSuccesfuly()
    {
        var action = ActionFactory.CreateInstance((int number) => number.ToString());
        var result = Trier<int, string>.CreateInstance(action, 1)
            .WithUnexpectedExceptionHandler()
            .Execute();
        
        Assert.Equal("1", result);
    }
    
    [Fact]
    public async Task ShouldExecuteFunctionActionSuccesfulyAsync()
    {
        var action = ActionFactory.CreateInstance((int number) => Task.FromResult(number.ToString()));
        var trier = Trier<int, string>.CreateInstance(action, 1).WithUnexpectedExceptionHandler();
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
        
        var trier = Trier<int, string>.CreateInstance(action, 1).WithUnexpectedExceptionHandler();

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
        
        var trier = Trier<int, string>.CreateInstance(action, 1).WithUnexpectedExceptionHandler();

        Assert.ThrowsAsync<Exception>(trier.ExecuteAsync);
    }

    [Fact]
    public void ShouldBlockCreateTrierWithNullInputAndFunctionAction()
    {
        var action = ActionFactory.CreateInstance((int number) => number.ToString());
        var trier = Trier<int, string>.CreateInstance(action, 0);

        Assert.Throws<Exception>(trier.WithUnexpectedExceptionHandler);
    }
    
    [Fact]
    public void ShouldBlockCreateTrierWithNullInputAndConsumerAction()
    {
        var action = ActionFactory.CreateInstance((int number) => {});
        var trier = Trier<int, Void?>.CreateInstance(action, 0);

        Assert.Throws<Exception>(trier.WithUnexpectedExceptionHandler);
    }
}