namespace FolderSyncService
{
    public class Application : IApplication
    {
        IServiceRunner _serviceRunner;

        public Application(IServiceRunner runner)
        {
            _serviceRunner = runner;
        }

        public void Start()
        {
            _serviceRunner.Start();
        }

        public void Stop()
        {
            _serviceRunner.Stop();
        }
    }
}
