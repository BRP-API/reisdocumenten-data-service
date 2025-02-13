using Newtonsoft.Json.Linq;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Shared.Util;
using System.Text.RegularExpressions;

namespace Rvig.HaalCentraalApi.Shared.Validation;
public static class ValidationHelperBase
{
    public static async Task ValidateGemeenteInschrijving(string? gemeenteVanInschrijving, IDomeinTabellenRepo domeinTabellenRepo)
    {
        if (string.IsNullOrEmpty(gemeenteVanInschrijving))
            return;

        if (gemeenteVanInschrijving.Length != 4 || !long.TryParse(gemeenteVanInschrijving, out long longValue) || longValue == 0 || string.IsNullOrEmpty(await domeinTabellenRepo.GetGemeenteNaam(longValue)))
            throw new InvalidParamsException(
                new List<InvalidParams>
                {
                    new InvalidParams {
                        Code = "table",
                        Name = "gemeenteVanInschrijving",
                        Reason = "Waarde komt niet voor in de tabel."
                    }
                }
            );
    }

    public static async Task ValidateVoorvoegsel(string? voorvoegsel, IDomeinTabellenRepo domeinTabellenRepo)
    {
        if (string.IsNullOrEmpty(voorvoegsel))
            return;

        if (!await domeinTabellenRepo.VoorvoegselExist(voorvoegsel))
            throw new InvalidParamsException(
                new List<InvalidParams>
                {
                    new InvalidParams {
                        Code = "table",
                        Name = "voorvoegsel",
                        Reason = "Waarde komt niet voor in de tabel."
                    }
                }
            );
    }

    public static void ValidateBurgerservicenummers(List<string>? burgerservicenummers)
    {
        if (burgerservicenummers == null || !burgerservicenummers.Any())
        {
            return;
        }

        var invalidParams = new List<InvalidParams>();
        var searchModelParam = "burgerservicenummer";

        burgerservicenummers.ForEach(x =>
        {
		if (x.Length < 9)
                invalidParams.Add(CreateInvalidParam(ValidationErrorMessages.MinLength.Replace(@"\d*", "9"), searchModelParam));
            if (x.Length > 9)
                invalidParams.Add(CreateInvalidParam(ValidationErrorMessages.MaxLength.Replace(@"\d*", "9"), searchModelParam));
            if (!Regex.IsMatch(x, "^[0-9]*$"))
                invalidParams.Add(CreateInvalidParam(ValidationErrorMessages.PatternBsn, searchModelParam));
        });

        if (invalidParams.Any())
            throw new InvalidParamsException(invalidParams);
    }

    public static void ValidateBurgerservicenummers(string? burgerservicenummer)
    {
        if (string.IsNullOrEmpty(burgerservicenummer))
        {
            return;
        }

        var burgerservicenummers = burgerservicenummer.Split(',').Distinct().ToList();
        ValidateBurgerservicenummers(burgerservicenummers);
    }

	public static void ValidateBurgerservicenummers(IEnumerable<string>? bsns)
	{
		var burgerservicenummers = bsns?.Distinct().ToList();

		if (burgerservicenummers?.Any() == true)
		{
			ValidateBurgerservicenummers(burgerservicenummers);
		}
		else
		{
			throw new InvalidParamCombinationException("Combinatie van gevulde velden was niet correct. De correcte veld combinatie is burgerservicenummer.");
		}
	}

	public static bool IsPeildatumBetweenStartAndEndDates(DateTime? peildatum, string? startString, string? endString)
	{
		var start = CreateDatumOnvolledig(startString);
		var end = CreateDatumOnvolledig(endString);

		var appliesOnStart = peildatum == null || start?.IsBefore(peildatum.Value) != false || start.IsOn(peildatum.Value);
		var appliesOnEnd = peildatum == null || end?.IsAfter(peildatum.Value) != false || (end.OnlyYearHasValue() && end.IsOn(peildatum.Value));

		return appliesOnStart && appliesOnEnd;
	}

	public static bool TimePeriodesOverlap(DateTime? datumVan, DateTime? datumTot, string? startString, string? endString)
	{
		var start = CreateDatumOnvolledig(startString);
		var end = CreateDatumOnvolledig(endString);

		var datumVanApplies = datumVan == null || end?.IsAfter(datumVan.Value) != false || (end.OnlyYearHasValue() && end.IsOn(datumVan.Value));
		var datumTotApplies = datumTot == null || start?.IsBefore(datumTot.Value) != false || start.IsOn(datumTot.Value);

		return datumVanApplies && datumTotApplies;
	}

	public static DatumOnvolledig? CreateDatumOnvolledig(string? date) => !string.IsNullOrWhiteSpace(date) ? new DatumOnvolledig(date) : null;

	public static void IsWildcardCorrectlyUsed(string? parameter, string name)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            return;
        }

        if (parameter.Contains("*"))
        {
            var result = parameter;

            // * at start of string is allowed
            if (result.Substring(0, 1).Equals("*"))
                result = result.Remove(0, 1);

            // * at end of string is allowed
            if (result.Length > 0 && result.Substring(result.Length - 1, 1).Equals("*"))
                result = result.Remove(result.Length - 1, 1);

            // * in the middle of the parameter is not allowed and * should be accompanied by at least 2 other characters
            if (result.Contains("*") || result.Length < 2)
                ThrowWildCardException(name, "*");
        }

        if (parameter.Contains("?"))
        {
            var result = parameter;

            // multiple ? at start of string is allowed
            while (result.Length > 0 && result.Substring(0, 1).Equals("?"))
                result = result.Remove(0, 1);

            // multiple ? at end of string is allowed
            while (result.Length > 0 && result.Substring(result.Length - 1, 1).Equals("?"))
                result = result.Remove(result.Length - 1, 1);

            // ? in the middle of the parameter is not allowed and ? should be accompanied by at least 2 other characters
            if (result.Contains("?") || result.Length < 2)
                ThrowWildCardException(name, "?");
        }
    }

    private static InvalidParams CreateInvalidParam(string errorMessage, string parameterName)
    {
        return new InvalidParams { Code = ValidationErrorMessages.GetInvalidParamCode(errorMessage), Name = parameterName, Reason = errorMessage };
    }

    private static void ThrowWildCardException(string paramName, string wildcard)
    {
        throw new InvalidParamsException(
            new List<InvalidParams>
            {
                    new InvalidParams {
                        Code = "wildcard",
                        Name = paramName,
                        Reason = $"Incorrect gebruik van wildcard karakter {wildcard}."
                    }
            }
        );
    }
}