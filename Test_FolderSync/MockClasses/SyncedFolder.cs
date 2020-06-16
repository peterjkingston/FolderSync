using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderSync.FileSystem.Editing;
using FolderSync.FileSystem.Listening;

namespace Test_FolderSync.MockClasses
{
	class SyncedFolder : ISyncedFolder
	{
		public string RemotePath { get; }

		public string LocalPath => ".\\";

		Checker _checker;

		public SyncedFolder(Checker checker)
		{
			RemotePath = Directory.CreateDirectory(".\\Remote").FullName;
			_checker = checker;
		}

		~SyncedFolder()
		{
			Directory.Delete(RemotePath);
		}

		public void SyncFile(string filePath, UpdateType updateType, string renameFilename = "")
		{
			_checker.Message = "SyncFile";
		}

		public void SyncFolder(string folderPath, UpdateType updateType, string renameFilename = "")
		{
			_checker.Message = "SyncFolder";
		}
	}
}
