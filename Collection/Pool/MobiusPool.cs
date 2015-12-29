namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// A pool of temporary objects that are drawn from a fixed-length array of objects.
    /// Values are cycled so all objects are returned before the first object is reused.
    /// </summary>
    /// <typeparam name="T">The type of object in the pool.</typeparam>
    public class MobiusPool<T> where T: class, new()
    {
        private readonly T[] ObjArray;
        private int Index = -1;//starts at -1, so the first element is #0

        /// <summary>
        /// Creates a new <see cref="MobiusPool{T}">MobiusPool</see> of the given size.
        /// </summary>
        /// <param name="size">The size of the internal storage array.</param>
        public MobiusPool(int size)
        {
            if (size <= 0) throw new ArgumentException("Pool size cannot be negative or zero! Size: " + size);
            ObjArray = new T[size];
            for(int i = 0; i < ObjArray.Length; i++)
            {
                ObjArray[i] = new T();
            }
        }

        /// <summary>
        /// Returns the next object in the pool.
        /// Returned values are not storage-safe.
        /// </summary>
        /// <returns>The next object.</returns>
        public T Get()
        {
            Index++;
            Index %= ObjArray.Length;
            return ObjArray[Index];
        }
    }
}
