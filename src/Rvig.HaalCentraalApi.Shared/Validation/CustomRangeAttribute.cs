using System.ComponentModel.DataAnnotations;

namespace Rvig.HaalCentraalApi.Shared.Validation;
/// <summary>
/// Range attribute doesnt have an option to have specific error message in case the value is above the maximum or below the minimum.
/// This custom attribute formats the error message based on one of the two cases.
/// This attribute is not parameterized because ValidationErrorMessages currently only deals with static message with a mapping to the corresponding type of the errormessage.
/// Currently not a problem since this type of attribute is currently only used once.
/// </summary>
public class CustomRange1To99999Attribute : RangeAttribute
{
    private int _value = 0;

    public CustomRange1To99999Attribute() : base(1, 99999) { }

    public override string FormatErrorMessage(string name)
    {
        if (_value < (int)Minimum)
        {
            return ValidationErrorMessages.RangeMinimum.Replace(@"\d*", Minimum.ToString());
        }

        if (_value > (int)Maximum)
        {
            return ValidationErrorMessages.RangeMaximum.Replace(@"\d*", Maximum.ToString());
		}

        return base.FormatErrorMessage(name);
    }

    public override bool IsValid(object? value)
    {
        if (value is int intValue)
        {
            _value = intValue;
        }

        return base.IsValid(value);
    }
}