using System.Collections.Generic;
using System.IO;
using Autofac;
using FolderSync.FileSystem.Editing;
using FolderSync.FileSystem.Listening;
using FolderSync.FileSystem.Validation;
using FolderSync.IO;
using FolderSync.Network;
using Newtonsoft.Json;

namespace FolderSyncService
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            ContainerBuilder builder = new ContainerBuilder();

            ISyncFolderPair[] pairs;
            {
                string sourcePath = @".\SyncedFolders";
                using (JsonReader reader = new JsonTextReader(new StreamReader(File.OpenRead(sourcePath))))
                {
                    pairs = (ISyncFolderPair[])(new JsonSerializer().Deserialize(reader));
                }
            }
            INetworkInfoProvider networkInfoProvider;
            {
                string sourcePath = @".\NetworkInfo";
                using (JsonReader reader = new JsonTextReader(new StreamReader(File.OpenRead(sourcePath))))
                {
                    networkInfoProvider = (INetworkInfoProvider)(new JsonSerializer().Deserialize(reader));
                }
            }
            IValidator validator = new RemoteConnectionValidator(networkInfoProvider);

            //Register types here, in order of program flow;
            builder.RegisterType<ServiceRunner>().As<IServiceRunner>();

            builder.RegisterInstance(pairs).As<ISyncFolderPair[]>();
            builder.RegisterType<SyncFolderGroup>().As<ISyncFolderGroup>();
            builder.RegisterType<ModifierHost>().As<IFileModifier>();
            builder.RegisterInstance(validator).As<IValidator>();



            return builder.Build();
        }
    }
}
