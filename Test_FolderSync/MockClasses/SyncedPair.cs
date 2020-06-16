using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using FolderSync.FileSystem.Listening;

namespace Test_FolderSync.MockClasses
{
	public class SyncedPair : ISyncFolderPair
	{
		public string LocalPath { get; } = @".\";

		public string RemotePath { get; } = @".\";

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}
	}
}
