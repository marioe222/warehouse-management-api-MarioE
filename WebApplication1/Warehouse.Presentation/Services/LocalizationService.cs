using System.Globalization;
using Warehouse.Application.Interfaces;
using Warehouse.Presentation.Resources;

namespace Warehouse.Presentation.Services;

public class LocalizationService : ILocalizationService
{
    public string GetString(string key)
    {
        return SharedResources.ResourceManager.GetString(key, CultureInfo.CurrentUICulture)
               ?? key;
    }
}