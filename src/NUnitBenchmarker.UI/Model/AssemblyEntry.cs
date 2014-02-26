using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace NUnitBenchmarker.UI.Model
{
	public class AssemblyEntry : ReflectionEntry
	{
		public override IEnumerable<ReflectionEntry> GetChildren()
		{
			Assembly assembly = Assembly.LoadFrom(Path);
			return assembly
				.Types()
				//.Where(type => type.Implements(interfaceType))
				//.Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
				.Select(x => new NameSpaceEntry
				{
					Path = Path,
					AssemblyFullName = assembly.FullName,
					Assembly = assembly,

					Name = NormalizeNamespace(x),
					Description = NormalizeNamespace(x),
					LeafEntry = false
				}).Distinct().OrderBy(e => e.Name).ToList();
		}

		//private static string GetTopLevelNamespace(Type t)
		//{
		//	var ns = t.Namespace ?? "";
		//	var firstDot = ns.IndexOf('.');
		//	return firstDot == -1 ? ns : ns.Substring(0, firstDot);
		//}
	}
}