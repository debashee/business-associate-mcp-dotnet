using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json; // Added for JSON deserialization
using System.Net.Http; // Added for HttpClient
using McpServerSample.Models; // Added to access BusinessAssociate record
using System.Collections.Generic; // Added for List<T>

namespace McpServerSample.Tools;

[McpServerToolType]
public class BusinessAssociateTools
{
    private readonly HttpClient _httpClient;

    public BusinessAssociateTools(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [McpServerTool, Description("Get the business associate details based on the type")]
    public async Task<List<BusinessAssociate>> GetBusinessAssociates(
        [Description("Business Associate Type")]
        string baType)
    {
        var response = await _httpClient.GetAsync($"https://business-associate-nodejs-a2hrenh7b6gmgyay.centralus-01.azurewebsites.net/api/v1/{baType}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var baResponse = JsonSerializer.Deserialize<BusinessAssociateApiResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if(baResponse is null || baResponse?.Success == false)
        {
            return [];
        }

        var businessAssociates = baResponse?.Data;
        return businessAssociates ?? [];
    }
}
