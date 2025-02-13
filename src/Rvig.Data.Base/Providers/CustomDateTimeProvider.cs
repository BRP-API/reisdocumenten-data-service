using System.Globalization;

namespace Rvig.Data.Base.Providers;
/// <summary>
/// Can be used to test the API at a date other than today.
/// </summary>
public class CustomDateTimeProvider : ICurrentDateTimeProvider
{
    private DateTime _customDate;

    public CustomDateTimeProvider(DateTime date)
    {
        _customDate = date;
    }

    public CustomDateTimeProvider(string date)
    {
        _customDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public DateTime Today()
    {
        return _customDate;
    }

    public void ChangeDate(string date)
    {
        _customDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
    }
}