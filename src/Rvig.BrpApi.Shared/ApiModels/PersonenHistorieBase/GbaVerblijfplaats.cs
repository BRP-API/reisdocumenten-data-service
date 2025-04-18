using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.PersonenHistorieBase
{
    [DataContract]
    public class GbaVerblijfplaats
    {
        /// <summary>
        /// Het eerste deel van een buitenlands adres. Vaak is dit een combinatie van de straat en huisnummer.
        /// </summary>
        /// <value>Het eerste deel van een buitenlands adres. Vaak is dit een combinatie van de straat en huisnummer. </value>
        [MaxLength(40)]
        [DataMember(Name = "regel1", EmitDefaultValue = false)]
        public string? Regel1 { get; set; }

        /// <summary>
        /// Het tweede deel van een buitenlands adres. Vaak is dit een combinatie van woonplaats eventueel in combinatie met de postcode.
        /// </summary>
        /// <value>Het tweede deel van een buitenlands adres. Vaak is dit een combinatie van woonplaats eventueel in combinatie met de postcode. </value>
        [MaxLength(50)]
        [DataMember(Name = "regel2", EmitDefaultValue = false)]
        public string? Regel2 { get; set; }

        /// <summary>
        /// Het derde deel van een buitenlands adres is optioneel. Het gaat om een of meer geografische gebieden van het adres in het buitenland.
        /// </summary>
        /// <value>Het derde deel van een buitenlands adres is optioneel. Het gaat om een of meer geografische gebieden van het adres in het buitenland. </value>
        [MaxLength(35)]
        [DataMember(Name = "regel3", EmitDefaultValue = false)]
        public string? Regel3 { get; set; }

        /// <summary>
        /// De verblijfplaats van de persoon kan een ligplaats, een standplaats of een verblijfsobject zijn.
        /// </summary>
        /// <value>De verblijfplaats van de persoon kan een ligplaats, een standplaats of een verblijfsobject zijn. </value>
        [RegularExpression("^[0-9]{16}$")]
        [DataMember(Name = "adresseerbaarObjectIdentificatie", EmitDefaultValue = false)]
        public string? AdresseerbaarObjectIdentificatie { get; set; }

        /// <summary>
        /// Unieke identificatie van een nummeraanduiding (en het bijbehorende adres) in de BAG.
        /// </summary>
        /// <value>Unieke identificatie van een nummeraanduiding (en het bijbehorende adres) in de BAG. </value>
        [RegularExpression("^[0-9]{16}$")]
        [DataMember(Name = "nummeraanduidingIdentificatie", EmitDefaultValue = false)]
        public string? NummeraanduidingIdentificatie { get; set; }

        /// <summary>
        /// Gets or Sets FunctieAdres
        /// </summary>
        [DataMember(Name = "functieAdres", EmitDefaultValue = false)]
        public Waardetabel? FunctieAdres { get; set; }

        /// <summary>
        /// Een woonplaats is een gedeelte van het grondgebied van de gemeente met een naam.
        /// </summary>
        /// <value>Een woonplaats is een gedeelte van het grondgebied van de gemeente met een naam. </value>
        [RegularExpression(@"^[a-zA-Z0-9À-ž \(\)\,\.\-\']{1,80}$")]
        [DataMember(Name = "woonplaats", EmitDefaultValue = false)]
        public string? Woonplaats { get; set; }

        /// <summary>
        /// Gets or Sets Straat
        /// </summary>
        [MaxLength(80)]
        [DataMember(Name = "straat", EmitDefaultValue = false)]
        public string? Straat { get; set; }

        /// <summary>
        /// Een nummer dat door de gemeente aan een adresseerbaar object is gegeven.
        /// </summary>
        /// <value>Een nummer dat door de gemeente aan een adresseerbaar object is gegeven. </value>
        [Range(1, 99999)]
        [DataMember(Name = "huisnummer", EmitDefaultValue = false)]
        public int? Huisnummer { get; set; }

        /// <summary>
        /// Een toevoeging aan een huisnummer in de vorm van een letter die door de gemeente aan een adresseerbaar object is gegeven.
        /// </summary>
        /// <value>Een toevoeging aan een huisnummer in de vorm van een letter die door de gemeente aan een adresseerbaar object is gegeven. </value>
        [RegularExpression("^[a-zA-Z]{1}$")]
        [DataMember(Name = "huisletter", EmitDefaultValue = false)]
        public string? Huisletter { get; set; }

        /// <summary>
        /// Een toevoeging aan een huisnummer of een combinatie van huisnummer en huisletter die door de gemeente aan een adresseerbaar object is gegeven.
        /// </summary>
        /// <value>Een toevoeging aan een huisnummer of een combinatie van huisnummer en huisletter die door de gemeente aan een adresseerbaar object is gegeven. </value>
        [RegularExpression(@"^[a-zA-Z0-9 \-]{1,4}$")]
        [DataMember(Name = "huisnummertoevoeging", EmitDefaultValue = false)]
        public string? Huisnummertoevoeging { get; set; }

        /// <summary>
        /// Gets or Sets AanduidingBijHuisnummer
        /// </summary>
        [DataMember(Name = "aanduidingBijHuisnummer", EmitDefaultValue = false)]
        public Waardetabel? AanduidingBijHuisnummer { get; set; }

        /// <summary>
        /// De door PostNL vastgestelde code die bij een bepaalde combinatie van een straatnaam en een huisnummer hoort.
        /// </summary>
        /// <value>De door PostNL vastgestelde code die bij een bepaalde combinatie van een straatnaam en een huisnummer hoort. </value>
        [RegularExpression("^[1-9]{1}[0-9]{3}[ ]?[A-Za-z]{2}$")]
        [DataMember(Name = "postcode", EmitDefaultValue = false)]
        public string? Postcode { get; set; }

        /// <summary>
        /// Omschrijving van de ligging van een verblijfsobject, standplaats of ligplaats.
        /// </summary>
        /// <value>Omschrijving van de ligging van een verblijfsobject, standplaats of ligplaats. </value>
        [MaxLength(35)]
        [DataMember(Name = "locatiebeschrijving", EmitDefaultValue = false)]
        public string? Locatiebeschrijving { get; set; }

        /// <summary>
        /// Gets or Sets Land
        /// </summary>
        [DataMember(Name = "land", EmitDefaultValue = false)]
        public Waardetabel? Land { get; set; }

        /// <summary>
        /// Gets or Sets DatumIngangGeldigheid
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumIngangGeldigheid", EmitDefaultValue = false)]
        public string? DatumIngangGeldigheid { get; set; }

        /// <summary>
        /// Gets or Sets DatumAanvangAdreshouding
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumAanvangAdreshouding", EmitDefaultValue = false)]
        public string? DatumAanvangAdreshouding { get; set; }

        /// <summary>
        /// Gets or Sets DatumAanvangAdresBuitenland
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumAanvangAdresBuitenland", EmitDefaultValue = false)]
        public string? DatumAanvangAdresBuitenland { get; set; }

        /// <summary>
        /// Gets or Sets NaamOpenbareRuimte
        /// </summary>
        [MaxLength(80)]
        [DataMember(Name = "naamOpenbareRuimte", EmitDefaultValue = false)]
        public string? NaamOpenbareRuimte { get; set; }

        /// <summary>
        /// Gets or Sets InOnderzoek
        /// </summary>
        [DataMember(Name = "inOnderzoek", EmitDefaultValue = false)]
        public GbaInOnderzoek? InOnderzoek { get; set; }
    }
}
