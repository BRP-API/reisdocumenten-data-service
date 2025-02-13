namespace Rvig.HaalCentraalApi.Shared.Interfaces;
public interface IDomeinTabellenRepo
{
	Task<IEnumerable<(string? code, string? omschrijving, string? soort)>> GetAllAdellijkeTitelsPredikaten();
	Task<IEnumerable<(int? code, string? omschrijving, int? nieuweCode)>> GetAllGemeenten();
	Task<IEnumerable<(int? code, string? omschrijving)>> GetAllLanden();
	Task<IEnumerable<(int? code, string? omschrijving)>> GetAllRedenOpnemenBeeindigenNationaliteit();
	Task<IEnumerable<(int? code, string? omschrijving)>> GetAllVerblijfstitels();
	Task<IEnumerable<(int? code, string? omschrijving)>> GetAllRniDeelnemers();
	Task<IEnumerable<(string? code, string? omschrijving)>> GetAllGezagsverhoudingen();
	Task<string?> GetGemeenteNaam(int? gemeenteCode);
    Task<string?> GetGemeenteNaam(long gemeenteCode);
    Task<bool> VoorvoegselExist(string voorvoegsel);
}