using Rvig.HaalCentraalApi.Shared.Exceptions;

namespace Rvig.HaalCentraalApi.Shared.Helpers;

/// <summary>
/// Base class for rubriek translation for protocollering and autorisations.
/// </summary>
/// <typeparam name="TBaseQueryObject">Base query object. For Personen it should be PersonenQuery, for reisdocumenten it would be ReisdocumentenQuery.</typeparam>
public abstract class HaalCentraalToRubriekCategoryHelperBase<TBaseQueryObject> where TBaseQueryObject : class
{
	protected abstract IDictionary<string, string> _fieldRubriekCategoryDictionary { get; }

	public abstract List<string> ConvertModelParamsToRubrieken(TBaseQueryObject model);

	public List<(string field, string rubriek)> ConvertFieldsToRubriekCategory(List<string> fields)
	{
		// String join and Split needed as some fields may result in multiple fields because of Haal Centraal logic.
		// These are already & separated so this is just a step to make every field a single item in the list.
		var correctedFields = new List<string>(fields);
		fields.ForEach(field =>
		{
			if (field.Contains("&"))
			{
				var splitFields = field.Split("&").ToList();
				splitFields.ForEach(splitField => correctedFields.Add(splitField));
				correctedFields.RemoveAt(correctedFields.IndexOf(field));
			}
		});
		var unknownFields = correctedFields.Where(field => !_fieldRubriekCategoryDictionary.ContainsKey(field));
		if (unknownFields?.Any() == true)
		{
			throw new AuthorizationException($"No translation available for field: {string.Join(", ", unknownFields)}.");
		}

		// String join and Split needed as some fields may result in multiple rubrieken of a category.
		// These are already comma separated so this is just a step to make every rubriek a single item in the list.
		var fieldRubrieken = correctedFields.ConvertAll(field => (field, rubriek: _fieldRubriekCategoryDictionary[field]));
		var correctedRubrieken = new List<(string field, string rubriek)>(fieldRubrieken);
		fieldRubrieken.ForEach(fieldRubriek =>
		{
			if (fieldRubriek.rubriek.Contains(", "))
			{
				var rubrieken = fieldRubriek.rubriek.Split(", ").ToList();
				rubrieken.ForEach(rubriek => correctedRubrieken.Add((fieldRubriek.field, rubriek)));
				correctedRubrieken.RemoveAt(correctedRubrieken.IndexOf(fieldRubriek));
			}
		});

		return correctedRubrieken;
	}
}
