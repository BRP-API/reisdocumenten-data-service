using System.Runtime.Serialization;
using Newtonsoft.Json;
using NJsonSchema.Converters;
using Rvig.BrpApi.Reisdocumenten.Util;

namespace Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten
{
    [DataContract]
    [JsonConverter(typeof(ReisdocumentenQueryJsonInheritanceConverter), "type")]
    [JsonInheritance("RaadpleegMetReisdocumentnummer", typeof(RaadpleegMetReisdocumentnummer))]
    [JsonInheritance("ZoekMetBurgerservicenummer", typeof(ZoekMetBurgerservicenummer))]
    public class ReisdocumentenQuery
    {
        /// <summary>
        /// Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public string? type { get; set; }

        /// <summary>
        /// Gets or Sets Fields
        /// </summary>
        [DataMember(Name = "fields", EmitDefaultValue = false)]
        public List<string> fields { get; set; } = new List<string>();
    }
}
