using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using FolderSync.Network;

namespace FolderSync.FileSystem.Validation
{
	public class RemoteConnectionValidator : IValidator
	{
		IPAddress _ipv4;

		public RemoteConnectionValidator(string ipv4)
		{
			IPAddress.TryParse(ipv4, out _ipv4);
			_ipv4 = _ipv4 != null && _ipv4.AddressFamily == AddressFamily.InterNetwork ? _ipv4 : null;
		}

		public RemoteConnectionValidator(IPAddress ipv4)
		{
			_ipv4 = ipv4.AddressFamily == AddressFamily.InterNetwork ? ipv4 : null;
		}

		public RemoteConnectionValidator(INetworkInfoProvider networkInfoProvider)
		{
			_ipv4 = networkInfoProvider.Ipv4;
			_ipv4 = _ipv4 != null && _ipv4.AddressFamily == AddressFamily.InterNetwork ? _ipv4 : null;
		}

		public bool Valid()
		{
			bool result = false;

			if(_ipv4 != null)
			{
				Ping p = new Ping();
				PingReply reply = p.SendPingAsync(_ipv4).Result;
				result = reply.Status == IPStatus.Success;
			}

			return result;
		}
	}
}
