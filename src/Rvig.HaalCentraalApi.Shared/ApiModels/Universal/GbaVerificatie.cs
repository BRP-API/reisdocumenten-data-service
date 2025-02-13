using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.ApiModels.Universal
{
	[DataContract]
	public class GbaVerificatie
	{
		/// <summary>
		/// Gets or Sets Datum
		/// </summary>
		[RegularExpression("^[0-9]{8}$")]
		[DataMember(Name = "datum", EmitDefaultValue = false)]
		public string? Datum { get; set; }

		/// <summary>
		/// Omschrijving van de verificatie van de rni-gegevens
		/// </summary>
		[RegularExpression("^[a-zA-Z0-9À-ž \\.\\-\\']{1,50}$")]
		[DataMember(Name = "omschrijving", EmitDefaultValue = false)]
		public string? Omschrijving { get; set; }
	}
}
