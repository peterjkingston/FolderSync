using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderSync.FileSystem.Editing;
using FolderSync.FileSystem.Validation;

namespace FolderSync.FileSystem.Listening
{
	public class SyncFolderGroup : ISyncFolderGroup
	{
		private IEnumerable<ISyncFolderPair> _syncFolderPairs;
		private IValidator _validator;
		private IFileModifier _fileModifier;

		public SyncFolderGroup(IEnumerable<ISyncFolderPair> syncFolderPairs, IValidator validator, IFileModifier fileModifier)
		{
			_syncFolderPairs = syncFolderPairs;
			_validator = validator;
			_fileModifier = fileModifier;
		}

		public ISyncFolderPair[] GetSyncFolders()
		{
			List<SyncedFolder> syncedFolders = new List<SyncedFolder>();
			foreach (ISyncFolderPair pair in _syncFolderPairs)
			{
				syncedFolders.Add(new SyncedFolder(pair, _validator, _fileModifier));
			}
			return (ISyncFolderPair[])syncedFolders.ToArray();
		}
	}
}
