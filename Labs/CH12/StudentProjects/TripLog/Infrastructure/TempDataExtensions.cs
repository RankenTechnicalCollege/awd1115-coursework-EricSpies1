using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace TripLog.Infrastructure
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            => tempData[key] = JsonSerializer.Serialize(value);

        public static T? Get<T>(this ITempDataDictionary tempData, string key)
            => tempData.TryGetValue(key, out var o) && o is string s
               ? JsonSerializer.Deserialize<T>(s)
               : default;
    }
}
