namespace Rvig.HaalCentraalApi.Shared.Fields;

public abstract class FieldsSettings
{
	public abstract FieldsSettingsModel GbaFieldsSettings { get; }

	protected abstract FieldsSettingsModel InitGbaFieldsSettings();
}
