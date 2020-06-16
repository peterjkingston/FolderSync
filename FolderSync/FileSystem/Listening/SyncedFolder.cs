using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using FolderSync.FileSystem.Editing;
using FolderSync.FileSystem.Validation;
using FolderSync.Services;

namespace FolderSync.FileSystem.Listening
{
    public class SyncedFolder : ISyncedFolder, IEventDrivenService
    {
        public string LocalPath { get; private set; }
        public string RemotePath { get; private set; }

        public Timer StayAlive { get; } = new Timer(60_000) { AutoReset = true };
        private IValidator _validator;
        private IFileModifier _fileModifier;
        FileSystemWatcher _watcher;

        public SyncedFolder(ISyncFolderPair syncedPair, IValidator validator, IFileModifier fileCopier)
        {
            LocalPath = syncedPair.LocalPath;
            _watcher = new FileSystemWatcher(LocalPath);
            _watcher.EnableRaisingEvents = true;
            _watcher.Changed += On_Watcher_Changed;
            _watcher.Created += On_Watcher_Created;
            _watcher.Deleted += On_Watcher_Deleted;
            _watcher.Renamed += On_Watcher_Renamed;

            RemotePath = syncedPair.RemotePath;

            _validator = validator;
            _fileModifier = fileCopier;
        }

        private void On_Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            SelectAction(UpdateType.Rename, e.OldFullPath, e.FullPath);
        }

        private void On_Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            SelectAction(UpdateType.Delete, e.FullPath);
        }

        private void On_Watcher_Created(object sender, FileSystemEventArgs e)
        {
            SelectAction(UpdateType.Create, e.FullPath);
        }

        private void On_Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            SelectAction(UpdateType.Change, e.FullPath);
        }

        private void SelectAction(UpdateType updateType, string pathActioned, string renameFilename = "")
        {
            if (File.Exists(pathActioned)) { SyncFile(pathActioned, updateType); }
            else if (Directory.Exists(pathActioned)) { SyncFolder(pathActioned, updateType); }
            else { throw new FileNotFoundException(); };
        }

        public void SyncFile(string filePath, UpdateType updateType, string renameFilename = "")
        {
            if (_validator.Valid())
            {
                _fileModifier.ModifyFile(filePath, updateType, renameFilename);
            }
        }

        public void SyncFolder(string folderPath, UpdateType updateType, string renameFile = "")
        {
            if (_validator.Valid())
            {
                _fileModifier.ModifyFolder(folderPath, updateType, renameFile);
            }
        }
    }
}
