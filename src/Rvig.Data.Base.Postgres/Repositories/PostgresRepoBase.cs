using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.HaalCentraalApi.Shared.Exceptions;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;
using System.Data;
using static Dapper.SqlMapper;

namespace Rvig.Data.Base.Postgres.Repositories;
public abstract class PostgresRepoBase
{
	protected readonly IOptions<DatabaseOptions> _databaseOptions;
	protected readonly ILoggingHelper _loggingHelper;

	protected PostgresRepoBase(IOptions<DatabaseOptions> databaseOptions, ILoggingHelper loggingHelper)
	{
		_databaseOptions = databaseOptions;
		_loggingHelper = loggingHelper;
	}

	public NpgsqlConnection GetConnection()
	{
		return new NpgsqlConnection(_databaseOptions.Value.ConnectionString);
	}

	protected static async Task<IEnumerable<TResponseType?>> ExecuteDapperQuery<TResponseType>((string, DynamicParameters) whereStringAndParams, string baseQuery, Func<string, DynamicParameters, Task<IEnumerable<TResponseType>>> executeQueryFunc)
	{
		var query = string.Format(baseQuery, whereStringAndParams.Item1);

		return await executeQueryFunc(query, whereStringAndParams.Item2);
	}

	protected async Task<List<TDataObject>> GetDataViaDapper<TDataObject>(string queryBase, DynamicParameters dynamicParameters, string whereCondition)
	{
		var query = string.Format(queryBase, whereCondition);

		return (await DapperQueryAsync<TDataObject>(query, dynamicParameters)).ToList();
	}

	protected Task<IEnumerable<TDataObject>> DapperQueryAsync<TDataObject>(string? query, DynamicParameters? dynamicParameters = null)
	{
		return DapperQueryAsync<TDataObject>(GetConnection(), query, dynamicParameters);
	}

	protected Task<IEnumerable<TDataObject>> DapperQueryAsync<TDataObject>(string? query, Type[] types, Func<object[], TDataObject> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		return DapperQueryAsync(GetConnection(), query, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
	}

	private Task<IEnumerable<TDataObject>> DapperQueryAsync<TDataObject>(NpgsqlConnection connection, string? query, Type[] types, Func<object[], TDataObject> map, object? param = null, IDbTransaction? transaction = null, bool buffered = true, string splitOn = "Id", int? commandTimeout = null, CommandType? commandType = null)
	{
		try
		{
			return connection.QueryAsync(query, types, map, param, transaction, buffered, splitOn, commandTimeout, commandType);
		}
		catch(NpgsqlException npgEx)
		{
			_loggingHelper.LogError("Connectivity issue database: " + npgEx.Message + ".");
			throw new ServiceUnavailableException(npgEx.Message, npgEx);
		}
		catch(Exception ex)
		{
			_loggingHelper.LogError("Unexpected exception during database call. Error is: " + ex.Message + ".");
			throw;
		}
	}

	private Task<IEnumerable<TDataObject>> DapperQueryAsync<TDataObject>(NpgsqlConnection connection, string? query, DynamicParameters? dynamicParameters = null)
	{
		try
		{
			if (dynamicParameters != null)
			{
				return connection.QueryAsync<TDataObject>(query, dynamicParameters);
			}

			return connection.QueryAsync<TDataObject>(query);
		}
		catch (NpgsqlException npgEx)
		{
			_loggingHelper.LogError("Connectivity issue database: " + npgEx.Message + ".");
			throw new ServiceUnavailableException(npgEx.Message, npgEx);
		}
		catch (Exception ex)
		{
			_loggingHelper.LogError("Unexpected exception during database call. Error is: " + ex.Message + ".");
			throw;
		}
	}

	protected async Task OpenConnectionAndLog(NpgsqlConnection connection)
	{
		try
		{
			await connection.OpenAsync();
			_loggingHelper.LogDebug("Connection opened processId: " + connection.ProcessID);
		}
		catch(NpgsqlException npgEx)
		{
			_loggingHelper.LogError("Connectivity issue database: " + npgEx.Message + ".");
			throw new ServiceUnavailableException(npgEx.Message, npgEx);
		}
		catch(Exception ex)
		{
			_loggingHelper.LogError("Unexpected exception during database call. Error is: " + ex.Message + ".");
			throw;
		}
	}
}