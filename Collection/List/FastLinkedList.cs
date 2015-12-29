namespace IROM.Util
{
	using System;
	using System.Threading;
	using System.Collections;
	using System.Collections.Generic;
	
	/// <summary>
	/// A doubly-linked list that has lockless, waitless, concurrent access.
	/// Also has a reduced memory footprint compared to <see cref="System.Collections.Generic.LinkedList{T}"/>.
	/// Follows the principle of speed over safety. Assumes inputs are valid, if you need validity checking, perform it yourself.
	/// </summary>
	public class FastLinkedList<E> : ICollection<E>, IStack<E>, IQueue<E>
	{
		private readonly DoubleNode<E> root = new DoubleNode<E>();
		
		public int Count 
		{
			get;
			private set;
		}
		
		public bool IsReadOnly {get{return false;}}
		
		/// <summary>
		/// Returns the first value of the <see cref="LinkedList{E}"/>, or default(E) if it is empty.
		/// </summary>
		/// <returns>The first value.</returns>
		public E First 
		{
			get 
			{
				DoubleNode<E> node = root.Next;
				if(node != root) return node.Value;
				else			 return default(E);
			}
		}

		/// <summary>
		/// Returns the last value of the <see cref="LinkedList{E}"/>, or default(E) if it is empty.
		/// </summary>
		/// <returns>The last value.</returns>
		public E Last 
		{
			get
			{
				DoubleNode<E> node = root.Prev;
				if(node != root) return node.Value;
				else			 return default(E);
			}
		}
		
		public E Top
		{
			get{return First;}
		}
		
		public FastLinkedList()
		{
			
		}

		public FastLinkedList(IEnumerable<E> collection)
		{
			foreach(E value in collection) 
			{
				Add(value);
			}
		}
		
		public void Add(E item)
		{
			AddLast(item);
		}
		
		/// <summary>
		/// Adds the given item to the end of the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void AddLast(E item)
		{
			DoubleNode<E> node = new DoubleNode<E>(item, root.Prev, root);
			root.Prev = root.Prev.Next = node;
			Count++;
		}
		
		/// <summary>
		/// Adds the given item to the front of the list.
		/// </summary>
		/// <param name="item">The item to add.</param>
		public void AddFirst(E item)
		{
			DoubleNode<E> node = new DoubleNode<E>(item, root.Prev, root);
			root.Next = root.Next.Prev = node;
			Count++;
		}
		
		public bool Remove(E item)
		{
			for(DoubleNode<E> node = root.Next; node != root; node = node.Next)
			{
				if(EqualityComparer<E>.Default.Equals(node.Value, item))
				{
					Remove(node);
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Removes the given node from the collection.
		/// Assumes that the node is actually part of the collection.
		/// </summary>
		/// <param name="node">The node to remove.</param>
		public void Remove(DoubleNode<E> node)
		{
			node.Prev.Next = node.Next;
			node.Next.Prev = node.Prev;
			Count--;
		}

		public void Push(E value)
		{
			AddFirst(value);
		}
		
		public E Pop()
		{
			DoubleNode<E> first = root.Next;
			if(first != root)
			{
				Remove(first);
				return first.Value;
			}else
			{
				return default(E);
			}
		}
		
		public void Queue(E value)
		{
			AddLast(value);
		}
		
		public E Poll()
		{
			//same operation as pop
			return Pop();
		}		
		
		public void Clear()
		{
			root.Next = root.Prev = root;
			Count = 0;
		}
		
		public bool Contains(E item)
		{
			for(DoubleNode<E> node = root.Next; node != root; node = node.Next)
			{
				if(EqualityComparer<E>.Default.Equals(node.Value, item))
					return true;
			}
			return false;
		}
		
		public void CopyTo(E[] array, int arrayIndex)
		{
			for(DoubleNode<E> node = root.Next; node != root; node = node.Next, arrayIndex++)
			{
				array[arrayIndex] = node.Value;
			}
		}
		
		public IEnumerator<E> GetEnumerator()
		{
			for(DoubleNode<E> node = root.Next; node != root; node = node.Next)
			{
				yield return node.Value;
			}
		}
		
		public IEnumerable<DoubleNode<E>> GetNodes()
		{
			for(DoubleNode<E> node = root.Next; node != root; node = node.Next)
			{
				yield return node;
			}
		}
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
