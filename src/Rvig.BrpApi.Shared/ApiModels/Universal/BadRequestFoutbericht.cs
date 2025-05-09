using System.Runtime.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.Universal
{
    [DataContract]
    public class BadRequestFoutbericht : Foutbericht
    {
        /// <summary>
        /// Foutmelding per fout in een parameter. Alle gevonden fouten worden Ã©Ã©n keer teruggemeld.
        /// </summary>
        /// <value>Foutmelding per fout in een parameter. Alle gevonden fouten worden Ã©Ã©n keer teruggemeld.</value>
        [DataMember(Name = "invalidParams", EmitDefaultValue = false)]
        public List<InvalidParams>? InvalidParams { get; set; }
    }
}
