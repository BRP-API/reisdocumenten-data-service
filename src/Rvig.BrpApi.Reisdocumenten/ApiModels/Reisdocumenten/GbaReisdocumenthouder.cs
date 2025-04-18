using Newtonsoft.Json;
using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Rvig.BrpApi.Reisdocumenten.ApiModels.Reisdocumenten
{
    [DataContract]
    public class GbaReisdocumenthouder : ReisdocumenthouderBasis
    {
        /// <summary>
        /// Gets or Sets GeheimhoudingPersoonsgegevens
        /// </summary>
        [DataMember(Name = "geheimhoudingPersoonsgegevens", EmitDefaultValue = false)]
        public int? GeheimhoudingPersoonsgegevens { get; set; }

        /// <summary>
        /// Gets or Sets InOnderzoek
        /// </summary>
        [DataMember(Name = "inOnderzoek", EmitDefaultValue = false)]
        public GbaInOnderzoek? InOnderzoek { get; set; }
    }
}
