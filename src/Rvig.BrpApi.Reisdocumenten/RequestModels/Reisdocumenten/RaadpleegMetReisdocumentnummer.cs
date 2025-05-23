using System.Runtime.Serialization;

namespace Rvig.BrpApi.Reisdocumenten.RequestModels.Reisdocumenten
{
    [DataContract]
    public class RaadpleegMetReisdocumentnummer : ReisdocumentenQuery
    {
        /// <summary>
        /// Gets or Sets Reisdocumentnummer
        /// </summary>
        [DataMember(Name = "reisdocumentnummer", EmitDefaultValue = false)]
        public List<string>? reisdocumentnummer { get; set; }
    }
}
