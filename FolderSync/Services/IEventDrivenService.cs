using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace FolderSync.Services
{
	public interface IEventDrivenService
	{
		Timer StayAlive { get; }
	}
}
