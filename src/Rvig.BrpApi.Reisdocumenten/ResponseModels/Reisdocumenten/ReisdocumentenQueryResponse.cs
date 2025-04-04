using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using NJsonSchema.Converters;

namespace Rvig.BrpApi.Reisdocumenten.ResponseModels.Reisdocumenten
{
    [DataContract]
    [JsonConverter(typeof(JsonInheritanceConverter), "type")]
    [JsonInheritance("RaadpleegMetReisdocumentnummer", typeof(RaadpleegMetReisdocumentnummerResponse))]
    [JsonInheritance("ZoekMetBurgerservicenummer", typeof(ZoekMetBurgerservicenummerResponse))]
    public class ReisdocumentenQueryResponse
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [Required]
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string? Type { get; set; }
    }
}
