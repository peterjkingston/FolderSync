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
		SerializableAnimal _expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
		List<SerializableAnimal> _expectedAnimalList = new List<SerializableAnimal>(new SerializableAnimal[] { new SerializableAnimal("Dog", "Brown", 15, 35) });
		ISerializer<SerializableAnimal> _animalSerializer = new Serializer<SerializableAnimal>();
		ISerializer<List<SerializableAnimal>> _animalListSerializer = new Serializer<List<SerializableAnimal>>();
		SerializationType _binary = SerializationType.Binary;
		SerializationType _xml = SerializationType.XML;
		SerializationType _json = SerializationType.JSON;

		Action<string> wrapup = (path) => 
		{ 
			if (File.Exists(path))
			{ 
				File.Delete(path); 
			} 
		};

		Func<SerializableAnimal, SerializableAnimal, bool> equal = (a, b) =>
		{
			return a.Name == b.Name &&
					a.Color == b.Color &&
					a.Height == b.Height &&
					a.Weight == b.Weight;
		};
		
		private T StandardDeserializeBinary<T>(string writePath) 
		{
			T actualAnimal;
			using (FileStream fs = File.OpenRead(writePath))
			{
				actualAnimal = (T)new BinaryFormatter().Deserialize(fs);
				fs.Close();
			}
			return actualAnimal;
		}

		private T StandardDeserializeXML<T>(string writePath)
		{
			T actualAnimal;
			using (FileStream fs = File.OpenRead(writePath))
			{
				actualAnimal = (T)new XmlSerializer(typeof(T)).Deserialize(fs);
				fs.Close();
			}
			return actualAnimal;
		}

		private T StandardDeserializeJson<T>(string writePath)
		{
			T actualAnimal;
			using (TextReader textReader = new StreamReader(File.OpenRead(writePath)))
			{
				using (JsonReader jsonReader = new JsonTextReader(textReader))
				{
					actualAnimal = new JsonSerializer().Deserialize<T>(jsonReader);
				}
				textReader.Close();
			}
			return actualAnimal;
		}

		[TestMethod]
		public void Serialize_WritesBinary()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act
			_animalSerializer.SerializeToFile(_expectedAnimal, _binary, writePath);
			actualAnimal = StandardDeserializeBinary<SerializableAnimal>(writePath);

			bool actual = equal(_expectedAnimal, actualAnimal);

			wrapup(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serial_WritesBinaryArray()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act
			_animalListSerializer.SerializeToFile(_expectedAnimalList, _binary, writePath);
			actualAnimal = StandardDeserializeBinary<List<SerializableAnimal>>(writePath)[0];

			bool actual = equal(_expectedAnimal, actualAnimal);

			wrapup(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serialize_WritesXML()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act
			_animalSerializer.SerializeToFile(_expectedAnimal, _xml, writePath);
			actualAnimal = StandardDeserializeXML<SerializableAnimal>(writePath);

			bool actual = equal(_expectedAnimal, actualAnimal);

			wrapup(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serial_WritesXMLArray()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			SerializableAnimal actualAnimal;
			bool expected = true;

			//Act
			_animalListSerializer.SerializeToFile(_expectedAnimalList, _xml, writePath);
			actualAnimal = StandardDeserializeXML<List<SerializableAnimal>>(writePath)[0];

			bool actual = equal(_expectedAnimal, actualAnimal);

			wrapup(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serialize_WritesJSON()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act
			_animalSerializer.SerializeToFile(_expectedAnimal, _json, writePath);

			SerializableAnimal actualAnimal = StandardDeserializeJson<SerializableAnimal>(writePath);

			bool actual = equal(_expectedAnimal, actualAnimal);
			wrapup(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Serial_WritesJSONArray()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act
			_animalListSerializer.SerializeToFile(_expectedAnimalList, _json, writePath);

			SerializableAnimal actualAnimal = StandardDeserializeJson<List<SerializableAnimal>>(writePath)[0];

			bool actual = equal(_expectedAnimal, actualAnimal);
			wrapup(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

	}
}
