# MCP Server Sample - .NET Core Web API

This is a sample .NET Core Web API application that demonstrates how to use the ModelContextProtocol package to create an MCP (Model Context Protocol) server.

## Features

- **Calculator Tools**: Basic arithmetic operations (add, subtract, multiply, divide)

- **Business Associate Tools**: Complete CRUD operations for managing business associates
  - Get business associates by type
  - Create new business associates
  - Update existing business associates
  - Delete business associates

## Project Structure

```
McpServerSample/
├── Models/
│   └── BusinessAssociate.cs  # Business Associate model and API response classes
├── Tools/


│   ├── CalculatorTools.cs    # Calculator operations decorated with MCP attributes
│   └── BusinessAssociateTools.cs # Business Associate CRUD operations
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
// Add services to the container
builder.Services.AddHttpClient(); // Register HttpClient for DI

// Register MCP Server with HTTP transport and tools from assembly
builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

// Register tool classes as singletons (optional, but recommended)
builder.Services.AddSingleton<CalculatorTools>();

builder.Services.AddSingleton<BusinessAssociateTools>();

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







### Business Associate Tools
- **GetBusinessAssociates**: Get business associates by type (vendor, customer, etc.)
- **CreateBusinessAssociate**: Create a new business associate with the following parameters:
  - `baType`: Business Associate Type (e.g., 'vendor', 'customer')
  - `baName`: Business Associate Name
  - `sapVendor`: SAP Vendor ID (optional)
  - `sapCustomer`: SAP Customer ID (optional)
  - `sapCompanyCode`: SAP Company Code (optional)
- **UpdateBusinessAssociate**: Update an existing business associate with the following parameters:
  - `baType`: Business Associate Type
  - `baId`: Business Associate ID
  - `baName`: Updated Business Associate Name (optional)
  - `sapVendor`: Updated SAP Vendor ID (optional)
  - `sapCustomer`: Updated SAP Customer ID (optional)
  - `sapCompanyCode`: Updated SAP Company Code (optional)
- **DeleteBusinessAssociate**: Delete a business associate by type and ID

## Requirements

- .NET 9.0 SDK or later
- ModelContextProtocol package (0.5.0-preview.1 or later)

## Configuration

### Business Associate API Endpoint

The Business Associate Tools connect to an API endpoint configured in the `BusinessAssociateTools.cs` file:

```csharp
private readonly string _baseUrl = "http://localhost:3000";
```

You can modify this URL to point to your Business Associate API service. The API is expected to support the following endpoints:

- `GET /api/v1/{baType}` - Get all business associates of a specific type
- `POST /api/v1/{baType}` - Create a new business associate
- `PUT /api/v1/{baType}/{baId}` - Update an existing business associate
- `DELETE /api/v1/{baType}/{baId}` - Delete a business associate

## Notes

- The ModelContextProtocol package is currently in preview

- The MCP server endpoints are automatically configured when calling `app.MapMcp()`
- Tool classes should be registered in the dependency injection container
- The Business Associate Tools require an HttpClient service, which is registered in Program.cs
- The API responses are expected to follow the structure defined in the response classes (CreateBusinessAssociateResponse, UpdateBusinessAssociateResponse, etc.)
