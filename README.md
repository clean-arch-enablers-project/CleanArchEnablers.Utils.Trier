# ✔️ CleanArchEnablers.Utils.Trier
C# Microsoft.NET edition

<br>

Welcome to the repository for the open source CAE Trier library!

### ▶️ The NuGet Package:
```xml
<ItemGroup>
    <PackageReference Include="CleanArchEnablers.Utils.Trier" Version="${LatestVersion}">
</ItemGroup>
```

<br>

State Symbol Key:

- ``✅`` — _Under release state_
- ``✔️`` — _Under snapshot state_
- ``⏳`` — _Under full development state_


## 📚 Key Concepts

### Actions

Actions are the runnable code inside of a function. Basically, is just a normal function.

They have some types:
- RunnableAction -> Has no Input and Output
- SupplierAction -> Has no Input but have Output
- ConsumerAction -> Has Input but don't have Output
- FunctionAction -> Has Input and Output

Action only have 1 input, we recommend you send DTO Records to Actions as Parameters.

### Auto Retainer

Auto Retainer is compatible with all exceptions on .NET, but you need to specify a limit for Retries.
You can define AutoRetainer on TrierBuilder

```csharp
Trier<VoidType?, ReturnType>.CreateInstance(action, null)
    .AutoRetryOn<MyException>(5); // Retry on MyException with a limit of 5 times
    
```

### Exception Handler

CAE by default, only work with MappedExceptions. So we need to transform our Generic Exception to a MappedException. For this, we have the IUnexpectedExceptionHandler interface.

Your will need create our own implementation for this.

<br>
<br>
<br>

<p align="center">
 💡 proper documentation will soon be available.
</p>
<p align="center">
    CAE — Clean Architecture made easy.
</p>

<br>
<br>
<br>

