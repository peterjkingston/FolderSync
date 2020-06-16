using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.IO;
using System.IO.Ports;
using Newtonsoft.Json;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FolderSync.IO;

namespace FolderSync.Network
{
	[Serializable()]
	public class NetworkInfoProvider : INetworkInfoProvider, ISerializable
	{
		public IPAddress Ipv4 { get; private set; } = null;
		public string HostName { get; private set; } = string.Empty;

		public event EventHandler<SocketException> SocketException;
		protected virtual void OnSocketError(SocketException ex)
		{
			SocketException?.Invoke(this, ex);
		}

		public NetworkInfoProvider()
		{

		}

		public NetworkInfoProvider(string connectionInfo, ConnectionInfoType connectionInfoType = ConnectionInfoType.Ipv4)
		{
			switch (connectionInfoType)
			{
				case ConnectionInfoType.Ipv4:
					FillFromIpv4(connectionInfo);
					break;

				case ConnectionInfoType.Ipv6:
					FillFromIpv6(connectionInfo);
					break;

				case ConnectionInfoType.HostName:
					FillFromHostName(connectionInfo);
					break;

				default:
					break;
			}
		}

		private void FillFromIpv6(string ipv6)
		{
			IPAddress temp;
			if(IPAddress.TryParse(ipv6, out temp))
			{
				IPHostEntry hostEntry = Dns.GetHostEntry(temp);
				Ipv4 = hostEntry.AddressList.First
					(
						(x) => x.AddressFamily == AddressFamily.InterNetwork
					);
				HostName = hostEntry.HostName;
			}
		}

		private void FillFromHostName(string hostName)
		{
			HostName = hostName;
			try
			{
				IPAddress[] addresses = Dns.GetHostAddresses(hostName);
				if (addresses.Length != 0)
				{
					Ipv4 = addresses.First
					(
						(x) => x.AddressFamily == AddressFamily.InterNetwork
					);
				}
			}
			catch (SocketException ex)
			{
				OnSocketError(ex);
			}
		}

		private void FillFromIpv4(string ipv4)
		{
			IPAddress temp;
			if(IPAddress.TryParse(ipv4, out temp))
			{
				Ipv4 = temp;
				try
				{
					HostName = Dns.GetHostEntry(Ipv4).HostName;
				}
				catch (SocketException ex)
				{
					OnSocketError(ex);
				}
			}
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Ipv4", Ipv4.ToString());
			info.AddValue("HostName", HostName);
		}

		public NetworkInfoProvider(SerializationInfo info, StreamingContext context)
		{
			Ipv4 = IPAddress.Parse((string)info.GetValue("Ipv4", typeof(string)));
			HostName = (string)info.GetValue("HostName", typeof(string));
		}
	}
}
