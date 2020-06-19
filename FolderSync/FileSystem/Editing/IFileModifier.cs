using FolderSync.FileSystem.Listening;

namespace FolderSync.FileSystem.Editing
{
	public interface IFileModifier
	{
		void ModifyFile(ISyncedFolder syncFolder, string filePath, UpdateType updateType, string renameFilename);
		void ModifyFolder(ISyncedFolder syncFolder, string folderPath, UpdateType updateType, string renameFile);
	}
}