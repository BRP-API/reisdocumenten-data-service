using Rvig.Data.Base.DatabaseModels;
using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.Data.Base.Postgres.Helpers;
using Rvig.Data.Base.Postgres.Mappers;
using Rvig.Data.Base.Postgres.Mappers.Helpers;
using Rvig.HaalCentraalApi.Reisdocumenten.ApiModels.Reisdocumenten;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Helpers;

namespace Rvig.Data.Reisdocumenten.Mappers;
public interface IRvIGDataReisdocumentenMapper
{
	IEnumerable<GbaReisdocument> MapReisdocumenten(IEnumerable<lo3_pl_reis_doc> dbReisdocumenten);
	GbaReisdocument? MapReisdocument(lo3_pl_reis_doc dbReisdocument);
}

public class RvIGDataReisdocumentenMapper : RvIGDataMapperBase, IRvIGDataReisdocumentenMapper
{
    public RvIGDataReisdocumentenMapper(IDomeinTabellenHelper domeinTabellenHelper)
		: base(domeinTabellenHelper)
    {
    }

	public GbaReisdocument? MapReisdocument(lo3_pl_reis_doc dbReisdocument)
	{
		GbaReisdocument target = new();
		foreach (string propertyName in ObjectHelper.GetPropertyNames<GbaReisdocument>())
		{
			switch(propertyName)
			{
				case nameof(GbaReisdocument.Reisdocumentnummer):
					target.Reisdocumentnummer = dbReisdocument.nl_reis_doc_nr;
					break;
				case nameof(GbaReisdocument.Soort):
					target.Soort = GbaMappingHelper.ParseToSoortReisdocumentEnum(dbReisdocument.nl_reis_doc_soort);
					break;
				case nameof(GbaReisdocument.DatumEindeGeldigheid):
					target.DatumEindeGeldigheid = GbaMappingHelper.ParseToDatumOnvolledig(dbReisdocument.nl_reis_doc_geldig_eind_datum);
					break;
				case nameof(GbaReisdocument.InhoudingOfVermissing):
					target.InhoudingOfVermissing = MapInhoudingOfVermissing(dbReisdocument);
					break;
				case nameof(GbaReisdocument.Houder):
					target.Houder = MapHouder(dbReisdocument);
					break;
				case nameof(GbaReisdocument.InOnderzoek):
					target.InOnderzoek = MapGbaInOnderzoek(dbReisdocument.onderzoek_gegevens_aand, dbReisdocument.onderzoek_start_datum, dbReisdocument.onderzoek_eind_datum);
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaReisdocument)} property {propertyName}.");
			}
		}

		return target;
	}

	public IEnumerable<GbaReisdocument> MapReisdocumenten(IEnumerable<lo3_pl_reis_doc> dbReisdocumenten)
	{
		if (dbReisdocumenten?.Any() != true)
		{
			return Enumerable.Empty<GbaReisdocument>();
		}

		var reisdocumenten = dbReisdocumenten
			.Select(MapReisdocument);

		return reisdocumenten.Where(dbReisdocument => dbReisdocument != null)!; // Where clause makes it impossible to return a list with null values.
	}

	private GbaInhoudingOfVermissing? MapInhoudingOfVermissing(lo3_pl_reis_doc dbReisdocument)
	{
		var inhoudingOfVermissing = new GbaInhoudingOfVermissing();
		if (dbReisdocument == null)
		{
			return null;
		}

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaInhoudingOfVermissing>())
		{
			switch (propertyName)
			{
				// Set in MapOpschortingBijhoudingBasis
				case nameof(GbaInhoudingOfVermissing.Aanduiding):
					if (!string.IsNullOrWhiteSpace(dbReisdocument.nl_reis_doc_weg_ind))
					{
						inhoudingOfVermissing.Aanduiding = GbaMappingHelper.ParseAanduidingInhoudingVermissingNederlandsReisdocument(dbReisdocument.nl_reis_doc_weg_ind);
					}
					break;
				case nameof(GbaInhoudingOfVermissing.Datum):
					inhoudingOfVermissing.Datum = GbaMappingHelper.ParseToDatumOnvolledig(dbReisdocument.nl_reis_doc_weg_datum);
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaInhoudingOfVermissing)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(inhoudingOfVermissing);
	}

	private GbaReisdocumenthouder? MapHouder(lo3_pl_reis_doc dbReisdocument)
	{
		var houder = new GbaReisdocumenthouder();
		if (dbReisdocument == null)
		{
			return null;
		}

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaReisdocumenthouder>())
		{
			switch (propertyName)
			{
				// Set in MapOpschortingBijhoudingBasis
				case nameof(GbaReisdocumenthouder.Burgerservicenummer):
					if (dbReisdocument.burger_service_nr.HasValue)
					{
						houder.Burgerservicenummer = dbReisdocument.burger_service_nr?.ToString().PadLeft(9, '0');
					}
					break;
				case nameof(GbaReisdocumenthouder.GeheimhoudingPersoonsgegevens):
					houder.GeheimhoudingPersoonsgegevens = dbReisdocument.pl_geheim_ind.HasValue && dbReisdocument.pl_geheim_ind != 0 ? dbReisdocument.pl_geheim_ind : null;
					break;
				case nameof(GbaReisdocumenthouder.InOnderzoek):
					houder.InOnderzoek = MapGbaInOnderzoek(dbReisdocument.pers_onderzoek_aand, dbReisdocument.pers_onderzoek_start_datum, dbReisdocument.pers_onderzoek_eind_datum);
					break;
				case nameof(GbaReisdocumenthouder.OpschortingBijhouding):
					houder.OpschortingBijhouding = MapOpschortingBijhouding(
							new lo3_pl
							{
								bijhouding_opschort_datum = dbReisdocument.pl_bijhouding_opschort_datum,
								bijhouding_opschort_reden = dbReisdocument.pl_bijhouding_opschort_reden
							}
						);
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaReisdocumenthouder)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(houder);
	}
}