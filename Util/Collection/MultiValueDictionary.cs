namespace IROM.Util
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	
	/// <summary>
	/// A <see cref="Dictionary{T, T2}">Dictionary</see> implementation that allows multiple values per key.
	/// </summary>
	public class MultiValueDictionary<K, V>
	{
		private readonly Dictionary<K, HashSet<V>> Underlier = new Dictionary<K, HashSet<V>>();

		/// <summary>
		/// Adds the specified element to the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The element.</param>
		/// <returns>True if the element is added, false if it was already present.</returns>
		public bool Add(K key, V value)
		{
			if(!Underlier.ContainsKey(key))
			{
				Underlier[key] = new HashSet<V>();
			}
			return Underlier[key].Add(value);
		}
		
		/// <summary>
		/// Removes the specified element from the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The element.</param>
		/// <returns>True if found and removed, false if not present.</returns>
		public bool Remove(K key, V value)
		{
			if(Underlier.ContainsKey(key))
			{
				return Underlier[key].Remove(value);
			}else
			{
				return false;
			}
		}
		
		/// <summary>
		/// Returns true if the given key contains the given element.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="value">The element.</param>
		/// <returns>True if present, else false.</returns>
		public bool Contains(K key, V value)
		{
			if(Underlier.ContainsKey(key))
			{
				return Underlier[key].Contains(value);
			}
			return false;
		}
		
		/// <summary>
		/// Removes all elements from the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		public void Clear(K key)
		{
			if(Underlier.ContainsKey(key))
			{
				Underlier[key].Clear();
			}
		}
		
		/// <summary>
		/// Gets the elements for the given key.
		/// </summary>
		public IEnumerable<V> this[K key]
		{
			get
			{
				if(Underlier.ContainsKey(key))
				{
					return Underlier[key];
				}else
				{
					return Enumerable.Empty<V>();
				}
			}
		}
	}
}
