namespace FolderSyncService
{
    public class Application : IApplication
    {
        IServiceRunner _folderSync;

        public Application(IServiceRunner folderSync)
        {
            _folderSync = folderSync;
        }

        public void Start()
        {
            _folderSync.Start();
        }

        public void Stop()
        {
            _folderSync.Stop();
        }
    }
}
