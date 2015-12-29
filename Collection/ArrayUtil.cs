namespace IROM.Util
{
	using System;
	using System.Threading;
	
	/// <summary>
	/// Provides helper functions for modifing arrays.
	/// </summary>
	public static class ArrayUtil
	{
		/// <summary>
		/// Adds the given value to the end of the given array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="value">The value to add.</param>
		public static void Add<E>(ref E[] array, E value)
		{
			Add(ref array, value, array.Length);
		}
		
		/// <summary>
		/// Adds the given value at the given index of the given array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="value">The value to add.</param>
		/// <param name="index">The index to put it.</param>
		public static void Add<E>(ref E[] array, E value, int index)
		{
			E[] newArray = new E[array.Length + 1];
			//copy old values
			for(int i = 0; i < index; i++) newArray[i] = array[i];
			//add new value
			newArray[index] = value;
			//copy rest of old values
			for(int i = index; i < array.Length; i++) newArray[i+1] = array[i];
			//set array to new
			array = newArray;
		}
		
		/// <summary>
		/// Removes the given value from the given array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="value">The value to remove.</param>
		/// <returns>Returns true if the value was found and removed.</returns>
		public static bool Remove<E>(ref E[] array, E value)
		{
			int index = Array.IndexOf(array, value);
			if(index == -1) return false;
			//find value in array and the remove the index
			RemoveIndex(ref array, index);
			return true;
		}
		
		/// <summary>
		/// Removes the given index from the given array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="index">The index to remove.</param>
		public static void RemoveIndex<E>(ref E[] array, int index)
		{
			//remove the index
			E[] newArray = new E[array.Length - 1];
			//copy values
			for(int i = 0;         i < index;          i++) newArray[i] = array[i];
			for(int i = index + 1; i < array.Length; i++) newArray[i - 1] = array[i];
			array = newArray;
		}
		
		/// <summary>
		/// Locks around the given array reference instead of a specific array object.
		/// </summary>
		/// <param name="array">The array reference to lock.</param>
		/// <returns>Returns the unlock action.</returns>
		public static IDisposable Lock<E>(ref E[] array)
		{
			while(true)
			{
				E[] snapshot = array;
				//get lock on array
				Monitor.Enter(snapshot);
				//if array changed while we waited, try entering again
				if(snapshot != array)
				{
					Monitor.Exit(snapshot);
					continue;
				}
				return new Unlocker{obj = snapshot};
			}
		}
		
		/// <summary>
		/// Lock object for a specific array.
		/// </summary>
		private struct Unlocker : IDisposable
		{
			internal object obj;
			public void Dispose(){Monitor.Exit(obj);}
		}
	}
}
