# Usage Guide

## Running the Server

Start the MCP server:

```powershell
dotnet run
```

The server will start and listen on `http://localhost:5267` (or the port specified in launchSettings.json).

## MCP Endpoints

The server exposes two main MCP endpoints:

- **SSE Endpoint**: `http://localhost:5267/sse` - Server-Sent Events for streaming communication
- **Messages Endpoint**: `http://localhost:5267/messages` - HTTP POST endpoint for client-to-server messages

## Available Tools

### Calculator Tools

1. **add** - Adds two numbers together
   - Parameters:
     - `a` (double): First number
     - `b` (double): Second number
   - Returns: Sum of the two numbers

2. **subtract** - Subtracts the second number from the first
   - Parameters:
     - `a` (double): First number
     - `b` (double): Second number
   - Returns: Difference between the two numbers

3. **multiply** - Multiplies two numbers
   - Parameters:
     - `a` (double): First number
     - `b` (double): Second number
   - Returns: Product of the two numbers

4. **divide** - Divides the first number by the second
   - Parameters:
     - `a` (double): Numerator
     - `b` (double): Denominator
   - Returns: Quotient of the division
   - Note: Throws DivideByZeroException if denominator is 0

### Business Associate Tools

1. **GetBusinessAssociates** - Get business associates by type
   - Parameters:
     - `baType` (string): Business Associate Type (e.g., 'vendor', 'customer')
   - Returns: List of BusinessAssociate objects

2. **CreateBusinessAssociate** - Create a new business associate
   - Parameters:
     - `baType` (string): Business Associate Type (e.g., 'vendor', 'customer')
     - `baName` (string): Business Associate Name
     - `sapVendor` (string, optional): SAP Vendor ID
     - `sapCustomer` (string, optional): SAP Customer ID
     - `sapCompanyCode` (int?, optional): SAP Company Code
   - Returns: Created BusinessAssociate object or null if failed

3. **UpdateBusinessAssociate** - Update an existing business associate
   - Parameters:
     - `baType` (string): Business Associate Type
     - `baId` (int): Business Associate ID
     - `baName` (string, optional): Updated Business Associate Name
     - `sapVendor` (string, optional): Updated SAP Vendor ID
     - `sapCustomer` (string, optional): Updated SAP Customer ID
     - `sapCompanyCode` (int?, optional): Updated SAP Company Code
   - Returns: Updated BusinessAssociate object or null if failed

4. **DeleteBusinessAssociate** - Delete a business associate
   - Parameters:
     - `baType` (string): Business Associate Type
     - `baId` (int): Business Associate ID
   - Returns: Boolean indicating success or failure

## Connecting with an MCP Client

### Using C# Client

```csharp
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;

var endPoint = "http://localhost:5267/sse";
var clientTransport = new SseClientTransport(new SseClientTransportOptions 
{ 
    Endpoint = new Uri(endPoint) 
});

var client = await McpClientFactory.CreateAsync(clientTransport);

// List available tools
var tools = await client.ListToolsAsync();

// Example: Call calculator tool
var result = await client.CallToolAsync("add", new Dictionary<string, object>
{
    ["a"] = 5.0,
    ["b"] = 3.0
});

// Example: Get business associates
var vendors = await client.CallToolAsync("GetBusinessAssociates", new Dictionary<string, object>
{
    ["baType"] = "vendor"
});

// Example: Create a new business associate
var newBA = await client.CallToolAsync("CreateBusinessAssociate", new Dictionary<string, object>
{
    ["baType"] = "vendor",
    ["baName"] = "Acme Corporation",
    ["sapVendor"] = "V12345",
    ["sapCompanyCode"] = 1000
});

// Example: Update a business associate
var updatedBA = await client.CallToolAsync("UpdateBusinessAssociate", new Dictionary<string, object>
{
    ["baType"] = "vendor",
    ["baId"] = 123,
    ["baName"] = "Acme Corp Updated",
    ["sapVendor"] = "V12345-NEW"
});

// Example: Delete a business associate
var deleteResult = await client.CallToolAsync("DeleteBusinessAssociate", new Dictionary<string, object>
{
    ["baType"] = "vendor",
    ["baId"] = 123
});
```

### Using GitHub Copilot or Other MCP Clients

Configure your MCP client to connect to:
- Transport: HTTP/SSE
- URL: `http://localhost:5267/sse`

The tools will be automatically discovered and available for use.

## Testing Individual Endpoints

### Weather Forecast (Default Endpoint)

```powershell
curl http://localhost:5267/weatherforecast
```

This will return sample weather forecast data from the default Web API template.

## Docker Deployment

The application can be containerized using .NET's built-in container support:

```powershell
dotnet publish --os linux --arch x64 -p:PublishProfile=DefaultContainer
```

## Configuration

### Business Associate API Endpoint

The Business Associate Tools are configured to connect to a backend API. By default, it's set to:

```csharp
private readonly string _baseUrl = "http://localhost:3000";
```

To change this, modify the `_baseUrl` field in `BusinessAssociateTools.cs` or consider making it configurable through `appsettings.json`.

### Required Backend API Endpoints

The Business Associate API should implement these endpoints:

- `GET /api/v1/{baType}` - Retrieve all business associates of a specific type
- `POST /api/v1/{baType}` - Create a new business associate
- `PUT /api/v1/{baType}/{baId}` - Update an existing business associate
- `DELETE /api/v1/{baType}/{baId}` - Delete a business associate

## Notes

- The MCP server uses Server-Sent Events (SSE) for streaming communication
- All tools are automatically discovered via the `WithToolsFromAssembly()` method
- Tools can be called asynchronously and support dependency injection
- The server follows the Model Context Protocol specification for tool invocation
- Business Associate Tools require the backend API to be running and accessible
- HttpClient is registered as a service and injected into BusinessAssociateTools
