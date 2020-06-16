using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync.IO
{
	public interface IDeserializer<T> where T : ISerializable, new()
	{
		T Deserialize(string sourcePath, SerializationType serializationType);
	}
}
