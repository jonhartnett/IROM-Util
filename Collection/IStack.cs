namespace IROM.Util
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	/// <summary>
	/// Represents a first in, last out collection of objects.
	/// </summary>
	public interface IStack<E> : ICollection<E>
	{
		/// <summary>
		/// Pushes the given value onto the stack.
		/// </summary>
		/// <param name="value">The value to push.</param>
		void Push(E value);
		
		/// <summary>
		/// Returns and removes the top value off the stack. 
		/// Returns null if <see cref="Count"/> is 0.
		/// </summary>
		/// <returns>The top value.</returns>
		E Pop();
		
		/// <summary>
		/// Returns the top value of the stack.
		/// Returns null if <see cref="Count"/> is 0.
		/// </summary>
		/// <returns>The top value.</returns>
		E Top
		{
			get;
		}
	}
}
