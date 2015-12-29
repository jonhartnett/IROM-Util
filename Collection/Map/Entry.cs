namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// An entry in a Dictionary.
	/// </summary>
	public class Entry<K, V>
	{
		public K Key;
		public V Value;
		
		public Entry()
		{
			
		}
		
		public Entry(K key, V value)
		{
			Key = key;
			Value = value;
		}
	}
}
