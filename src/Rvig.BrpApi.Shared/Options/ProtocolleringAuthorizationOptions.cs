namespace Rvig.BrpApi.Shared.Options;

public class ProtocolleringAuthorizationOptions
{
    public const string ProtocolleringAuthorization = "ProtocolleringAuthorization";

    public bool UseProtocollering { get; set; }
    public bool UseAuthorizationChecks { get; set; }
}