using FluentValidation;

namespace Rvig.HaalCentraalApi.Shared.Validation.RequestModelValidators;

public abstract class HaalCentraalBaseValidator<T> : AbstractValidator<T> where T : class
{
	protected const string _requiredErrorMessage = "Parameter is verplicht.";
	protected const string _datePattern = @"\d{4}-\d{2}-\d{2}";
	protected const string _dateErrorMessage = "Waarde is geen geldige datum.";
	protected const string _booleanErrorMessage = "Waarde is geen geldige boolean.";
	protected const string _integerErrorMessage = "Waarde is geen geldige integer.";
	protected const string _minItemsErrorMessage = "Array bevat minder dan {0} items.";
	protected const string _maxItemsErrorMessage = "Array bevat meer dan {0} items.";
	protected const string _fieldsPattern = "^[a-zA-Z0-9\\._]{1,200}$";
	protected const string _gemeenteVanInschrijvingPattern = "^[0-9]{4}$";
	protected string GetPatternErrorMessage(string pattern) => $"Waarde voldoet niet aan patroon {pattern}.";
}
