namespace Rvig.HaalCentraalApi.Shared.Options;
public class HaalcentraalApiOptions
{
    public const string HaalcentraalApi = "HaalcentraalApi";

    public string BrpHostName { get; set; } = "{brpserverurl}";
    public string BagHostName { get; set; } = "{bagserverurl}";
}