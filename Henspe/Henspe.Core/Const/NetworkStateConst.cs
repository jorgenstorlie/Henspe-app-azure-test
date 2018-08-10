using System;

namespace Henspe.Core.Const
{
	public class NetworkStateConst
	{
		public const int noNetwork = -1;
		public const int mobileNetworkOnlyButCouldNotReachHost = 10;
		public const int mobileNetworkOnlyHostReached = 11;
		public const int wifiNetworkButCouldNotReachHost = 20;
		public const int wifiNetworkHostReached = 21;

		public NetworkStateConst ()
		{
		}
	}
}