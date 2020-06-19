using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderSync.FileSystem.Listening;

namespace FolderSync.FileSystem.Editing
{
	public class ModifierHost : IFileModifier
	{
        public ISyncedFolder SyncedFolder { get; set; }

        public ModifierHost()
        {
        }


		public void ModifyFile(ISyncedFolder syncFolder, string filePath, UpdateType updateType, string renameFilename)
		{
            SyncedFolder = syncFolder;
            string remotePath = GetRemotePath(filePath);
            switch (updateType)
            {
                case UpdateType.Change | UpdateType.Create:
                    File.Copy(filePath, remotePath, true);
                    break;

                case UpdateType.Delete:
                    if (File.Exists(remotePath)) { File.Delete(remotePath); }
                    break;

                case UpdateType.Rename:
                    if (File.Exists(remotePath)) { File.Move(remotePath, renameFilename); }
                    break;

                default:
                    break;
            }
        }

		public void ModifyFolder(ISyncedFolder syncFolder, string folderPath, UpdateType updateType, string renameFile)
		{
            SyncedFolder = syncFolder;
            string remotePath = GetRemotePath(folderPath);
            switch (updateType)
            {
                case UpdateType.Change:
                    DirectoryInfo localInfo = new DirectoryInfo(folderPath);
                    DirectoryInfo remoteInfo = new DirectoryInfo(remotePath);
                    remoteInfo.Attributes = localInfo.Attributes;
                    break;

                case UpdateType.Create:
                    Directory.CreateDirectory(remotePath);
                    break;

                case UpdateType.Delete:
                    Directory.Delete(remotePath);
                    break;

                case UpdateType.Rename:
                    Directory.Move(remotePath, renameFile);
                    break;

                default:
                    break;
            }
        }

        private string GetRemotePath(string filePath)
        {
            char separator = Path.PathSeparator;
            string[] localPathNodes = SyncedFolder.LocalPath.Split(separator);
            string[] remotePathNodes = SyncedFolder.RemotePath.Split(separator);
            string[] thisFilePathNodes = filePath.Split(separator);
            List<string> resultingNodeList = new List<string>(remotePathNodes);

            for(int i = localPathNodes.Length-1; i < thisFilePathNodes.Length; i++)
            {
                resultingNodeList.Add(thisFilePathNodes[i]);
            }

            return Path.Combine(resultingNodeList.ToArray());
        }
    }
}
