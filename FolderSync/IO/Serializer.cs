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
	public class Serializer<T> : ISerializer<T> where T : new()
	{
		public void SerializeToFile(T serializable, SerializationType serializationType, string outfile)
		{
			switch (serializationType)
			{
				case SerializationType.Binary:
					WriteAsBinary(serializable, outfile);
					break;

				case SerializationType.XML:
					WriteAsXML(serializable, outfile);
					break;

				case SerializationType.JSON:
					WriteAsJSON(serializable, outfile);
					break;

				default:
					break;
			}
		}

		private void WriteAsJSON(T serializable, string outfile)
		{
			JsonSerializer js = new JsonSerializer();
			using(TextWriter tw = new StreamWriter(outfile))
			{
				js.Serialize(tw,serializable);
				tw.Close();
			}
		}

		private void WriteAsBinary(T serializable, string outFile)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using(FileStream fs = File.OpenWrite(outFile))
			{
				bf.Serialize(fs,serializable);
				fs.Close();
			}
			
		}

		private void WriteAsXML(T serializable, string outFile)
		{
			XmlSerializer xs = new XmlSerializer(typeof(T));
			using(TextWriter tw = new StreamWriter(outFile))
			{
				xs.Serialize(tw, serializable);
				tw.Close();
			}
		}
	}
}
