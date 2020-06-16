using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FolderSync.FileSystem.Listening
{
	[Serializable]
	public class SyncedPair : ISyncFolderPair, ISerializable
	{

		public string LocalPath { get; } = string.Empty;

		public string RemotePath { get; } = string.Empty;

		public SyncedPair()
		{

		}

		public SyncedPair(string localPath, string remotePath)
		{
			LocalPath = localPath;
			RemotePath = remotePath;
		}

		public SyncedPair(SerializationInfo info, StreamingContext context)
		{
			LocalPath = (string)info.GetValue("LocalPath", typeof(string));
			RemotePath = (string)info.GetValue("RemotePath", typeof(string));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("LocalPath", LocalPath);
			info.AddValue("RemotePath", RemotePath);
		}
	}
}
