namespace FolderSync.FileSystem.Listening
{
	public interface ISyncFolderGroup
	{
		ISyncFolderPair[] GetSyncFolders();
	}
}