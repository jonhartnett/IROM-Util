namespace IROM.Util
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	
	/// <summary>
	/// A <see cref="Dictionary{T, T2}">Dictionary</see> implementation that allows multiple values per key.
	/// </summary>
	public class MultiValueDictionary<K, V> : Dictionary<K, V[]>
	{
		/// <summary>
		/// Returns the values for the given key, or an empty array if the key does not exist.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns>Returns the values.</returns>
		public V[] GetValues(K key)
		{
			V[] result;
			if(!TryGetValue(key, out result))
			{
				result = new V[0];
			}
			return result;
		}
		
		/// <summary>
		/// Adds the specified element to the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The element.</param>
		public void Add(K key, V value)
		{
			V[] array;
			if(TryGetValue(key, out array))
			{
				ArrayUtil.Add(ref array, value);
			}else
			{
				array = new V[]{value};
			}
			this[key] = array;
		}
		
		/// <summary>
		/// Removes the specified element from the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The element.</param>
		/// <returns>True if found and removed, false if not present.</returns>
		public bool Remove(K key, V value)
		{
			V[] array;
			if(TryGetValue(key, out array))
			{
				bool found = ArrayUtil.Remove(ref array, value);
				this[key] = array;
				return found;
			}
			return false;
		}
		
		/// <summary>
		/// Removes all elements from the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Clear(K key)
		{
			this[key] = new V[0];
		}
	}
}
