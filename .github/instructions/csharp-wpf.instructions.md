---
applyTo: "workspace/csharp/**/*.cs"
---

# C# / WPF Conventions — Digital Pattern Engine

These instructions apply to all C# files under `workspace/csharp/`. Follow these conventions when generating,
reviewing, or editing C# code for the digital Pattern Engine service or WPF operator dashboard.

## Framework and Language Versions

- Target `.NET 8` with `<LangVersion>12</LangVersion>` in `*.csproj`.
- Use C# 12 features: primary constructors, collection expressions, `required` modifier.
- Use `record` types for immutable domain objects (`PatternJob`, `DyeHeadActivation`, `MisfireDetectedEvent`).
- Use `class` for services that hold mutable state or implement interfaces.

## Project Structure

```text
workspace/csharp/
├── Models/          # Immutable records and enums
├── Services/        # Business logic (interfaces + implementations)
├── Events/          # Domain event records
├── Infrastructure/  # SQL, TCP, SignalR adapters
├── Api/             # Minimal API endpoints or controllers
└── Tests/           # xUnit test project
```

## Naming Conventions

- Use `PascalCase` for types, methods, and properties.
- Use `camelCase` for local variables and parameters.
- Prefix interfaces with `I`: `ISignalMapAdapter`, `IDyeHeadSensor`.
- Suffix event records with `Event`: `MisfireDetectedEvent`, `PatternJobStartedEvent`.
- Suffix service implementations with the interface name minus `I`: `TcpSignalMapAdapter` implements
  `ISignalMapAdapter`.

## Dependency Injection

- Register all services in `Program.cs` using `builder.Services.Add*`.
- Never use `new` to instantiate services inside other services; use constructor injection.
- Use `IOptions<T>` for all configuration values; define a `*Options` record per service.
- Use `ILogger<T>` for structured logging in every service class.

## Async Patterns

- Use `async Task` (not `async void`) for all async methods.
- Pass `CancellationToken` through every async call chain.
- Wrap external I/O (TCP, SQL) in try/catch; log and rethrow as domain exceptions.
- Do not use `Task.Result` or `.Wait()` inside async code paths.

## SQL and Data Access

- Use `Microsoft.Data.SqlClient` with parameterized queries only.
- Parameter names must match column names: `@jobId`, `@headIndex`, `@detectedAt`.
- Never build SQL strings by concatenation; use `@paramName` placeholders.
- Dispose `SqlConnection` and `SqlCommand` with `using` statements.
- Always use `DATETIMEOFFSET` for timestamps; use `DateTimeOffset.UtcNow` in C# code.

## Error Handling

- Use `ArgumentException` for invalid argument values (not `Exception`).
- Use `InvalidOperationException` for state-machine violations.
- Use `throw new ...Exception(message, innerException)` to preserve stack traces.
- Log `Warning` for recoverable errors (misfire detected, retry); log `Error` for unrecoverable failures.

## Testing

- Use xUnit with `Moq` for unit tests.
- Test class name: `{TypeUnderTest}Tests` (e.g., `PatternRendererTests`).
- Test method name: `{MethodName}_{Condition}_{ExpectedResult}`.
- Mock all external dependencies (`ISignalMapAdapter`, `IDyeHeadSensor`, `ILogger`).
- Assert `Times.Once()` on mock interactions where exactly-once behavior is required.
- Keep each test under 30 lines; use `[Theory]` with `[InlineData]` for parameterized cases.

## WPF (Operator Dashboard)

- Use MVVM pattern: `ViewModel` classes with `INotifyPropertyChanged`.
- Bind UI to `ObservableCollection<T>` for dye-head status grid.
- Use `Dispatcher.InvokeAsync` to marshal SignalR callbacks to the UI thread.
- Keep code-behind files minimal; push all logic to ViewModels.
- Use `ResourceDictionary` for colors: `MisfireRed`, `ActiveGreen`, `IdleGray`.

## Documentation

- Add XML doc comments (`/// <summary>`) on all `public` and `internal` members.
- Include `/// <param name="...">` and `/// <returns>` for non-trivial methods.
- Add an inline comment explaining the misfire threshold business rule wherever it appears.
