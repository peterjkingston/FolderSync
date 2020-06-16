using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Test_FolderSync.MockClasses
{
	[Serializable]
	public class SerializableAnimal : ISerializable
	{
		public string Name { get; set; }
		public string Color { get; set; }
		public int Height { get; set; }
		public int Weight { get; set; }

		public SerializableAnimal()
		{

		}

		public SerializableAnimal(string name, string color, int height, int weight)
		{
			Name = name;
			Color = color;
			Height = height;
			Weight = weight;
		}

		public SerializableAnimal(SerializationInfo info, StreamingContext context)
		{
			Name = (string)info.GetValue("Name", typeof(string));
			Color = (string)info.GetValue("Color", typeof(string));
			Height = (int)info.GetValue("Height", typeof(int));
			Weight = (int)info.GetValue("Weight", typeof(int));
		}

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Name", Name);
			info.AddValue("Color", Color);
			info.AddValue("Height", Height);
			info.AddValue("Weight", Weight);
		}
	}
}
