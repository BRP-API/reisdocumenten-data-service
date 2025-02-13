using Rvig.Data.Base.Providers;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Util;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Rvig.Data.Base.Postgres.Mappers.Helpers;
public static class GbaMappingHelper
{
    public static Waardetabel? ParseToGeslachtEnum(string? geslachtCode)
    {
        return (geslachtCode?.ToUpper()) switch
        {
            "M" => new Waardetabel { Code = geslachtCode, Omschrijving = "man" },
            "V" => new Waardetabel { Code = geslachtCode, Omschrijving = "vrouw" },
            "O" => new Waardetabel { Code = geslachtCode, Omschrijving = "onbekend" },
            _ => null
        };
    }

    public static Waardetabel? ParseToRedenOpschortingBijhoudingEnum(string? code)
    {
        return (code?.ToUpper()) switch
        {
            "E" => new Waardetabel { Code = code, Omschrijving = "emigratie" },
            "F" => new Waardetabel { Code = code, Omschrijving = "fout" },
            "M" => new Waardetabel { Code = code, Omschrijving = "ministerieel besluit" },
            "O" => new Waardetabel { Code = code, Omschrijving = "overlijden" },
            "R" => new Waardetabel { Code = code, Omschrijving = "pl is aangelegd in de rni" },
            "." => new Waardetabel { Code = code, Omschrijving = "onbekend" },
            _ => null
        };
	}

    public static Waardetabel? ParseToSoortReisdocumentEnum(string? code)
    {
        return (code?.ToUpper()) switch
        {
			".." => new Waardetabel { Code = code, Omschrijving = "Onbekend" },
			"BJ" => new Waardetabel { Code = code, Omschrijving = "Identiteitskaart (toeristenkaart) BJ" },
			"EK" => new Waardetabel { Code = code, Omschrijving = "Europese identiteitskaart" },
			"IA" => new Waardetabel { Code = code, Omschrijving = "Identiteitskaart (toeristenkaart) A" },
			"IB" => new Waardetabel { Code = code, Omschrijving = "Identiteitskaart (toeristenkaart) B" },
			"IC" => new Waardetabel { Code = code, Omschrijving = "Identiteitskaart (toeristenkaart) C" },
			"ID" => new Waardetabel { Code = code, Omschrijving = "Gemeentelijke Identiteitskaart" },
			"KE" => new Waardetabel { Code = code, Omschrijving = "Kaart met paspoortboekje, 64 pag." },
			"KN" => new Waardetabel { Code = code, Omschrijving = "Kaart met paspoortboekje, 32 pag." },
			"KZ" => new Waardetabel { Code = code, Omschrijving = "Kaart zonder paspoortboekje" },
			"LP" => new Waardetabel { Code = code, Omschrijving = "Laissez-passer" },
			"NB" => new Waardetabel { Code = code, Omschrijving = "Nooddocument (model reisdocument vreemdelingen)" },
			"NI" => new Waardetabel { Code = code, Omschrijving = "Nederlandse identiteitskaart" },
			"NN" => new Waardetabel { Code = code, Omschrijving = "Noodpaspoort (model nationaal paspoort)" },
			"NP" => new Waardetabel { Code = code, Omschrijving = "Noodpaspoort" },
			"NV" => new Waardetabel { Code = code, Omschrijving = "Nooddocument (model reisdocument vluchtelingen)" },
			"PB" => new Waardetabel { Code = code, Omschrijving = "Reisdocument voor vreemdelingen" },
			"PD" => new Waardetabel { Code = code, Omschrijving = "Diplomatiek paspoort" },
			"PF" => new Waardetabel { Code = code, Omschrijving = "Faciliteitenpaspoort" },
			"PN" => new Waardetabel { Code = code, Omschrijving = "Nationaal paspoort" },
			"PV" => new Waardetabel { Code = code, Omschrijving = "Reisdocument voor vluchtelingen" },
			"PZ" => new Waardetabel { Code = code, Omschrijving = "Dienstpaspoort" },
			"R1" => new Waardetabel { Code = code, Omschrijving = "Reisdocument ouder1" },
			"R2" => new Waardetabel { Code = code, Omschrijving = "Reisdocument ouder2" },
			"RD" => new Waardetabel { Code = code, Omschrijving = "Reisdocument voogd" },
			"RM" => new Waardetabel { Code = code, Omschrijving = "Reisdocument moeder" },
			"RV" => new Waardetabel { Code = code, Omschrijving = "Reisdocument vader" },
			"TE" => new Waardetabel { Code = code, Omschrijving = "Tweede paspoort (zakenpaspoort)" },
			"TN" => new Waardetabel { Code = code, Omschrijving = "Tweede paspoort" },
			"ZN" => new Waardetabel { Code = code, Omschrijving = "Nationaal paspoort (zakenpaspoort)" },
			_ => null
        };
	}

    public static Waardetabel? ParseAanduidingInhoudingVermissingNederlandsReisdocument(string? code)
    {
        return (code?.ToUpper()) switch
        {
			"." => new Waardetabel { Code = code, Omschrijving = "onbekend" },
			"I" => new Waardetabel { Code = code, Omschrijving = "ingehouden of ingeleverd" },
			"R" => new Waardetabel { Code = code, Omschrijving = "van rechtswege vervallen" },
			"V" => new Waardetabel { Code = code, Omschrijving = "vermist" },
			_ => null
        };
	}

    public static string? ParseToDatumOnvolledig(int? incompleteDate)
    {
        if (!incompleteDate.HasValue)
        {
            return null;
        }
        var incompleteDateString = incompleteDate.Value.ToString();

        if (incompleteDateString.Length == 1)
        {
            return "00000000";
        }

        var jaar = incompleteDateString.Length >= 4 ? int.Parse(incompleteDateString.Substring(0, 4)).ToString().PadLeft(4, '0') : null;
        var maand = incompleteDateString.Length >= 6 ? int.Parse(incompleteDateString.Substring(4, 2)).ToString().PadLeft(2, '0') : null;
        var dag = incompleteDateString.Length == 8 ? int.Parse(incompleteDateString.Substring(6, 2)).ToString().PadLeft(2, '0') : null;

        return jaar + maand + dag;
    }

    public static bool IsDatumOnvolledigNullOrFutureDate(string? datum, ICurrentDateTimeProvider currentDateProvider)
    {
        if (string.IsNullOrWhiteSpace(datum) || datum.Equals("00000000"))
        {
            return true;
        }

        var today = currentDateProvider.Today();

        int? jaar = int.Parse(datum.Substring(0, 4));
        int? maand = int.Parse(datum.Substring(4, 2));
        int? dag = int.Parse(datum.Substring(6, 2));

        return jaar > today.Year ||
                (jaar == today.Year && (!maand.HasValue || maand == 0 || maand > today.Month)) ||
                (jaar == today.Year && maand == today.Month && (!dag.HasValue || dag == 0 || dag > today.Day));
    }

    public static bool IsDatumOnvolledigBeforeOrOnPeildatum(string? datumOnvolledig, DateTime? peildatum)
	{
		if (string.IsNullOrWhiteSpace(datumOnvolledig) || !peildatum.HasValue)
		{
			return false;
		}
		else if (datumOnvolledig.Equals("0") || datumOnvolledig.Equals("0000-00-00"))
		{
			datumOnvolledig = "00000000";
		}

		int? jaar = int.Parse(datumOnvolledig.Substring(0, 4));
        int? maand = int.Parse(datumOnvolledig.Substring(4, 2));
        int? dag = int.Parse(datumOnvolledig.Substring(6, 2));

        return jaar != 0 && (jaar < peildatum.Value.Year ||
                (jaar == peildatum.Value.Year && (maand.HasValue && maand != 0 && maand < peildatum.Value.Month)) ||
                (jaar == peildatum.Value.Year && maand == peildatum.Value.Month && (dag.HasValue && dag != 0 && dag < peildatum.Value.Day)) ||
				jaar == peildatum.Value.Year && maand == peildatum.Value.Month && dag == peildatum.Value.Day);
    }

    public static bool IsDatumVolledigBetweenVanTot(string? currentDate, string? nextDate, DateTime? van, DateTime? tot)
	{
		var currentDatumOnvolledig = new DatumOnvolledig(currentDate);
		var nextDatumOnvolledig = new DatumOnvolledig(nextDate);
		var currentOnzekerheidsperiode = CreateOnzekerheidsPeriodeDateTimes(currentDatumOnvolledig);
		var beginDateCurrentOnzekerheidsperiode = currentOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime;
		var nextOnzekerheidsperiode = CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig);
		var beginDateNextOnzekerheidsperiode = /*eindOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime.Equals(DateTime.MinValue) ? null :*/ nextOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime;

		if (currentDatumOnvolledig.IsCompleteOnvolledig())
		{
			return false;
		}
		else if (currentDatumOnvolledig.IsOnvolledig())
		{
			return
				(
					string.IsNullOrWhiteSpace(nextDate)
					&&
					(
						(currentDatumOnvolledig.Jaar < van?.Year)
						||
						(currentDatumOnvolledig.Jaar == van?.Year && currentDatumOnvolledig.Maand.HasValue && currentDatumOnvolledig.Maand < van?.Month)
					)
				)
				||
				(
				// todo
					!string.IsNullOrWhiteSpace(nextDate)
					&&
					(
						currentOnzekerheidsperiode.endOnzekerheidsPeriodeDateTime < tot && van < nextOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime
					)
				);
		}
		else
		{
			return
				(
					string.IsNullOrWhiteSpace(nextDate)
					&&
					(currentOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime < tot && van < DateTime.MaxValue)
				)
				||
				(
					!string.IsNullOrWhiteSpace(nextDate)
					&&
					!nextDatumOnvolledig.IsOnvolledig()
					&&
					currentOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime < tot && van < nextOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime
				)
				||
				(
					!string.IsNullOrWhiteSpace(nextDate)
					&&
					nextDatumOnvolledig.IsOnvolledig()
					&&
					(currentOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime < tot && van < (currentOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime > nextOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime ? currentOnzekerheidsperiode.endOnzekerheidsPeriodeDateTime : nextOnzekerheidsperiode.startOnzekerheidsPeriodeDateTime?.AddDays(-1)))
				);
		}

		//if (currentDatumOnvolledig.IsCompleteOnvolledig()
		//	||
		//	(
		//		currentDatumOnvolledig.IsOnvolledig()
		//		&&
		//		(
		//			(!string.IsNullOrWhiteSpace(nextDate) && beginDateCurrentOnzekerheidsperiode < tot && van < beginDateNextOnzekerheidsperiode)
		//		)
		//	)
		//)
		//{
		//	return false;
		//}

		//if (string.IsNullOrWhiteSpace(nextDate))
		//{
		//	return beginDateCurrentOnzekerheidsperiode < tot && van < DateTime.MaxValue;
		//}

		//var end = nextDatumOnvolledig.IsCompleteOnvolledig() ? currentOnzekerheidsperiode.endOnzekerheidsPeriodeDateTime : beginDateNextOnzekerheidsperiode;
		//return beginDateCurrentOnzekerheidsperiode < tot && van < end;

		//if (startDatumOnvolledig.IsCompleteOnvolledig() || (startDatumOnvolledig.IsOnvolledig() && TwoOnzekerheidsPeriodesOverlap((beginDateStartOnzekerheidsperiode, beginDateEindOnzekerheidsperiode), (van, tot))))
		//{
		//	return false;
		//}
		//if (startDatumOnvolledig.IsCompleteOnvolledig()
		//	||
		//		(
		//			startDatumOnvolledig.IsOnvolledig()
		//			&&
		//				(
		//					TwoOnzekerheidsPeriodesOverlap(CreateOnzekerheidsPeriodeDateTimes(startDatumOnvolledig), (van, tot))
		//					//|| (startDatumOnvolledig.Jaar < van?.Year)
		//					//|| (startDatumOnvolledig.Jaar == van?.Year && startDatumOnvolledig.Maand < van?.Month)
		//				)
		//		)
		//	)
		//{
		//	return false;
		//}

		//DateTime? start = null;
		//if (startDatumOnvolledig.IsOnvolledig())
		//{
		//	if (startDatumOnvolledig.Maand.HasValue)
		//	{
		//		start = DateTime.Parse($"{startDatumOnvolledig.Jaar!.Value}-{startDatumOnvolledig.Maand!.Value + 1}-01");
		//	}
		//	else if (!startDatumOnvolledig.Maand.HasValue)
		//	{
		//		start = DateTime.Parse($"{startDatumOnvolledig.Jaar!.Value + 1}-01-01");
		//	}
		//}
		//else
		//{
		//	start = DateTime.Parse(startDatumOnvolledig.Datum!);
		//}

		//DateTime? eind = null;
		//if (string.IsNullOrWhiteSpace(eindDatum))
		//{
		//	eind = DateTime.MaxValue;
		//}
		//else
		//{
		//	if (eindDatumOnvolledig.IsCompleteOnvolledig())
		//	{
		//		eind = start!.Value.AddDays(1);
		//	}
		//	else if (eindDatumOnvolledig.IsOnvolledig())
		//	{
		//		if (eindDatumOnvolledig.Maand.HasValue)
		//		{
		//			if (eindDatumOnvolledig.Jaar.Equals(start!.Value.Year) && eindDatumOnvolledig.Maand.Equals(start!.Value.Month))
		//			{
		//				eind = DateTime.Parse($"{eindDatumOnvolledig.Jaar!.Value}-{eindDatumOnvolledig.Maand!.Value}-{start.Value.Day + 1}");
		//			}
		//			else
		//			{
		//				eind = DateTime.Parse($"{eindDatumOnvolledig.Jaar!.Value}-{eindDatumOnvolledig.Maand!.Value}-01");
		//			}
		//		}
		//		else if (!eindDatumOnvolledig.Maand.HasValue)
		//		{
		//			eind = DateTime.Parse($"{eindDatumOnvolledig.Jaar!.Value - 1}-01-01");
		//			eind = eind < van ? eind.Value.AddYears(1) : eind;
		//			eind = eind > start ? eind : (startDatum == eindDatum ? start : start!.Value.AddDays(1));
		//		}
		//	}
		//	else
		//	{
		//		eind = DateTime.Parse(eindDatumOnvolledig.Datum!);
		//	}
		//}

		//return (start < tot && eind > van) || (van < eind && tot > start);
	}

    public static bool IsPeildatumBeforeDatumOnvolledig(string? datumOnvolledig, DateTime? peildatum)
	{
		if (string.IsNullOrWhiteSpace(datumOnvolledig) || !peildatum.HasValue)
		{
			return false;
		}
		else if (datumOnvolledig.Equals("0") || datumOnvolledig.Equals("0000-00-00"))
		{
			datumOnvolledig = "00000000";
		}

		int? jaar = int.Parse(datumOnvolledig.Substring(0, 4));
        int? maand = int.Parse(datumOnvolledig.Substring(4, 2));
        int? dag = int.Parse(datumOnvolledig.Substring(6, 2));

        return jaar > peildatum.Value.Year ||
                (jaar == peildatum.Value.Year && (maand.HasValue && maand != 0 && maand > peildatum.Value.Month)) ||
                (jaar == peildatum.Value.Year && maand == peildatum.Value.Month && (dag.HasValue && dag != 0 && dag > peildatum.Value.Day));
    }

    public static bool IsDatumOnvolledigPotentiallyOnOrAfterPeildatum(string? datumOnvolledig, DateTime? peildatum)
	{
		if (string.IsNullOrWhiteSpace(datumOnvolledig) || !peildatum.HasValue)
		{
			return false;
		}
		else if (datumOnvolledig.Equals("0") || datumOnvolledig.Equals("0000-00-00"))
		{
			datumOnvolledig = "00000000";
		}

		int? jaar = int.Parse(datumOnvolledig.Substring(0, 4));
		int? maand = int.Parse(datumOnvolledig.Substring(4, 2));
		int? dag = int.Parse(datumOnvolledig.Substring(6, 2));

		return (!jaar.HasValue || jaar == 0 || peildatum.Value.Year > jaar) ||
				(peildatum.Value.Year == jaar && (!maand.HasValue || maand == 0 || peildatum.Value.Month > maand)) ||
				(peildatum.Value.Year == jaar && peildatum.Value.Month == maand && (!dag.HasValue || dag == 0 || peildatum.Value.Day >= dag));
	}

	/// <summary>
	/// Peildatum may not be the same date as the second date.
	/// </summary>
	/// <param name="firstDatumOnvolledigString"></param>
	/// <param name="secondDatumOnvolledigString"></param>
	/// <param name="peildatum"></param>
	/// <returns></returns>
    public static bool IsPeildatumBetweenTwoDatumOnvolledig(string? firstDatumOnvolledigString, string? secondDatumOnvolledigString, DateTime? peildatum)
	{
		if (!peildatum.HasValue || string.IsNullOrWhiteSpace(firstDatumOnvolledigString))
		{
			return false;
		}

		var firstDatumOnvolledig = new DatumOnvolledig(firstDatumOnvolledigString);
		var secondDatumOnvolledig = new DatumOnvolledig(secondDatumOnvolledigString);

		var peildatumYearAfterFirstDateYear = firstDatumOnvolledig.Jaar == null || firstDatumOnvolledig.Jaar.Equals(0) ? true : peildatum.Value.Year > firstDatumOnvolledig.Jaar;
		var peildatumYearOnFirstDateYear = firstDatumOnvolledig.Jaar == null || firstDatumOnvolledig.Jaar.Equals(0) ? false : peildatum.Value.Year == firstDatumOnvolledig.Jaar;
		var peildatumYearAfterSecondDateYear = secondDatumOnvolledig.Jaar == null || secondDatumOnvolledig.Jaar.Equals(0) ? false : peildatum.Value.Year > secondDatumOnvolledig.Jaar;
		var peildatumYearOnSecondDateYear = secondDatumOnvolledig.Jaar == null || secondDatumOnvolledig.Jaar.Equals(0) ? false : peildatum.Value.Year == secondDatumOnvolledig.Jaar;

		var peildatumMonthAfterFirstDateMonth = firstDatumOnvolledig.Maand == null || firstDatumOnvolledig.Maand.Equals(0) ? true : peildatum.Value.Month > firstDatumOnvolledig.Maand;
		var peildatumMonthOnFirstDateMonth = firstDatumOnvolledig.Maand == null || firstDatumOnvolledig.Maand.Equals(0) ? false : peildatum.Value.Month == firstDatumOnvolledig.Maand;
		var peildatumMonthAfterSecondDateMonth = secondDatumOnvolledig.Maand == null || secondDatumOnvolledig.Maand.Equals(0) ? false : peildatum.Value.Month > secondDatumOnvolledig.Maand;
		var peildatumMonthOnSecondDateMonth = secondDatumOnvolledig.Maand == null || secondDatumOnvolledig.Maand.Equals(0) ? false : peildatum.Value.Month == secondDatumOnvolledig.Maand;

		var peildatumDayAfterFirstDateDay = firstDatumOnvolledig.Dag == null || firstDatumOnvolledig.Dag.Equals(0) ? true : peildatum.Value.Day > firstDatumOnvolledig.Dag;
		var peildatumDayOnFirstDateDay = firstDatumOnvolledig.Dag == null || firstDatumOnvolledig.Dag.Equals(0) ? false : peildatum.Value.Day == firstDatumOnvolledig.Dag;
		var peildatumDayAfterSecondDateDay = secondDatumOnvolledig.Dag == null || secondDatumOnvolledig.Dag.Equals(0) ? false : peildatum.Value.Day > secondDatumOnvolledig.Dag;
		var peildatumDayOnSecondDateDay = secondDatumOnvolledig.Dag == null || secondDatumOnvolledig.Dag.Equals(0) ? false : peildatum.Value.Day == secondDatumOnvolledig.Dag;

		return (peildatumYearAfterFirstDateYear && !peildatumYearOnFirstDateYear && ((!peildatumYearAfterSecondDateYear && !peildatumYearOnSecondDateYear)
																																																|| (!peildatumYearAfterSecondDateYear && peildatumYearOnSecondDateYear && !peildatumMonthAfterSecondDateMonth && !peildatumMonthOnSecondDateMonth)
																																																|| (!peildatumYearAfterSecondDateYear && peildatumYearOnSecondDateYear && !peildatumMonthAfterSecondDateMonth && peildatumMonthOnSecondDateMonth && !peildatumDayAfterSecondDateDay && !peildatumDayOnSecondDateDay)
																																																)) // option 1: peildatum year is not on the same year as date1 but after and before and not on the year of date2 meaning peildatum is 100% guaranteed to be between.

			|| (!peildatumYearAfterFirstDateYear && peildatumYearOnFirstDateYear && peildatumMonthAfterFirstDateMonth && !peildatumMonthOnFirstDateMonth && ((!peildatumYearAfterSecondDateYear && !peildatumYearOnSecondDateYear)
																																																|| (!peildatumYearAfterSecondDateYear && peildatumYearOnSecondDateYear && !peildatumMonthAfterSecondDateMonth && !peildatumMonthOnSecondDateMonth)
																																																|| (!peildatumYearAfterSecondDateYear && peildatumYearOnSecondDateYear && !peildatumMonthAfterSecondDateMonth && peildatumMonthOnSecondDateMonth && !peildatumDayAfterSecondDateDay && !peildatumDayOnSecondDateDay)
																																																)) // option 2 peildatum year is not after date1 year but on the date1 year but the peildatum month is later than the date1 month but not on the date1 month.
																																																   // Additionally peildatum year is not after and not on the date2 year OR on the date2 year but peildatum month is before and not on the date2 month OR on the date2 year but peildatum month is before and on the date2 month but peildatum day is before and not on the date2 day.
			|| (!peildatumYearAfterFirstDateYear && peildatumYearOnFirstDateYear && !peildatumMonthAfterFirstDateMonth && peildatumMonthOnFirstDateMonth && (peildatumDayAfterFirstDateDay || peildatumDayOnFirstDateDay) && ((!peildatumYearAfterSecondDateYear && !peildatumYearOnSecondDateYear)
																																																|| (!peildatumYearAfterSecondDateYear && peildatumYearOnSecondDateYear && !peildatumMonthAfterSecondDateMonth && !peildatumMonthOnSecondDateMonth)
																																																|| (!peildatumYearAfterSecondDateYear && peildatumYearOnSecondDateYear && !peildatumMonthAfterSecondDateMonth && peildatumMonthOnSecondDateMonth && !peildatumDayAfterSecondDateDay && !peildatumDayOnSecondDateDay)
																																																)) // option 3 peildatum year is not after date1 year but on the date1 year and the peildatum month is not later than the date1 month but on the date1 month. Also peildatum day is either after or on the day of date1.
																																																   // Additionally peildatum year is not after and not on the date2 year OR on the date2 year but peildatum month is before and not on the date2 month OR on the date2 year but peildatum month is before and on the date2 month but peildatum day is before and not on the date2 day.
																																																;
	}

	public static bool IsMogelijkeBewonerByInOnderzoek(DateTime? van, DateTime? tot, int? inOnderzoekAand, string? inOnderzoekStartDate, string? inOnderzoekEndDate, string currentStartDate, string? previousStartDate = null, string? nextStartDate = null)
	{
		if (string.IsNullOrWhiteSpace(nextStartDate) || string.IsNullOrWhiteSpace(inOnderzoekStartDate)
			|| string.IsNullOrWhiteSpace(inOnderzoekEndDate) || (inOnderzoekAand != 89999 && inOnderzoekAand != 589999))
		{
			return false;
		}
		var inOnderzoekStartDateTime = DateTime.ParseExact(inOnderzoekStartDate, "yyyyMMdd", CultureInfo.InvariantCulture);
		var inOnderzoekEndDateTime = DateTime.ParseExact(inOnderzoekEndDate, "yyyyMMdd", CultureInfo.InvariantCulture);

		var nextDatumOnvolledig = new DatumOnvolledig(nextStartDate);
		var nextOnzekerheidsPeriode = CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig);

		if (!(inOnderzoekStartDateTime < tot && inOnderzoekEndDateTime > van)
			||
			(inOnderzoekEndDateTime.AddDays(-1) == nextOnzekerheidsPeriode.startOnzekerheidsPeriodeDateTime
			&& nextOnzekerheidsPeriode.startOnzekerheidsPeriodeDateTime == van
			&& inOnderzoekEndDateTime == tot)
			||
			(nextOnzekerheidsPeriode.startOnzekerheidsPeriodeDateTime > inOnderzoekEndDateTime)
			)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Peildatum may not be the same date as the second date.
	/// </summary>
	/// <param name="currentDatumOnvolledigString"></param>
	/// <param name="previousDatumOnvolledigString"></param>
	/// <param name="nextDatumOnvolledigString"></param>
	/// <param name="periodeVan"></param>
	/// <param name="periodeTot"></param>
	/// <returns></returns>
	public static bool IsPotentiallyBewoner(string? currentDatumOnvolledigString, string? previousDatumOnvolledigString, string? nextDatumOnvolledigString, DateTime? periodeVan, DateTime? periodeTot)
	{
		if (!periodeVan.HasValue || !periodeTot.HasValue || string.IsNullOrWhiteSpace(currentDatumOnvolledigString))
		{
			return false;
		}

		var currentDatumOnvolledig = new DatumOnvolledig(currentDatumOnvolledigString);
		var previousDatumOnvolledig = new DatumOnvolledig(previousDatumOnvolledigString);
		var nextDatumOnvolledig = new DatumOnvolledig(nextDatumOnvolledigString);

		// Rule 1: een persoon met onbekende aanvang adreshouding, geen vorige en volgende adreshouding, is in een periode een mogelijke bewoner voor dat deel van de periode dat in de onzekerheidsperiode van de gevraagde adreshouding ligt
		if (currentDatumOnvolledig.IsOnvolledig() && string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && string.IsNullOrWhiteSpace(nextDatumOnvolledigString)
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot))
		{
			return true;
		}

		// Rule 2: een persoon met onbekende aanvang adreshouding, en een bekende datum volgende adreshouding die na de onzekerheidsperiode van de gevraagde adreshouding ligt, is een mogelijke bewoner voor dat deel van de periode dat binnen de onzekerheidsperiode van de gevraagde adreshouding ligt
		if (currentDatumOnvolledig.IsOnvolledig() && nextDatumOnvolledig?.IsOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(previousDatumOnvolledigString)
					|| periodeVan >= CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
				)
			&& !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == false
			&& CurrentDateOnzekerheidsPeriodeBeforeVanTotPeriode(currentDatumOnvolledig, DateTime.Parse(nextDatumOnvolledig.Datum!), DateTime.Parse(nextDatumOnvolledig.Datum!).AddDays(1))
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot))
		{
			return true;
		}

		// Rule 3: een persoon met bekende aanvang volgende adreshouding die in de onzekerheidsperiode van de onbekende aanvang adreshouding ligt, is een mogelijke bewoner voor dat deel van de periode dat in de onzekerheidsperiode vóór de datum aanvang volgende adreshouding ligt
		if (currentDatumOnvolledig.IsOnvolledig() && nextDatumOnvolledig?.IsOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(previousDatumOnvolledigString)
					|| periodeVan >= CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
				)
			&& !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == false
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot, null, null, CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, DateTime.Parse(nextDatumOnvolledig.Datum!), DateTime.MaxValue), nextDatumOnvolledig)
			&& (string.IsNullOrWhiteSpace(previousDatumOnvolledigString) || (periodeTot > CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig).startOnzekerheidsPeriodeDateTime))
			)
		{
			return true;
		}

		// Rule 4: een persoon met bekende aanvang adreshouding die niet in de onzekerheidsperiode van de deels onbekende aanvang volgende adreshouding ligt, is mogelijke bewoner voor dat deel van de periode dat in de onzekerheidsperiode van het volgende adreshouding ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(previousDatumOnvolledigString)
					|| periodeVan >= CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
				)
			&& !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == true && nextDatumOnvolledig?.IsCompleteOnvolledig() == false
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(nextDatumOnvolledig, periodeVan, periodeTot, CurrentDateOnzekerheidsPeriodeAfterVanTotPeriode(nextDatumOnvolledig, DateTime.Parse(currentDatumOnvolledig.Datum!), DateTime.Parse(currentDatumOnvolledig.Datum!).AddDays(1)), currentDatumOnvolledig)
			&& !CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig).startOnzekerheidsPeriodeDateTime, CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig).endOnzekerheidsPeriodeDateTime))
		{
			return true;
		}

		// Rule 5: een persoon met deels onbekende aanvang adreshouding, deels onbekende aanvang volgende adreshouding en niet-overlappende onzekerheidsperiodes, is een mogelijke bewoner voor dat deel van de periode dat in één van de onzekerheidsperiodes ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == true && currentDatumOnvolledig?.IsCompleteOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(previousDatumOnvolledigString)
					|| periodeVan >= CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
				)
			&& !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == true && nextDatumOnvolledig?.IsCompleteOnvolledig() == false
			&&
			!TwoOnzekerheidsPeriodesOverlap(CreateOnzekerheidsPeriodeDateTimes(currentDatumOnvolledig), CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig))
			&&
			(
				CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot)
				|| CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(nextDatumOnvolledig, periodeVan, periodeTot)
			)
		)
		{
			return true;
		}

		// Rule 6: een persoon met bekende aanvang adreshouding die in de onzekerheidsperiode van het deels/geheel onbekende volgende aanvang adreshouding ligt, is een mogelijke bewoner voor het deel van de periode dat in de onzekerheidsperiode na datum aanvang adreshouding ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(previousDatumOnvolledigString)
					|| periodeVan >= CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
				)
			&& !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == true
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(nextDatumOnvolledig, DateTime.Parse(currentDatumOnvolledig.Datum!), DateTime.Parse(currentDatumOnvolledig.Datum!).AddDays(1))
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot, null, null, CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(nextDatumOnvolledig, DateTime.Parse(currentDatumOnvolledig.Datum!), DateTime.Parse(currentDatumOnvolledig.Datum!).AddDays(1)), nextDatumOnvolledig)
			&& periodeVan > DateTime.Parse(currentDatumOnvolledig!.Datum!)
		)
		{
			return true;
		}

		// Rule 7: een persoon met onbekende aanvang adreshouding, onbekende aanvang volgende adreshouding en deels/geheel overlappende onzekerheidsperiodes, is mogelijke bewoner voor de periode tussen de eerste dag van de onzekerheidsperiode van de gevraagde adreshouding en de laatste dag van de onzekerheidsperiode van de volgende adreshouding.
		if (currentDatumOnvolledig?.IsOnvolledig() == true
			&& (
					string.IsNullOrWhiteSpace(previousDatumOnvolledigString)
					|| periodeVan >= CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
				)
			&& !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == true
			&& IsOverlapBetweenTwoPeriods(CreateOnzekerheidsPeriodeDateTimes(currentDatumOnvolledig), CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig!))
			&& IsOverlapBetweenTwoPeriods(CreateOnOnzekerheidsPeriodeWithFirstDayOfCurrentAndLastDayOfNext(currentDatumOnvolledig, nextDatumOnvolledig!), (periodeVan, periodeTot))
		)
		{
			return true;
		}

		// Rule 8: een persoon met bekende aanvang vorige adreshouding die vóór de onzekerheidsperiode van deels onbekende aanvang adreshouding ligt, is een mogelijke bewoner voor het deel van de periode dat in de onzekerheidsperiode van de gevraagde adreshouding ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == true && currentDatumOnvolledig?.IsCompleteOnvolledig() == false && !string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && previousDatumOnvolledig?.IsOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(nextDatumOnvolledigString)
					|| periodeTot < CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig!).startOnzekerheidsPeriodeDateTime
				)
			&& CurrentDateOnzekerheidsPeriodeAfterVanTotPeriode(currentDatumOnvolledig, DateTime.Parse(previousDatumOnvolledig.Datum!), DateTime.Parse(previousDatumOnvolledig.Datum!).AddDays(1))
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot)
		)
		{
			return true;
		}

		// Rule 9: een persoon met bekende aanvang vorige adreshouding die in de onzekerheidsperiode van de onbekende aanvang adreshouding ligt, is een mogelijke bewoner voor het deel van de periode in de onzekerheidsperiode dat na de datum aanvang vorige adreshouding ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == true && !string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && previousDatumOnvolledig?.IsOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(nextDatumOnvolledigString)
					|| periodeTot < CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig!).startOnzekerheidsPeriodeDateTime
				)
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, DateTime.Parse(previousDatumOnvolledig.Datum!), DateTime.Parse(previousDatumOnvolledig.Datum!).AddDays(1))
			&& DateTime.Parse(previousDatumOnvolledig.Datum!) < periodeTot && periodeVan < CreateOnzekerheidsPeriodeDateTimes(currentDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
			&& !(DateTime.Parse(previousDatumOnvolledig.Datum!) == periodeVan && DateTime.Parse(previousDatumOnvolledig.Datum!).AddDays(1) == periodeTot)
		)
		{
			return true;
		}

		// Rule 10: een persoon met bekende aanvang vorige adreshouding en bekende aanvang volgende adreshouding die beide in de onzekerheidsperiode van de onbekende aanvang adreshouding ligt, is een mogelijke bewoner voor het deel van de periode dat in de onzekerheidsperiode na de datum aanvang vorige adreshouding en voor datum aanvang volgende adreshouding ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == true && !string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && previousDatumOnvolledig?.IsOnvolledig() == false && !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == false
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, DateTime.Parse(previousDatumOnvolledig.Datum!), DateTime.Parse(previousDatumOnvolledig.Datum!).AddDays(1))
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, DateTime.Parse(nextDatumOnvolledig.Datum!), DateTime.Parse(nextDatumOnvolledig.Datum!).AddDays(1))
			&& periodeTot <= DateTime.Parse(nextDatumOnvolledig!.Datum!)
			&& periodeVan > DateTime.Parse(previousDatumOnvolledig!.Datum!)
		)
		{
			return true;
		}

		// Rule 11: een persoon met deels onbekende aanvang adreshouding, deels onbekende aanvang vorige adreshouding en niet-overlappende onzekerheidsperiodes, is een mogelijke bewoner voor het deel van de periode dat in de onzekerheidsperiode van de gevraagde adreshouding ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == true && currentDatumOnvolledig?.IsCompleteOnvolledig() == false && !string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && previousDatumOnvolledig?.IsOnvolledig() == true && previousDatumOnvolledig?.IsCompleteOnvolledig() == false
			&& (
					string.IsNullOrWhiteSpace(nextDatumOnvolledigString)
					|| periodeTot < CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig!).startOnzekerheidsPeriodeDateTime
				)
			&& !TwoOnzekerheidsPeriodesOverlap(CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig), CreateOnzekerheidsPeriodeDateTimes(currentDatumOnvolledig))
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot)
			&& periodeVan > CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig).startOnzekerheidsPeriodeDateTime
		)
		{
			return true;
		}

		// Rule 12: een persoon met onbekende aanvang adreshouding, onbekende aanvang vorige adreshouding en gelijke onzekerheidsperiode, is een mogelijke bewoner voor het deel van de periode dat in de onzekerheidsperiode ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == true && !string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && previousDatumOnvolledig?.IsOnvolledig() == true
			&& (
					string.IsNullOrWhiteSpace(nextDatumOnvolledigString)
					|| periodeTot <= CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig!).startOnzekerheidsPeriodeDateTime
				)
			&& currentDatumOnvolledig.Jaar == previousDatumOnvolledig.Jaar && currentDatumOnvolledig.Maand == previousDatumOnvolledig.Maand && currentDatumOnvolledig.Dag == previousDatumOnvolledig.Dag
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, periodeVan, periodeTot)
		)
		{
			return true;
		}

		// Rule 13 old: een persoon met onbekende aanvang adreshouding, deels onbekende aanvang vorige adreshouding en deels/geheel overlappende onzekerheidsperiodes, is een mogelijke bewoner voor het deel van de periode dat in de onzekerheidsperiode van de adreshouding ligt én op of na de eerste dag van de onzekerheidsperiode van de vorige adreshouding
		// Rule 13: een persoon met onbekende aanvang adreshouding, onbekende aanvang vorige adreshouding en deels/geheel overlappende onzekerheidsperiodes, is een mogelijke bewoner voor het deel van de periode dat in de onzekerheidsperiode van de adreshouding ligt én op of na de eerste dag van de onzekerheidsperiode van de vorige adreshouding
		if (currentDatumOnvolledig?.IsOnvolledig() == true && !string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && previousDatumOnvolledig?.IsOnvolledig() == true
			&& (
					string.IsNullOrWhiteSpace(nextDatumOnvolledigString)
					|| periodeTot < CreateOnzekerheidsPeriodeDateTimes(nextDatumOnvolledig!).startOnzekerheidsPeriodeDateTime
				)
			&& IsOverlapBetweenTwoPeriods(CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig), CreateOnzekerheidsPeriodeDateTimes(currentDatumOnvolledig))
			&& IsOverlapBetweenTwoPeriods(CreateOnOnzekerheidsPeriodeWithFirstDayOfCurrentAndLastDayOfNext(previousDatumOnvolledig, currentDatumOnvolledig), (periodeVan, periodeTot))
			&& periodeVan >= CreateOnzekerheidsPeriodeDateTimes(currentDatumOnvolledig).startOnzekerheidsPeriodeDateTime
		)
		{
			return true;
		}

		// Rule Melvin (14): Een persoon met geheel/deels onbekend datum aanvang adreshouding, geheel/deels onbekend datum aanvang volgend adreshouding en een bekend datum aanvang vorig adreshouding die in de onzekerheidsperiode ligt, is mogelijke bewoner in (een deel van) de gevraagde periode als (dat deel van) de gevraagde periode in één van de onzekerheidsperiodes ná datum aanvang vorig adreshouding ligt
		if (currentDatumOnvolledig?.IsOnvolledig() == true && !string.IsNullOrWhiteSpace(nextDatumOnvolledigString) && nextDatumOnvolledig?.IsOnvolledig() == true && !string.IsNullOrWhiteSpace(previousDatumOnvolledigString) && previousDatumOnvolledig?.IsOnvolledig() == false
			&& CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(currentDatumOnvolledig, CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).startOnzekerheidsPeriodeDateTime, CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime)
			&& periodeVan >= CreateOnzekerheidsPeriodeDateTimes(previousDatumOnvolledig!).endOnzekerheidsPeriodeDateTime
			&& IsOverlapBetweenTwoPeriods(CreateOnOnzekerheidsPeriodeWithFirstDayOfCurrentAndLastDayOfNext(currentDatumOnvolledig, nextDatumOnvolledig), (periodeVan, periodeTot))
		)
		{
			return true;
		}

		return false;
	}

	private static bool CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(DatumOnvolledig current, DateTime? periodeVan, DateTime? periodeTot, bool? previousDateOverlapsWithCurrent = false, DatumOnvolledig? previous = null, bool? nextDateOverlapsWithCurrent = false, DatumOnvolledig? next = null)
	{
		if (!periodeVan.HasValue || !periodeTot.HasValue)
		{
			return true;
		}

		if (current.IsCompleteOnvolledig())
		{
			if ((next == null || !CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(next ?? new DatumOnvolledig(""), periodeVan, periodeTot, null, null, true, new DatumOnvolledig(DateTime.MaxValue.ToString("yyyyMMdd")))) && (previous == null || !CurrentDateOnzekerheidsPeriodeOverlapVanTotPeriode(previous ?? new DatumOnvolledig(""), periodeVan, periodeTot)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		(DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) = CreateOnzekerheidsPeriodeDateTimes(current);

		if (nextDateOverlapsWithCurrent.HasValue && nextDateOverlapsWithCurrent.Value && next?.IsOnvolledig() == false)
		{
			endOnzekerheidsPeriodeDateTime = DateTime.Parse(next.Datum!);
		}
		else if (nextDateOverlapsWithCurrent.HasValue && nextDateOverlapsWithCurrent.Value && next?.IsOnvolledig() == true)
		{
			endOnzekerheidsPeriodeDateTime = CreateOnOnzekerheidsPeriodeWithFirstDayOfCurrentAndLastDayOfNext(current, next).endOnzekerheidsPeriodeDateTime;
		}

		if (previousDateOverlapsWithCurrent.HasValue && !previousDateOverlapsWithCurrent.Value && previous?.IsOnvolledig() == false)
		{
			startOnzekerheidsPeriodeDateTime = DateTime.Parse(previous.Datum!);
		}
		else if (previousDateOverlapsWithCurrent.HasValue && !previousDateOverlapsWithCurrent.Value && previous?.IsOnvolledig() == true)
		{
			startOnzekerheidsPeriodeDateTime = CreateOnOnzekerheidsPeriodeWithFirstDayOfCurrentAndLastDayOfNext(previous, current).startOnzekerheidsPeriodeDateTime;
		}

		if (startOnzekerheidsPeriodeDateTime.HasValue && endOnzekerheidsPeriodeDateTime.HasValue)
		{
			return startOnzekerheidsPeriodeDateTime < periodeTot && periodeVan < endOnzekerheidsPeriodeDateTime;
		}

		return false;
	}

	private static bool IsOverlapBetweenTwoPeriods((DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) first, (DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) second)
	{
		return (first.startOnzekerheidsPeriodeDateTime < second.endOnzekerheidsPeriodeDateTime && first.endOnzekerheidsPeriodeDateTime > second.startOnzekerheidsPeriodeDateTime) ||
				(second.startOnzekerheidsPeriodeDateTime < first.endOnzekerheidsPeriodeDateTime && second.endOnzekerheidsPeriodeDateTime > first.startOnzekerheidsPeriodeDateTime);
	}

	private static (DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) CreateOnOnzekerheidsPeriodeWithFirstDayOfCurrentAndLastDayOfNext(DatumOnvolledig current, DatumOnvolledig next)
	{
		(DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) = (null, null);

		if (current.IsCompleteOnvolledig())
		{
			startOnzekerheidsPeriodeDateTime = DateTime.MinValue;
		}
		else
		{
			startOnzekerheidsPeriodeDateTime = CreateOnzekerheidsPeriodeDateTimes(current).startOnzekerheidsPeriodeDateTime;
		}

		if (next.IsCompleteOnvolledig())
		{
			endOnzekerheidsPeriodeDateTime = DateTime.MaxValue;
		}
		else
		{
			endOnzekerheidsPeriodeDateTime = CreateOnzekerheidsPeriodeDateTimes(next).endOnzekerheidsPeriodeDateTime;
		}

		return (startOnzekerheidsPeriodeDateTime, endOnzekerheidsPeriodeDateTime);
	}

	private static bool CurrentDateOnzekerheidsPeriodeBeforeVanTotPeriode(DatumOnvolledig current, DateTime? periodeVan, DateTime? periodeTot)
	{
		if (!periodeVan.HasValue || !periodeTot.HasValue)
		{
			return true;
		}
		if (current.IsCompleteOnvolledig() || !current.IsOnvolledig())
		{
			return false;
		}

		(DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) = CreateOnzekerheidsPeriodeDateTimes(current);

		if (startOnzekerheidsPeriodeDateTime.HasValue && endOnzekerheidsPeriodeDateTime.HasValue)
		{
			return periodeVan > endOnzekerheidsPeriodeDateTime;
		}

		return false;
	}

	private static bool CurrentDateOnzekerheidsPeriodeAfterVanTotPeriode(DatumOnvolledig current, DateTime? periodeVan, DateTime? periodeTot)
	{
		if (!periodeVan.HasValue || !periodeTot.HasValue)
		{
			return true;
		}
		if (current.IsCompleteOnvolledig() || !current.IsOnvolledig())
		{
			return false;
		}

		(DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) = CreateOnzekerheidsPeriodeDateTimes(current);

		if (startOnzekerheidsPeriodeDateTime.HasValue && endOnzekerheidsPeriodeDateTime.HasValue)
		{
			return periodeVan < endOnzekerheidsPeriodeDateTime && periodeTot < startOnzekerheidsPeriodeDateTime;
		}

		return false;
	}

	private static bool TwoOnzekerheidsPeriodesOverlap((DateTime? start, DateTime? end) firstOnzekerheidsPeriod, (DateTime? start, DateTime? end) secondOnzekerheidsPeriod)
	{
		if (!firstOnzekerheidsPeriod.end.HasValue || !secondOnzekerheidsPeriod.start.HasValue)
		{
			return true;
		}

		return (firstOnzekerheidsPeriod.start < secondOnzekerheidsPeriod.end && firstOnzekerheidsPeriod.end > secondOnzekerheidsPeriod.start) ||
			(secondOnzekerheidsPeriod.start < firstOnzekerheidsPeriod.end && secondOnzekerheidsPeriod.end > firstOnzekerheidsPeriod.start);
	}

	private static (DateTime? startOnzekerheidsPeriodeDateTime, DateTime? endOnzekerheidsPeriodeDateTime) CreateOnzekerheidsPeriodeDateTimes(DatumOnvolledig current)
	{
		DateTime? startOnzekerheidPeriodeDateTime = null;
		DateTime? endOnzekerheidPeriodeDateTime = null;

		if (!current.IsOnvolledig())
		{
			startOnzekerheidPeriodeDateTime = DateTime.Parse(current.Datum!);
			endOnzekerheidPeriodeDateTime = DateTime.Parse(current.Datum!).AddDays(1);
		}
		else if (current.IsCompleteOnvolledig())
		{
			startOnzekerheidPeriodeDateTime = DateTime.MinValue;
			endOnzekerheidPeriodeDateTime = DateTime.MaxValue;
		}
		else if (!current.Maand.HasValue)
		{
			var startOnzekerheidPeriode = $"{current.Jaar!.Value}-01-01";
			var endOnzekerheidPeriode = $"{current.Jaar!.Value + 1}-01-01";

			startOnzekerheidPeriodeDateTime = DateTime.Parse(startOnzekerheidPeriode);
			endOnzekerheidPeriodeDateTime = DateTime.Parse(endOnzekerheidPeriode);
		}
		else/* if (!current.Dag.HasValue)*/
		{
			var startOnzekerheidPeriode = $"{current.Jaar!.Value}-{current.Maand!.Value}-01";
			var endOnzekerheidPeriode = $"{(current.Maand!.Value == 12 ? current.Jaar!.Value + 1 : current.Jaar!.Value)}-{(current.Maand!.Value == 12 ? 1 : current.Maand!.Value + 1)}-01";

			startOnzekerheidPeriodeDateTime = DateTime.Parse(startOnzekerheidPeriode);
			endOnzekerheidPeriodeDateTime = DateTime.Parse(endOnzekerheidPeriode);
		}
		//else
		//{
		//	startOnzekerheidPeriodeDateTime = DateTime.Parse(current.Datum!);
		//	endOnzekerheidPeriodeDateTime = DateTime.Parse(current.Datum!).AddDays(1);
		//}

		return (startOnzekerheidPeriodeDateTime, endOnzekerheidPeriodeDateTime);
	}

    public static Waardetabel? MapBinnenOfBuitenlandsePlaats(int? landCode, string? plaats, string? plaatsOmschrijving)
    {
        if (string.IsNullOrEmpty(plaats))
        {
            return null;
        }

        if (!Regex.IsMatch(plaats, @"^\d{1,4}$"))
        {
            return new Waardetabel
            {
                Omschrijving = !string.IsNullOrEmpty(plaatsOmschrijving) ? plaatsOmschrijving : plaats
            };
        }

		return new Waardetabel
        {
            Code = plaats.PadLeft(4, '0'),
            Omschrijving = plaatsOmschrijving
		};
    }

    public static Waardetabel? ParseToSoortAdresEnum(string? code)
    {
        return (code?.ToUpper()) switch
        {
            "W" => new Waardetabel { Code = code, Omschrijving = "woonadres" },
            "B" => new Waardetabel { Code = code, Omschrijving = "briefadres" },
            "WOONADRES" => new Waardetabel { Code = "W", Omschrijving = "woonadres" },
            "BRIEFADRES" => new Waardetabel { Code = "B", Omschrijving = "briefadres" },
            _ => null
        };
    }

    public static Waardetabel? ParseToAanduidingBijHuisnummerEnum(string? code)
    {
        return (code?.ToLower()) switch
        {
            "by" => new Waardetabel{ Code = code, Omschrijving = "bij" },
            "to" => new Waardetabel { Code = code, Omschrijving = "tegenover" },
            _ => null,
        };
	}

    public static AdellijkeTitelPredicaatSoort? ParseToSoortAdellijkeTitelPredikaatEnum(string? code)
    {
        return (code?.ToUpper()) switch
        {
            "TITEL" => AdellijkeTitelPredicaatSoort.TitelEnum,
            "PREDICAAT" => AdellijkeTitelPredicaatSoort.PredicaatEnum,
            _ => null,
        };
    }

    public static Waardetabel? ParseToNaamgebruikEnum(string? code)
    {
        return (code?.ToUpper()) switch
        {
            "E" => new Waardetabel {  Code = code, Omschrijving = "eigen geslachtsnaam" },
            "N" => new Waardetabel {  Code = code, Omschrijving = "geslachtsnaam echtgenoot/geregistreerd partner na eigen geslachtsnaam" },
            "P" => new Waardetabel {  Code = code, Omschrijving = "geslachtsnaam echtgenoot/geregistreerd partner" },
            "V" => new Waardetabel { Code = code, Omschrijving = "geslachtsnaam echtgenoot/geregistreerd partner voor eigen geslachtsnaam" },
            _ => null
        };
    }

    public static Waardetabel? ParseToSoortVerbintenisEnum(string? code)
    {
        return (code?.ToUpper()) switch
        {
            "H" => new Waardetabel { Code = code, Omschrijving = "huwelijk" },
            "P" => new Waardetabel { Code = code, Omschrijving = "geregistreerd partnerschap" },
            "." => new Waardetabel { Code = code, Omschrijving = "onbekend" },
            _ => null,
		};
    }
}