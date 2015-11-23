namespace IROM.Util
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	/// <summary>
	/// A simple collection of objects that allows basic add/remove/clear/size/enumerate functions with lockless, waitless, concurrent access.
	/// </summary>
	public class LocklessLinkedList<E> : IEnumerable<E>
	{
		/// <summary>
		/// The underlying data structure.
		/// </summary>
		private class Entry
		{
			internal E Value;
			internal Entry Next;
			internal Entry Prev;
			
			internal Entry()
			{
				Value = default(E);
				Prev = this;
				Next = this;
			}
			
			internal Entry(E value, Entry prev, Entry next)
			{
				Value = value;
				Prev = prev;
				Next = next;
			}
		}
		
		private readonly Entry Header = new Entry();
		
		/// <summary>
		/// The size of this List.
		/// </summary>
		public int Size
		{
			get;
			private set;
		}
		
		/// <summary>
		/// The first value of this List.
		/// </summary>
		public E First
		{
			get
			{
				return Header.Next.Value;
			}
		}
		
		/// <summary>
		/// The last value of this List.
		/// </summary>
		public E Last
		{
			get
			{
				return Header.Prev.Value;
			}
		}
		
		/// <summary>
		/// Adds the given value to the list.
		/// </summary>
		/// <param name="value">The value to add.</param>
		public void Add(E value)
		{
			Entry entry = new Entry(value, Header.Prev, Header);
			Header.Prev.Next = entry;
			Header.Prev = entry;
			Size++;
		}
		
		/// <summary>
		/// Removes the given value from the list.
		/// </summary>
		/// <param name="value">The value to remove.</param>
		/// <returns>True if the list contained the element.</returns>
		public bool Remove(E value)
		{
			for(Entry entry = Header.Next; entry != Header; entry = entry.Next)
			{
				if(entry.Equals(value))
				{
					Remove(entry);
					return true;
				}
			}
			return false;
		}
		
		private void Remove(Entry entry)
		{
			entry.Next.Prev = entry.Prev;
			entry.Prev.Next = entry.Next;
			Size--;
		}
		
		/// <summary>
		/// Clears the list of all elements.
		/// </summary>
		public void Clear()
		{
			//short circuit all iterators
			for(Entry entry = Header.Prev; entry != Header; entry = entry.Prev)
			{
				entry.Next = Header;
			}
			Header.Next = Header;
			Header.Prev = Header;
			Size = 0;
		}
		
		public IEnumerator<E> GetEnumerator()
		{
			Entry CurrentEntry = Header.Next;
			while(CurrentEntry != Header)
			{
				yield return CurrentEntry.Value;
				CurrentEntry = CurrentEntry.Next;
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
