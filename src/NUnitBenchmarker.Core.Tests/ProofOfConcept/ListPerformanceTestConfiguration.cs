using System;
using System.Collections.Generic;

namespace NUnitBenchmarker.Core.Tests.ProofOfConcept
{
	/// <summary>
	/// Class ListPerformanceTestConfiguration.
	/// TODO: Revise if generic T parameter is useful or overengineered
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListPerformanceTestConfiguration<T> 
	{
		private readonly Random random;

		/// <summary>
		/// Gets the size to be test.
		/// </summary>
		/// <value>The size.</value>
		public int Size { get; private set; }
		
		/// <summary>
		/// Gets the test target list implementation.
		/// </summary>
		/// <value>The list.</value>
		public IList<T> List { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ListPerformanceTestConfiguration{T}"/> class.
		/// </summary>
		/// <param name="list">Test target list.</param>
		/// <param name="size">The target list size in test cases.</param>
		public ListPerformanceTestConfiguration(IList<T> list, int size)
		{
			List = list;
			Size = size;
			random = new Random(0);
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
			return string.Format("{0}", Size);
		}

		/// <summary>
		/// Creates the new item to use in performance tests.
		/// </summary>
		/// <returns>Created item</returns>
		public T CreateNewItem()
		{
			if (typeof (T) == typeof (int))
			{
				return (T) (object) random.Next(int.MinValue, int.MaxValue);
			}

			return Activator.CreateInstance<T>();
		}
	}
}