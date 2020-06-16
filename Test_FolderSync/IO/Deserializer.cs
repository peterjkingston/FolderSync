using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using FolderSync.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Test_FolderSync.MockClasses;

namespace Test_FolderSync.IO
{
	[TestClass]
	public class Deserializer
	{
		[TestMethod]
		public void Deserialize_ReadsBinary()
		{
			//Arrange
			SerializableAnimal expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
			FolderSync.IO.IDeserializer<SerializableAnimal> deserializer = new FolderSync.IO.Deserializer<SerializableAnimal>();
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializationType serializationType = SerializationType.Binary;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act
			
			using (FileStream fs = File.OpenWrite(writePath))
			{
				new BinaryFormatter().Serialize(fs,expectedAnimal);
				fs.Close();
			}
			actualAnimal = deserializer.Deserialize(writePath, serializationType);
			bool actual = expectedAnimal.Name == actualAnimal.Name &&
						  expectedAnimal.Color == actualAnimal.Color &&
						  expectedAnimal.Height == actualAnimal.Height &&
						  expectedAnimal.Weight == actualAnimal.Weight;
			File.Delete(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Deserialize_ReadsXML()
		{
			//Arrange
			SerializableAnimal expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
			FolderSync.IO.IDeserializer<SerializableAnimal> deserializer = new FolderSync.IO.Deserializer<SerializableAnimal>();
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializationType serializationType = SerializationType.XML;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act

			using (FileStream fs = File.OpenWrite(writePath))
			{
				new XmlSerializer(typeof(SerializableAnimal)).Serialize(fs, expectedAnimal);
				fs.Close();
			}
			actualAnimal = deserializer.Deserialize(writePath, serializationType);
			bool actual = expectedAnimal.Name == actualAnimal.Name &&
						  expectedAnimal.Color == actualAnimal.Color &&
						  expectedAnimal.Height == actualAnimal.Height &&
						  expectedAnimal.Weight == actualAnimal.Weight;
			File.Delete(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Deserialize_ReadsJSON()
		{
			//Arrange
			SerializableAnimal expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
			FolderSync.IO.IDeserializer<SerializableAnimal> deserializer = new FolderSync.IO.Deserializer<SerializableAnimal>();
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializationType serializationType = SerializationType.JSON;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act

			using (TextWriter tw = new StreamWriter(File.OpenWrite(writePath)))
			{

				new JsonSerializer().Serialize(tw,expectedAnimal);
				tw.Close();
			}

			actualAnimal = deserializer.Deserialize(writePath, serializationType);
			bool actual = expectedAnimal.Name == actualAnimal.Name &&
						  expectedAnimal.Color == actualAnimal.Color &&
						  expectedAnimal.Height == actualAnimal.Height &&
						  expectedAnimal.Weight == actualAnimal.Weight;
			File.Delete(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
