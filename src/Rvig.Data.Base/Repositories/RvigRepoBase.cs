using Microsoft.Extensions.Options;
using Rvig.BrpApi.Shared.Options;

namespace Rvig.Data.Base.Repositories;

public abstract class RvigRepoBase<T> where T : class, new()
{
	protected HaalcentraalApiOptions _haalcentraalApiOptions { get; set; }

	protected RvigRepoBase(IOptions<HaalcentraalApiOptions> haalcentraalApiOptions)
	{
		_haalcentraalApiOptions = haalcentraalApiOptions.Value;
	}
}