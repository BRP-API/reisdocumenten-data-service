using Microsoft.Extensions.Options;
using Rvig.HaalCentraalApi.Shared.Helpers;
using Rvig.HaalCentraalApi.Shared.Options;

namespace Rvig.Data.Base.Repositories;

public abstract class RvigRepoBase<T> where T : class, new()
{
	protected HaalcentraalApiOptions _haalcentraalApiOptions { get; set; }

	protected RvigRepoBase(IOptions<HaalcentraalApiOptions> haalcentraalApiOptions, ILoggingHelper loggingHelper)
	{
		_haalcentraalApiOptions = haalcentraalApiOptions.Value;
	}
}