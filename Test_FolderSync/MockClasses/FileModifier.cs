using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderSync.FileSystem.Editing;
using FolderSync.FileSystem.Listening;

namespace Test_FolderSync.MockClasses
{
	class FileModifier : IFileModifier
	{
		public Checker _checker { get; private set; }

		public FileModifier(Checker checker)
		{
			_checker = checker;
		}


		public void ModifyFile(ISyncedFolder syncFolder, string filePath, UpdateType updateType, string renameFilename)
		{
			_checker.Message = "ModifyFile";
		}

		public void ModifyFolder(ISyncedFolder syncFolder, string folderPath, UpdateType updateType, string renameFile)
		{
			_checker.Message = "ModifyFolder";
		}
	}
}
