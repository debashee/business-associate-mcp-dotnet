using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json; // Added for JSON deserialization
using System.Net.Http; // Added for HttpClient
using McpServerSample.Models; // Added to access BusinessAssociate record
using System.Collections.Generic; // Added for List<T>
using System.Text; // Added for StringContent

namespace McpServerSample.Tools;

[McpServerToolType]
public class BusinessAssociateTools
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://localhost:3000"; // https://business-associate-nodejs-a2hrenh7b6gmgyay.centralus-01.azurewebsites.net

    public BusinessAssociateTools(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    [McpServerTool, Description("Get the business associate details based on the type")]
    public async Task<List<BusinessAssociate>> GetBusinessAssociates(
        [Description("Business Associate Type")]
        string baType)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/{baType}");
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

    [McpServerTool, Description("Create a new business associate")]
    public async Task<BusinessAssociate?> CreateBusinessAssociate(
        [Description("Business Associate Type (e.g., 'vendor', 'customer')")]
        string baType,
        [Description("Business Associate Name")]
        string baName,
        [Description("SAP Vendor ID (optional)")]
        string? sapVendor = null,
        [Description("SAP Customer ID (optional)")]
        string? sapCustomer = null,
        [Description("SAP Company Code (optional)")]
        int? sapCompanyCode = null)
    {
        var newBusinessAssociate = new
        {
            BAName = baName,
            SAPVendor = sapVendor,
            SAPCustomer = sapCustomer,
            SAPCompanyCode = sapCompanyCode
        };

        var json = JsonSerializer.Serialize(newBusinessAssociate);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"{_baseUrl}/api/v1/{baType}", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var createResponse = JsonSerializer.Deserialize<CreateBusinessAssociateResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return createResponse?.Data;
    }

    [McpServerTool, Description("Update an existing business associate")]
    public async Task<BusinessAssociate?> UpdateBusinessAssociate(
        [Description("Business Associate Type (e.g., 'vendor', 'customer')")]
        string baType,
        [Description("Business Associate ID")]
        int baId,
        [Description("Updated Business Associate Name (optional)")]
        string? baName = null,
        [Description("Updated SAP Vendor ID (optional)")]
        string? sapVendor = null,
        [Description("Updated SAP Customer ID (optional)")]
        string? sapCustomer = null,
        [Description("Updated SAP Company Code (optional)")]
        int? sapCompanyCode = null)
    {
        var updateData = new Dictionary<string, object?>();
        
        if (!string.IsNullOrEmpty(baName))
            updateData["BAName"] = baName;
        if (!string.IsNullOrEmpty(sapVendor))
            updateData["SAPVendor"] = sapVendor;
        if (!string.IsNullOrEmpty(sapCustomer))
            updateData["SAPCustomer"] = sapCustomer;
        if (sapCompanyCode.HasValue)
            updateData["SAPCompanyCode"] = sapCompanyCode;

        var json = JsonSerializer.Serialize(updateData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PutAsync($"{_baseUrl}/api/v1/{baType}/{baId}", content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var updateResponse = JsonSerializer.Deserialize<UpdateBusinessAssociateResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return updateResponse?.Data;
    }

    [McpServerTool, Description("Delete a business associate")]
    public async Task<bool> DeleteBusinessAssociate(
        [Description("Business Associate Type (e.g., 'vendor', 'customer')")]
        string baType,
        [Description("Business Associate ID")]
        int baId)
    {
        var response = await _httpClient.DeleteAsync($"{_baseUrl}/api/v1/{baType}/{baId}");
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var deleteResponse = JsonSerializer.Deserialize<DeleteBusinessAssociateResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return deleteResponse?.Success ?? false;
    }
}

// Response classes for API operations
public class CreateBusinessAssociateResponse
{
    public bool Success { get; set; }
    public BusinessAssociate? Data { get; set; }
    public string? Message { get; set; }
}

public class UpdateBusinessAssociateResponse
{
    public bool Success { get; set; }
    public BusinessAssociate? Data { get; set; }
    public string? Message { get; set; }
}

public class DeleteBusinessAssociateResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
}
