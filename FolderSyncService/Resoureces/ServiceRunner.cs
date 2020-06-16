using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using FolderSync;
using FolderSync.FileSystem.Listening;
using FolderSync.Services;

namespace FolderSyncService
{
    public class ServiceRunner : IServiceRunner
    {
        public event EventHandler Started;
        public event EventHandler Stopped;

        private IEnumerable<IEventDrivenService> _services;

        /// <summary>
        /// Default constructor. Designated for use with Topshelf.HostFactory.
        /// </summary>
        public ServiceRunner(IEnumerable<IEventDrivenService> services)
        {
            _services = services;
        }

        private void Time_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        /// <summary>
        /// Designated start method for use with Topshelf.HostFactory. Starts the service.
        /// </summary>
        public void Start()
        {
            foreach (IEventDrivenService service in _services)
            {
                service.StayAlive.Elapsed += Time_Elapsed;
                service.StayAlive.Start();
            }
            OnStarted(EventArgs.Empty);
        }

        /// <summary>
        /// Designated stop method for use with Topshelf.HostFactory. Stops the service.
        /// </summary>
        public void Stop()
        {
            foreach(IEventDrivenService service in _services)
            {
                service.StayAlive.Stop();
            }
            OnStopped(new EventArgs());
        }

        protected virtual void OnStarted(EventArgs e)
        {
            Started(this, e);
        }

        protected virtual void OnStopped(EventArgs e)
        {
            Stopped(this, e);
        }
    }
}
