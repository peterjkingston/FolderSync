using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FolderSync.FileSystem.Editing;
using FolderSync.FileSystem.Listening;
using FolderSync.FileSystem.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Test_FolderSync.MockClasses;

namespace Test_FolderSync.FileSystem.Listening
{
	[TestClass]
	public class SyncedFolder
	{
		[TestMethod]
		public void SyncFile_ChecksValidation_GivenValidator()
		{
			//Arrange
			Checker checker = new Checker();
			Validator validator = new Validator(true, checker);
			FileModifier fileMod = new FileModifier(new Checker());
			ISyncFolderPair pair = new MockClasses.SyncedPair();
			ISyncedFolder syncedFolder = new FolderSync.FileSystem.Listening.SyncedFolder(pair, validator, fileMod);
			
			string expected = "Valid";

			//Act
			syncedFolder.SyncFile(syncedFolder.LocalPath, UpdateType.Change);
			string actual = checker.Message;

			//Assert
			Assert.AreEqual(expected,actual);
		}

		[TestMethod]
		public void SyncFolder_ChecksValidation_GivenValidator()
		{
			//Arrange
			Checker checker = new Checker();
			Validator validator = new Validator(true, checker);
			FileModifier fileMod = new FileModifier(new Checker());
			ISyncFolderPair pair = new MockClasses.SyncedPair();
			ISyncedFolder syncedFolder = new FolderSync.FileSystem.Listening.SyncedFolder(pair, validator, fileMod);

			string expected = "Valid";

			//Act
			syncedFolder.SyncFolder(syncedFolder.LocalPath, UpdateType.Change);
			string actual = checker.Message;

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Class_SelectsFileOnEvent_GivenFile()
		{
			//Arrange
			Checker checker = new Checker();
			Validator validator = new Validator(true, checker);
			FileModifier fileMod = new FileModifier(checker);
			ISyncFolderPair pair = new MockClasses.SyncedPair();
			ISyncedFolder syncedFolder = new FolderSync.FileSystem.Listening.SyncedFolder(pair, validator, fileMod);
			string tempPath = MethodBase.GetCurrentMethod().Name;
			using (FileStream fs = File.Create(tempPath)) { fs.Close(); }
			Thread.Sleep(2);

			string expected = "ModifyFile";

			//Act
			string actual = checker.Message;
			File.Delete(tempPath);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Class_NotSelectsFileOnEvent_GivenFolder()
		{
			//Arrange
			Checker checker = new Checker();
			Validator validator = new Validator(true, checker);
			FileModifier fileMod = new FileModifier(checker);
			ISyncFolderPair pair = new MockClasses.SyncedPair();
			ISyncedFolder syncedFolder = new FolderSync.FileSystem.Listening.SyncedFolder(pair, validator, fileMod);
			string tempPath = MethodBase.GetCurrentMethod().Name;

			string expected = "ModifyFile";

			//Act
			DirectoryInfo info = Directory.CreateDirectory(tempPath);
			Thread.Sleep(1);
			string actual = checker.Message;
			Directory.Delete(info.FullName);

			//Assert
			Assert.AreNotEqual(expected, actual);
		}

		[TestMethod]
		public void Class_SelectsFolderOnEvent_GivenFolder()
		{
			//Arrange
			Checker checker = new Checker();
			Validator validator = new Validator(true, checker);
			FileModifier fileMod = new FileModifier(checker);
			ISyncFolderPair pair = new MockClasses.SyncedPair();
			ISyncedFolder syncedFolder = new FolderSync.FileSystem.Listening.SyncedFolder(pair, validator, fileMod);
			string tempPath = MethodBase.GetCurrentMethod().Name;

			string expected = "ModifyFolder";

			//Act
			DirectoryInfo info = Directory.CreateDirectory(tempPath);
			Thread.Sleep(1);
			string actual = checker.Message;
			Directory.Delete(info.FullName);

			//Assert
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void Class_NotSelectsFolderOnEvent_GivenFile()
		{
			//Arrange
			Checker checker = new Checker();
			Validator validator = new Validator(true, checker);
			FileModifier fileMod = new FileModifier(checker);
			ISyncFolderPair pair = new MockClasses.SyncedPair();
			ISyncedFolder syncedFolder = new FolderSync.FileSystem.Listening.SyncedFolder(pair, validator, fileMod);
			string tempPath = MethodBase.GetCurrentMethod().Name;

			string expected = "ModifyFolder";
			

			//Act
			using (FileStream fs = File.Create(tempPath))
			{
				fs.Close();
			}
			string actual = checker.Message;
			File.Delete(tempPath);

			//Assert
			Assert.AreNotEqual(expected, actual);
		}
	}
}
