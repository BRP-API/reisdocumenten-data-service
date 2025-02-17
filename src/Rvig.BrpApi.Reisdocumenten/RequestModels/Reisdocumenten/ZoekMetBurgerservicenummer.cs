using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten
{
    [DataContract]
    public class ZoekMetBurgerservicenummer : ReisdocumentenQuery
    {
        /// <summary>
        /// Gets or Sets Burgerservicenummer
        /// </summary>
        [DataMember(Name = "burgerservicenummer", EmitDefaultValue = false)]
        public string? burgerservicenummer { get; set; }
    }
}
