namespace Rvig.HaalCentraalApi.Shared.Fields;
public class FieldsSettingsModel
{
	public FieldsSettingsModel(string parameterName)
	{
		ParameterName = parameterName;
		PropertyMapping = new Dictionary<string, string>();
		AllowedProperties = new List<string>();
		ForbiddenProperties = new List<string>();
		MandatoryProperties = new List<string>();
		SetChildPropertiesIfExistInScope = new Dictionary<string, string>();
		SetPropertiesIfContextPropertyNotNull = new Dictionary<string, string>();
		PropertiesToDiscard = new List<string>();
		ShortHandMappings = new Dictionary<string, string>();
	}

	/// <summary>
	/// Name of the input scope parameter (for example: fields)
	/// </summary>
	public string ParameterName { get; set; }

	/// <summary>
	/// Some properties need mapping when the input property name is different than the actual property name
	/// </summary>
	public Dictionary<string, string> PropertyMapping { get; set; }

	/// <summary>
	/// Only these properties are allowed to be added to the scope.
	/// </summary>
	public List<string> AllowedProperties { get; set; }

	/// <summary>
	/// Properties that are part of the object but are not allowed to be part of the properties
	/// Example: _embedded or _links
	/// </summary>
	public List<string> ForbiddenProperties { get; set; }

	/// <summary>
	/// Properties that are mandatory, for example signaling properties
	/// Example: indicatieGeheim
	/// </summary>
	public List<string> MandatoryProperties { get; set; }

	/// <summary>
	/// The child properties of the property in the key field are set when the property set in the value field exists with or without childproperties in the scope
	/// Example: keyvaluepair: key="naam.inOnderzoek", value="naam". in this case childproperties of "naam.inOnderzoek" will only be set when the scope contains the corresponding "naam" or "naam.{childpropertyname}"
	/// Example: childproperty "naam.inOnderzoek.voornamen" will only be set when the scope contains "naam" or "naam.voornamen".
	/// This however does not work for cases where the parent property must always be shown even without being set in the scope.
	/// Example: keyvaluepair: key="persoonInOnderzoek", value="". In this case the childproperties of "persoonInOnderzoek" will not be set when the scope does not contain persoonInOnderzoek.
	/// This is because persoonInOnderzoek is a mandatory property set in the root object.
	/// </summary>
	public Dictionary<string, string> SetChildPropertiesIfExistInScope { get; set; }

	/// <summary>
	/// Properties which should be set when the given context property not is null
	/// Key = property to set, value = context property to check
	/// Example: inOnderzoek.datumIngangOnderzoek will  be set when inOnderzoek not is null
	/// </summary>
	public Dictionary<string, string> SetPropertiesIfContextPropertyNotNull { get; set; }

	/// <summary>
	/// Properties that need to be set to its default value
	/// Example: _embedded
	/// </summary>
	public List<string> PropertiesToDiscard { get; set; }

	/// <summary>
	/// Fields implementation must now support shorter field options. Dictionary is filled with option users can enter and a value corresponding with the actual field option.
	/// Example: Users enter voornamen while actually meaning naam.voornamen or users enter aandeel while actually meaning zakelijkGerechtigden.tenaamstelling.aandeel.
	/// </summary>
	public IDictionary<string, string> ShortHandMappings { get; set; }
}