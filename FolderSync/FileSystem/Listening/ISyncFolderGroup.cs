using FolderSync.Services;

namespace FolderSync.FileSystem.Listening
{
	public interface ISyncFolderGroup
	{
		IEventDrivenService[] GetServices();
	}
}