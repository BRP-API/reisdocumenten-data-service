using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.Universal
{
    [DataContract]
    public class Waardetabel
    {
        /// <summary>
        /// Gets or Sets Code
        /// </summary>
        [RegularExpression("^[a-zA-Z0-9 \\.]+$")]
        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string? Code { get; set; }

        /// <summary>
        /// Gets or Sets Omschrijving
        /// </summary>
        [RegularExpression("^[a-zA-Z0-9À-ž \\'\\,\\(\\)\\.\\-]{1,200}$")]
        [DataMember(Name = "omschrijving", EmitDefaultValue = false)]
        public string? Omschrijving { get; set; }
    }
}
