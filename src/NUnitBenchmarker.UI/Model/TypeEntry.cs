using System.Collections.Generic;

namespace NUnitBenchmarker.UI.Model
{
	public class TypeEntry : ReflectionEntry
	{
		public string TypeFullName { get; set; }
		public override IEnumerable<ReflectionEntry> GetChildren()
		{
			return new List<ReflectionEntry>();
		}
	}
}