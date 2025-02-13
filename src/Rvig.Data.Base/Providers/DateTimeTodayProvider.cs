namespace Rvig.Data.Base.Providers;
public class DateTimeTodayProvider : ICurrentDateTimeProvider
{
    public DateTime Today()
    {
        return DateTime.Today;
    }
}