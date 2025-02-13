using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Rvig.BrpApi.Shared.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class ValidateDateAttribute : ValidationAttribute
{
    private readonly string _format;

    public ValidateDateAttribute(string format)
    {
        _format = format;
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
            return true;

        if (value is not string)
            return false;

        try
        {
            var dateString = (string)value;
            DateOnly.ParseExact(dateString, _format, CultureInfo.InvariantCulture);
            return true;
        }
        catch
        {
            return false;
        }
    }
}