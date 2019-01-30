using System;

namespace Henspe.Core.Communication
{
	/// <summary>
	/// FIXME Dummy ICredentials. Used only for satisfying SNLA Http Client
	/// </summary>
	public interface ICredentials
	{
		double server_dato { get; set; }
		int server_kl { get; set; }
		double server_local_millisec_diff { get; set; }
	}
}