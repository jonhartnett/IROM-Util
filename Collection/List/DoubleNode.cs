namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A node in a doubly-linked list.
	/// </summary>
	public class DoubleNode<E>
	{
		public E Value;
		public DoubleNode<E> Next;
		public DoubleNode<E> Prev;
		
		public DoubleNode()
		{
			Value = default(E);
			Prev = this;
			Next = this;
		}
		
		public DoubleNode(E value, DoubleNode<E> prev, DoubleNode<E> next)
		{
			Value = value;
			Prev = prev;
			Next = next;
		}
	}
}
