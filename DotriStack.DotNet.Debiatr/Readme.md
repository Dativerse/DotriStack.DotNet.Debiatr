# DotriStack.DotNet.Debiatr

A lightweight and efficient mediator implementation for .NET applications, providing a simple way to implement the mediator pattern with clean separation of concerns.

## Features

- 🚀 **Lightweight**: Minimal overhead with clean, simple implementation
- 🔧 **Easy Integration**: Simple dependency injection setup
- 🎯 **Type Safe**: Strongly typed request/response pattern
- ⚡ **Async Support**: Built-in async/await support with cancellation tokens
- 🏗️ **Auto Discovery**: Automatic handler registration via reflection
- 📦 **NET 9.0**: Built for the latest .NET framework

## Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package DotriStack.DotNet.Debiatr
```

Or via Package Manager Console:

```powershell
Install-Package DotriStack.DotNet.Debiatr
```

## Quick Start

### 1. Register the Mediator

Add the mediator to your dependency injection container:

```csharp
using DotriStack.DotNet.Debiatr;

// In Program.cs or Startup.cs
builder.Services.AddMediator();

// Or specify a specific assembly for handler discovery
builder.Services.AddMediator(typeof(MyHandler).Assembly);
```

### 2. Create a Request

Define your request by implementing `IRequest<TResponse>`:

```csharp
public class GetUserQuery : IRequest<User>
{
    public int UserId { get; set; }
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
```

### 3. Create a Handler

Implement the corresponding handler:

```csharp
public class GetUserHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
    }
}
```

### 4. Use the Mediator

Inject and use the mediator in your controllers or services:

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id, CancellationToken cancellationToken)
    {
        var query = new GetUserQuery { UserId = id };
        var user = await _mediator.Send(query, cancellationToken);
        return Ok(user);
    }
}
```

## Advanced Examples

### Command Pattern

```csharp
// Command
public class CreateUserCommand : IRequest<int>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

// Handler
public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
{
    private readonly IUserRepository _userRepository;

    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User 
        { 
            Name = request.Name, 
            Email = request.Email 
        };
        
        return await _userRepository.CreateAsync(user, cancellationToken);
    }
}
```

### Void Operations

For operations that don't return a value, use `Unit` or create a custom response type:

```csharp
public class DeleteUserCommand : IRequest<bool>
{
    public int UserId { get; set; }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        return await _userRepository.DeleteAsync(request.UserId, cancellationToken);
    }
}
```

## Architecture Benefits

The mediator pattern helps achieve:

- **Decoupling**: Controllers don't directly depend on business logic
- **Single Responsibility**: Each handler has one specific purpose
- **Testability**: Easy to unit test handlers in isolation
- **Maintainability**: Clean separation between request/response handling
- **Scalability**: Easy to add new operations without modifying existing code

## API Reference

### IMediator

```csharp
public interface IMediator
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
}
```

### IRequest<TResponse>

```csharp
public interface IRequest<TResponse>
{
}
```

### IRequestHandler<TRequest, TResponse>

```csharp
public interface IRequestHandler<in TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
}
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Repository

Visit the [GitHub repository](https://github.com/Dativerse/DotriStack.DotNet.Debiatr) for source code and issues.

---

Built with ❤️ by Dattiverse