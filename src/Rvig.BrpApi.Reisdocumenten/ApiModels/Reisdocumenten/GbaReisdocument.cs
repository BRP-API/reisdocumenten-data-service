using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Reisdocumenten.ApiModels.Reisdocumenten
{
    [DataContract]
    public class GbaReisdocument
    {
        /// <summary>
        /// Gets or Sets Reisdocumentnummer
        /// </summary>
        [RegularExpression("^[A-Z0-9]{9}$")]
        [DataMember(Name = "reisdocumentnummer", EmitDefaultValue = false)]
        public string? Reisdocumentnummer { get; set; }

        /// <summary>
        /// Gets or Sets Soort
        /// </summary>
        [DataMember(Name = "soort", EmitDefaultValue = false)]
        public Waardetabel? Soort { get; set; }

        /// <summary>
        /// Gets or Sets DatumEindeGeldigheid
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datumEindeGeldigheid", EmitDefaultValue = false)]
        public string? DatumEindeGeldigheid { get; set; }

        /// <summary>
        /// Gets or Sets InhoudingOfVermissing
        /// </summary>
        [DataMember(Name = "inhoudingOfVermissing", EmitDefaultValue = false)]
        public GbaInhoudingOfVermissing? InhoudingOfVermissing { get; set; }

        /// <summary>
        /// Gets or Sets Houder
        /// </summary>
        [DataMember(Name = "houder", EmitDefaultValue = false)]
        public GbaReisdocumenthouder? Houder { get; set; }

        /// <summary>
        /// Gets or Sets InOnderzoek
        /// </summary>
        [DataMember(Name = "inOnderzoek", EmitDefaultValue = false)]
        public GbaInOnderzoek? InOnderzoek { get; set; }
    }
}
