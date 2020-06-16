using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Test_FolderSync.Network
{
	[TestClass]
	public class NetworkInfoProvider
	{
		[TestMethod]
		public void Ipv4_NotNull_GivenValidIpv4String()
		{
			//Arrange
			string myIpv4 = Dns.GetHostEntry(string.Empty).AddressList.First((x) => x.AddressFamily == AddressFamily.InterNetwork).ToString();
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myIpv4);
			bool expected = true;

			//Act
			bool actual = networkInfoProvider.Ipv4 != null;

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Ipv4_IsNull_GivenInvalidIpv4String()
		{
			//Arrange
			string myIpv4 = "AAA.AAA.AAA.AAA";
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myIpv4);
			bool expected = true;

			//Act
			bool actual = networkInfoProvider.Ipv4 == null;

			//Assert
			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void Ipv4_IsNotNull_GivenReachableHostName()
		{
			//Arrange 
			string myHostName = Dns.GetHostName();
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myHostName, FolderSync.Network.ConnectionInfoType.HostName);
			bool expected = true;

			//Act 
			bool actual = networkInfoProvider.Ipv4 != null;

			//Assert
			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void Ipv4_IsNull_GivenUnreachableHostName()
		{
			//Arrange 
			string myHostName = Dns.GetHostName() + "ABCDEFG";
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myHostName, FolderSync.Network.ConnectionInfoType.HostName);
			bool expected = true;

			//Act 
			bool actual = networkInfoProvider.Ipv4 == null;

			//Assert
			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void HostName_IsNotEmpty_GivenValidIpv4()
		{
			//Arrange 
			string myIpv4 = Dns.GetHostEntry(string.Empty).AddressList.First((x)=>x.AddressFamily == AddressFamily.InterNetwork).ToString();
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myIpv4, FolderSync.Network.ConnectionInfoType.Ipv4);
			bool expected = true;

			//Act 
			bool actual = networkInfoProvider.HostName != string.Empty;

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void HostName_IsEmpty_GivenUnreachableIpv4()
		{
			//Arrange 
			string myIpv4 = "123.123.123.12";
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myIpv4, FolderSync.Network.ConnectionInfoType.Ipv4);
			bool expected = true;

			//Act 
			bool actual = networkInfoProvider.HostName == string.Empty;

			//Assert
			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void HostName_IsEmpty_GivenInvalidIpv4()
		{
			//Arrange 
			string myIpv4 = "AAA.AAA.AAA.AAA";
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myIpv4, FolderSync.Network.ConnectionInfoType.Ipv4);
			bool expected = true;

			//Act 
			bool actual = networkInfoProvider.HostName == string.Empty;

			//Assert
			Assert.AreEqual(expected, actual);
		}


		[TestMethod]
		public void Serializes()
		{
			//Arrange 
			string myIpv4 = "123.123.123.12";
			FolderSync.Network.NetworkInfoProvider networkInfoProvider = new FolderSync.Network.NetworkInfoProvider(myIpv4, FolderSync.Network.ConnectionInfoType.Ipv4);
			FolderSync.IO.Serializer<FolderSync.Network.NetworkInfoProvider> serializer = new FolderSync.IO.Serializer<FolderSync.Network.NetworkInfoProvider>();
			bool expected = true;

			//Act 
			serializer.SerializeToFile(networkInfoProvider, FolderSync.IO.SerializationType.JSON, "networkInfo.json");
			bool actual = File.Exists("networkInfo.json");

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
