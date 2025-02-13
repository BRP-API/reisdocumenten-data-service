using Newtonsoft.Json;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;

namespace Rvig.HaalCentraalApi.Shared.Util;

public class StringToNullableIntegerJsonConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(int?);
	}

	public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
	{
		var value = reader.Value?.ToString()?.ToLower()?.Trim();

		// Empty string is allowed but will be blocked from validation when used for search queries in the API.
		if (int.TryParse(value, out var intValue))
		{
			return intValue;
		}
		else if (value == null || value == "")
		{
			return null;
		}
		throw new JsonSerializationException("Waarde is geen geldig getal.");
	}

	public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
	{
	}
}
