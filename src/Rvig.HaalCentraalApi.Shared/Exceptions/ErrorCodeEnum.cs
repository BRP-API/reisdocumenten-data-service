using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Exceptions
{
    public enum ErrorCode
    {
        [EnumMember(Value = "paramsRequired")]
        paramsRequired,

        [EnumMember(Value = "paramsCombination")]
        paramsCombination,

        [EnumMember(Value = "unsupportedCombi")]
        unsupportedCombi,

        [EnumMember(Value = "paramsValidation")]
        paramsValidation,

        [EnumMember(Value = "tooManyResults")]
        tooManyResults,

        [EnumMember(Value = "authentication")]
        authentication,

        [EnumMember(Value = "authorization")]
		authorization,

        [EnumMember(Value = "unauthorizedField")]
		unauthorizedField,

        [EnumMember(Value = "unauthorizedParameter")]
		unauthorizedParameter,

        [EnumMember(Value = "unauthorized")]
		unauthorized,

        [EnumMember(Value = "notFound")]
        notFound,

        [EnumMember(Value = "notAcceptable")]
        notAcceptable,

        [EnumMember(Value = "crsNotAcceptable")]
        crsNotAcceptable,

        [EnumMember(Value = "contentCrsMissing")]
        contentCrsMissing,

        [EnumMember(Value = "acceptCrsMissing")]
        acceptCrsMissing,

        [EnumMember(Value = "crsNotSupported")]
        crsNotSupported,

        [EnumMember(Value = "serverError")]
        serverError,

        [EnumMember(Value = "sourceUnavailable ")]
        sourceUnavailable,

        [EnumMember(Value = "notUnique")]
        notUnique,

        [EnumMember(Value = "syntaxError")]
        syntaxError,

        [EnumMember(Value = "unsupportedMediaType")]
        unsupportedMediaType,

        [EnumMember(Value = "methodNotAllowed")]
		methodNotAllowed
	}
}
