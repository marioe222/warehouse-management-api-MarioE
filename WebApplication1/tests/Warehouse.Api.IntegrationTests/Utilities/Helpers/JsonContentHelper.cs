using System.Text;
using System.Text.Json;

namespace Warehouse.Api.IntegrationTests.Utilities.Helpers;

public static class JsonContentHelper
{
    public static StringContent Create(object value)
    {
        var json = JsonSerializer.Serialize(value);

        return new StringContent(
            json,
            Encoding.UTF8,
            "application/json");
    }
}