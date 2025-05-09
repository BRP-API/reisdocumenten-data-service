using Rvig.BrpApi.Shared.ApiModels.Universal;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Rvig.BrpApi.Shared.ApiModels.PersonenHistorieBase
{
    [DataContract]
    public class GbaPartner
    {
        /// <summary>
        /// Gets or Sets Burgerservicenummer
        /// </summary>
        [RegularExpression("^[0-9]{9}$")]
        [DataMember(Name = "burgerservicenummer", EmitDefaultValue = false)]
        public string? Burgerservicenummer { get; set; }

        /// <summary>
        /// Gets or Sets Geslacht
        /// </summary>
        [DataMember(Name = "geslacht", EmitDefaultValue = false)]
        public Waardetabel? Geslacht { get; set; }

        /// <summary>
        /// Gets or Sets SoortVerbintenis
        /// </summary>
        [DataMember(Name = "soortVerbintenis", EmitDefaultValue = false)]
        public Waardetabel? SoortVerbintenis { get; set; }

        /// <summary>
        /// Gets or Sets Naam
        /// </summary>
        [DataMember(Name = "naam", EmitDefaultValue = false)]
        public GbaNaamBasis? Naam { get; set; }

        /// <summary>
        /// Gets or Sets Geboorte
        /// </summary>
        [DataMember(Name = "geboorte", EmitDefaultValue = false)]
        public GbaGeboorte? Geboorte { get; set; }

        /// <summary>
        /// Gets or Sets InOnderzoek
        /// </summary>
        [DataMember(Name = "inOnderzoek", EmitDefaultValue = false)]
        public GbaInOnderzoek? InOnderzoek { get; set; }

        /// <summary>
        /// Gets or Sets AangaanHuwelijkPartnerschap
        /// </summary>
        [DataMember(Name = "aangaanHuwelijkPartnerschap", EmitDefaultValue = false)]
        public GbaAangaanHuwelijkPartnerschap? AangaanHuwelijkPartnerschap { get; set; }

        /// <summary>
        /// Gets or Sets OntbindingHuwelijkPartnerschap
        /// </summary>
        [DataMember(Name = "ontbindingHuwelijkPartnerschap", EmitDefaultValue = false)]
        public virtual GbaOntbindingHuwelijkPartnerschap? OntbindingHuwelijkPartnerschap { get; set; }
    }
}
