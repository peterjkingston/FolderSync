using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FolderSync.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Test_FolderSync.MockClasses;

namespace Test_FolderSync.IO
{
	[TestClass]
	public class Serializer
	{
		[TestMethod]
		public void Serialize_WritesBinary()
		{
			//Arrange
			SerializableAnimal expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
			FolderSync.IO.ISerializer<SerializableAnimal> serializer = new FolderSync.IO.Serializer<SerializableAnimal>();
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializationType serializationType = SerializationType.Binary;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act
			serializer.SerializeToFile(expectedAnimal, serializationType, writePath);
			using(FileStream fs = File.OpenRead(writePath))
			{
				actualAnimal = (SerializableAnimal)new BinaryFormatter().Deserialize(fs);
				fs.Close();
			}
			bool actual = expectedAnimal.Name == actualAnimal.Name &&
						  expectedAnimal.Color == actualAnimal.Color &&
						  expectedAnimal.Height == actualAnimal.Height &&
						  expectedAnimal.Weight == actualAnimal.Weight;
			File.Delete(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serialize_WritesXML()
		{
			//Arrange
			SerializableAnimal expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
			FolderSync.IO.ISerializer<SerializableAnimal> serializer = new FolderSync.IO.Serializer<SerializableAnimal>();
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializationType serializationType = SerializationType.XML;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act
			serializer.SerializeToFile(expectedAnimal, serializationType, writePath);
			using (FileStream fs = File.OpenRead(writePath))
			{
				actualAnimal = (SerializableAnimal)new XmlSerializer(typeof(SerializableAnimal)).Deserialize(fs);
				fs.Close();
			}
			bool actual = expectedAnimal.Name == actualAnimal.Name &&
						  expectedAnimal.Color == actualAnimal.Color &&
						  expectedAnimal.Height == actualAnimal.Height &&
						  expectedAnimal.Weight == actualAnimal.Weight;
			File.Delete(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serialize_WritesJSON()
		{
			//Arrange
			SerializableAnimal expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
			FolderSync.IO.ISerializer<SerializableAnimal> serializer = new FolderSync.IO.Serializer<SerializableAnimal>();
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializationType serializationType = SerializationType.JSON;
			SerializableAnimal actualAnimal;
			JsonSerializer js = new JsonSerializer();
			bool expected = true;

			//Act
			serializer.SerializeToFile(expectedAnimal, serializationType, writePath);
			using (TextReader textReader = new StreamReader(File.OpenRead(writePath)))
			{
				using (JsonReader jsonReader = new JsonTextReader(textReader))
				{
					actualAnimal = js.Deserialize<SerializableAnimal>(jsonReader);
				}
				textReader.Close();
			}
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
