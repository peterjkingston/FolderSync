using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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
		SerializableAnimal _expectedAnimal = new SerializableAnimal("Dog", "Brown", 15, 35);
		List<SerializableAnimal> _expectedAnimalList = new List<SerializableAnimal>(new SerializableAnimal[] { new SerializableAnimal("Dog", "Brown", 15, 35) });
		IDeserializer<SerializableAnimal> _animalDeserializer = new Deserializer<SerializableAnimal>();
		IDeserializer<List<SerializableAnimal>> _animalListDeserializer = new Deserializer<List<SerializableAnimal>>();
		SerializationType _binary = SerializationType.Binary;
		SerializationType _xml = SerializationType.XML;
		SerializationType _json = SerializationType.JSON;

		Action<string> WrapUp = (path) =>
		{
            if (File.Exists(path)) { File.Delete(path); }
		};

		Func<SerializableAnimal, SerializableAnimal, bool> equal = (a, b) =>
		{
			return a.Name == b.Name &&
						  a.Color == b.Color &&
						  a.Height == b.Height &&
						  a.Weight == b.Weight;
		};

		private void StandardSerializeBinary<T>(string writePath, T graph)
        {
			using (FileStream fs = File.OpenWrite(writePath))
			{
				new BinaryFormatter().Serialize(fs, graph);
				fs.Close();
			}
		}

		private void StandardSerializeXML<T>(string writePath, T graph)
        {
			using (FileStream fs = File.OpenWrite(writePath))
			{
				new XmlSerializer(typeof(T)).Serialize(fs, graph);
				fs.Close();
			}
		}

		private void StandardSerializeJSON<T>(string writePath, T graph)
        {
			using (TextWriter tw = new StreamWriter(File.OpenWrite(writePath)))
			{

				new JsonSerializer().Serialize(tw, graph);
				tw.Close();
			}
		}

		[TestMethod]
		public void Deserialize_ReadsBinary()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act
			StandardSerializeBinary(writePath, _expectedAnimal);
			
			SerializableAnimal actualAnimal = _animalDeserializer.Deserialize(writePath, _binary);
			bool actual = equal(_expectedAnimal, actualAnimal);
			WrapUp(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Deserialize_ReadsBinaryList()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act
			StandardSerializeBinary(writePath, _expectedAnimalList);

			SerializableAnimal actualAnimal = _animalListDeserializer.Deserialize(writePath, _binary)[0];
			bool actual = equal(_expectedAnimal, actualAnimal);
			WrapUp(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Deserialize_ReadsXML()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act

			StandardSerializeXML(writePath, _expectedAnimal);
			SerializableAnimal actualAnimal = _animalDeserializer.Deserialize(writePath, _xml);
			bool actual = equal(_expectedAnimal, actualAnimal);
			WrapUp(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Deserialize_ReadsXMLList()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act

			StandardSerializeXML(writePath, _expectedAnimalList);
			SerializableAnimal actualAnimal = _animalListDeserializer.Deserialize(writePath, _xml)[0];
			bool actual = equal(_expectedAnimal, actualAnimal);
			WrapUp(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Deserialize_ReadsJSON()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act
			StandardSerializeJSON(writePath, _expectedAnimal);

			SerializableAnimal actualAnimal = _animalDeserializer.Deserialize(writePath, _json);
			bool actual = equal(_expectedAnimal, actualAnimal);
			WrapUp(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Deserialize_ReadsJSONList()
		{
			//Arrange
			string writePath = System.Reflection.MethodBase.GetCurrentMethod().Name;
			bool expected = true;

			//Act
			StandardSerializeJSON(writePath, _expectedAnimalList);

			SerializableAnimal actualAnimal = _animalListDeserializer.Deserialize(writePath, _json)[0];
			bool actual = equal(_expectedAnimal, actualAnimal);
			WrapUp(writePath);

			//Assert
			Assert.AreEqual(expected, actual);
		}
	}
}
