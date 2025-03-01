﻿using Newtonsoft.Json;
using Rvig.BrpApi.Shared.ApiModels.Universal;
using Rvig.BrpApi.Shared.Exceptions;

namespace Rvig.BrpApi.Shared.Util;

public class BooleanJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(bool);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        switch (reader.Value?.ToString()?.ToLower()?.Trim())
        {
            case "true":
                return true;
            case "false":
                return false;
            default:
                throw new JsonSerializationException("Waarde is geen boolean.");
        }
    }

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
    }
}
