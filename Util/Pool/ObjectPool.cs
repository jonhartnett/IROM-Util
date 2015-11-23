namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
    /// <summary>
    /// A thread-safe pool of temporary objects where instances are claimed and released when no longer needed.
    /// </summary>
    /// <typeparam name="T">The type of object in the pool.</typeparam>
    public static class ObjectPool<T> where T: class, new()
    {
        private readonly static Stack<T> ObjPool = new Stack<T>();

        /// <summary>
        /// Returns an object from the pool.
        /// Returned values are storage-safe.
        /// </summary>
        /// <returns>The next object.</returns>
        public static T Get()
        {
        	T val;
        	lock(ObjPool)
        	{
        		if(ObjPool.Count > 0)
        		{
        			val = ObjPool.Pop();
        		}else
        		{
        			val = new T();
        		}
        	}
            return val;
        }

        /// <summary>
        /// Releases the given object back into the pool.
        /// </summary>
        /// <param name="value">The object to release/param>
        public static void Release(T value)
        {
        	lock(ObjPool)
        	{
            	ObjPool.Push(value);
        	}
        }
    }
}
