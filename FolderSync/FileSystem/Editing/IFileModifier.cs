namespace FolderSync.FileSystem.Editing
{
	public interface IFileModifier
	{
		void ModifyFile(string filePath, UpdateType updateType, string renameFilename);
		void ModifyFolder(string folderPath, UpdateType updateType, string renameFile);
	}
}