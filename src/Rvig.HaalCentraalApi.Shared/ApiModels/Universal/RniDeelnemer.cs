using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.HaalCentraalApi.Shared.ApiModels.Universal
{
	[DataContract]
	public class RniDeelnemer
	{
		/// <summary>
		/// Gets or Sets Deelnemer
		/// </summary>
		[DataMember(Name = "deelnemer", EmitDefaultValue = false)]
		public Waardetabel? Deelnemer { get; set; }

		/// <summary>
		/// Omschrijving van het verdrag op basis waarvan een zusterorganisatie in het buitenland de gegevens bij de RNI-deelnemer heeft aangeleverd.
		/// </summary>
		[RegularExpression("^[a-zA-Z0-9À-ž \\.\\-\\']{1,50}$")]
		[DataMember(Name = "omschrijvingVerdrag", EmitDefaultValue = false)]
		public string? OmschrijvingVerdrag { get; set; }

		/// <summary>
		/// Naam van categorie waarop de RNI-deelnemer gegevens heeft aangeleverd
		/// </summary>
		[RegularExpression("^[a-zA-Z0-9À-ž \\.\\-\\'\\/]{1,40}$")]
		[DataMember(Name = "categorie", EmitDefaultValue = false)]
		public string? Categorie { get; set; }
	}
}
