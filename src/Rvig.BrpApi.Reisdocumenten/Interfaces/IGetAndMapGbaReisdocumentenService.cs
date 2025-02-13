using Rvig.BrpApi.Reisdocumenten.ApiModels.Reisdocumenten;

namespace Rvig.BrpApi.Reisdocumenten.Interfaces;

public interface IGetAndMapGbaReisdocumentenService
{
    Task<(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode)> GetReisdocumentByReisdocumentnummers(List<string> reisdocumentnummers, bool checkAuthorization);
    Task<(IEnumerable<(GbaReisdocument reisdocument, long pl_id)>? reisdocumentenPlIds, int afnemerCode)> GetReisdocumentByBurgerservicenummer(string burgerservicenummer, bool checkAuthorization);
}