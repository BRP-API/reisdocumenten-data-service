using Newtonsoft.Json;
using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.PersonenHistorieBase
{
    /// <summary>
    /// Gegevens over de voltrekking van het huwelijk of het aangaan van het geregistreerd partnerschap. * **datum** - De datum waarop het huwelijk is voltrokken of het partnerschap is aangegaan.  * **land** - Het land waar het huwelijk is voltrokken of het partnerschap is aangegaan. Wordt gevuld op basis van de waarden die voorkomen in de tabel &#39;Landen&#39; uit de Haal-Centraal-BRP-tabellen-bevragen API. * **plaats** - De gemeente waar het huwelijk is voltrokken of het partnerschap is aangegaan. Wordt gevuld op basis van de waarden die voorkomen in de tabel \&quot;Gemeenten\&quot; voor een gemeente in Nederland of de omschrijving van een buitenlandse plaats.
    /// </summary>
    [DataContract]
    public class GbaAangaanHuwelijkPartnerschap
    {
        /// <summary>
        /// Gets or Sets Datum
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datum", EmitDefaultValue = false)]
        public string? Datum { get; set; }

        /// <summary>
        /// Gets or Sets Land
        /// </summary>
        [DataMember(Name = "land", EmitDefaultValue = false)]
        public Waardetabel? Land { get; set; }

        /// <summary>
        /// Gets or Sets Plaats
        /// </summary>
        [DataMember(Name = "plaats", EmitDefaultValue = false)]
        public Waardetabel? Plaats { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public int? DatumJaar => !string.IsNullOrWhiteSpace(Datum) && !Datum.Equals("00000000") && Datum.Length >= 4 ? int.Parse(Datum.Substring(0, 4)) : null;

        [JsonIgnore]
        [XmlIgnore]
        public int? DatumMaand => !string.IsNullOrWhiteSpace(Datum) && !Datum.Equals("00000000") && Datum.Length >= 6 ? int.Parse(Datum.Substring(4, 2)) : null;

        [JsonIgnore]
        [XmlIgnore]
        public int? DatumDag => !string.IsNullOrWhiteSpace(Datum) && !Datum.Equals("00000000") && Datum.Length == 8 ? int.Parse(Datum.Substring(6, 2)) : null;
    }
}
