using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using FolderSync.FileSystem.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_FolderSync.FileSystem.Validation
{
	[TestClass]
	public class RemoteConnectionValidator
	{
		[TestMethod]
		public void Valid_ReturnsTrue_GivenValidConnectedIpv4()
		{
			//Arrange
			IPAddress myIpv4 = Dns.GetHostEntry(string.Empty).AddressList.First((x) => x.AddressFamily != AddressFamily.InterNetworkV6);
			IValidator validator = new FolderSync.FileSystem.Validation.RemoteConnectionValidator(myIpv4);
			bool expected = true;

			//Act
			bool actual = validator.Valid();

			//Assert
			Assert.AreEqual(expected,actual);
		}

		[TestMethod]
		public void Valid_ReturnsFalse_GivenValidDisconnectedIpv4()
		{
			//Arrange
			string assumedBogusIpv4 = "123.123.123.12";
			IValidator validator = new FolderSync.FileSystem.Validation.RemoteConnectionValidator(assumedBogusIpv4);
			bool expected = false;

			//Act
			bool actual = validator.Valid();

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Valid_ReturnsFalse_GivenInvalidIpv4()
		{
			//Arrange
			IValidator validator = new FolderSync.FileSystem.Validation.RemoteConnectionValidator("AAA.AAA.AAA.AAA");
			bool expected = false;

			//Act
			bool actual = validator.Valid();

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
