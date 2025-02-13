using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
    public enum InvalidParamCode
    {
        [EnumMember(Value = "minLength")]
        minLength,

        [EnumMember(Value = "minItems")]
		minItems,

        [EnumMember(Value = "maxItems")]
		maxItems,

        [EnumMember(Value = "maxLength")]
        maxLength,

        [EnumMember(Value = "required")]
        required,

        [EnumMember(Value = "minimum")]
        minimum,

        [EnumMember(Value = "maximum")]
        maximum,

        [EnumMember(Value = "pattern")]
        pattern,

        [EnumMember(Value = "enum")]
        _enum,

        [EnumMember(Value = "string")]
        _string,

        [EnumMember(Value = "integer")]
        integer,

        [EnumMember(Value = "boolean")]
        boolean,

        [EnumMember(Value = "date")]
        date,

        [EnumMember(Value = "unknownParam")]
        unknownParam,

        [EnumMember(Value = "value")]
        value,

		[EnumMember(Value = "syntaxError")]
		syntaxError
	}
}
