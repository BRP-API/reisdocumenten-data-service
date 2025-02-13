using Rvig.BrpApi.Shared.Fields;

namespace Rvig.BrpApi.Reisdocumenten.Fields;
public class ReisdocumentenFieldsSettings : FieldsSettings
{
    public override FieldsSettingsModel GbaFieldsSettings { get; }

    public ReisdocumentenFieldsSettings()
    {
        GbaFieldsSettings = InitGbaFieldsSettings();
    }

    protected override FieldsSettingsModel InitGbaFieldsSettings()
    {
        return new FieldsSettingsModel("fields")
        {
            ForbiddenProperties = new List<string>()
            {
                "inOnderzoek", "houder.inOnderzoek", "houder.opschortingBijhouding", "houder.geheimhoudingPersoonsgegevens"
            },
            PropertiesToDiscard = new List<string>(),
            MandatoryProperties = new List<string>
            {
                "inOnderzoek", "houder.inOnderzoek", "houder.opschortingBijhouding", "houder.geheimhoudingPersoonsgegevens"
            },
            SetChildPropertiesIfExistInScope = new Dictionary<string, string>
            {
                { "houder.inOnderzoek", "houder" },
                { "houder.opschortingBijhouding", "houder" }
            },
            SetPropertiesIfContextPropertyNotNull = new Dictionary<string, string>
            {
                { "inOnderzoek", "" }, { "inOnderzoek.datumIngangOnderzoek", "inOnderzoek" },
                { "houder.inOnderzoek", "houder" }, { "houder.inOnderzoek.datumIngangOnderzoek", "houder.inOnderzoek" },
                { "houder.opschortingBijhouding", "houder" }
            },
            ShortHandMappings = new Dictionary<string, string>
            {
				// DATE FIELDS
				{ "datumEindeGeldigheid.type", "datumEindeGeldigheid" },
                { "datumEindeGeldigheid.langFormaat", "datumEindeGeldigheid" },
                { "datumEindeGeldigheid.datum", "datumEindeGeldigheid" },
                { "datumEindeGeldigheid.jaar", "datumEindeGeldigheid" },
                { "datumEindeGeldigheid.maand", "datumEindeGeldigheid" },
                { "datumEindeGeldigheid.onbekend", "datumEindeGeldigheid" },

                { "inhoudingOfVermissing.datum.type", "inhoudingOfVermissing.datum" },
                { "inhoudingOfVermissing.datum.langFormaat", "inhoudingOfVermissing.datum" },
                { "inhoudingOfVermissing.datum.datum", "inhoudingOfVermissing.datum" },
                { "inhoudingOfVermissing.datum.jaar", "inhoudingOfVermissing.datum" },
                { "inhoudingOfVermissing.datum.maand", "inhoudingOfVermissing.datum" },
                { "inhoudingOfVermissing.datum.onbekend", "inhoudingOfVermissing.datum" },

                { "houder.opschortingBijhouding.datum.type", "houder.opschortingBijhouding.datum" },
                { "houder.opschortingBijhouding.datum.langFormaat", "houder.opschortingBijhouding.datum" },
                { "houder.opschortingBijhouding.datum.datum", "houder.opschortingBijhouding.datum" },
                { "houder.opschortingBijhouding.datum.jaar", "houder.opschortingBijhouding.datum" },
                { "houder.opschortingBijhouding.datum.maand", "houder.opschortingBijhouding.datum" },
                { "houder.opschortingBijhouding.datum.onbekend", "houder.opschortingBijhouding.datum" },

				// WAARDE TABELLEN
				{ "soort.code", "soort" },
                { "soort.omschrijving", "soort" },

                { "inhoudingOfVermissing.aanduiding.code", "inhoudingOfVermissing.aanduiding" },
                { "inhoudingOfVermissing.aanduiding.omschrijving", "inhoudingOfVermissing.aanduiding" },

                { "houder.opschortingBijhouding.reden.code", "houder.opschortingBijhouding.reden" },
                { "houder.opschortingBijhouding.reden.omschrijving", "ihouder.opschortingBijhouding.reden" },
            }
        };
    }
}