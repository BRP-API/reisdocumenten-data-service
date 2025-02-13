using Rvig.HaalCentraalApi.Reisdocumenten.ApiModels.Reisdocumenten;

namespace Rvig.HaalCentraalApi.Reisdocumenten.Interfaces;

public interface IGetAndMapGbaReisdocumentenService
{
	Task<(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode)> GetReisdocumentByReisdocumentnummers(List<string> reisdocumentnummers, bool checkAuthorization);
	Task<(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode)> GetReisdocumentByBurgerservicenummer(string burgerservicenummer, bool checkAuthorization);
}