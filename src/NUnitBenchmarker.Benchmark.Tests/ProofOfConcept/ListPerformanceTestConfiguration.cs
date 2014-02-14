using System;
using System.Collections.Generic;

namespace NUnitBenchmarker.Benchmark.Tests.ProofOfConcept
{
	/// <summary>
	/// Class ListPerformanceTestConfiguration.
	/// TODO: Revise if generic T parameter is useful or overengineered
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListPerformanceTestConfiguration<T> 
	{
		private readonly Random random;

		public string Identifier { get; set; }
		public Type ListType { get; set; }
		public int Size { get; private set; }
		public int TestDelay { get; private set; }

		
		public ListPerformanceTestConfiguration(string identifier, Type listType, int size, int testDelay)
		{
			Identifier = identifier;
			ListType = listType;
			Size = size;
			TestDelay = testDelay;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			// TODO: Configuration's string representation is currently used (with int.Parse) PlotResults
			// This holds the chance a future breaking change. Refactor this with a dictionary like data 
			// exchange like key/value or an anonymous type where the propertyname/value pairs serves the 
			// same data exchange functionality.
			return string.Format("{0}_{1}", Size, TestDelay);
			//return string.Format("{0}", Size);
		}
	}
}