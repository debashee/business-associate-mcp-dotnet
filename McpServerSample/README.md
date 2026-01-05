# MCP Server Sample - .NET Core Web API

This is a sample .NET Core Web API application that demonstrates how to use the ModelContextProtocol package to create an MCP (Model Context Protocol) server.

## Features

- **Calculator Tools**: Basic arithmetic operations (add, subtract, multiply, divide)
- **String Tools**: String manipulation operations (concatenate, reverse, case conversion, word count)

## Project Structure

```
McpServerSample/
├── Tools/
│   ├── CalculatorTools.cs   # Calculator operations decorated with MCP attributes
│   └── StringTools.cs        # String operations decorated with MCP attributes
├── Program.cs                # Application startup and MCP server configuration
└── McpServerSample.csproj    # Project file with ModelContextProtocol package
```

## How It Works

### 1. Tool Classes

Tool classes are decorated with the `[McpServerToolType]` attribute:

```csharp
[McpServerToolType]
public class CalculatorTools
{
    // Tool methods...
}
```

### 2. Tool Methods

Individual tool methods are decorated with the `[McpServerTool]` attribute and `[Description]` attribute from `System.ComponentModel`:

```csharp
[McpServerTool, Description("Adds two numbers together")]
public double Add(
    [Description("First number")]
    double a,
    [Description("Second number")]
    double b)
{
    return a + b;
}
```

### 3. Configuration

In `Program.cs`, the MCP server is configured and registered:

```csharp
// Register MCP Server with HTTP transport and tools from assembly
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

// Register tool classes as singletons (optional, but recommended)
builder.Services.AddSingleton<CalculatorTools>();
builder.Services.AddSingleton<StringTools>();

// Map MCP Server endpoints
app.MapMcp();
```

## Running the Application

1. Build the project:
   ```powershell
   dotnet build
   ```

2. Run the application:
   ```powershell
   dotnet run
   ```

3. The application will start and listen on the configured ports (typically `http://localhost:5000` and `https://localhost:5001`)

## Available Tools

### Calculator Tools
- **add**: Adds two numbers together
- **subtract**: Subtracts the second number from the first
- **multiply**: Multiplies two numbers
- **divide**: Divides the first number by the second

### String Tools
- **concatenate**: Concatenates multiple strings together with an optional separator
- **reverse**: Reverses a string
- **toUpperCase**: Converts a string to uppercase
- **toLowerCase**: Converts a string to lowercase
- **wordCount**: Counts the number of words in a string

## Requirements

- .NET 9.0 SDK or later
- ModelContextProtocol package (0.5.0-preview.1 or later)

## Notes

- The ModelContextProtocol package is currently in preview
- The MCP server endpoints are automatically configured when calling `app.MapMcpServer()`
- Tool classes should be registered in the dependency injection container
