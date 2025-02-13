using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.Data.Base.Postgres.DatabaseModels;
using Rvig.Data.Base.Postgres.Repositories.Queries;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Postgres.Repositories;
public interface IProtocolleringRepo
{
    Task<int> Insert(DbProtocollering protocolleringRecord);
    Task<int> Insert(List<DbProtocollering> protocolleringRecords);
}

public class ProtocolleringRepo : RvigRepoPostgresBase<DbProtocollering>, IProtocolleringRepo
{
    public ProtocolleringRepo(IOptions<DatabaseOptions> databaseOptions, IOptions<HaalcentraalApiOptions> haalcentraalApiOptions, ILoggingHelper loggingHelper) : base(databaseOptions, haalcentraalApiOptions, loggingHelper)
    {
        _haalcentraalApiOptions = haalcentraalApiOptions.Value;
	}
	protected override void SetMappings() => CreateMappingsFromWhereMappings();
	protected override void SetWhereMappings() => WhereMappings = RvIGBaseWhereMappingsHelper.GetProtocolleringMappings();

	public Task<int> Insert(DbProtocollering protocolleringRecord)
    {
		var query = $"insert into haalcentraal_vraag({{0}}) values(@{nameof(DbProtocollering.request_id)},@{nameof(DbProtocollering.afnemer_code)},@{nameof(DbProtocollering.pl_id)},@{nameof(DbProtocollering.request_zoek_rubrieken)},@{nameof(DbProtocollering.request_gevraagde_rubrieken)},@{nameof(DbProtocollering.verwerkt)})";
		// Remove request_datum from params for insert because this column has a default in the table (default should be now())
		var command = CreateInsertCommand(query, Mappings.Where(mapping => !mapping.Value.Equals(nameof(DbProtocollering.request_datum))).Select(o => o.Key).Aggregate((i, j) => i + "," + j));
		command.Parameters.Add(new NpgsqlParameter(nameof(DbProtocollering.request_id), protocolleringRecord.request_id));
		command.Parameters.Add(new NpgsqlParameter(nameof(DbProtocollering.afnemer_code), protocolleringRecord.afnemer_code));
		command.Parameters.Add(new NpgsqlParameter(nameof(DbProtocollering.pl_id), protocolleringRecord.pl_id));
		command.Parameters.Add(new NpgsqlParameter(nameof(DbProtocollering.request_zoek_rubrieken), protocolleringRecord.request_zoek_rubrieken));
		command.Parameters.Add(new NpgsqlParameter(nameof(DbProtocollering.request_gevraagde_rubrieken), protocolleringRecord.request_gevraagde_rubrieken));
		command.Parameters.Add(new NpgsqlParameter(nameof(DbProtocollering.verwerkt), protocolleringRecord.verwerkt));

		return InsertAsync(command);
    }

	public Task<int> Insert(List<DbProtocollering> protocolleringRecords)
    {
		var parameters = new List<NpgsqlParameter>();
		var query = $"insert into haalcentraal_vraag({{0}}) values";
		var protocolleringIndex = 0;
		var queryWhereParts = protocolleringRecords.Select(protocolleringRecord =>
		{
			protocolleringIndex++;
			parameters.Add(new NpgsqlParameter($"RequestId{protocolleringIndex}", protocolleringRecord.request_id));
			parameters.Add(new NpgsqlParameter($"AfnemerCode{protocolleringIndex}", protocolleringRecord.afnemer_code));
			parameters.Add(new NpgsqlParameter($"plId{protocolleringIndex}", protocolleringRecord.pl_id));
			parameters.Add(new NpgsqlParameter($"requestZoekRubrieken{protocolleringIndex}", protocolleringRecord.request_zoek_rubrieken));
			parameters.Add(new NpgsqlParameter($"requestGevraagdeRubrieken{protocolleringIndex}", protocolleringRecord.request_gevraagde_rubrieken));
			parameters.Add(new NpgsqlParameter($"Verwerkt{protocolleringIndex}", protocolleringRecord.verwerkt));
			return $"(@RequestId{protocolleringIndex}, @AfnemerCode{protocolleringIndex}, @plId{protocolleringIndex}, @requestZoekRubrieken{protocolleringIndex}, @requestGevraagdeRubrieken{protocolleringIndex}, @Verwerkt{protocolleringIndex})";
		});
		var queryWherePartsJoined = string.Join(", ", queryWhereParts);
		query += queryWherePartsJoined;

		// Remove request_datum from params for insert because this column has a default in the table (default should be now())
		var command = CreateInsertCommand(query, Mappings.Where(mapping => !mapping.Value.Equals(nameof(DbProtocollering.request_datum))).Select(o => o.Key).Aggregate((i, j) => i + "," + j));
		command.Parameters.AddRange(parameters.ToArray());

		return InsertAsync(command);
    }
}