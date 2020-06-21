using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync.IO
{
	public interface ISerializer<T> where T: new()
	{
		void SerializeToFile(T serializable, SerializationType serializationType, string outfile);
	}
}
