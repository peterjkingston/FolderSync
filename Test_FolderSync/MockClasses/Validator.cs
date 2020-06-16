using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FolderSync.FileSystem.Validation;

namespace Test_FolderSync.MockClasses
{
	class Validator : IValidator
	{
		public Checker _checker { get; private set; }
		bool _returnVal;
		public bool Ran = false;

		public Validator(bool returnVal, Checker checker)
		{
			_checker = checker;
			_returnVal = returnVal;
		}
		public bool Valid()
		{
			_checker.Message = "Valid";
			Ran = true;
			return _returnVal;
		}
	}
}
