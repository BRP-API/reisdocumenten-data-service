using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.Data.Base.Postgres.Helpers;
using Rvig.Data.Base.Postgres.Mappers.Helpers;
using Rvig.HaalCentraalApi.Shared.ApiModels.Universal;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.ApiModels.PersonenHistorieBase;
using Rvig.HaalCentraalApi.Shared.Exceptions;

namespace Rvig.Data.Base.Postgres.Mappers;

public class RvIGDataMapperBase
{
	protected readonly IDomeinTabellenHelper _domeinTabellenHelper;

	public RvIGDataMapperBase(IDomeinTabellenHelper domeinTabellenHelper)
	{
		_domeinTabellenHelper = domeinTabellenHelper;
	}

	protected async Task<T?> MapVerblijfplaats<T>(lo3_pl_verblijfplaats dbVerblijfplaats, lo3_adres? dbAdres, Waardetabel? gemeenteVanInschrijving, IEnumerable<string>? excludePropNames = null) where T : GbaVerblijfplaats
	{
		var verblijfplaats = Activator.CreateInstance<T>();

		var propNames = excludePropNames == null
							? ObjectHelper.GetPropertyNames<T>()
							: ObjectHelper.GetPropertyNames<T>(excludePropNames);
		foreach (var propertyName in propNames)
		{
			switch (propertyName)
			{
				case nameof(GbaVerblijfplaats.FunctieAdres):
					verblijfplaats.FunctieAdres = GbaMappingHelper.ParseToSoortAdresEnum(dbVerblijfplaats!.adres_functie);
					break;
				case nameof(GbaVerblijfplaats.AdresseerbaarObjectIdentificatie):
					if (dbAdres?.verblijf_plaats_ident_code?.Length == 16 && long.TryParse(dbAdres?.verblijf_plaats_ident_code, out _))
					{
						verblijfplaats.AdresseerbaarObjectIdentificatie = dbAdres?.verblijf_plaats_ident_code;
					}
					break;
				case nameof(GbaVerblijfplaats.NummeraanduidingIdentificatie):
					if (dbAdres?.nummer_aand_ident_code?.Length == 16 && long.TryParse(dbAdres?.nummer_aand_ident_code, out _))
					{
						verblijfplaats.NummeraanduidingIdentificatie = dbAdres?.nummer_aand_ident_code;
					}
					break;
				case nameof(GbaVerblijfplaats.Locatiebeschrijving):
					verblijfplaats.Locatiebeschrijving = dbAdres?.diak_locatie_beschrijving ?? dbAdres?.locatie_beschrijving;
					break;
				case nameof(GbaVerblijfplaats.Straat):
					verblijfplaats.Straat = dbAdres?.diak_straat_naam ?? dbAdres?.straat_naam;
					break;
				case nameof(GbaVerblijfplaats.AanduidingBijHuisnummer):
					verblijfplaats.AanduidingBijHuisnummer = GbaMappingHelper.ParseToAanduidingBijHuisnummerEnum(dbAdres?.huis_nr_aand);
					break;
				case nameof(GbaVerblijfplaats.DatumIngangGeldigheid):
					verblijfplaats.DatumIngangGeldigheid = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaats?.geldigheid_start_datum);
					break;
				case nameof(GbaVerblijfplaats.DatumAanvangAdreshouding):
					verblijfplaats.DatumAanvangAdreshouding = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaats?.adreshouding_start_datum);
					break;
				case nameof(GbaVerblijfplaats.InOnderzoek):
					verblijfplaats.InOnderzoek = MapGbaInOnderzoek(dbVerblijfplaats?.onderzoek_gegevens_aand, dbVerblijfplaats?.onderzoek_start_datum, dbVerblijfplaats?.onderzoek_eind_datum);
					break;
				case nameof(GbaVerblijfplaats.DatumAanvangAdresBuitenland):
					verblijfplaats.DatumAanvangAdresBuitenland = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfplaats?.vertrek_datum);
					break;
				case nameof(GbaVerblijfplaats.Regel1):
					verblijfplaats.Regel1 = dbVerblijfplaats?.vertrek_land_adres_1;
					break;
				case nameof(GbaVerblijfplaats.Regel2):
					verblijfplaats.Regel2 = dbVerblijfplaats?.vertrek_land_adres_2;
					break;
				case nameof(GbaVerblijfplaats.Regel3):
					verblijfplaats.Regel3 = dbVerblijfplaats?.vertrek_land_adres_3;
					break;
				case nameof(GbaVerblijfplaats.Land):
					if (dbVerblijfplaats?.vertrek_land_code != null && dbVerblijfplaats.vertrek_land_code != 6030)
					{
						verblijfplaats.Land = new Waardetabel {
							Code = dbVerblijfplaats.vertrek_land_code?.ToString().PadLeft(4, '0'),
							Omschrijving = dbVerblijfplaats.vertrek_land_naam ?? await _domeinTabellenHelper.GetLandOmschrijving(dbVerblijfplaats.vertrek_land_code)
						};
					}
					break;
				case nameof(GbaVerblijfplaats.NaamOpenbareRuimte):
					verblijfplaats.NaamOpenbareRuimte = dbAdres?.diak_open_ruimte_naam ?? dbAdres?.open_ruimte_naam;
					break;
				case nameof(GbaVerblijfplaats.Huisnummer):
					verblijfplaats.Huisnummer = dbAdres?.huis_nr;
					break;
				case nameof(GbaVerblijfplaats.Huisletter):
					verblijfplaats.Huisletter = dbAdres?.huis_letter;
					break;
				case nameof(GbaVerblijfplaats.Huisnummertoevoeging):
					verblijfplaats.Huisnummertoevoeging = dbAdres?.huis_nr_toevoeging;
					break;
				case nameof(GbaVerblijfplaats.Postcode):
					verblijfplaats.Postcode = dbAdres?.postcode;
					break;
				case nameof(GbaVerblijfplaats.Woonplaats):
					// Docs: https://github.com/VNG-Realisatie/Haal-Centraal-BRP-bevragen/blob/v1.3.0/features/woonplaats.feature
					verblijfplaats.Woonplaats = !string.IsNullOrEmpty(dbAdres?.diak_woon_plaats_naam) ? dbAdres?.diak_woon_plaats_naam : dbAdres?.woon_plaats_naam;
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaVerblijfplaats)} property {propertyName}");
			}
		}

		// Docs: https://github.com/VNG-Realisatie/Haal-Centraal-BRP-bevragen/blob/v1.3.0/features/adres.feature
		// This feature is not meant for GBA
		//PersoonMappingHelper.SetAdresRegels(verblijfplaats, gemeenteVanInschrijving)

		return ObjectHelper.InstanceOrNullWhenDefault(verblijfplaats);
	}

	protected async Task<Waardetabel?> GetGemeenteVanInschrijving(lo3_pl_verblijfplaats? verblijfplaats)
	{
		return verblijfplaats?.inschrijving_gemeente_code != null
									?
										new Waardetabel
										{
											Code = verblijfplaats?.inschrijving_gemeente_code?.ToString().PadLeft(4, '0'),
											Omschrijving = verblijfplaats?.inschrijving_gemeente_naam ?? await _domeinTabellenHelper.GetGemeenteOmschrijving(verblijfplaats?.inschrijving_gemeente_code)
										}
									: null;
	}

	protected static GbaInOnderzoek? MapGbaInOnderzoek(int? inOnderzoekAanduiding, int? inOnderzoekBeginDatum, int? inOnderzoekEindDatum)
	{
		if ((!inOnderzoekAanduiding.HasValue && !inOnderzoekBeginDatum.HasValue) || inOnderzoekEindDatum.HasValue)
		{
			return null;
		}

		return new GbaInOnderzoek
		{
			AanduidingGegevensInOnderzoek = (inOnderzoekAanduiding?.ToString().PadLeft(6, '0')),
			DatumIngangOnderzoek = GbaMappingHelper.ParseToDatumOnvolledig(inOnderzoekBeginDatum)
		};
	}

	protected async Task<GbaNaamBasis?> MapNaam(lo3_pl_persoon dbPersoon, GbaNaamBasis naam)
	{
		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaNaamBasis>())
		{
			switch (propertyName)
			{
				case nameof(GbaNaamBasis.Voornamen):
					naam.Voornamen = dbPersoon.diak_voor_naam ?? dbPersoon.voor_naam;
					break;
				case nameof(GbaNaamBasis.Geslachtsnaam):
					naam.Geslachtsnaam = dbPersoon.diak_geslachts_naam ?? dbPersoon.geslachts_naam;
					break;
				case nameof(GbaNaamBasis.Voorvoegsel):
					naam.Voorvoegsel = dbPersoon.geslachts_naam_voorvoegsel;
					break;
				case nameof(GbaNaamBasis.AdellijkeTitelPredicaat):
					if (!string.IsNullOrWhiteSpace(dbPersoon.titel_predicaat))
					{
						var adellijkeTitelPredicaatOmschrijvingSoort = await _domeinTabellenHelper.GetAdellijkeTitelPredikaatOmschrijvingEnSoort(dbPersoon.titel_predicaat);
						naam.AdellijkeTitelPredicaat = new AdellijkeTitelPredicaatType
						{
							Code = dbPersoon.titel_predicaat,
							Omschrijving = adellijkeTitelPredicaatOmschrijvingSoort.omschrijving,
							Soort = GbaMappingHelper.ParseToSoortAdellijkeTitelPredikaatEnum(adellijkeTitelPredicaatOmschrijvingSoort.soort)
						};
					}
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaNaamBasis)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(naam);
	}

	protected async Task<GbaGeboorte?> MapGeboorte(lo3_pl_persoon dbPersoon)
	{
		var geboorte = new GbaGeboorte();

		MapGeboorteBeperkt(dbPersoon, geboorte);

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaGeboorte>())
		{
			switch (propertyName)
			{
				case nameof(GbaGeboorte.Plaats):
					geboorte.Plaats = await MapPlaats(dbPersoon.geboorte_land_code, dbPersoon.geboorte_plaats, dbPersoon.geboorte_plaats_naam);
					break;
				case nameof(GbaGeboorte.Land):
					if (dbPersoon.geboorte_land_code.HasValue)
					{
						geboorte.Land = new Waardetabel
						{
							Code = dbPersoon.geboorte_land_code?.ToString().PadLeft(4, '0'),
							Omschrijving = dbPersoon.geboorte_land_naam ?? await _domeinTabellenHelper.GetLandOmschrijving(dbPersoon.geboorte_land_code)
						};
					}
					break;
				// This must be mapped in MapGeboorteBeperkt.
				case nameof(GbaGeboorte.Datum):
					break;
				// These are auto mapped in Datum in MapGeboorteBeperkt.
				case nameof(GbaGeboorte.DatumJaar):
				case nameof(GbaGeboorte.DatumMaand):
				case nameof(GbaGeboorte.DatumDag):
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaGeboorte)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(geboorte);
	}

	protected async Task<Waardetabel?> MapPlaats(short? geboorteLandCode, string? geboortePlaatsCode, string? geboortePlaatsNaam)
	{
		var plaatsOmschrijving = geboortePlaatsNaam;
		_ = int.TryParse(geboortePlaatsCode, out var plaatsCode);
		if (plaatsCode != 0 || (geboortePlaatsCode?.Equals("0000") == true && plaatsCode == 0))
		{
			plaatsOmschrijving ??= await _domeinTabellenHelper.GetGemeenteOmschrijving(plaatsCode);
		}
		return GbaMappingHelper.MapBinnenOfBuitenlandsePlaats(geboorteLandCode, geboortePlaatsCode, plaatsOmschrijving);
	}

	protected void MapGeboorteBeperkt(lo3_pl_persoon dbPersoon, GbaGeboorteBeperkt geboorte)
	{
		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaGeboorteBeperkt>())
		{
			switch (propertyName)
			{
				case nameof(GbaGeboorteBeperkt.Datum):
					geboorte.Datum = GbaMappingHelper.ParseToDatumOnvolledig(dbPersoon.geboorte_datum);
					break;
				// These are auto mapped in Datum.
				case nameof(GbaGeboorteBeperkt.DatumJaar):
				case nameof(GbaGeboorteBeperkt.DatumMaand):
				case nameof(GbaGeboorteBeperkt.DatumDag):
					break;

				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaGeboorteBeperkt)} property {propertyName}");
			}
		}
	}

	protected async Task<IEnumerable<T?>> MapPartners<T>(List<lo3_pl_persoon> dbPartners, IEnumerable<string>? excludePropNames = null) where T : GbaPartner
	{
		var partnerTasks = dbPartners
			.Where(x => x.onjuist_ind == null)
			.OrderBy(x => x.stapel_nr)
			.GroupBy(x => x.stapel_nr)
			.Select(async stapel =>
			{
				var currentPartner = stapel.FirstOrDefault(x => x.volg_nr == 0);
				if (currentPartner == null)
				{
					return null;
				}
				var mappedPartner = await MapPartner<GbaPartner>(currentPartner, excludePropNames);

				if (mappedPartner != null && mappedPartner.AangaanHuwelijkPartnerschap == null && stapel.Any(x => x.volg_nr > 0))
				{
					var oldestRecordOfCurrentPartner = stapel
						.Where(partnerIteration => partnerIteration.volg_nr != 0)
						.OrderBy(partnerIteration => partnerIteration.volg_nr)
						.FirstOrDefault(partnerIteration =>
								partnerIteration.relatie_start_datum.HasValue
								|| partnerIteration.relatie_start_land_code.HasValue
								|| !string.IsNullOrWhiteSpace(partnerIteration.relatie_start_plaats)
								|| !string.IsNullOrWhiteSpace(partnerIteration.relatie_start_plaats_naam)
								|| !string.IsNullOrWhiteSpace(partnerIteration.relatie_start_land_naam)
								);
					if (oldestRecordOfCurrentPartner != null)
					{
						mappedPartner.AangaanHuwelijkPartnerschap = (await MapPartner<GbaPartner>(oldestRecordOfCurrentPartner, excludePropNames))?.AangaanHuwelijkPartnerschap;
					}
				}
				return mappedPartner;
			});

			return (await Task.WhenAll(partnerTasks))
			.Where(x => x != null)
			.Cast<T?>();
	}
	protected async Task<T?> MapPartner<T>(lo3_pl_persoon dbPartner, IEnumerable<string>? excludePropNames = null) where T : GbaPartner
	{
		// Because of GBA feature, the GBA API must always return an empty GbaPartner object
		//if (dbPartner == null) return (null, null);

		var partner = Activator.CreateInstance<T>();

		var propNames = excludePropNames == null
							? ObjectHelper.GetPropertyNames<T>()
							: ObjectHelper.GetPropertyNames<T>(excludePropNames);
		foreach (var propertyName in propNames)
		{
			switch (propertyName)
			{
				case nameof(GbaPartner.SoortVerbintenis):
					partner.SoortVerbintenis = GbaMappingHelper.ParseToSoortVerbintenisEnum(dbPartner.verbintenis_soort);
					break;
				case nameof(GbaPartner.Geslacht):
					partner.Geslacht = GbaMappingHelper.ParseToGeslachtEnum(dbPartner.geslachts_aand);
					break;
				case nameof(GbaPartner.Naam):
					partner.Naam = await MapNaam(dbPartner, new GbaNaamBasis());
					break;
				case nameof(GbaPartner.InOnderzoek):
					partner.InOnderzoek = MapGbaInOnderzoek(dbPartner.onderzoek_gegevens_aand, dbPartner.onderzoek_start_datum, dbPartner.onderzoek_eind_datum);
					break;
				case nameof(GbaPartner.AangaanHuwelijkPartnerschap):
					partner.AangaanHuwelijkPartnerschap = await MapAangaanHuwelijkPartnerschap(dbPartner);
					break;
				case nameof(GbaPartner.Burgerservicenummer):
					partner.Burgerservicenummer = dbPartner.burger_service_nr?.ToString().PadLeft(9, '0');
					break;
				case nameof(GbaPartner.Geboorte):
					partner.Geboorte = await MapGeboorte(dbPartner);
					break;
				case nameof(GbaPartner.OntbindingHuwelijkPartnerschap):
					partner.OntbindingHuwelijkPartnerschap = MapOntbindingHuwelijkPartnerschap(dbPartner);
					break;

				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaPartner)} property {propertyName}");
			}
		}

		if (IsPartnerNonExistent(partner))
		{
			return null;
		}

		// Because of GBA feature, the GBA API must always return an empty GbaPartner object
		return partner;
		//return (ObjectHelper.InstanceOrNullWhenDefault(GbaPartner), partnerHelperModel);
	}

	// This has to do with incorrect inserts in the database logic. Below is the rule of Haal Centraal written in an partner-gba.feature.
	// Een partner wordt alleen teruggegeven als minimaal één gegeven in de identificatienummers (groep 01), naam (groep 02), geboorte (groep 03), aangaan (groep 06), ontbinding (groep 07) of 15 (soort verbintenis) van de partner een waarde heeft.
	private bool IsPartnerNonExistent(GbaPartner partner)
	{
		return string.IsNullOrWhiteSpace(partner.Burgerservicenummer) && partner.Naam == null
			&& partner.Geboorte == null && partner.Geslacht == null
			&& partner.AangaanHuwelijkPartnerschap == null
			&& partner.OntbindingHuwelijkPartnerschap == null
			&& partner.SoortVerbintenis == null;
	}

	protected async Task<GbaAangaanHuwelijkPartnerschap?> MapAangaanHuwelijkPartnerschap(lo3_pl_persoon dbPartner)
	{
		var aangaanHuwelijkPartnerschap = new GbaAangaanHuwelijkPartnerschap();

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaAangaanHuwelijkPartnerschap>())
		{
			switch (propertyName)
			{
				case nameof(GbaAangaanHuwelijkPartnerschap.Datum):
					aangaanHuwelijkPartnerschap.Datum = GbaMappingHelper.ParseToDatumOnvolledig(dbPartner.relatie_start_datum);
					break;
				case nameof(GbaAangaanHuwelijkPartnerschap.Plaats):
					aangaanHuwelijkPartnerschap.Plaats = await MapPlaats(dbPartner.relatie_start_land_code, dbPartner.relatie_start_plaats, dbPartner.relatie_start_plaats_naam);
					break;
				case nameof(GbaAangaanHuwelijkPartnerschap.Land):
					if (dbPartner.relatie_start_land_code.HasValue)
					{
						aangaanHuwelijkPartnerschap.Land = new Waardetabel
						{
							Code = dbPartner.relatie_start_land_code?.ToString().PadLeft(4, '0'),
							Omschrijving = dbPartner.relatie_start_land_naam ?? await _domeinTabellenHelper.GetLandOmschrijving(dbPartner.relatie_start_land_code)
						};
					}
					break;
				// These are auto mapped in Datum.
				case nameof(GbaAangaanHuwelijkPartnerschap.DatumJaar):
				case nameof(GbaAangaanHuwelijkPartnerschap.DatumMaand):
				case nameof(GbaAangaanHuwelijkPartnerschap.DatumDag):
					break;

				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaAangaanHuwelijkPartnerschap)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(aangaanHuwelijkPartnerschap);
	}

	protected GbaOntbindingHuwelijkPartnerschap? MapOntbindingHuwelijkPartnerschap(lo3_pl_persoon dbPartner)
	{
		var ontbindingHuwelijkPartnerschap = new GbaOntbindingHuwelijkPartnerschap();

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaOntbindingHuwelijkPartnerschap>())
		{
			switch (propertyName)
			{
				case nameof(GbaOntbindingHuwelijkPartnerschap.Datum):
					ontbindingHuwelijkPartnerschap.Datum = GbaMappingHelper.ParseToDatumOnvolledig(dbPartner.relatie_eind_datum);
					break;
				// These are auto mapped in Datum.
				case nameof(GbaAangaanHuwelijkPartnerschap.DatumJaar):
				case nameof(GbaAangaanHuwelijkPartnerschap.DatumMaand):
				case nameof(GbaAangaanHuwelijkPartnerschap.DatumDag):
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaOntbindingHuwelijkPartnerschap)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(ontbindingHuwelijkPartnerschap);
	}

	protected async Task<IEnumerable<T>?> MapNationaliteiten<T>(IEnumerable<lo3_pl_nationaliteit> nationaliteiten, bool mapHistory = false) where T : GbaNationaliteit
	{
		if (nationaliteiten == null)
		{
			return Enumerable.Empty<T>();
		}
		return (await Task.WhenAll(nationaliteiten
			.Where(x => x.onjuist_ind == null)
			.OrderBy(x => x.stapel_nr)
			.GroupBy(x => x.stapel_nr)
			.Select(async x =>
			{
				var current = x.FirstOrDefault(x => x.volg_nr == 0);

				if (current == null || ((current.nationaliteit_code == null && current.bijzonder_nl_aand == null) || current.nl_nat_verlies_reden != null) && !mapHistory)
				{
					return null;
				}

				if (string.IsNullOrWhiteSpace(current.bijzonder_nl_aand))
				{
					current.nationaliteit_code = current.nationaliteit_code != null ? current.nationaliteit_code : x.FirstOrDefault(y => y.nationaliteit_code != null)?.nationaliteit_code;
					current.nationaliteit_oms = current.nationaliteit_oms != null ? current.nationaliteit_oms : x.FirstOrDefault(y => y.nationaliteit_oms != null)?.nationaliteit_oms;

					var earliestDatumGeldigheid = x.Where(x => x.geldigheid_start_datum != 0).MinBy(x => x.geldigheid_start_datum ?? int.MaxValue)?.geldigheid_start_datum;
					return await MapNationaliteit<T>(current, earliestDatumGeldigheid ?? current.geldigheid_start_datum);
				}

				return await MapNationaliteit<T>(current, current.geldigheid_start_datum);
			}))).Where(x => x != null).Cast<T>();
	}

	protected async Task<T?> MapNationaliteit<T>(lo3_pl_nationaliteit dbNationaliteit, int? ingangsdatum) where T : GbaNationaliteit
	{
		var nationaliteit = Activator.CreateInstance<T>();
		var redenOpname = dbNationaliteit.nl_nat_verkrijg_reden_oms ?? await _domeinTabellenHelper.GetRedenOpnemenBeeindigenNationaliteitOmschrijving(dbNationaliteit.nl_nat_verkrijg_reden);
		var redenVerlies = dbNationaliteit.nl_nat_verlies_reden_oms ?? await _domeinTabellenHelper.GetRedenOpnemenBeeindigenNationaliteitOmschrijving(dbNationaliteit.nl_nat_verlies_reden);

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaNationaliteit>())
		{
			switch (propertyName)
			{
				case nameof(GbaNationaliteit.AanduidingBijzonderNederlanderschap):
					nationaliteit.AanduidingBijzonderNederlanderschap = dbNationaliteit.bijzonder_nl_aand;
					break;
				case nameof(GbaNationaliteit.Nationaliteit):
					nationaliteit.Nationaliteit = dbNationaliteit.nationaliteit_code.HasValue ? new Waardetabel
					{
						Code = dbNationaliteit.nationaliteit_code?.ToString().PadLeft(4, '0'),
						Omschrijving = dbNationaliteit.nationaliteit_oms
					} : null;
					break;
				case nameof(GbaNationaliteit.RedenOpname):
					nationaliteit.RedenOpname = dbNationaliteit.nl_nat_verkrijg_reden.HasValue ? new Waardetabel
					{
						Code = dbNationaliteit.nl_nat_verkrijg_reden.ToString()?.PadLeft(3, '0'),
						Omschrijving = redenOpname
					} : null;

					if (nationaliteit.RedenOpname == null)
					{
						nationaliteit.RedenOpname = dbNationaliteit.nl_nat_verlies_reden.HasValue ? new Waardetabel
						{
							Code = dbNationaliteit.nl_nat_verlies_reden.ToString()?.PadLeft(3, '0'),
							Omschrijving = redenVerlies
						} : null;
					}
					break;
				case nameof(GbaNationaliteit.DatumIngangGeldigheid):
					nationaliteit.DatumIngangGeldigheid = GbaMappingHelper.ParseToDatumOnvolledig(ingangsdatum);
					break;
				case nameof(GbaNationaliteit.InOnderzoek):
					nationaliteit.InOnderzoek = MapGbaInOnderzoek(dbNationaliteit.onderzoek_gegevens_aand, dbNationaliteit.onderzoek_start_datum, dbNationaliteit.onderzoek_eind_datum);
					break;
				case nameof(GbaNationaliteit._datumOpneming):
					nationaliteit._datumOpneming = GbaMappingHelper.ParseToDatumOnvolledig(dbNationaliteit.opneming_datum);
					break;
				case nameof(GbaNationaliteitHistorie.RedenBeeindigen):
					if (nationaliteit is not GbaNationaliteitHistorie historieNationaliteit)
					{
						break;
					}
					historieNationaliteit.RedenBeeindigen = dbNationaliteit.nl_nat_verlies_reden.HasValue ? new Waardetabel
					{
						Code = dbNationaliteit.nl_nat_verlies_reden.ToString()?.PadLeft(3, '0'),
						Omschrijving = redenVerlies
					} : null;
					break;

				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaNationaliteit)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(nationaliteit);
	}

	protected async Task<GbaVerblijfstitel?> MapVerblijfstitel(lo3_pl_verblijfstitel dbVerblijfstitel)
	{
		var verblijfstitel = new GbaVerblijfstitel();
		if (dbVerblijfstitel == null)
		{
			return null;
		}

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaVerblijfstitel>())
		{
			switch (propertyName)
			{
				case nameof(GbaVerblijfstitel.Aanduiding):
					if (dbVerblijfstitel.verblijfstitel_aand.HasValue)
					{
						verblijfstitel.Aanduiding = new Waardetabel {
							Code = dbVerblijfstitel.verblijfstitel_aand?.ToString()?.PadLeft(2, '0'),
							Omschrijving = dbVerblijfstitel.verblijfstitel_aand_oms ?? await _domeinTabellenHelper.GetVerblijfstitelOmschrijving(dbVerblijfstitel.verblijfstitel_aand)
						};
					}
					break;
				case nameof(GbaVerblijfstitel.DatumIngang):
					verblijfstitel.DatumIngang = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfstitel.verblijfstitel_start_datum);
					break;
				case nameof(GbaVerblijfstitel.DatumEinde):
					verblijfstitel.DatumEinde = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfstitel.verblijfstitel_eind_datum);
					break;
				case nameof(GbaVerblijfstitel.InOnderzoek):
					verblijfstitel.InOnderzoek = MapGbaInOnderzoek(dbVerblijfstitel.onderzoek_gegevens_aand, dbVerblijfstitel.onderzoek_start_datum, dbVerblijfstitel.onderzoek_eind_datum);
					break;
				case nameof(GbaVerblijfstitel._datumOpneming):
					verblijfstitel._datumOpneming = GbaMappingHelper.ParseToDatumOnvolledig(dbVerblijfstitel.opneming_datum);
					break;

				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaVerblijfstitel)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(verblijfstitel);
	}

	protected GbaOpschortingBijhouding? MapOpschortingBijhouding(lo3_pl inschrijving)
	{
		var opschortingBijhouding = new GbaOpschortingBijhouding();
		if (inschrijving == null)
		{
			return null;
		}

		MapOpschortingBijhoudingBasis(inschrijving, opschortingBijhouding);

		foreach (var propertyName in ObjectHelper.GetPropertyNames<GbaOpschortingBijhouding>())
		{
			switch (propertyName)
			{
				// Set in MapOpschortingBijhoudingBasis
				case nameof(GbaOpschortingBijhouding.Reden):
					break;
				case nameof(GbaOpschortingBijhouding.Datum):
					opschortingBijhouding.Datum = GbaMappingHelper.ParseToDatumOnvolledig(inschrijving.bijhouding_opschort_datum);
					break;
				default:
					throw new CustomNotImplementedException($"Mapping not implemented for {nameof(GbaOpschortingBijhouding)} property {propertyName}");
			}
		}

		return ObjectHelper.InstanceOrNullWhenDefault(opschortingBijhouding);
	}

	private void MapOpschortingBijhoudingBasis(lo3_pl inschrijving, OpschortingBijhoudingBasis opschortingBijhouding)
	{
		foreach (var propertyName in ObjectHelper.GetPropertyNames<OpschortingBijhoudingBasis>())
		{
			opschortingBijhouding.Reden = propertyName switch
			{
				nameof(OpschortingBijhoudingBasis.Reden) => GbaMappingHelper.ParseToRedenOpschortingBijhoudingEnum(inschrijving.bijhouding_opschort_reden),
				_ => throw new CustomNotImplementedException($"Mapping not implemented for {nameof(OpschortingBijhoudingBasis)} property {propertyName}"),
			};
		}
	}
}