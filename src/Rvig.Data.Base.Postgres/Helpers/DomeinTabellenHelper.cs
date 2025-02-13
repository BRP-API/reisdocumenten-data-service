using Microsoft.Extensions.Options;
using Rvig.BrpApi.Shared.Interfaces;
using Rvig.BrpApi.Shared.Options;
using System.Collections.Concurrent;

namespace Rvig.Data.Base.Postgres.Helpers;

public class DomeinTabellenHelper : IDomeinTabellenHelper
{
	private readonly IDomeinTabellenRepo _domeinTabellenRepo;
	public DatabaseOptions _databaseOptions { get; set; }

	// Last time adellijke titels predikaten were retrieved.
	private DateTime? _adellijkeTitelsPredikatenLastMomentOfRetrieval;
	private readonly ConcurrentDictionary<string, (string? omschrijving, string? soort)> _adellijkeTitelsPredikaten = new ConcurrentDictionary<string, (string? omschrijving, string? soort)>();
	// Last time gemeenten were retrieved.
	private DateTime? _gemeentenLastMomentOfRetrieval;
	private readonly ConcurrentDictionary<int, (string? omschrijving, int? nieuweCode)> _gemeenten = new ConcurrentDictionary<int, (string? omschrijving, int? nieuweCode)>();
	// Last time landen were retrieved.
	private DateTime? _landenPredikatenLastMomentOfRetrieval;
	private readonly ConcurrentDictionary<int, string?> _landen = new ConcurrentDictionary<int, string?>();
	// Last time redenen opnemen beeindigen nationaliteit were retrieved.
	private DateTime? _redenenOpnemenBeeindigenNationaliteitLastMomentOfRetrieval;
	private readonly ConcurrentDictionary<int, string?> _redenenOpnemenBeeindigenNationaliteit = new ConcurrentDictionary<int, string?>();
	// Last time verblijfstitels were retrieved.
	private DateTime? _verblijfstitelsLastMomentOfRetrieval;
	private readonly ConcurrentDictionary<int, string?> _verblijfstitels = new ConcurrentDictionary<int, string?>();
	// Last time rni deelnemers were retrieved.
	private DateTime? _rniDeelnemersLastMomentOfRetrieval;
	private readonly ConcurrentDictionary<int, string?> _rniDeelnemers = new ConcurrentDictionary<int, string?>();
	// Last time gezagsverhoudingen were retrieved.
	private DateTime? _gezagsverhoudingenLastMomentOfRetrieval;
	private readonly ConcurrentDictionary<string, string?> _gezagsverhoudingen = new ConcurrentDictionary<string, string?>();

	public DomeinTabellenHelper(IDomeinTabellenRepo domeinTabellenRepo, IOptions<DatabaseOptions> databaseOptions)
	{
		_domeinTabellenRepo = domeinTabellenRepo;
		_databaseOptions = databaseOptions.Value;
	}

	/// <summary>
	/// Return omschrijving of table based on unique code
	/// </summary>
	/// <param name="code">Table unique code</param>
	/// <returns>Omschrijving based on unique code</returns>
	public async Task<string?> GetOmschrijvingBaseTabel(int? code, Func<Task<IEnumerable<(int? code, string? omschrijving)>>> getTabelValueFunc, ConcurrentDictionary<int, string?> tabelDictionary, DateTime? tableDictionaryLastMomentOfRetrieval, Func<DateTime?, DateTime?> updateLastMomentOfRetrievalFunc)
	{
		if (!code.HasValue)
		{
			return null;
		}
		if (tabelDictionary.IsEmpty || MustNewValueBeRetrieved(tableDictionaryLastMomentOfRetrieval))
		{
			updateLastMomentOfRetrievalFunc(DateTime.Now);
			foreach (var tabelWaarde in (await getTabelValueFunc()))
			{
				if (tabelWaarde.code.HasValue)
				{
					tabelDictionary.TryAdd(tabelWaarde.code.Value, tabelWaarde.omschrijving);
				}
			}
		}
		return tabelDictionary.ContainsKey(code.Value) ? tabelDictionary[code.Value] : null;
	}

	/// <summary>
	/// Return omschrijving of adellijke titel/predikaat based on unique code
	/// </summary>
	/// <param name="code">Adellijke titel/predikaat code</param>
	/// <returns>Omschrijving based on unique code</returns>
	public async Task<(string? omschrijving, string? soort)> GetAdellijkeTitelPredikaatOmschrijvingEnSoort(string? code)
	{
		if (string.IsNullOrWhiteSpace(code))
		{
			return default;
		}
		if (_adellijkeTitelsPredikaten.IsEmpty || MustNewValueBeRetrieved(_adellijkeTitelsPredikatenLastMomentOfRetrieval))
		{
			_adellijkeTitelsPredikatenLastMomentOfRetrieval = DateTime.Now;
			foreach (var atp in (await _domeinTabellenRepo.GetAllAdellijkeTitelsPredikaten()))
			{
				if (!string.IsNullOrWhiteSpace(atp.code))
				{
					_adellijkeTitelsPredikaten.TryAdd(atp.code, (atp.omschrijving, atp.soort));
				}
			}
		}
		return _adellijkeTitelsPredikaten.ContainsKey(code) ? _adellijkeTitelsPredikaten[code] : default;
	}

	/// <summary>
	/// Return omschrijving of gemeente based on unique code
	/// </summary>
	/// <param name="code">Gemeente code</param>
	/// <returns>Omschrijving based on unique code</returns>
	public async Task<string?> GetGemeenteOmschrijving(int? code)
	{
		if (!code.HasValue)
		{
			return null;
		}
		await RefreshGemeentenIfNecessary();
		return _gemeenten.ContainsKey(code.Value) ? _gemeenten[code.Value].omschrijving : null;
	}

	/// <summary>
	/// Return omschrijving of gemeente based on unique code
	/// </summary>
	/// <param name="code">Gemeente code</param>
	/// <returns>Omschrijving based on unique code</returns>
	public async Task<int?> GetNewGemeenteCode(int? code)
	{
		if (!code.HasValue)
		{
			return null;
		}
		await RefreshGemeentenIfNecessary();
		return _gemeenten.ContainsKey(code.Value) ? _gemeenten[code.Value].nieuweCode : null;
	}

	private async Task RefreshGemeentenIfNecessary()
	{
		if (_gemeenten.IsEmpty || MustNewValueBeRetrieved(_gemeentenLastMomentOfRetrieval))
		{
			_gemeentenLastMomentOfRetrieval = DateTime.Now;
			foreach (var tabelWaarde in (await _domeinTabellenRepo.GetAllGemeenten()))
			{
				if (tabelWaarde.code.HasValue)
				{
					_gemeenten.TryAdd(tabelWaarde.code.Value, (tabelWaarde.omschrijving, tabelWaarde.nieuweCode));
				}
			}
		}
	}

	/// <summary>
	/// Return omschrijving of land based on unique code
	/// </summary>
	/// <param name="code">Land code</param>
	/// <returns>Omschrijving based on unique code</returns>
	public Task<string?> GetLandOmschrijving(int? code)
	{
		return GetOmschrijvingBaseTabel(code, _domeinTabellenRepo.GetAllLanden, _landen, _landenPredikatenLastMomentOfRetrieval, (DateTime? datetime) => _landenPredikatenLastMomentOfRetrieval = datetime);
	}

	/// <summary>
	/// Return omschrijving of reden opnemen/beëindigen nationaliteit based on unique code
	/// </summary>
	/// <param name="code">Reden opnemen/beëindigen nationaliteit code</param>
	/// <returns>Omschrijving based on unique code</returns>
	public Task<string?> GetRedenOpnemenBeeindigenNationaliteitOmschrijving(int? code)
	{
		return GetOmschrijvingBaseTabel(code, _domeinTabellenRepo.GetAllRedenOpnemenBeeindigenNationaliteit, _redenenOpnemenBeeindigenNationaliteit, _redenenOpnemenBeeindigenNationaliteitLastMomentOfRetrieval, (DateTime? datetime) => _redenenOpnemenBeeindigenNationaliteitLastMomentOfRetrieval = datetime);
	}

	/// <summary>
	/// Return omschrijving of verblijfstitel based on unique code
	/// </summary>
	/// <param name="code">Verblijfstitel code</param>
	/// <returns>Omschrijving based on unique code</returns>
	public Task<string?> GetVerblijfstitelOmschrijving(int? code)
	{
		return GetOmschrijvingBaseTabel(code, _domeinTabellenRepo.GetAllVerblijfstitels, _verblijfstitels, _verblijfstitelsLastMomentOfRetrieval, (DateTime? datetime) => _verblijfstitelsLastMomentOfRetrieval = datetime);
	}

	public Task<string?> GetRniDeelnemerOmschrijving(int? code)
	{
		return GetOmschrijvingBaseTabel(code, _domeinTabellenRepo.GetAllRniDeelnemers, _rniDeelnemers, _rniDeelnemersLastMomentOfRetrieval, (DateTime? datetime) => _rniDeelnemersLastMomentOfRetrieval = datetime);
	}

	public async Task<string?> GetGezagsverhoudingOmschrijving(string? code)
	{
		if (string.IsNullOrWhiteSpace(code))
		{
			return null;
		}
		if (_gezagsverhoudingen.IsEmpty || MustNewValueBeRetrieved(_gezagsverhoudingenLastMomentOfRetrieval))
		{
			_gezagsverhoudingenLastMomentOfRetrieval ??= DateTime.Now;
			foreach (var gezagsverhouding in (await _domeinTabellenRepo.GetAllGezagsverhoudingen()))
			{
				if (!string.IsNullOrWhiteSpace(gezagsverhouding.code))
				{
					_gezagsverhoudingen.TryAdd(gezagsverhouding.code, gezagsverhouding.omschrijving);
				}
			}
		}
		return _gezagsverhoudingen.ContainsKey(code) ? _gezagsverhoudingen[code] : default;
	}

	/// <summary>
	/// Must retrieve new values for landelijke tabel if last retrieval moment has been more than 24 hours ago.
	/// </summary>
	/// <param name="lastMomentOfRetrieval"></param>
	/// <returns></returns>
	private bool MustNewValueBeRetrieved(DateTime? lastMomentOfRetrieval)
	{
		if (!lastMomentOfRetrieval.HasValue)
		{
			return true;
		}

		return DateTime.Now > lastMomentOfRetrieval.Value.AddMilliseconds(_databaseOptions.RefreshLandelijkeTabellen);
	}
}
