namespace Rvig.HaalCentraalApi.Shared.Options;
public class DatabaseOptions
{
	public const string DatabaseSection = "Database";

	public string Host {  get; set; } = string.Empty;
	public string Port {  get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Database { get; set; } = string.Empty;
	public string ConnectionString => $"Host={Host};{(!string.IsNullOrWhiteSpace(Port) ? $"Port={Port};" : "")}Username={Username};Password={Password};Database={Database}";
	public bool LogQueryAsMultiLiner { get; set; }
	public double RefreshLandelijkeTabellen { get; set; }
}