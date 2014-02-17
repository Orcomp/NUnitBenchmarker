using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NUnitBenchmarker.UIService.Data
{
	[DataContract]
	public class BenchmarkResult
	{
		[DataMember]
		public string Key { get; set; }

		[DataMember]
		public Dictionary<string, List<KeyValuePair<string, double>>> Values { get; set; }

		[DataMember]
		public string[] TestCases { get; set; }

		[DataMember]
		public bool IsLast { get; set; }
	}
}
