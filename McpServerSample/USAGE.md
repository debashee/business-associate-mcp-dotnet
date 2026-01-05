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

### String Tools

1. **concatenate** - Concatenates multiple strings together
   - Parameters:
     - `strings` (string[]): Array of strings to concatenate
     - `separator` (string, optional): Separator to use between strings
   - Returns: Concatenated string

2. **reverse** - Reverses a string
   - Parameters:
     - `input` (string): String to reverse
   - Returns: Reversed string

3. **toUpperCase** - Converts a string to uppercase
   - Parameters:
     - `input` (string): String to convert
   - Returns: Uppercase string

4. **toLowerCase** - Converts a string to lowercase
   - Parameters:
     - `input` (string): String to convert
   - Returns: Lowercase string

5. **wordCount** - Counts the number of words in a string
   - Parameters:
     - `input` (string): String to count words in
   - Returns: Number of words (integer)

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

// Call a tool
var result = await client.CallToolAsync("add", new Dictionary<string, object>
{
    ["a"] = 5.0,
    ["b"] = 3.0
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

## Notes

- The MCP server uses Server-Sent Events (SSE) for streaming communication
- All tools are automatically discovered via the `WithToolsFromAssembly()` method
- Tools can be called asynchronously and support dependency injection
- The server follows the Model Context Protocol specification for tool invocation
