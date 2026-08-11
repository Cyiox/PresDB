using WebPresDB.Models;

namespace WebPresDB.Services;

public class PropertyService : IPropertyService
{
    public async Task<List<PropertyModel>> SearchPropertiesAsync(string searchTerm, string city, string propertyId)
    {
        // Mock data response simulating database latency
        await Task.Delay(200);

        return new List<PropertyModel>
        {
            new PropertyModel { PropertyId = "P-101", Name = "Oakridge Apartments", City = "Boston" },
            new PropertyModel { PropertyId = "P-102", Name = "Harbor View Towers", City = "Cambridge" }
        };
    }
}