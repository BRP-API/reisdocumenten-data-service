using Microsoft.Extensions.Options;
using Npgsql;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;
using System.Text.RegularExpressions;

namespace Rvig.Data.Base.Postgres.Repositories;
public abstract class PostgresSqlQueryRepoBase<T> : PostgresRepoBase where T : class, new()
{
	protected PostgresSqlQueryRepoBase(IOptions<DatabaseOptions> databaseOptions, ILoggingHelper loggingHelper)
		:	base(databaseOptions, loggingHelper)
	{
	}

	/// <summary>
	/// Datasourcename (columnname), propertyname (in csharp class)
	/// </summary>
	protected IDictionary<string, string> Mappings { get; set; } = new Dictionary<string, string>();
	protected IDictionary<string, string> WhereMappings { get; set; } = new Dictionary<string, string>();

	/// <summary>
	/// Override and set mappings from database columnnames to propertynames of table class.
	/// </summary>
	protected abstract void SetMappings();
	protected abstract void SetWhereMappings();

	protected static NpgsqlCommand CreateDbCommand(string query) => new(query);

	protected NpgsqlCommand CreateFilterCommand(string baseQuery, string mappings, (string where, IEnumerable<NpgsqlParameter> parameters) whereStringAndParams)
	{
		var query = string.Format(baseQuery, mappings, whereStringAndParams.where);
		var command = CreateDbCommand(query);
		command.Parameters.AddRange(whereStringAndParams.parameters.ToArray());
		return command;
	}

	protected NpgsqlCommand CreateInsertCommand(string baseQuery, string mappings) => CreateDbCommand(string.Format(baseQuery, mappings));

	protected IDictionary<string, string> CreateMappingsFromWhereMappings(IDictionary<string, string> whereMappings)
	{
		return whereMappings.ToDictionary(whereMapping =>
		{
			if (whereMapping.Key.Contains(" AS ", StringComparison.OrdinalIgnoreCase))
			{
				return whereMapping.Key.Substring(whereMapping.Key.LastIndexOf(" AS ", StringComparison.OrdinalIgnoreCase) + 4);
			}
			return whereMapping.Key.Substring(whereMapping.Key.IndexOf('.') + 1);
		}, whereMapping => whereMapping.Value);
	}

	protected void CreateMappingsFromWhereMappings()
	{
		Mappings = CreateMappingsFromWhereMappings(WhereMappings);
	}

	protected virtual async Task<IEnumerable<T>> GetFilterResultAsync(NpgsqlCommand command, IDictionary<string, string>? additionalMappings = null)
	{
		var mergedMappings = additionalMappings != null ? Mappings.Concat(additionalMappings) : Mappings;
		var records = new List<T>();

		using var connection = GetConnection();

		await OpenConnectionAndLog(connection);

		command.Connection = connection;

		var reader = await command.ExecuteReaderAsync();
		while (reader.Read())
		{
			var record = Activator.CreateInstance<T>();

			foreach (var mapping in mergedMappings)
			{
				if (!ResultContainsColumn(reader, mapping.Key))
				{
					continue;
				}
				var columnValue = reader[mapping.Key];
				if (columnValue != null && columnValue is not DBNull)
				{
					SetValue(mapping.Value, record, columnValue);
				}
			}

			records.Add(record);
		}

		if (_databaseOptions.Value.LogQueryAsMultiLiner)
		{
			_loggingHelper.LogDebug("The query that was executed =  \r\n" + command.CommandText);
		}
		else
		{
			_loggingHelper.LogDebug("The query that was executed = " + Regex.Replace(Regex.Replace(Regex.Replace(command.CommandText, "\r\n", ""), "\n", ""), "\t", " "));
		}

		return records;
	}

	protected virtual async Task<int> InsertAsync(NpgsqlCommand command)
	{
		using var connection = GetConnection();

		await OpenConnectionAndLog(connection);

		command.Connection = connection;

		var numberOfRowsAffected = await command.ExecuteNonQueryAsync();

		if (_databaseOptions.Value.LogQueryAsMultiLiner)
		{
			_loggingHelper.LogDebug("The query that was executed =  \r\n" + command.CommandText + "\r\n Number of rows affected: " + numberOfRowsAffected);
		}
		else
		{
			_loggingHelper.LogDebug("The query that was executed = " + Regex.Replace(Regex.Replace(Regex.Replace(command.CommandText, "\r\n", ""), "\n", ""), "\t", " ") + " Number of rows affected: " + numberOfRowsAffected);
		}

		return numberOfRowsAffected;
	}

	private static void SetValue(string propPath, object? parent, object? value)
	{
		if (parent != null)
		{
			if (!propPath.Contains("."))
			{
				parent?.GetType()?.GetProperty(propPath)?.SetValue(parent, value);
			}
			else
			{
				var propertyParts = propPath.Split(".");
				foreach (var part in propertyParts)
				{
					if (propertyParts.Last().Equals(part))
					{
						parent?.GetType()?.GetProperty(part)?.SetValue(parent, value);
					}
					else
					{
						parent = parent?.GetType()?.GetProperty(part)?.GetValue(parent);
					}
				}
			}
		}
	}

	private static bool ResultContainsColumn(NpgsqlDataReader reader, string columnName)
	{
		for (var i = 0; i < reader.FieldCount; i++)
		{
			var paramName = reader.GetName(i);

			if (string.Equals(columnName, paramName, StringComparison.CurrentCultureIgnoreCase))
			{
				return true;
			}
		}

		return false;
	}
}