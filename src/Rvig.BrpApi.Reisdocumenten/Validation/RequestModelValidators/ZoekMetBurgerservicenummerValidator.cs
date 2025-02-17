﻿using FluentValidation;
using Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten;

namespace Rvig.BrpApi.Reisdocumenten.Validation.RequestModelValidators;

public class ZoekMetBurgerservicenummerValidator : HaalCentraalReisdocumentenBaseValidator<ZoekMetBurgerservicenummer>
{
    const string _bsnPattern = "^[0-9]{9}$";

    public ZoekMetBurgerservicenummerValidator()
    {
        RuleFor(x => x.burgerservicenummer)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage(_requiredErrorMessage)
            .Matches(_bsnPattern).WithMessage(GetPatternErrorMessage(_bsnPattern));
    }
}
