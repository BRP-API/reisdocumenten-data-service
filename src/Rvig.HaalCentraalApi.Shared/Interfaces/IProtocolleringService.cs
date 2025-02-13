namespace Rvig.HaalCentraalApi.Shared.Interfaces
{
	public interface IProtocolleringService
	{
		public Task Insert(int afnemerCode, long? plIdRequestedPerson, string? zoekRubrieken, string? gevraagdeRubrieken);
		public Task Insert(int afnemerCode, List<long> plIdRequestedPersons, string? zoekRubrieken, string? gevraagdeRubrieken);
	}
}
