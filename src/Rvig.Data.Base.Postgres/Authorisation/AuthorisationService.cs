using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Rvig.Data.Base.Postgres.Authorisation;

public static class AuthorisationService
{
	private static readonly AutorisationMappings _autorisationActueelMappings = AutorisationHelper.CreateRubriekPropertyMapping<DbPersoonActueelWrapper>();

	private static readonly AutorisationMappings _autorisationHistorieMappings = AutorisationHelper.CreateRubriekPropertyMapping<DbPersoonHistorieWrapper>();
	private static readonly List<int> _implicietAuthorizedRubrieken = new()
	{
		// InOnderzoek
		18310, 18320, 18330, 28310, 28320, 28330, 38310, 38320, 38330, 48310, 48320, 48330, 58310, 58320, 58330, 68310, 68320, 68330
		, 88310, 88320, 88330, 98310, 98320, 98330, 108310, 108320, 108330, 118310, 118320, 118330, 128310, 128320, 128330

		// rni_deelnemer
		, 18810, 18820, 48810, 48820, 68810, 68820, 78810, 78820, 88810, 88820

		// verificatie
		, 77110, 77120

		// opschorting
		, 76710, 76720

		// geheimhouding
		, 77010

		// onjuist_ind
		, 18410, 28410, 38410, 58410, 68410, 98410
	};

	/// <summary>
	/// Protocollering has a column called 'gevraagde_rubrieken'. This column may not contain the _implicietAuthorizedRubrieken.
	/// </summary>
	/// <param name="protocolleringRubrieken"></param>
	/// <returns></returns>
	public static string? RemoveImplicitRubriekenForProtocllering(string? protocolleringRubrieken)
	{
		if (string.IsNullOrWhiteSpace(protocolleringRubrieken))
		{
			return protocolleringRubrieken;
		}
		return string.Join(", ", protocolleringRubrieken.Split(", ").Where(rubriek => !_implicietAuthorizedRubrieken.Contains(int.Parse(rubriek))));
	}

	public static List<int> GetAuthorizedRubrieken(DbAutorisatie autorisatie)
	{
		if (string.IsNullOrEmpty(autorisatie?.ad_hoc_rubrieken))
		{
			return new List<int>();
		}

		var authorizedRubrieken = autorisatie.ad_hoc_rubrieken
											.Split(' ')
											.Select(x => int.Parse(x))
											.ToList();

		// Add InOnderzoek and rni_deelnemer rubrieken because ad_hoc_rubrieken do not contain them even though everyone has access to them.
		authorizedRubrieken.AddRange(_implicietAuthorizedRubrieken);

		return authorizedRubrieken;
	}

	public static void CheckAuthorizationSearchRubrieken(List<int> authorizedRubrieken, List<(string field, string rubriek)> searchedRubrieken)
	{
		var stringifiedAuthorizedRubrieken = authorizedRubrieken.Select(rubriek => rubriek.ToString().PadLeft(6, '0'));

		var unAuthorizedFields = searchedRubrieken.Where(searchedRubriek => !stringifiedAuthorizedRubrieken.Contains(searchedRubriek.rubriek)).ToList();
		if (unAuthorizedFields.Any(searchedRubriek => searchedRubriek.field.Contains("ouderAanduiding"))
			// If there is authorisation for at least one rubriek of category 2 and one rubriek of category 3.
			// Exclude inOnderzoek rubrieken because these are available for everyone.
			&& stringifiedAuthorizedRubrieken.Any(x => Regex.IsMatch(x, "(?!028310|028320|028330|028410|028510|028610)02[0-9]{4}"))
			&& stringifiedAuthorizedRubrieken.Any(x => Regex.IsMatch(x, "(?!038310|038320|038330|038410|038510|038610)03[0-9]{4}")))
		{
			unAuthorizedFields.Remove(("ouders.ouderAanduiding", ""));
		}
		if (unAuthorizedFields.Count > 0)
		{
			throw new UnauthorizedFieldException();
		}
	}

	public static bool IsBinnenGemeentelijk(short? afnemerGemeenteCode, string? requestGemeenteVanInschrijving) =>
		!string.IsNullOrWhiteSpace(requestGemeenteVanInschrijving)
			&& afnemerGemeenteCode.HasValue
			&& afnemerGemeenteCode.Value
					.ToString()
					.PadLeft(requestGemeenteVanInschrijving.Length, '0')
					.Equals(requestGemeenteVanInschrijving);

	public static bool IsBinnenGemeentelijk(short? afnemerGemeenteCode, short? requestGemeenteVanInschrijving) =>
		requestGemeenteVanInschrijving.HasValue
			&& afnemerGemeenteCode.HasValue
			&& afnemerGemeenteCode.Value
					.Equals(requestGemeenteVanInschrijving);

	// For non Actual BRP for now. TOdDO: Fix this for others.
	public static T? Apply<T>(T? dbPersoon, DbAutorisatie autorisatie, bool isBinnenGemeentelijk) where T : DbPersoonBaseWrapper
    {
        if (dbPersoon == null)
            return null;

		if (isBinnenGemeentelijk)
		{
			return dbPersoon;
		}

		if (string.IsNullOrEmpty(autorisatie?.ad_hoc_rubrieken))
            return null;

        if (autorisatie.geheimhouding_ind == 1 && new short?[] { 2, 4, 6, 7 }.Contains(dbPersoon.Inschrijving.geheim_ind))
            return null;

        if (autorisatie.bijzondere_betrekking_kind_verstrekken == 0)
            dbPersoon.Kinderen = dbPersoon.Kinderen.Where(x => x.registratie_betrekking?.ToUpper() != "L").ToList();

        var authorizedRubrieken = autorisatie.ad_hoc_rubrieken.Split(' ').Select(x => int.Parse(x)).ToList();
		// Add InOnderzoek and rni_deelnemer rubrieken because ad_hoc_rubrieken do not contain them even though everyone has access to them.
		authorizedRubrieken.AddRange(_implicietAuthorizedRubrieken);

		return ApplyFilter(dbPersoon, authorizedRubrieken.ToArray(), dbPersoon is DbPersoonActueelWrapper ? _autorisationActueelMappings : _autorisationHistorieMappings);
    }

    public static T? Apply<T>(T? dbPersoon, DbAutorisatie autorisatie, bool isBinnenGemeentelijk, List<int> authorizedRubrieken) where T : DbPersoonBaseWrapper
    {
        if (dbPersoon == null)
            return null;

        if (isBinnenGemeentelijk)
		{
			return dbPersoon;
		}

        if (autorisatie.geheimhouding_ind == 1 && new short?[] { 2, 4, 6, 7 }.Contains(dbPersoon.Inschrijving.geheim_ind))
            return null;

		return ApplyFilter(dbPersoon, authorizedRubrieken.ToArray(), dbPersoon is DbPersoonActueelWrapper ? _autorisationActueelMappings : _autorisationHistorieMappings);
	}

	public static object? Apply(object? dbObject, DbAutorisatie autorisatie, bool isBinnenGemeentelijk, List<int> authorizedRubrieken)
	{
		if (dbObject == null)
			return null;

		if (isBinnenGemeentelijk)
		{
			return dbObject;
		}

		//if (autorisatie.geheimhouding_ind == 1 && new short?[] { 2, 4, 6, 7 }.Contains(dbPersoon.Inschrijving.geheim_ind))
		//    return null;

		return ApplyFilter(dbObject, authorizedRubrieken.ToArray(), AutorisationHelper.CreateRubriekPropertyMapping(dbObject.GetType()));
	}

	private static T ApplyFilter<T>(T dbPersoon, int[] authorizedRubrieken, AutorisationMappings autorisationMappings) where T : DbPersoonBaseWrapper
    {
		var filteredPersoon = Activator.CreateInstance<T>();
        var listInstances = new Dictionary<object, object>();

        foreach (var rubriek in authorizedRubrieken)
        {
            if (autorisationMappings.MappingActueel.TryGetValue(rubriek, out (PropertyInfo, List<PropertyInfo>) propTree1))
            {
                AutorisationHelper.CopyProperty(dbPersoon, filteredPersoon, propTree1, listInstances, false);
            }

            if (autorisationMappings.MappingHistorisch.TryGetValue(rubriek, out (PropertyInfo, List<PropertyInfo>) propTree2))
            {
                AutorisationHelper.CopyProperty(dbPersoon, filteredPersoon, propTree2, listInstances, true);
            }
        }

        foreach (var propTreeAlways in autorisationMappings.AlwaysAuthorized)
        {
            AutorisationHelper.CopyProperty(dbPersoon, filteredPersoon, (propTreeAlways.Item1, new List<PropertyInfo> { propTreeAlways.Item2 }), listInstances);
        }

        return filteredPersoon;
	}

	private static object? ApplyFilter(object dbObject, int[] authorizedRubrieken, AutorisationMappings autorisationMappings)
	{
		var filteredPersoon = Activator.CreateInstance(dbObject.GetType());
		var listInstances = new Dictionary<object, object>();

		foreach (var rubriek in authorizedRubrieken)
		{
			if (autorisationMappings.MappingActueel.TryGetValue(rubriek, out (PropertyInfo, List<PropertyInfo>) propTree1))
			{
				AutorisationHelper.CopyProperty(dbObject, filteredPersoon, propTree1, listInstances, false);
			}

			if (autorisationMappings.MappingHistorisch.TryGetValue(rubriek, out (PropertyInfo, List<PropertyInfo>) propTree2))
			{
				AutorisationHelper.CopyProperty(dbObject, filteredPersoon, propTree2, listInstances, true);
			}
		}

		foreach (var propTreeAlways in autorisationMappings.AlwaysAuthorized)
		{
			AutorisationHelper.CopyProperty(dbObject, filteredPersoon, (propTreeAlways.Item1, new List<PropertyInfo> { propTreeAlways.Item2 }), listInstances);
		}

		return filteredPersoon;
	}
}
