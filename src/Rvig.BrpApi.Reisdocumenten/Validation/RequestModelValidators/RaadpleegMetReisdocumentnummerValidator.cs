using FluentValidation;
using Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten;

namespace Rvig.BrpApi.Reisdocumenten.Validation.RequestModelValidators;

public class RaadpleegMetReisdocumentnummerValidator : HaalCentraalReisdocumentenBaseValidator<RaadpleegMetReisdocumentnummer>
{
    const string _reisdocumentnummerPattern = "^[A-Z0-9]{9}$";

    public RaadpleegMetReisdocumentnummerValidator()
    {
        RuleFor(x => x.reisdocumentnummer)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(_requiredErrorMessage)
            .Must(x => x?.Count > 0).WithMessage(string.Format(_minItemsErrorMessage, 1))
            .Must(x => x?.Count <= 1).WithMessage(string.Format(_maxItemsErrorMessage, 1));

        RuleForEach(x => x.reisdocumentnummer)
            .Matches(_reisdocumentnummerPattern).WithMessage(GetPatternErrorMessage(_reisdocumentnummerPattern));
    }
}
