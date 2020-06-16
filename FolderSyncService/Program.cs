using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FolderSyncService
{
	static class Program
	{
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //Using the Topshelf method.
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<IApplication>(s =>
                {
                    s.ConstructUsing(pDaddy => GetApplication());
                    s.WhenStarted(pDaddy => pDaddy.Start());
                    s.WhenStopped(pDaddy => pDaddy.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("FolderSync");
                x.SetDisplayName("FolderSync");
                x.SetDescription("This service modifies files from specified directories to another whenever a change is made to the originating directory.");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }

        private static IApplication GetApplication()
        {
            var container = ContainerConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var app = scope.Resolve<IApplication>();

                return app;
            }
        }
    }
}
