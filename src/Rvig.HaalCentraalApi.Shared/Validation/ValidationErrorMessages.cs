using Rvig.HaalCentraalApi.Shared.Exceptions;
using System.Text.RegularExpressions;

namespace Rvig.HaalCentraalApi.Shared.Validation;
public static class ValidationErrorMessages
{
	public static string Required { get => "Parameter is verplicht."; }
	public static string PatternBsn { get => "Waarde voldoet niet aan patroon ^[0-9]*$."; }
	public static string PatternPostcode { get => "Waarde voldoet niet aan patroon ^[1-9]{{1}}[0-9]{{3}}[A-Z]{{2}}$."; }
	public static string PatternGemeenteVanInschrijving { get => "Waarde voldoet niet aan patroon ^[0-9]{{4}}$."; }
	public static string MinLength { get => @"Waarde is korter dan minimale lengte \d*."; }
	public static string MinItems { get => @"Array bevat minder dan \d* items."; }
	public static string MaxItems { get => @"Array bevat meer dan \d* items."; }
	public static string MaxLength { get => @"Waarde is langer dan maximale lengte \d*."; }
	public static string RangeMinimum { get => @"Waarde is lager dan minimum \d*."; }
	public static string RangeMaximum { get => @"Waarde is hoger dan maximum \d*."; }

	public static string UnexpectedParam { get => "Parameter is niet verwacht."; }

	public static string EnumParse { get => "Waarde heeft geen geldige waarde uit de enumeratie."; }
	public static string StringParse { get => "Waarde is geen geldige string."; }
	public static string IntParse { get => "Waarde is geen geldig getal."; }
	public static string BoolParse { get => "Waarde is geen boolean."; }
	public static string DateParse { get => "Waarde is geen geldige datum."; }
	public static string Value { get => "Waarde is geen geldig zoek type."; }

	private static IDictionary<string, InvalidParamCode> _errorCodeLookup = new Dictionary<string, InvalidParamCode>();
	private static IDictionary<InvalidParamCode, string> _parseErrorMessageLookup = new Dictionary<InvalidParamCode, string>();

	/// <summary>
	/// This lookup is used to get the correct validation type for an error message when creating the error response based on an invalid modelstate.
	/// Alternative implementations are:
	/// 1. find the corresponding property validation attribute with the same error, base the type on the type of this attribute. Cons: reflection needed, harder to maintain, annoying to deal with custom attributes.
	/// 2. add the type to the start or end of the errormessage itself, when creating the error reponse split the errormessage to retrieve the type en the message itself.
	///    This is probably the best way to achieve parametrisation of error messages if ever wanted/needed.
	/// </summary>
	/// <returns></returns>
	private static Dictionary<string, InvalidParamCode> CreateErrorCodeLookup()
	{
		return new Dictionary<string, InvalidParamCode>
			{
				{ Required, InvalidParamCode.required },
				{ PatternBsn, InvalidParamCode.pattern },
				{ PatternPostcode, InvalidParamCode.pattern },
				{ PatternGemeenteVanInschrijving, InvalidParamCode.pattern },
				{ "must match the regular expression", InvalidParamCode.pattern },
				{ "Waarde voldoet niet aan patroon", InvalidParamCode.pattern },
				{ "Waardes voldoen niet aan patroon", InvalidParamCode.pattern },
				{ MinItems, InvalidParamCode.minItems },
				{ MaxItems, InvalidParamCode.maxItems },
				{ MinLength, InvalidParamCode.minLength },
				{ MaxLength, InvalidParamCode.maxLength },
				{ RangeMinimum, InvalidParamCode.minimum },
				{ RangeMaximum, InvalidParamCode.maximum },

				{ UnexpectedParam, InvalidParamCode.unknownParam },

				{ EnumParse, InvalidParamCode._enum },
				{ StringParse, InvalidParamCode._string },
				{ IntParse, InvalidParamCode.integer },
				{ BoolParse, InvalidParamCode.boolean },
				{ DateParse, InvalidParamCode.date },
				{ Value, InvalidParamCode.value }
			};
	}

	private static Dictionary<InvalidParamCode, string> CreateParseErrorMessageLookup()
	{
		return new Dictionary<InvalidParamCode, string>
			{
				{ InvalidParamCode._enum, EnumParse },
				{ InvalidParamCode._string, StringParse},
				{ InvalidParamCode.integer, IntParse },
				{ InvalidParamCode.boolean, BoolParse},
				{ InvalidParamCode.date, DateParse},
				{ InvalidParamCode.value, Value}
			};
	}

	public static string? GetInvalidParamCode(string errorMessage)
	{
		if (_errorCodeLookup.Count == 0)
			_errorCodeLookup = CreateErrorCodeLookup();

		if (_errorCodeLookup.Any(code => Regex.IsMatch(errorMessage, code.Key)))
		{
			return _errorCodeLookup.SingleOrDefault(code => Regex.IsMatch(errorMessage, code.Key)).Value.ToString().Replace("_", ""); //remove _ for _enum and _string
		}
		else if (_errorCodeLookup.ContainsKey(errorMessage))
		{
			return _errorCodeLookup[errorMessage].ToString().Replace("_", ""); //remove _ for _enum and _string
		}

		return null;
	}

	public static string? GetParseErrorMessage(InvalidParamCode? type)
	{
		if (_parseErrorMessageLookup.Count == 0)
			_parseErrorMessageLookup = CreateParseErrorMessageLookup();

		if (!type.HasValue || !_parseErrorMessageLookup.ContainsKey(type.Value))
			return null;

		return _parseErrorMessageLookup[type.Value];
	}
}