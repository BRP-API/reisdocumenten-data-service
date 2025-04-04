using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Reisdocumenten.ApiModels.Reisdocumenten
{
    [DataContract]
    public class ReisdocumenthouderBasis
    {
        /// <summary>
        /// Gets or Sets Burgerservicenummer
        /// </summary>
        [RegularExpression("^[0-9]{9}$")]
        [DataMember(Name = "burgerservicenummer", EmitDefaultValue = false)]
        public string? Burgerservicenummer { get; set; }

        /// <summary>
        /// Gets or Sets OpschortingBijhouding
        /// </summary>
        [DataMember(Name = "opschortingBijhouding", EmitDefaultValue = false)]
        public GbaOpschortingBijhouding? OpschortingBijhouding { get; set; }
    }
}
