using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Fields;
using Rvig.HaalCentraalApi.Shared.Helpers;
using System.Reflection;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.Fields;
public class FieldsFilterService
{
	private string? _scope;
	/// <summary>
	/// Validates if the properties in the scope are valid if fields list has values.
	/// </summary>
	/// <param name="objectType"></param>
	/// <param name="fieldsettings"></param>
	/// <param name="fields"></param>
	public void ValidateScope(Type objectType, FieldsSettingsModel fieldsettings, List<string>? fields)
	{
		if (fields?.Any() == true)
		{
			ValidateScope(objectType, string.Join(",", fields), fieldsettings);
		}
	}

	/// <summary>
	/// Validates if the properties in the scope are valid
	/// </summary>
	/// <param name="scope"></param>
	/// <param name="filterSettings"></param>
	/// <returns></returns>
	public void ValidateScope<T>(string? scope, FieldsSettingsModel filterSettings) => ValidateScope(typeof(T), scope, filterSettings, true);

	/// <summary>
	/// Validates if the properties in the scope are valid
	/// </summary>
	/// <param name="objectType"></param>
	/// <param name="scope"></param>
	/// <param name="filterSettings"></param>
	/// <param name="ignoreDefaultProperties">Ignore properties that are set in MandatoryProperties, SetChildPropertiesIfExistInScope and SetPropertiesIfContextPropertyNotNull.</param>
	/// <returns></returns>
	public void ValidateScope(Type objectType, string? scope, FieldsSettingsModel filterSettings, bool? ignoreDefaultProperties = false)
	{
		if (string.IsNullOrEmpty(scope))
		{
			return;
		}
		_scope = scope;

		var scopeProperties = scope.Split(",");
		if (filterSettings.AllowedProperties != null && filterSettings.AllowedProperties.Any())
		{
			var unallowedProperties = scopeProperties.Where(scopeItem => !filterSettings.AllowedProperties.Any(allowed => scopeItem.StartsWith(allowed))).ToList();
			if (unallowedProperties?.Any() == true)
			{
				var invalidParams = new List<InvalidParams>();
				unallowedProperties.ForEach(unallowedProperty => invalidParams.Add(CreateThrowParameterValidationInvalidParam(filterSettings.ParameterName, unallowedProperty)));

				ThrowParameterValidationException(invalidParams);
			}
		}
		else
		{
			if (filterSettings.ForbiddenProperties != null)
			{
				var unallowedProperties = scopeProperties.Where(scopeItem => filterSettings.ForbiddenProperties.Any(forbidden => scopeItem.StartsWith(forbidden))).ToList();
				if (ignoreDefaultProperties == true && unallowedProperties?.Any() == true)
				{
					unallowedProperties = unallowedProperties.Where(scopeItem => !filterSettings.MandatoryProperties.Contains(scopeItem) && !filterSettings.SetChildPropertiesIfExistInScope.ContainsKey(scopeItem) && !filterSettings.SetPropertiesIfContextPropertyNotNull.ContainsKey(scopeItem)).ToList();
				}

				if (unallowedProperties?.Any() == true)
				{
					var invalidParams = new List<InvalidParams>();
					unallowedProperties.ForEach(unallowedProperty => invalidParams.Add(CreateThrowParameterValidationInvalidParam(filterSettings.ParameterName, unallowedProperty, true)));

					ThrowParameterValidationException(invalidParams);
				}
			}
		}

		foreach (var propertyName in scopeProperties)
		{
			var fieldToExamine = propertyName;
			if (filterSettings.ShortHandMappings != null && filterSettings.ShortHandMappings.Any() && filterSettings.ShortHandMappings.ContainsKey(fieldToExamine))
			{
				fieldToExamine = filterSettings.ShortHandMappings[fieldToExamine];
			}

			foreach (var field in fieldToExamine.Split("&"))
			{
				var propertyOrMappedProperty = ReplacePropertyNamePartIfMapped(field, filterSettings.PropertyMapping);

				//get propertytree throws exception if it cant find a part of the property var propertyTree =
				GetPropertyTree(propertyOrMappedProperty, objectType, filterSettings.ParameterName, field);
			}
		}
	}

	/// <summary>
	/// Apply the scopefilter to a given object
	/// Warning: deep cloning of objects not implemented, MemberwiseClone now used to copy the source object when no scopefilter given.
	/// Warning: when properties are discarded (set by ScopeFilterSettings.PropertiesToDiscard), this might result in the original object losing values on shared references
	/// Warning: for IngeschrevenPersoon only member _embedded is currently discarded, so memberwise clone is enough
	/// Warning: when making this method generic to support all object types, proper deep cloning of objects should be implemented
	/// </summary>
	/// <param name="source"></param>
	/// <param name="scope"></param>
	/// <param name="filterSettings"></param>
	/// <param name="target"></param>
	/// <returns>Object with only the properties set that exist in the scope or that are mandatory</returns>
	public T ApplyScope<T>(T source, string? scope, FieldsSettingsModel filterSettings, T? target = null) where T : class
	{
		if (string.IsNullOrEmpty(scope))
			return (T)DiscardProperties(filterSettings.PropertiesToDiscard, target ?? ObjectHelper.DeepClone(source), filterSettings.ParameterName);
		_scope = scope;

		ValidateScope<T>(scope, filterSettings);

		var listInstancesLookup = new Dictionary<object, Dictionary<object, object>>();
		var scopeProperties = scope.Split(',');
		target ??= (T)Activator.CreateInstance(typeof(T))!;

		//1. set all the properties that exist in the scope from source to the target
		foreach (var propertyName in scopeProperties)
		{
			var fieldToExamine = propertyName;
			if (filterSettings.ShortHandMappings != null && filterSettings.ShortHandMappings.Any() && filterSettings.ShortHandMappings.ContainsKey(fieldToExamine))
			{
				fieldToExamine = filterSettings.ShortHandMappings[fieldToExamine];
			}
			var fieldsToExamine = fieldToExamine.Split("&").ToList();
			foreach (var field in fieldsToExamine)
			{
				if (filterSettings.PropertiesToDiscard == null || !filterSettings.PropertiesToDiscard.Any(x => field.StartsWith(x)))
				{
					var propertyOrMappedProperty = ReplacePropertyNamePartIfMapped(field, filterSettings.PropertyMapping);
					SetPropertyValueFromSourceToTargetScopeFilter(listInstancesLookup, source, target, propertyOrMappedProperty, filterSettings.ParameterName, field);
				}
			}
		}

		//2. set all the properties that always should be set
		if (filterSettings.MandatoryProperties != null)
		{
			foreach (var property in filterSettings.MandatoryProperties)
			{
				SetPropertyValueFromSourceToTarget(listInstancesLookup, source, target, property, filterSettings.ParameterName);
			}
		}

		//3. set properties that are based on existence of other properties in the scope
		if (filterSettings.SetChildPropertiesIfExistInScope != null)
		{
			foreach (var propertyPair in filterSettings.SetChildPropertiesIfExistInScope)
			{
				SetChildPropertiesIfExistsInScope(listInstancesLookup, source, target, scopeProperties, propertyPair.Key, propertyPair.Value, filterSettings.ParameterName);
			}
		}

		//4. set properties based on other properties having a value or not
		if (filterSettings.SetPropertiesIfContextPropertyNotNull != null)
		{
			foreach (var propertyPair in filterSettings.SetPropertiesIfContextPropertyNotNull.Where(x => IsContextPropertyNotNull(x.Value, target, filterSettings.ParameterName)))
			{
				SetPropertyValueFromSourceToTarget(listInstancesLookup, source, target, propertyPair.Key, filterSettings.ParameterName);
			}
		}

		//5. Discard properties
		return (T)DiscardProperties(filterSettings.PropertiesToDiscard!, target, filterSettings.ParameterName);
	}

	/// <summary>
	/// Sets the child properties of the property based on the existence of the contextProperty in the scope
	/// </summary>
	/// <param name="source"></param>
	/// <param name="target"></param>
	/// <param name="scopeProperties"></param>
	/// <param name="propertyName"></param>
	/// <param name="contextPropertyName"></param>
	/// <param name="parameterName"></param>
	private void SetChildPropertiesIfExistsInScope(Dictionary<object, Dictionary<object, object>> listInstancesLookup, object source, object target, string[] scopeProperties, string propertyName, string contextPropertyName, string parameterName)
	{
		var propertyTree = GetPropertyTree(propertyName, target.GetType(), parameterName, propertyName);
		var propertyType = propertyTree.Last().PropertyType;

		var childProperties = propertyType.GetProperties().Select(x => x.GetCustomAttribute<DataMemberAttribute>()?.Name).Where(x => x != null).ToList();

		if (!childProperties.Any())
		{
			SetPropertyValueFromSourceToTarget(listInstancesLookup, source, target, propertyName, parameterName);
			return;
		}

		foreach (var child in childProperties)
		{
			if (scopeProperties == null || //if no scope, set everything
				scopeProperties.Length == 0 ||
				scopeProperties.Contains(contextPropertyName) || //if scope contains contextProperty all the children should be set
				scopeProperties.Contains(!string.IsNullOrEmpty(contextPropertyName) ? $"{contextPropertyName}.{child}" : child)) //if the scope contains contextProperty with this particulair child, set the child property
			{
				var propertyNameWithChild = $"{propertyName}.{child}";
				SetPropertyValueFromSourceToTarget(listInstancesLookup, source, target, propertyNameWithChild, parameterName);
			}
		}
	}

	/// <summary>
	/// Replaces the propertyparts that are available in the mapping.
	/// Example: propertyMapping = ouders > _embedded.ouders, property = ouders.naam, result = _embedded.ouders.naam
	/// </summary>
	/// <param name="property"></param>
	/// <param name="propertyMapping"></param>
	/// <returns></returns>
	private string ReplacePropertyNamePartIfMapped(string property, Dictionary<string, string> propertyMapping)
	{
		//always take the longest possible match from the mapping
		//example: mapping can contain ouders and ouders.ingeschrevenpersonen, should take the longest in case the property is ouders.ingeschrevenpersonen
		var key = propertyMapping.Keys.Where(x => property.StartsWith(x)).OrderByDescending(x => x.Length).FirstOrDefault();

		if (!string.IsNullOrEmpty(key))
			return property.Replace(key, propertyMapping[key]);

		return property;
	}

	/// <summary>
	/// Validates if the properties in the scope are valid
	/// </summary>
	/// <param name="scope"></param>
	/// <param name="allowedScope">Properties that are allowed, if null, everything is allowed except the forbidden properties</param>
	/// <param name="forbiddenScope">Properties that are not allowed, only applicable if allowedScope is null or empty</param>
	/// <returns></returns>
	private void ValidateScope(string scope, List<string> allowedScope, List<string> forbiddenScope, string parameterName)
	{
		var scopeProperties = scope.Split(',');

		if (allowedScope != null && allowedScope.Any())
		{
			var unallowedProperties = scopeProperties.Where(scopeItem => !allowedScope.Any(allowed => scopeItem.StartsWith(allowed))).ToList();
			if (unallowedProperties?.Any() == true)
			{
				var invalidParams = new List<InvalidParams>();
				unallowedProperties.ForEach(unallowedProperty => invalidParams.Add(CreateThrowParameterValidationInvalidParam(parameterName, unallowedProperty)));

				ThrowParameterValidationException(invalidParams);
			}
		}
		else
		{
			if (forbiddenScope != null)
			{
				var unallowedProperties = scopeProperties.Where(scopeItem => forbiddenScope.Any(forbidden => scopeItem.StartsWith(forbidden))).ToList();
				if (unallowedProperties?.Any() == true)
				{
					var invalidParams = new List<InvalidParams>();
					unallowedProperties.ForEach(unallowedProperty => invalidParams.Add(CreateThrowParameterValidationInvalidParam(parameterName, unallowedProperty)));

					ThrowParameterValidationException(invalidParams);
				}
			}
		}
	}

	/// <summary>
	/// Checks if the context property is not null
	/// </summary>
	/// <param name="rootObject"></param>
	/// <returns></returns>
	private bool IsContextPropertyNotNull(string contextProperty, object rootObject, string parameterName)
	{
		if (string.IsNullOrEmpty(contextProperty))
		{
			return rootObject != null;
		}

		var propertyTree = GetPropertyTree(contextProperty, rootObject.GetType(), parameterName, contextProperty);

		return !IsDefault(rootObject, propertyTree);
	}

	/// <summary>
	/// Set the given properties to its default value on the given target instance.
	/// (Warning: doesn support lists at the moment since only _embedded are set to null)
	/// </summary>
	/// <param name="properties"></param>
	/// <param name="target"></param>
	/// <returns></returns>
	private object DiscardProperties(List<string> properties, object target, string parameterName)
	{
		if (properties == null)
			return target;

		foreach (var property in properties)
		{
			var propertyTree = GetPropertyTree(property, target.GetType(), parameterName, property);

			if (!IsDefault(target, propertyTree))
			{
				object propertyOwner = target;

				foreach (var propertyInfo in propertyTree)
				{
					if (propertyInfo == propertyTree.Last())
					{
						if (propertyInfo.GetType().IsValueType)
							propertyInfo.SetValue(propertyOwner, Activator.CreateInstance(propertyInfo.GetType()));
						else
							propertyInfo.SetValue(propertyOwner, null);
					}
					else
					{
						propertyOwner = propertyInfo.GetValue(propertyOwner)!;
					}
				}
			}
		}

		return target;
	}


	private void SetPropertyValueFromSourceToTarget(Dictionary<object, Dictionary<object, object>> listInstancesLookup, object source, object target, string propertyMapped, string parameterName, string? propertyOriginal = null)
	{
		var propertyTree = GetPropertyTree(propertyMapped, target.GetType(), parameterName, propertyOriginal ?? propertyMapped);

		//to avoid parent properties to be initialized when the property is not filled
		//property(tree) is only set when the value not is default
		if (!IsDefault(source, propertyTree))
		{
			SetProperty(source, target, propertyTree, listInstancesLookup);
		}
	}

	private void SetPropertyValueFromSourceToTargetScopeFilter(Dictionary<object, Dictionary<object, object>> listInstancesLookup, object source, object target, string propertyMapped, string parameterName, string? propertyOriginal = null)
	{
		var propertyTree = GetPropertyTree(propertyMapped, target.GetType(), parameterName, propertyOriginal ?? propertyMapped);

		//to avoid parent properties to be initialized when the property is not filled
		//property(tree) is only set when the value not is default
		(object? parent, bool isDefault) isPropertyBranchDefault = (source, false);
		foreach (var propertyBranch in propertyTree)
		{
			if (isPropertyBranchDefault.parent == null)
			{
				break;
			}

			if (isPropertyBranchDefault.parent?.GetType().IsGenericType == true && isPropertyBranchDefault.parent.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
			{
				var parentList = ((IEnumerable)isPropertyBranchDefault.parent).Cast<object>().Select(parentItem => IsPropertyBranchDefault(parentItem, propertyBranch));
				if (parentList != null && parentList.All(parentItem => parentItem.isDefault))
				{
					propertyTree.RemoveAll(propBranch => propertyTree.IndexOf(propBranch) >= propertyTree.IndexOf(propertyBranch));
					break;
				}
				if (parentList != null)
				{
					isPropertyBranchDefault = parentList.FirstOrDefault();
				}
			}
			else
			{
				// parent already checked on above
				isPropertyBranchDefault = IsPropertyBranchDefault(isPropertyBranchDefault.parent!, propertyBranch);
				if (isPropertyBranchDefault.isDefault)
				{
					propertyTree.RemoveAll(propBranch => propertyTree.IndexOf(propBranch) >= propertyTree.IndexOf(propertyBranch));
					break;
				}
			}
		}

		var originalPropertyTree = GetPropertyTree(propertyMapped, target.GetType(), parameterName, propertyOriginal ?? propertyMapped);

		SetPropertyScopeFilter(source, target, propertyTree, listInstancesLookup, originalPropertyTree);
	}

	public List<PropertyInfo> GetPropertyTree(string propertyNameMapped, Type type, string parameterName, string originalPropertyName)
	{
		var propertyParts = propertyNameMapped.Split('.');
		var propertyTree = new List<PropertyInfo>();

		foreach (var part in propertyParts)
		{
			var property = GetProperty(part, !propertyTree.Any() ? type : propertyTree.Last().PropertyType);

			if (property == null)
			{
				var invalidParams = new List<InvalidParams> { CreateThrowParameterValidationInvalidParam(parameterName, originalPropertyName) };
				ThrowParameterValidationException(invalidParams);
			}

			propertyTree.Add(property!); // validation exception is already thrown if null
		}

		return propertyTree;
	}

	private void SetProperty(object source, object target, List<PropertyInfo> propertyTree, Dictionary<object, Dictionary<object, object>> listInstancesLookup)
	{
		object currentTargetInstance = target;
		object currentSourceInstance = source;

		foreach (var property in propertyTree)
		{
			if (currentTargetInstance.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
			{
				//check if the targetlist is not already set with the sourcelist,
				//this can happen if the scope already contained a property pointing at the whole list
				if (currentTargetInstance != currentSourceInstance)
				{
					var tree = propertyTree.SkipWhile(x => x != property).ToList();
					var sourceList = ((IList)currentSourceInstance).Cast<object>().Where(x => !IsDefault(x, tree)).ToList();
					var targetList = (IList)currentTargetInstance;

					//loookup table for list instances is used to be able to map properties from a item in the sourcelist
					//to the same item in the target list
					//this cant be solved by using the order of listitems since the list can contain items
					//with properties which are null
					listInstancesLookup.TryGetValue(currentSourceInstance, out var instancesLookup);

					if (instancesLookup == null)
					{
						instancesLookup = new Dictionary<object, object>();
						listInstancesLookup.Add(currentSourceInstance, instancesLookup);
					}

					foreach (var item in sourceList)
					{
						//check if the instance is already created for an earlier property in the scope
						if (instancesLookup.TryGetValue(item, out object? targetInstance))
						{
							SetProperty(item, targetInstance, tree, listInstancesLookup);
						}
						else
						{
							var targetItem = Activator.CreateInstance(item.GetType())!;
							if (targetList.Count > 0 && targetList.Count >= sourceList.IndexOf(item) && targetList[sourceList.IndexOf(item)] != null)
							{
								targetItem = targetList[sourceList.IndexOf(item)]!;
								SetProperty(item, targetItem, tree, listInstancesLookup);
							}
							else
							{
								SetProperty(item, targetItem, tree, listInstancesLookup);
								targetList.Add(targetItem);
							}

							instancesLookup.Add(item, targetItem);
						}
					}
				}

				break;
			}
			else
			{
				if (property == propertyTree.Last())
				{
					property.SetValue(currentTargetInstance, property.GetValue(currentSourceInstance));
				}
				else
				{
					//property is always a reference type since it's not the last property in the list and owns the property or its parent
					var propertyInstance = property.GetValue(currentTargetInstance) ?? Activator.CreateInstance(property.PropertyType)!;
					property.SetValue(currentTargetInstance, propertyInstance);

					//set instances so that the next property(child) can be set
					currentTargetInstance = propertyInstance;
					currentSourceInstance = property.GetValue(currentSourceInstance)!; //not null should be already checked with IsDefault.
				}
			}
		}
	}

	private void SetPropertyScopeFilter(object source, object target, List<PropertyInfo> propertyTree, Dictionary<object, Dictionary<object, object>> listInstancesLookup, List<PropertyInfo> originalPropertyTree)
	{
		object currentTargetInstance = target;
		object currentSourceInstance = source;

		foreach (var property in propertyTree)
		{
			if (currentTargetInstance.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
			{
				//check if the targetlist is not already set with the sourcelist,
				//this can happen if the scope already contained a property pointing at the whole list
				if (currentTargetInstance != currentSourceInstance)
				{
					var tree = propertyTree.SkipWhile(x => x != property).ToList();
					var areAllSourceItemsDefault = ((IList)currentSourceInstance).Cast<object>().Select(x => IsDefault(x, tree));
					var sourceList = areAllSourceItemsDefault.All(x => x)
						? new List<object>()
						: ((IList)currentSourceInstance).Cast<object>().ToList();
					var targetList = (IList)currentTargetInstance;

					//loookup table for list instances is used to be able to map properties from a item in the sourcelist
					//to the same item in the target list
					//this cant be solved by using the order of listitems since the list can contain items
					//with properties which are null
					listInstancesLookup.TryGetValue(currentSourceInstance, out var instancesLookup);

					if (instancesLookup == null)
					{
						instancesLookup = new Dictionary<object, object>();
						listInstancesLookup.Add(currentSourceInstance, instancesLookup);
					}

					foreach (var item in sourceList)
					{
						//check if the instance is already created for an earlier property in the scope
						if (instancesLookup.TryGetValue(item, out object? targetInstance))
						{
							SetPropertyScopeFilter(item, targetInstance, tree, listInstancesLookup, originalPropertyTree);
						}
						else
						{
							var targetItem = Activator.CreateInstance(item.GetType())!;
							SetPropertyScopeFilter(item, targetItem, tree, listInstancesLookup, originalPropertyTree);
							targetList.Add(targetItem);

							instancesLookup.Add(item, targetItem);
						}
					}
				}

				break;
			}
			else
			{
				var sourcePropertyValue = property.GetValue(currentSourceInstance);
				if (property == propertyTree.Last()
					&& sourcePropertyValue?.GetType().IsGenericType == true && sourcePropertyValue.GetType().GetInterfaces().Contains(typeof(IEnumerable))
					&& !propertyTree.Last().Equals(originalPropertyTree.Last()))
				{
					var propertyValueList = sourcePropertyValue as IList;
					if (propertyValueList == null)
					{
						property.SetValue(currentTargetInstance, Activator.CreateInstance(property.PropertyType));
					}
					else if (!(property.GetValue(currentTargetInstance) is IList currentTargetList) || currentTargetList == null || currentTargetList.Count < 1)
					{
						listInstancesLookup.TryGetValue(propertyValueList, out var instancesLookup);

						if (instancesLookup == null)
						{
							instancesLookup = new Dictionary<object, object>();
							listInstancesLookup.Add(propertyValueList, instancesLookup);
						}
						var itemCount = propertyValueList!.Count;
						var newTargetList = Activator.CreateInstance(property.PropertyType) as IList;
						var listItemType = newTargetList!.GetType().GetGenericArguments().Single();
						for (var index = 0; index < itemCount; index++)
						{
							var newTargetItem = Activator.CreateInstance(listItemType!);
							newTargetList.Add(newTargetItem);
							instancesLookup.Add(propertyValueList[index]!, newTargetItem!);
						}
						property.SetValue(currentTargetInstance, newTargetList);
					}
				}
				else if (property == propertyTree.Last())
				{
					if (!propertyTree.Last().Equals(originalPropertyTree.Last())
						&& !(property.PropertyType.IsValueType || property.PropertyType.Equals(typeof(string))))
					{
						if (IsDefault(currentTargetInstance, propertyTree))
						{
							property.SetValue(currentTargetInstance, Activator.CreateInstance(property.PropertyType));
						}
					}
					else
					{
						property.SetValue(currentTargetInstance, sourcePropertyValue);
					}
				}
				else
				{
					//property is always a reference type since it's not the last property in the list and owns the property or its parent
					var propertyInstance = property.GetValue(currentTargetInstance) ?? Activator.CreateInstance(property.PropertyType)!;
					property.SetValue(currentTargetInstance, propertyInstance);

					//set instances so that the next property(child) can be set
					currentTargetInstance = propertyInstance;
					currentSourceInstance = sourcePropertyValue != null && !IsDefault(sourcePropertyValue, sourcePropertyValue.GetType())
						? sourcePropertyValue
						: Activator.CreateInstance(property.PropertyType)!;
				}
			}
		}
	}

	/// <summary>
	/// Get the property with a certain DataMemberAttribute name from the given type
	/// </summary>
	/// <param name="name"></param>
	/// <param name="type"></param>
	/// <returns>Return property if it exists, otherwise throw exception</returns>
	private PropertyInfo? GetProperty(string name, Type type)
	{
		var actualType = type;

		if (type.IsGenericType && type.GetInterfaces().Contains(typeof(IEnumerable)))
		{
			actualType = type.GenericTypeArguments.First();
		}

		foreach (var property in actualType.GetProperties())
		{
			var attribute = property.GetCustomAttribute<DataMemberAttribute>();

			if (attribute != null && attribute.Name == name)
			{
				return property;
			}
		}

		return null;
	}

	/// <summary>
	/// Check if the value of the property at the end of the propertytree is not default
	/// </summary>
	/// <returns></returns>
	private (object? parent, bool isDefault) IsPropertyBranchDefault(object parent, PropertyInfo propertyBranch)
	{
		var propertyBranchValue = propertyBranch.GetValue(parent);
		if (propertyBranch.PropertyType.IsValueType && Nullable.GetUnderlyingType(propertyBranch.PropertyType) == null)
		{
			return (propertyBranchValue, propertyBranchValue?.Equals(Activator.CreateInstance(propertyBranch.PropertyType)) == true);
		}
		else if (propertyBranchValue == null)
		{
			return (propertyBranchValue, true);
		}

		return (propertyBranchValue, false);
	}

	/// <summary>
	/// Check if the value of the property at the end of the propertytree is not default
	/// </summary>
	/// <param name="instance"></param>
	/// <param name="propertyTree"></param>
	/// <returns></returns>
	private bool IsDefault(object instance, List<PropertyInfo> propertyTree)
	{
		object? propertyValue = instance;

		foreach (var property in propertyTree)
		{
			if (propertyValue?.GetType().IsGenericType == true && propertyValue.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
			{
				var list = ((IEnumerable)propertyValue).Cast<object>().ToList();
				var tree = propertyTree.SkipWhile(x => x != property).ToList();

				return !list.Any(x => !IsDefault(x, tree));
			}
			else
			{
				propertyValue = property.GetValue(propertyValue);

				if (IsDefault(propertyValue, property.PropertyType))
				{
					return true;
				}
			}
		}

		return false;
	}

	public bool IsDefault(object? obj, Type type)
	{
		//value type and not nullable
		if (type.IsValueType && Nullable.GetUnderlyingType(type) == null)
		{
			//Have to use equals because value type will be boxed.
			return obj?.Equals(Activator.CreateInstance(type)) == true;
		}
		return obj == null;
	}

	/// <summary>
	/// Throw exception with total list of all invalidParams.
	/// </summary>
	/// <param name="invalidParams"></param>
	/// <exception cref="InvalidParamsException"></exception>
	private void ThrowParameterValidationException(List<InvalidParams> invalidParams)
	{
		throw new InvalidParamsException(invalidParams);
	}

	/// <summary>
	/// Throw exception when an invalid param is used.
	/// </summary>
	/// <param name="parameterName"></param>
	/// <param name="wrongParameterPart"></param>
	/// <param name="isNotAllowedParameter">Used for fields where values are used in fields that are Forbidden.</param>
	/// <exception cref="InvalidParamsException"></exception>
	private InvalidParams CreateThrowParameterValidationInvalidParam(string parameterName, string wrongParameterPart, bool isNotAllowedParameter = false)
	{
		var code = parameterName;
		if (parameterName.Equals("fields") && !string.IsNullOrWhiteSpace(_scope) && _scope.IndexOf(wrongParameterPart) != -1)
		{
			var fieldScopeList = _scope.Split(',')?.ToList();
			var index = fieldScopeList?.IndexOf(wrongParameterPart);
			parameterName = $"{parameterName}[{index}]";
		}

		return new InvalidParams
		{
			Code = code,
			Name = parameterName,
			Reason = isNotAllowedParameter ? "Parameter bevat een niet toegestane veldnaam." : "Parameter bevat een niet bestaande veldnaam.",
		};
	}
}