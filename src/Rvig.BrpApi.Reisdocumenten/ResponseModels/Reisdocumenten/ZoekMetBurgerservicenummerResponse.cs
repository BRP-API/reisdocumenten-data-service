using System.Runtime.Serialization;
using Rvig.BrpApi.Reisdocumenten.ApiModels.Reisdocumenten;

namespace Rvig.BrpApi.Reisdocumenten.ResponseModels.Reisdocumenten
{
    [DataContract]
    public class ZoekMetBurgerservicenummerResponse : ReisdocumentenQueryResponse
    {
        /// <summary>
        /// Gets or Sets Reisdocumenten
        /// </summary>
        [DataMember(Name = "reisdocumenten", EmitDefaultValue = false)]
        public List<GbaReisdocument>? Reisdocumenten { get; set; }
    }
}
