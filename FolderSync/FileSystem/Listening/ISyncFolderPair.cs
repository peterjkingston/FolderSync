using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync.FileSystem.Listening
{
	public interface ISyncFolderPair : ISerializable
	{
		string LocalPath { get; }
		string RemotePath { get; }
	}
}
