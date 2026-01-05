using System.Collections.Generic; // Added for List<T>

namespace McpServerSample.Models;

public record BusinessAssociate(int BAID, string BAName, string? SAPVendor, string? SAPCustomer, int? SAPCompanyCode);

public class BusinessAssociateApiResponse
{
    public bool Success { get; set; }
    public int Count { get; set; }
    public List<BusinessAssociate>? Data { get; set; }
}