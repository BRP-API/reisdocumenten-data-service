using FluentValidation;
using Rvig.HaalCentraalApi.Reisdocumenten.RequestModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Shared.Validation.RequestModelValidators;

namespace Rvig.HaalCentraalApi.Reisdocumenten.Validation.RequestModelValidators;

public class HaalCentraalReisdocumentenBaseValidator<T> : HaalCentraalBaseValidator<T> where T : ReisdocumentenQuery
{
	public HaalCentraalReisdocumentenBaseValidator()
	{
		RuleFor(x => x.type)
			.Cascade(CascadeMode.Stop)
			.NotEmpty().WithMessage(_requiredErrorMessage);

		RuleFor(x => x.fields)
			.Cascade(CascadeMode.Stop)
			.NotNull().WithMessage(_requiredErrorMessage)
			.Must(x => x?.Count > 0).WithMessage(string.Format(_minItemsErrorMessage, 1))
			.Must(x => x?.Count <= 25).WithMessage(string.Format(_maxItemsErrorMessage, 25));

		RuleForEach(x => x.fields)
			.Matches(_fieldsPattern).WithMessage(GetPatternErrorMessage(_fieldsPattern));
	}
}
