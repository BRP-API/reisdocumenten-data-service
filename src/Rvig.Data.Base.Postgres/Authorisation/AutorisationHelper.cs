using Rvig.Data.Base.Authorisation;
using Rvig.Data.Base.Postgres.DatabaseModels;
using System.Reflection;

namespace Rvig.Data.Base.Postgres.Authorisation;

public record AutorisationMappings(
    Dictionary<int, (PropertyInfo, List<PropertyInfo>)> MappingActueel,
    Dictionary<int, (PropertyInfo, List<PropertyInfo>)> MappingHistorisch,
    List<(PropertyInfo, PropertyInfo)> AlwaysAuthorized);

public static class AutorisationHelper
{
	public static AutorisationMappings CreateRubriekPropertyMapping<T>() where T : DbPersoonBaseWrapper
		=> CreateRubriekPropertyMapping(typeof(T));

	public static AutorisationMappings CreateRubriekPropertyMapping(Type objectType)
    {
        var mappingActueel = new Dictionary<int, (PropertyInfo, List<PropertyInfo>)>();
        var mappingHistorisch = new Dictionary<int, (PropertyInfo, List<PropertyInfo>)>();
        var alwaysAuthorized = new List<(PropertyInfo, PropertyInfo)>();

        foreach (var categoryProp in objectType.GetProperties())
        {
            foreach (var rubriekCategory in categoryProp.GetCustomAttributes<RubriekCategoryAttribute>())
            {
                var categoryType = categoryProp.PropertyType;
                foreach (var elementProp in (categoryType.IsGenericType ? categoryType.GenericTypeArguments[0] : categoryType).GetProperties())
                {
                    var rubriekCategoryOverride = elementProp.GetCustomAttribute<RubriekCategoryAttribute>();
                    var rubriekElement = elementProp.GetCustomAttribute<RubriekElementAttribute>();
                    var alwaysAuthorizedAttr = elementProp.GetCustomAttribute<AlwaysAuthorizedAttribute>();
                    var typeTree = (categoryProp, elementProp);

                    if (alwaysAuthorizedAttr != null)
                        alwaysAuthorized.Add(typeTree);

                    if ((rubriekCategoryOverride != null && rubriekCategoryOverride.Actueel != rubriekCategory.Actueel) || rubriekElement == null)
                        continue;

                    var elementNummerWithoutPoint = rubriekElement.ElementNummers.Remove(2, 1);
                    var actueleRubriek = int.Parse($"{rubriekCategory.Actueel}{elementNummerWithoutPoint}");
                    AddMapping(mappingActueel, actueleRubriek, typeTree);

                    if (rubriekCategory.Historisch != null)
                    {
                        var historischeRubriek = int.Parse($"{rubriekCategory.Historisch}{elementNummerWithoutPoint}");
                        AddMapping(mappingHistorisch, historischeRubriek, typeTree);
                    }
                }
            }
        }

        return new(mappingActueel, mappingHistorisch, alwaysAuthorized);
    }

    private static void AddMapping(Dictionary<int, (PropertyInfo, List<PropertyInfo>)> mapping, int rubriek, (PropertyInfo, PropertyInfo) propTree)
    {
        if (mapping.ContainsKey(rubriek))
        {
            var currentTree = mapping[rubriek];
            currentTree.Item2.Add(propTree.Item2);
            return;
        }

        var newPropList = new List<PropertyInfo>() { propTree.Item2 };
        mapping.Add(rubriek, (propTree.Item1, newPropList));
    }

    public static void CopyProperty<T>(T source, T target, (PropertyInfo, List<PropertyInfo>) propTree, Dictionary<object, object> listInstances, bool? isHistorisch = null) where T : DbPersoonBaseWrapper
    {
        var categorySource = propTree.Item1.GetValue(source)!;
        var categoryTarget = propTree.Item1.GetValue(target)!;

        if (categorySource is IList sourceList)
        {
            var targetList = (IList)categoryTarget;
            foreach (var sourceListItem in sourceList)
            {
                if (!VolgnummerMatch(isHistorisch, sourceListItem))
                    continue;

                if (!listInstances.ContainsKey(sourceListItem))
                {
                    var newTargetInstance = Activator.CreateInstance(sourceListItem.GetType())!;
                    listInstances.Add(sourceListItem, newTargetInstance);
                    targetList.Add(newTargetInstance);
                }

                var targetListItem = listInstances[sourceListItem];

                foreach (var prop in propTree.Item2)
                {
                    var propValue = prop.GetValue(sourceListItem);
                    prop.SetValue(targetListItem, propValue);
                }
            }
            return;
        }

        // use volg_nr of lo3_pl_verblijfplaats for joined lo3_adres to check if adres is historic or not.
        if (source is DbPersoonActueelWrapper dbPersoonActueelWrapper && !VolgnummerMatch(isHistorisch, categorySource is lo3_adres ? dbPersoonActueelWrapper.Verblijfplaats : categorySource))
            return;

        foreach (var prop in propTree.Item2)
        {
            var propValue = prop.GetValue(categorySource);
            prop.SetValue(categoryTarget, propValue);
        }
    }

    public static void CopyProperty(object source, object? target, (PropertyInfo, List<PropertyInfo>) propTree, Dictionary<object, object> listInstances, bool? isHistorisch = null)
    {
        var categorySource = propTree.Item1.GetValue(source)!;
        var categoryTarget = propTree.Item1.GetValue(target)!;

        if (categorySource is IList sourceList)
        {
            var targetList = (IList)categoryTarget;
            foreach (var sourceListItem in sourceList)
            {
                if (!VolgnummerMatch(isHistorisch, sourceListItem))
                    continue;

                if (!listInstances.ContainsKey(sourceListItem))
                {
                    var newTargetInstance = Activator.CreateInstance(sourceListItem.GetType())!;
                    listInstances.Add(sourceListItem, newTargetInstance);
                    targetList.Add(newTargetInstance);
                }

                var targetListItem = listInstances[sourceListItem];

                foreach (var prop in propTree.Item2)
                {
                    var propValue = prop.GetValue(sourceListItem);
                    prop.SetValue(targetListItem, propValue);
                }
            }
            return;
        }

        // use volg_nr of lo3_pl_verblijfplaats for joined lo3_adres to check if adres is historic or not.
        if (source is DbPersoonActueelWrapper dbPersoonActueelWrapper && !VolgnummerMatch(isHistorisch, categorySource is lo3_adres ? dbPersoonActueelWrapper.Verblijfplaats : categorySource))
            return;

        foreach (var prop in propTree.Item2)
        {
            var propValue = prop.GetValue(categorySource);
            prop.SetValue(categoryTarget, propValue);
        }
    }

    /// <summary>
    /// For copying properties, historiche rubrieken should have volg_nr >0, actuele volg_nr = 0
    /// </summary>
    /// <param name="isHistorisch"></param>
    /// <param name="categoryGroupValue"></param>
    /// <returns></returns>
    private static bool VolgnummerMatch(bool? isHistorisch, object categoryGroupValue)
    {
        return isHistorisch == null
            || (isHistorisch == false && categoryGroupValue is not IVolgnummer)
            || (isHistorisch == false && categoryGroupValue is IVolgnummer { volg_nr: 0 })
            || (isHistorisch == true && categoryGroupValue is IVolgnummer { volg_nr: > 0 });
    }
}