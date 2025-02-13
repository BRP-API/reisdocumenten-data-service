using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;

namespace Rvig.HaalCentraalApi.Shared.Util
{
	public class JsonArrayConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return typeof(IEnumerable<object>).IsAssignableFrom(objectType);
		}

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}

			if (reader.TokenType == JsonToken.String)
			{
				throw new InvalidParamsException(new List<InvalidParams> { new InvalidParams { Code = "array", Name = reader.Path, Reason = "Parameter is geen array." } });
			}

			// Load JObject from stream
			JToken jToken = JToken.Load(reader);

			//// Create target object based on JObject
			var resultObject = Activator.CreateInstance(objectType);

			if (resultObject != null)
			{
				// Populate the object properties
				// N.B.: After loading a JObject from the reader, the JsonReader source will have been consumed, so use the JObject to construct another reader.
				serializer.Populate(jToken.CreateReader(), resultObject);
			}

			return resultObject;
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			throw new CustomNotImplementedException();
		}
	}
}