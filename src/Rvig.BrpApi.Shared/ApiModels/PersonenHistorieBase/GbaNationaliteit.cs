using Newtonsoft.Json;
using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.PersonenHistorieBase
{
    [DataContract]
    public class GbaNationaliteit
    {
        /// <summary>
        /// Gets or Sets AanduidingBijzonderNederlanderschap
        /// </summary>
        [RegularExpression("^(B|V)$")]
        [DataMember(Name = "aanduidingBijzonderNederlanderschap", EmitDefaultValue = false)]
        public string? AanduidingBijzonderNederlanderschap { get; set; }

        /// <summary>
        /// Gets or Sets DatumIngangGeldigheid
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumIngangGeldigheid", EmitDefaultValue = false)]
        public string? DatumIngangGeldigheid { get; set; }

        /// <summary>
        /// Gets or Sets Nationaliteit
        /// </summary>
        [DataMember(Name = "nationaliteit", EmitDefaultValue = false)]
        public Waardetabel? Nationaliteit { get; set; }

        /// <summary>
        /// Gets or Sets RedenOpname
        /// </summary>
        [DataMember(Name = "redenOpname", EmitDefaultValue = false)]
        public Waardetabel? RedenOpname { get; set; }

        /// <summary>
        /// Gets or Sets InOnderzoek
        /// </summary>
        [DataMember(Name = "inOnderzoek", EmitDefaultValue = false)]
        public GbaInOnderzoek? InOnderzoek { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        public string? _datumOpneming { get; set; }
    }
}
