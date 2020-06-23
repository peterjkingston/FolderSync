using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace FolderSync.IO
{
	public class Deserializer<T> : IDeserializer<T> where T : new()
	{
		public T Deserialize(string sourcePath, SerializationType serializationType)
		{
			T result;
			switch (serializationType)
			{
				case SerializationType.Binary:
					result = FromBinary(sourcePath);
					break;

				case SerializationType.XML:
					result = FromXML(sourcePath);
					break;

				case SerializationType.JSON:
					result = FromJSON(sourcePath);
					break;

				default:
					result = default(T);
					break;
			}

			return result;
		}

		private T FromJSON(string sourcePath)
		{
			T result = default;
			JsonSerializer js = new JsonSerializer();
			
			using (TextReader textReader = new StreamReader(File.OpenRead(sourcePath)))
			{
				using(JsonReader jsonReader = new JsonTextReader(textReader))
				{
					result = js.Deserialize<T>(jsonReader);
				}
				textReader.Close();
			}

			return result;
		}

		private T FromXML(string sourcePath)
		{
			T result = default;
			XmlSerializer xs = new XmlSerializer(typeof(T));

			using(FileStream fs = File.OpenRead(sourcePath))
			{
				result = (T)xs.Deserialize(fs);
			}
			return result;
		}

		private T FromBinary(string sourcePath)
		{
			T result = default;
			BinaryFormatter bf = new BinaryFormatter();
			using(FileStream fs = File.OpenRead(sourcePath))
			{
				result = (T)bf.Deserialize(fs);
				fs.Close();
			}
			return result;
		}
	}
}
