using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.Universal
{
    /// <summary>
    /// * **datum**: de datum waarop de bijhouding van de persoonsgegevens is gestaakt.
    /// </summary>
    [DataContract]
    public class GbaOpschortingBijhouding : OpschortingBijhoudingBasis
    {
        /// <summary>
        /// Gets or Sets Datum
        /// </summary>
        [RegularExpression("^[0-9]{8}$")]
        [DataMember(Name = "datum", EmitDefaultValue = false)]
        public string? Datum { get; set; }
    }
}
