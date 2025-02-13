using Rvig.HaalCentraalApi.Reisdocumenten.ApiModels.Reisdocumenten;

namespace Rvig.HaalCentraalApi.Reisdocumenten.Helper
{
	public static class GbaReisdocumentenApiHelper
	{
		/// <summary>
		/// This method will remove inonderzoek when it is not necessary to show it.
		/// </summary>
		/// <param name="reisdocument"></param>
		public static void FixInOnderzoek(List<string> fields, GbaReisdocument? reisdocument)
		{
			if (reisdocument != null && reisdocument.InOnderzoek != null && fields.All(field => field.Contains("houder")))
			{
				reisdocument.InOnderzoek = null;
			}
			if (reisdocument != null && reisdocument.Houder != null
				&& reisdocument.Houder.InOnderzoek != null && fields.All(field => !field.Contains("houder")))
			{
				reisdocument.Houder = null;
			}
		}

		/// <summary>
		/// This has to do with reisdocument logic. Below is the rule of Haal Centraal written in an zoek-met-burgerservicenummer\gba\actuele-reisdocumenten-gba.feature.
		/// een reisdocument wordt alleen geleverd wanneer er ten minste één gegeven uit groep 35 een waarde heeft.
		/// Een standaardwaarde geldt hier als waarde
		/// Een reisdocument wordt niet geleverd wanneer alleen gegevens uit groep 36, 81, 82, 83, 85 en/of 86 een waarde hebben
		/// </summary>
		/// <param name="reisdocument"></param>
		public static bool IsReisdocumentNonExistent(GbaReisdocument? reisdocument)
		{
			return reisdocument?.Soort == null && string.IsNullOrWhiteSpace(reisdocument?.Reisdocumentnummer)
				&& string.IsNullOrWhiteSpace(reisdocument?.DatumEindeGeldigheid)
				&& (reisdocument?.InhoudingOfVermissing == null || (reisdocument?.InhoudingOfVermissing?.Aanduiding == null && !string.IsNullOrWhiteSpace(reisdocument?.InhoudingOfVermissing?.Datum)));
		}
	}
}
