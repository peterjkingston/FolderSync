using System;
using System.IO;
using System.Reflection;
using System.Threading;
using FolderSync.FileSystem.Editing;
using FolderSync.FileSystem.Listening;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test_FolderSync.MockClasses;

namespace Test_FolderSync.FileSystem.Editing
{
	[TestClass]
	public class ModifierHost
	{
		[TestMethod]
		public void ModifyFile_Changes_GivenValidFile()
		{
			//Arrange
			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			string tempPath = MethodBase.GetCurrentMethod().Name;
			string otherPath = Path.Combine(@".\Remote",MethodBase.GetCurrentMethod().Name);
			string testMessage = "Test";

			using (FileStream fs = File.Create(tempPath)) { fs.Close(); }
			File.WriteAllText(tempPath, testMessage);

			using (FileStream fs = File.Create(otherPath)) { fs.Close(); }

			UpdateType updateType = UpdateType.Change;

			string expected = testMessage;

			//Act
			modifierHost.ModifyFile(folder, tempPath, updateType, "");
			string actual = File.ReadAllText(otherPath);
			File.Delete(tempPath);
			File.Delete(otherPath);

			//Assert
			Assert.AreNotEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyFile_Creates_GivenValidInfo()
		{
			//Arrange
			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			string tempPath = MethodBase.GetCurrentMethod().Name;

			using (FileStream fs = File.Create(tempPath)) { fs.Close(); }
			Thread.Sleep(1);

			UpdateType updateType = UpdateType.Create;

			bool expected = true;

			//Act
			modifierHost.ModifyFile(folder, tempPath, updateType, "");
			Thread.Sleep(5);
			bool actual = File.Exists(tempPath);
			File.Delete(tempPath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyFile_Deletes_GivenValidFile()
		{
			//Arrange
			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			string tempPath = MethodBase.GetCurrentMethod().Name;
			string remotePath = $"Remote\\{tempPath}";

			using (FileStream fs = File.Create(tempPath)) { fs.Close(); }
			using (FileStream fs = File.Create(remotePath)) { fs.Close(); }
			Thread.Sleep(1);

			UpdateType updateType = UpdateType.Delete;

			bool expected = false;

			//Act
			modifierHost.ModifyFile(folder, tempPath, updateType, "");
			Thread.Sleep(1);
			bool actual = File.Exists(remotePath);
			if (File.Exists(tempPath)){ File.Delete(tempPath); }
			if (File.Exists(remotePath)) { File.Delete(remotePath); }

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyFile_Renames_GivenValidFile()
		{
			//Arrange
			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			string tempPath = MethodBase.GetCurrentMethod().Name;
			string otherPath = MethodBase.GetCurrentMethod().Name + "somethingorRather";

			using (FileStream fs = File.Create(tempPath)) { fs.Close(); }

			using (FileStream fs = File.Create(otherPath)) { fs.Close(); }

			UpdateType updateType = UpdateType.Rename;

			string expected = Path.GetFileName(tempPath);

			//Act
			modifierHost.ModifyFile(folder, tempPath, updateType, tempPath);
			Thread.Sleep(1);
			string actual = Path.GetFileName(otherPath);
			File.Delete(tempPath);
			File.Delete(otherPath);

			//Assert
			Assert.AreNotEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyFolder_Changes_GivenValidFolder()
		{
			//Arrange
			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			string tempPath = MethodBase.GetCurrentMethod().Name;
			string remotePath = $"Remote\\{tempPath}";

			Directory.CreateDirectory(tempPath);
			new DirectoryInfo(tempPath).Attributes = FileAttributes.Hidden;

			Directory.CreateDirectory(remotePath);
			Thread.Sleep(1);

			UpdateType updateType = UpdateType.Change;

			bool expected = true;

			//Act
			modifierHost.ModifyFolder(folder, tempPath, updateType, "");
			Thread.Sleep(1);
			bool actual = new DirectoryInfo(remotePath).Attributes == new DirectoryInfo(tempPath).Attributes;
			if (Directory.Exists(tempPath)) { Directory.Delete(tempPath); }
			if (Directory.Exists(remotePath)) { Directory.Delete(remotePath); }

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyFolder_Creates_GivenValidInfo()
		{
			//Arrange
			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			string tempPath = MethodBase.GetCurrentMethod().Name;
			string remotePath = $"Remote\\{tempPath}";

			Directory.CreateDirectory(tempPath);
			Thread.Sleep(1);

			UpdateType updateType = UpdateType.Create;

			bool expected = true;

			//Act
			modifierHost.ModifyFolder(folder, tempPath, updateType, "");
			Thread.Sleep(1);
			bool actual = Directory.Exists(remotePath);
			if (Directory.Exists(tempPath)) { Directory.Delete(tempPath); }
			if (Directory.Exists(remotePath)) { Directory.Delete(remotePath); }

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyFolder_Deletes_GivenValidFolder()
		{
			//Arrange
			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			string tempPath = MethodBase.GetCurrentMethod().Name;
			string remotePath = $"Remote\\{tempPath}";

			Directory.CreateDirectory(tempPath);
			Directory.CreateDirectory(remotePath);
			Thread.Sleep(1);

			UpdateType updateType = UpdateType.Delete;

			bool expected = false;

			//Act
			modifierHost.ModifyFolder(folder, tempPath, updateType, "");
			Thread.Sleep(1);
			bool actual = Directory.Exists(remotePath);
			if (Directory.Exists(tempPath)) { Directory.Delete(tempPath); }
			if (Directory.Exists(remotePath)) { Directory.Delete(remotePath); }

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void ModifyFolder_Renames_GivenValidFolder()
		{
			//Arrange
			string tempPath = MethodBase.GetCurrentMethod().Name;
			string otherPath = MethodBase.GetCurrentMethod().Name + "somethingorRather";

			Directory.CreateDirectory(tempPath);
			Directory.CreateDirectory(otherPath);

			Checker checker = new Checker();
			ISyncedFolder folder = new MockClasses.SyncedFolder(checker);
			FolderSync.FileSystem.Editing.ModifierHost modifierHost = new FolderSync.FileSystem.Editing.ModifierHost();
			

			UpdateType updateType = UpdateType.Rename;

			string expected = Path.GetFileName(tempPath);

			//Act
			modifierHost.ModifyFile(folder, tempPath, updateType, tempPath);
			Thread.Sleep(1);
			string actual = Path.GetFileName(otherPath);
			Directory.Delete(tempPath);
			Directory.Delete(otherPath);

			//Assert
			Assert.AreNotEqual(expected, actual);
		}
	}
}
