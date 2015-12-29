namespace IROM.Util
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	/// <summary>
	/// Represents a first in, first out collection of objects.
	/// </summary>
	public interface IQueue<E> : ICollection<E>
	{
		/// <summary>
		/// Adds the given value to the queue.
		/// </summary>
		/// <param name="value">The value to queue.</param>
		void Queue(E value);
		
		/// <summary>
		/// Returns and removes the first value from the queue 
		/// Returns default(E) if the queue is empty.
		/// </summary>
		/// <returns>The first value.</returns>
		E Poll();
		
		/// <summary>
		/// Returns the first value off the stack.
		/// Returns default(E) if the queue is empty.
		/// </summary>
		/// <returns>The first value.</returns>
		E First
		{
			get;
		}
	}
}
