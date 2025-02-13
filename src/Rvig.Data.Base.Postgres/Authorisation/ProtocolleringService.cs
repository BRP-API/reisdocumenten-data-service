using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.Data.Base.Providers;
using Rvig.Data.Base.Postgres.Repositories;
using Rvig.HaalCentraalApi.Shared.Interfaces;

namespace Rvig.Data.Base.Postgres.Authorisation
{
	public class ProtocolleringService : IProtocolleringService
	{
		private readonly IProtocolleringRepo _protocolleringRepo;
		public ICurrentDateTimeProvider _currentDateTimeProvider { get; set; }

		public ProtocolleringService(IProtocolleringRepo protocolleringRepo, ICurrentDateTimeProvider currentDateTimeProvider)
		{
			_protocolleringRepo = protocolleringRepo;
			_currentDateTimeProvider = currentDateTimeProvider;
		}

		public async Task Insert(int afnemerCode, long? plIdRequestedPerson, string? zoekRubrieken, string? gevraagdeRubrieken)
		{
			gevraagdeRubrieken = AuthorisationService.RemoveImplicitRubriekenForProtocllering(gevraagdeRubrieken);
			var protocolleringRecord = new DbProtocollering
			{
				request_id = Guid.NewGuid().ToString(),
				afnemer_code = afnemerCode,
				pl_id = plIdRequestedPerson,
				request_gevraagde_rubrieken = gevraagdeRubrieken,
				request_zoek_rubrieken = zoekRubrieken,
				verwerkt = false
			};

			await _protocolleringRepo.Insert(protocolleringRecord);
		}

		public async Task Insert(int afnemerCode, List<long> plIdRequestedPersons, string? zoekRubrieken, string? gevraagdeRubrieken)
		{
			gevraagdeRubrieken = AuthorisationService.RemoveImplicitRubriekenForProtocllering(gevraagdeRubrieken);
			var protocolleringRecords = new List<DbProtocollering>();

			// When protocollering multiple results, they should all share the same request id because it was within the same request but no it doesn't......
			//var requestId = Guid.NewGuid().ToString();
			plIdRequestedPersons.ForEach(plId =>
			{
				protocolleringRecords.Add(new DbProtocollering
				{
					request_id = Guid.NewGuid().ToString(),
					afnemer_code = afnemerCode,
					pl_id = plId,
					request_gevraagde_rubrieken = gevraagdeRubrieken,
					request_zoek_rubrieken = zoekRubrieken,
					verwerkt = false
				});
			});

			await _protocolleringRepo.Insert(protocolleringRecords);
		}
	}
}
