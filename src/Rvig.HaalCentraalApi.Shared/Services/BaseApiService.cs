using Microsoft.Extensions.Options;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Fields;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Interfaces;
using Rvig.HaalCentraalApi.Shared.Options;
using Rvig.HaalCentraalApi.Shared.Validation;

namespace Rvig.HaalCentraalApi.Shared.Services
{
	public abstract class BaseApiService
	{
		protected IDomeinTabellenRepo _domeinTabellenRepo;
		protected readonly IOptions<ProtocolleringAuthorizationOptions> _protocolleringAuthorizationOptions;
		private readonly IProtocolleringService _protocolleringService;
		private readonly ILoggingHelper _loggingHelper;

		protected abstract FieldsSettings _fieldsSettings { get; }
		protected readonly FieldsFilterService _fieldsExpandFilterService = new();

		protected BaseApiService(IDomeinTabellenRepo domeinTabellenRepo, IProtocolleringService protocolleringService, ILoggingHelper loggingHelper, IOptions<ProtocolleringAuthorizationOptions> protocolleringAuthorizationOptions)
		{
			_domeinTabellenRepo = domeinTabellenRepo;
			_protocolleringService = protocolleringService;
			_loggingHelper = loggingHelper;
			_protocolleringAuthorizationOptions = protocolleringAuthorizationOptions;
		}

		protected static List<T>? FilterByPeildatum<T>(DateTime? peildatum, List<T> objectsToFilter, string? beginDatePropName, string? endDatePropName) where T : class
		{
			return objectsToFilter
							?.Where(x => ValidationHelperBase.IsPeildatumBetweenStartAndEndDates(peildatum, GetValue(x, beginDatePropName) as string, GetValue(x, endDatePropName) as string))
							.ToList();
		}

		protected List<T>? FilterByPeildatumAndFields<T>(DateTime? peildatum, List<T> objectsToFilter, string? beginDatePropName, string? endDatePropName) where T : class
		{
			return FilterByPeildatum(peildatum, objectsToFilter, beginDatePropName, endDatePropName);
		}

		public static List<T>? FilterByDatumVanDatumTot<T>(DateTime? datumVan, DateTime? datumTot, List<T> objectsToFilter, string? beginDatePropName, string? endDatePropName) where T : class
		{
			return objectsToFilter
							?.Where(x => ValidationHelperBase.TimePeriodesOverlap(datumVan, datumTot, GetValue(x, beginDatePropName) as string, GetValue(x, endDatePropName) as string))
							.ToList();
		}

		protected List<T>? FilterByDatumVanDatumTotAndFields<T>(DateTime? datumVan, DateTime? datumTot, List<string> fields, List<T> objectsToFilter, FieldsSettingsModel fieldsModel, string? beginDatePropName, string? endDatePropName) where T : class
		{
			return FilterByDatumVanDatumTot(datumVan, datumTot, objectsToFilter, beginDatePropName, endDatePropName)
							?.Select(x => _fieldsExpandFilterService.ApplyScope(x!, string.Join(",", fields), fieldsModel))
							.ToList();
		}

		private static object? GetValue(object? sourceObject, string? propName)
		{
			var parentType = sourceObject?.GetType();
			if (string.IsNullOrWhiteSpace(propName))
			{
				return null;
			}
			var propertyParts = propName.Split('.');
			(object? prev, object? current, bool first) = (null, null, true);
			foreach (var propPart in propertyParts)
			{
				if (!first)
				{
					parentType = prev?.GetType();
					sourceObject = prev;
				}

				var prop = parentType?.GetProperty(propPart);
				if (prop == null)
				{
					return null;
				}
				current = prop.GetValue(sourceObject);
				prev = current;
				first = false;
			}
			return current;
		}

		protected async Task LogProtocolleringInDb(int afnemerCode, long? pl_id, List<string> searchedRubrieken, List<string> gevraagdeRubrieken)
		{
			try
			{
				_loggingHelper.LogDebug("Inserting protocollering.");
				// autorisatie is already validated in GetAfnemerAutorisatie as autorisatie.
				await _protocolleringService.Insert(afnemerCode, pl_id, string.Join(", ", searchedRubrieken.Distinct()), string.Join(", ", gevraagdeRubrieken.Distinct()));
				_loggingHelper.LogDebug("Inserted protocollering.");
			}
			catch (Exception e)
			{
				_loggingHelper.LogError("Failed to insert protocollering.");
				throw new CustomInvalidOperationException("Interne server fout.", new CustomInvalidOperationException("Protocollering is mislukt. Request is beëindigd.", e));
			}
		}

		protected async Task LogProtocolleringInDb(int afnemerCode, List<long>? pl_ids, List<string> searchedRubrieken, List<string> gevraagdeRubrieken)
		{
			if (pl_ids?.Any() == true)
			{
				pl_ids = pl_ids!.Where(pl_id => pl_id != 0).ToList();
				try
				{
					_loggingHelper.LogDebug("Inserting protocollering.");
					// autorisatie is already validated in GetAfnemerAutorisatie as autorisatie.
					if (pl_ids!.Count == 1)
					{
						await _protocolleringService.Insert(afnemerCode, pl_ids!.Single(), string.Join(", ", searchedRubrieken.Distinct()), string.Join(", ", gevraagdeRubrieken.Distinct()));
					}
					else
					{
						await _protocolleringService.Insert(afnemerCode, pl_ids!, string.Join(", ", searchedRubrieken.Distinct()), string.Join(", ", gevraagdeRubrieken.Distinct()));
					}
					_loggingHelper.LogDebug("Inserted protocollering.");
				}
				catch (Exception e)
				{
					_loggingHelper.LogError("Failed to insert protocollering.");
					throw new CustomInvalidOperationException("Interne server fout.", new CustomInvalidOperationException("Protocollering is mislukt. Request is beëindigd.", e));
				}
			}
		}
	}
}