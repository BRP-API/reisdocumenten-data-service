using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.PersonenHistorieBase
{
    [DataContract]
    public class AdellijkeTitelPredicaatType : Waardetabel
    {
        /// <summary>
        /// Gets or Sets Soort
        /// </summary>
        [DataMember(Name = "soort", EmitDefaultValue = false)]
        public AdellijkeTitelPredicaatSoort? Soort { get; set; }
    }
}
