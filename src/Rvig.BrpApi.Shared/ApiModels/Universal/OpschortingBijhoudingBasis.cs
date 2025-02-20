using System.Runtime.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.Universal
{
    /// <summary>
    /// * **reden** - wordt gevuld op basis van de waarden die voorkomen in de tabel &#39;redenopschortingbijhouding&#39; uit de Haal-Centraal-BRP-tabellen-bevragen API.
    /// </summary>
    [DataContract]
    public class OpschortingBijhoudingBasis
    {
        /// <summary>
        /// Gets or Sets Reden
        /// </summary>
        [DataMember(Name = "reden", EmitDefaultValue = false)]
        public Waardetabel? Reden { get; set; }
    }
}
