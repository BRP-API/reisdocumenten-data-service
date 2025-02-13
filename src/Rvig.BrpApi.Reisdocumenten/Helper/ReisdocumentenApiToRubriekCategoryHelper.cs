using Rvig.BrpApi.Shared.Exceptions;
using Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten;
using Rvig.BrpApi.Shared.Helpers;

namespace Rvig.BrpApi.Reisdocumenten.Helper;

public class ReisdocumentenApiToRubriekCategoryHelper : HaalCentraalToRubriekCategoryHelperBase<ReisdocumentenQuery>
{
    protected override IDictionary<string, string> _fieldRubriekCategoryDictionary => new Dictionary<string, string>
    {
		// Houder (GbaPersoon)
		{ "houder", "010120, 018310, 018320, 076710, 076720, 077010" },
        { "houder.burgerservicenummer", "010120" },
        { "houder.inOnderzoek", "018310, 018320" }, // By default geaccepteerd
		{ "houder.inOnderzoek.aanduidingGegevensInOnderzoek", "018310" }, // By default geaccepteerd
		{ "houder.inOnderzoek.datumIngangOnderzoek", "018320" }, // By default geaccepteerd
		{ "houder.opschortingBijhouding", "076710, 076720" },
        { "houder.opschortingBijhouding.datum", "076710" },
        { "houder.opschortingBijhouding.reden", "076720" },
        { "houder.opschortingBijhouding.reden.code", "076720" },
        { "houder.opschortingBijhouding.reden.omschrijving", "076720" },
        { "houder.geheimhoudingPersoonsgegevens", "077010" },

		// Gemeente van inschrijving
		{ "gemeenteVanInschrijving", "080910" },

		// Reisdocument
		{ "soort", "123510" },
        { "soort.code", "123510" },
        { "soort.omschrijving", "123510" },
        { "reisdocumentnummer", "123520" },
		//{ "", "123530" },
		//{ "", "123540" },
		{ "datumEindeGeldigheid", "123550" },
        { "inhoudingOfVermissing", "123560, 123570" },
        { "inhoudingOfVermissing.datum", "123560" },
        { "inhoudingOfVermissing.aanduiding", "123570" },
        { "inhoudingOfVermissing.aanduiding.code", "123570" },
        { "inhoudingOfVermissing.aanduiding.omschrijving", "123570" },
		//{ "", "128210" },
		//{ "", "128220" },
		//{ "", "128230" },
		{ "inOnderzoek", "128310, 128320" }, // By default geaccepteerd
		{ "inOnderzoek.aanduidingGegevensInOnderzoek", "128310" }, // By default geaccepteerd
		{ "inOnderzoek.datumIngangOnderzoek", "128320" }, // By default geaccepteerd
		//{ "inOnderzoek.datumEindeOnderzoek", "128330" },
		//{ "", "128510" },
		//{ "", "128610" },
	};

    public override List<string> ConvertModelParamsToRubrieken(ReisdocumentenQuery model)
    {
        return model switch
        {
            RaadpleegMetReisdocumentnummer _ => ConvertReisdocumentnummerModelParamsToRubrieken(),
            ZoekMetBurgerservicenummer _ => ConvertBurgerservicenummerModelParamsToRubrieken(),
            _ => throw new CustomInvalidOperationException($"Onbekend type query: {model}"),
        };
    }

    public List<string> ConvertReisdocumentnummerModelParamsToRubrieken()
    {
        var rubrieken = new List<string>();

        if (_fieldRubriekCategoryDictionary.ContainsKey(nameof(RaadpleegMetReisdocumentnummer.reisdocumentnummer)))
        {
            rubrieken.Add(_fieldRubriekCategoryDictionary[nameof(RaadpleegMetReisdocumentnummer.reisdocumentnummer)]);
        }

        return rubrieken;
    }

    public List<string> ConvertBurgerservicenummerModelParamsToRubrieken()
    {
        var rubrieken = new List<string>();

        if (_fieldRubriekCategoryDictionary.ContainsKey($"houder.{nameof(ZoekMetBurgerservicenummer.burgerservicenummer)}"))
        {
            rubrieken.Add(_fieldRubriekCategoryDictionary[$"houder.{nameof(ZoekMetBurgerservicenummer.burgerservicenummer)}"]);
        }

        return rubrieken;
    }
}
