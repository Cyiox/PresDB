using WebPresDB.Models;

namespace WebPresDB.Services;

public interface IPropertyService
{
    Task<List<PropertyModel>> SearchPropertiesAsync(string searchTerm, string city, string propertyId);
}