using System;
using System.Net;
using System.Net.Sockets;

namespace FolderSync.Network
{
	public interface INetworkInfoProvider
	{
		event EventHandler<SocketException> SocketException;

		IPAddress Ipv4 { get; }
		string HostName { get; }
	}
}