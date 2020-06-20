using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
                string sourcePath = @".\SyncedFolders.json";
                if (!File.Exists(sourcePath)) { using (FileStream fs = File.Create(sourcePath)) { fs.Close(); }  }
                using (JsonReader reader = new JsonTextReader(new StreamReader(File.OpenRead(sourcePath))))
                {
                    pairs = new JsonSerializer().Deserialize<ISyncFolderPair[]>(reader);
                }
                if(pairs == null) { pairs = new SyncedPair[] { new SyncedPair("", "") }; }
            }
            INetworkInfoProvider networkInfoProvider;
            {
                string sourcePath = @".\NetworkInfo.json";
                if (!File.Exists(sourcePath)) { CreateNetworkInfo(sourcePath); }
                using (JsonReader reader = new JsonTextReader(new StreamReader(File.OpenRead(sourcePath))))
                {
                    networkInfoProvider = new JsonSerializer().Deserialize<NetworkInfoProvider>(reader);
                }
            }
            IValidator validator = new RemoteConnectionValidator(networkInfoProvider);

            //Register types here, in order of program flow;
            builder.RegisterType<Application>().As<IApplication>();
            builder.RegisterType<ServiceRunner>().As<IServiceRunner>();

            builder.RegisterInstance(networkInfoProvider).As<INetworkInfoProvider>();

            builder.RegisterInstance(pairs).As<ISyncFolderPair[]>();
            builder.RegisterType<SyncFolderGroup>().As<ISyncFolderGroup>();
            builder.RegisterType<ModifierHost>().As<IFileModifier>();
            builder.RegisterInstance(validator).As<IValidator>();

            
            


            return builder.Build();
        }

        private static void CreateNetworkInfo(string sourcePath)
        {
            Serializer<NetworkInfoProvider> serializer = new Serializer<NetworkInfoProvider>();
            NetworkInfoProvider info = new NetworkInfoProvider();
            serializer.SerializeToFile(info, SerializationType.JSON, sourcePath);
        }
    }
}
