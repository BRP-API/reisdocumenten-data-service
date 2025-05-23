using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Reisdocumenten.ApiModels.Reisdocumenten
{
	[DataContract]
    public class GbaInhoudingOfVermissing
    {
        /// <summary>
        /// Gets or Sets Datum
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name="datum", EmitDefaultValue=false)]
        public string? Datum { get; set; }

        /// <summary>
        /// Gets or Sets Aanduiding
        /// </summary>
        [DataMember(Name="aanduiding", EmitDefaultValue=false)]
        public Waardetabel? Aanduiding { get; set; }
    }
}
