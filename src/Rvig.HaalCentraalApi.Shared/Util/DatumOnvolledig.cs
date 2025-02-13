namespace Rvig.HaalCentraalApi.Shared.Util;

public class DatumOnvolledig
{
	private string? _datum = null;

	public DatumOnvolledig(string? date)
	{
		ParseToDatumOnvolledig(date);
	}

	/// <summary>
	/// Als de volledige datum bekend is wordt de datum gevuld die in de date definitie past.
	/// </summary>
	/// <value>Als de volledige datum bekend is wordt de datum gevuld die in de date definitie past.</value>
	public string? Datum
	{
		get
		{
			if (string.IsNullOrEmpty(_datum) && Jaar.HasValue && Maand.HasValue && Dag.HasValue)
			{
				_datum = $"{Jaar}-{Maand.Value.ToString().PadLeft(2, '0')}-{Dag.Value.ToString().PadLeft(2, '0')}";
			}
			return _datum;
		}
		set
		{
			//set is needed for settings this property when applying fields (scope)
			_datum = value;
		}
	}

	/// <summary>
	/// Als het jaar van de datum bekend is wordt dit element gevuld, ook als de volledige datum bekend is.
	/// </summary>
	/// <value>Als het jaar van de datum bekend is wordt dit element gevuld, ook als de volledige datum bekend is.</value>
	public int? Jaar { get; set; }

	/// <summary>
	/// Als de maand van een datum bekend is wordt dit element gevuld, ook als de volledige datum bekend is.
	/// </summary>
	/// <value>Als de maand van een datum bekend is wordt dit element gevuld, ook als de volledige datum bekend is.</value>
	public int? Maand { get; set; }

	/// <summary>
	/// Als de dag van de datum bekend is wordt dit element gevuld, ook als de volledige datum bekend is.
	/// </summary>
	/// <value>Als de dag van de datum bekend is wordt dit element gevuld, ook als de volledige datum bekend is.</value>
	public int? Dag { get; set; }

	public bool OnlyYearHasValue()
	{
		return Jaar != null && (Maand == null || Dag == null);
	}

	public bool IsOnvolledig()
	{
		return Jaar == null || (Jaar != null && Maand == null) || (Jaar != null && Maand != null && Dag == null);
	}

	public bool IsCompleteOnvolledig()
	{
		return Jaar == null && Maand == null && Dag == null;
	}

	public bool IsBefore(DateTime? date)
	{
		if (date == null || Jaar == null)
		{
			return false;
		}

		return Jaar < date.Value.Year ||
			Jaar == date.Value.Year && Maand != null && Maand < date.Value.Month ||
			Jaar == date.Value.Year && Maand == date.Value.Month && Dag != null && Dag < date.Value.Day;
	}

	public bool IsOn(DateTime? date)
	{
		if (date == null || Jaar == null)
		{
			return false;
		}

		return Jaar == date.Value.Year && (Maand == null || Maand == date.Value.Month && (Dag == null || Dag == date.Value.Day));
	}

	public bool IsAfter(DateTime? date)
	{
		if (date == null || Jaar == null)
		{
			return false;
		}

		return Jaar > date.Value.Year ||
			Jaar == date.Value.Year && Maand != null && Maand > date.Value.Month ||
			Jaar == date.Value.Year && Maand == date.Value.Month && Dag != null && Dag > date.Value.Day;
	}

	private void ParseToDatumOnvolledig(string? incompleteDateString)
	{
		if (!string.IsNullOrWhiteSpace(incompleteDateString) && incompleteDateString != "00000000" && incompleteDateString != "0" && incompleteDateString != "0000-00-00")
		{
			int? jaar;
			int? maand;
			int? dag;
			if (incompleteDateString.Contains("-"))
			{
				jaar = incompleteDateString.Replace("-", "").Length >= 4 ? int.Parse(incompleteDateString.Substring(0, 4)) : (int?)null;
				maand = incompleteDateString.Replace("-", "").Length >= 6 ? int.Parse(incompleteDateString.Substring(5, 2)) : (int?)null;
				dag = incompleteDateString.Replace("-", "").Length == 8 ? int.Parse(incompleteDateString.Substring(8, 2)) : (int?)null;
			}
			else
			{
				jaar = int.Parse(incompleteDateString.Substring(0, 4));
				maand = int.Parse(incompleteDateString.Substring(4, 2));
				dag = int.Parse(incompleteDateString.Substring(6, 2));
			}

			Jaar = jaar != 0 ? jaar : null;
			Maand = maand != 0 ? maand : null;
			Dag = dag != 0 ? dag : null;
		}
	}
}