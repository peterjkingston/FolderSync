using System.Timers;
using FolderSync.FileSystem.Editing;

namespace FolderSync.FileSystem.Listening
{
	public interface ISyncedFolder
	{
		string RemotePath { get; }
		string LocalPath { get; }

		void SyncFile(string filePath, UpdateType updateType, string renameFilename = "");
		void SyncFolder(string folderPath, UpdateType updateType, string renameFilename = "");
	}
}