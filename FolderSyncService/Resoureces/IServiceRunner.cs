using System;

namespace FolderSyncService
{
	public interface IServiceRunner
	{
		event EventHandler Started;
		event EventHandler Stopped;

		void Start();
		void Stop();
	}
}