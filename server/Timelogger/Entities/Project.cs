using System;
using Timelogger.Infrastructure.Entities.Base;

namespace Timelogger.Infrastructure.Entities
{
	public class Project : BaseEntity
	{
		public string Name { get; set; }
		public DateTime Deadline { get; set; }
	}
}
