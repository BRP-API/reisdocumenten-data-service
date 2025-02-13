Made the following edits to generated GBA classes:
- Added all required and validation attributes that were missing in both request as response classes
- Replaced JsonSubTypes with JsonInheritance on the PersonenQuery and PersonenQueryResponse classes and replaced initial JsonConverter with JsonInheritanceConverter.
- Removed CustomEnumConverter on all enums (for example [TypeConverter(typeof(CustomEnumConverter<AdellijkeTitelPredicaatSoort>))])
- Fixed all unnecessarily escaped regex validation attributes on request and response classes. For example Geslachtsnaam from [RegularExpression("^[a-zA-Z0-9Ã€-Å¾ \\.\\-\\']{1,200}$|^[a-zA-Z0-9Ã€-Å¾ \\.\\-\\']{3,199}\\*{1}$")] to [RegularExpression(@"^[a-zA-Z0-9À-ž \.\-\']{1,200}$|^[a-zA-Z0-9À-ž \.\-\']{3,199}\*{1}$")]
	- Not only was it escaped wrongly, the À-ž was also wrongly generated as Ã€-Å¾
- Changed all model properties to camel case.
- Lowercased all model properties because request param related errors use nameof and therefore use the C# naming convention and not the YAML specification naming convention.
- Override gemeenteVanInschrijving in ZoekMetNaamEnGemeenteVanInschrijving and ZoekMetStraatHuisnummerEnGemeenteVanInschrijving because gemeenteVanInschrijving is required in these classes while not in others.
- Changed EmitDefaultValue=false to EmitDefaultValue=false in all random places. Only acceptable ones were inclusiefOverledenPersonen in ZoekMetNaamEnGemeenteVanInschrijving, ZoekMetNummeraanduidingIdentificatie, ZoekMetPostcodeEnHuisnummer and ZoekMetStraatHuisnummerEnGemeenteVanInschrijving.
	- Also accepted huisnummer with true value in ZoekMetPostcodeEnHuisnummer and ZoekMetStraatHuisnummerEnGemeenteVanInschrijving because these are required and not optional. Values must always be emited even when default.
- All DateTime properties set as string. Also added the following attributes for validation
		[Required(ErrorMessageResourceType = typeof(ValidationErrorMessages), ErrorMessageResourceName = nameof(ValidationErrorMessages.Required))]
		[ValidateDate("yyyy-MM-dd", ErrorMessageResourceType = typeof(ValidationErrorMessages), ErrorMessageResourceName = nameof(ValidationErrorMessages.DateParse))]
- All list/array request model properties gain the following converter so we can throw an invalid array exception when one uses an invalid value as an array: 
		[JsonConverter(typeof(JsonArrayConverter))]
- Added to all request models fluent validation validators
	- Removed all validation data annotation attributes from models because fluent validation validators take care of this.
- Watch out. Because of NJsonSchema and inheritance, children types will not be able to use System.Text.Json because NJsonSchema only supports Newtonsoft.Json (Json.Net) when using JsonInheritanceConverter.
	- For this reason, RaadpleegMetBurgerservicenummer.burgerservicenummer must use Newtonsoft.Json.JsonConverter and not System.Text.Json.Serialization.JsonConverter.
- Added custom JsonConverter 'BooleanConverter' to all bool properties in request models because Newtonsoft.Json likes to convert integers to booleans (value == 0 -> false value > 0 -> true)........................
- Added DatumJaar, DatumMaand, DatumDag to AangaanHuwelijkPartnerschap & OntbindingHuwelijkPartnerschap because of sorting.