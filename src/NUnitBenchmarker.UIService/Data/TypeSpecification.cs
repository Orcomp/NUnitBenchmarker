using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace NUnitBenchmarker.UIService.Data
{
	[DataContract]
	public class TypeSpecification
	{
		public TypeSpecification()
		{
		}

		public TypeSpecification(Type type)
		{
			FullName = type.FullName;
			AssemblyPath = type.Assembly.CodeBase;
		}

		[DataMember]
		public string AssemblyPath { get; set; }

		[DataMember]
		public string FullName { get; set; }
	}
}
