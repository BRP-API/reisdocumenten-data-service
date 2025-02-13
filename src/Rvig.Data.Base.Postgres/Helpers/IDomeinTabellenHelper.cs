namespace Rvig.Data.Base.Postgres.Helpers;

public interface IDomeinTabellenHelper
{
	Task<(string? omschrijving, string? soort)> GetAdellijkeTitelPredikaatOmschrijvingEnSoort(string? code);
	Task<string?> GetGemeenteOmschrijving(int? code);
	Task<int?> GetNewGemeenteCode(int? code);
	Task<string?> GetLandOmschrijving(int? code);
	Task<string?> GetRedenOpnemenBeeindigenNationaliteitOmschrijving(int? code);
	Task<string?> GetVerblijfstitelOmschrijving(int? code);
	Task<string?> GetRniDeelnemerOmschrijving(int? code);
	Task<string?> GetGezagsverhoudingOmschrijving(string? code);
}
