using Newtonsoft.Json;

namespace Rvig.BrpApi.Shared.Helpers;

public static class ObjectHelper
{
    public static IEnumerable<string> GetPropertyNames<T>(IEnumerable<string> excludePropNames) => typeof(T).GetProperties()
                                                                                                                    .Select(prop => prop.Name)
                                                                                                                    .Where(name => !excludePropNames.Contains(name)).ToList();
    public static IEnumerable<string> GetPropertyNames<T>() => typeof(T).GetProperties().Select(prop => prop.Name).ToList();
    public static T? InstanceOrNullWhenDefault<T>(T mappedObject) => !AllPropertiesDefault(mappedObject) ? mappedObject : default;
    public static bool AllPropertiesDefault<T>(T source) => typeof(T).GetProperties().Where(propInfo => !propInfo.Name.StartsWith("_")).All(propInfo => propInfo.GetValue(source) == default);

    public static T DeepClone<T>(T source)
    {
        var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
        var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
        var serializedObject = JsonConvert.SerializeObject(source, serializeSettings);
        return JsonConvert.DeserializeObject<T>(serializedObject, deserializeSettings) ?? source;
    }
}