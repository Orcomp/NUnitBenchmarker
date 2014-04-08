using System.Collections.Generic;

namespace NUnitBenchmarker.Tests.Targets.IList
{
    /// <summary>
    /// A dummy implementation of IList interface for testing purposes
    /// Uses simple inheritance to fall back to the original .NET implementation.
    ///
    /// Represents a strongly typed list of objects that can be accessed by index. 
    /// Provides methods to search, sort, and manipulate lists.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class ListImplementationA<T> : List<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1"/> class that is empty and has the default initial capacity.
        /// </summary>
        public ListImplementationA()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1"/> class that is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0. </exception>
        public ListImplementationA(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.List`1"/> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param><exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public ListImplementationA(IEnumerable<T> collection)
            : base(collection)
        {
        }
    }
}
