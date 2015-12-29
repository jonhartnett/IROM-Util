namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	
    /// <summary>
    /// A thread-safe pool of temporary objects that are recycled once per "tick". 
    /// Exact meaning of a "tick" depends on the specific program.
    /// </summary>
    /// <typeparam name="T">The type of object in the pool.</typeparam>
    public static class ReferencePool<T> where T: class, new()
    {
        private static readonly List<T> ObjPool = new List<T>();
        // disable once StaticFieldInGenericType
        private static int Index = 0;

        /// <summary>
        /// Returns the next object in the pool.
        /// Returned object are not storage-safe beyond the span of the current tick.
        /// </summary>
        /// <returns>The next pool object.</returns>
        public static T Get()
        {
            T value;
            lock(ObjPool)
            {
	            if(Index >= ObjPool.Count)
	            {
	                value = new T();
	                ObjPool.Add(value);
	            }else
	            {
	                value = ObjPool[Index];
	            }
	            Index++;
            }
            return value;
        }

        /// <summary>
        /// Removes the given object from the pool for permanent storage.
        /// </summary>
        /// <param name="value">The value to claim.</param>
        public static void Claim(T value)
        {
        	lock(ObjPool)
            {
	            ObjPool.Remove(value);
	            Index--;
        	}
        }

        /// <summary>
        /// Resets this pool. Should be called once per "tick".
        /// </summary>
        public static void Reset()
        {
        	lock(ObjPool)
            {
            	Index = 0;
        	}
        }
    }
}
