using Newtonsoft.Json.Linq;
using NJsonSchema.Converters;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Validation;

namespace Rvig.HaalCentraalApi.Shared.Util
{
	public abstract class QueryBaseJsonInheritanceConverter : JsonInheritanceConverter
	{
		// Types to which query object can mutate to.
		protected abstract List<string> _subTypes { get; }
		// Mutation is based on discriminator. Discriminator is a property. Value of this param is the name of the property on the query object.
		protected abstract string _discriminator { get; }
		protected QueryBaseJsonInheritanceConverter()
		{
		}

		protected QueryBaseJsonInheritanceConverter(string discriminatorName) : base(discriminatorName)
		{
		}

		protected QueryBaseJsonInheritanceConverter(Type baseType) : base(baseType)
		{
		}

		protected QueryBaseJsonInheritanceConverter(string discriminatorName, bool readTypeProperty) : base(discriminatorName, readTypeProperty)
		{
		}

		protected QueryBaseJsonInheritanceConverter(Type baseType, string discriminatorName) : base(baseType, discriminatorName)
		{
		}

		protected QueryBaseJsonInheritanceConverter(Type baseType, string discriminatorName, bool readTypeProperty) : base(baseType, discriminatorName, readTypeProperty)
		{
		}

		protected override Type GetDiscriminatorType(JObject jObject, Type objectType, string discriminatorValue)
        {
            if (discriminatorValue == null)
            {
                throw GetValidationException(InvalidParamCode.required);
            }
            else if (string.IsNullOrWhiteSpace(discriminatorValue) || !_subTypes.Contains(discriminatorValue))
            {
                throw GetValidationException(InvalidParamCode.value);
            }

            return base.GetDiscriminatorType(jObject, objectType, discriminatorValue);
		}

		private InvalidParamsException GetValidationException(InvalidParamCode invalidParamCode)
		{
			string errorMessage = $"De foutieve parameter(s) zijn: {_discriminator}.";
			var invalidParams = new List<InvalidParams>
			{
				new InvalidParams
				{
					Code = invalidParamCode.ToString(),
					Name = _discriminator,
					Reason = invalidParamCode == InvalidParamCode.required
					? ValidationErrorMessages.Required
					: ValidationErrorMessages.Value
				}
			};
			return new InvalidParamsException(errorMessage, invalidParams);
		}
	}
}