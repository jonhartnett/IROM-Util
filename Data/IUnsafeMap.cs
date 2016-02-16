namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Represents a DataMap that allows unsafe access.
	/// </summary>
	public unsafe interface IUnsafeMap
	{
		/// <summary>
        /// Begins an unsafe render to or from this <see cref="DataMap{T}">DataMap</see>. 
        /// Returns the pointer to the underlying data, assumed to be in the format of an array of the map's type with "index = (x + (y * width)) * stride" access.
        /// </summary>
        /// <returns>The data pointer.</returns>
        byte* BeginUnsafeOperation();
        
        /// <summary>
        /// Ends an unsafe copy to or from this <see cref="DataMap{T}">DataMap</see>. 
        /// </summary>
        void EndUnsafeOperation();
        
        /// <summary>
        /// The distance between data points in the array in bytes.
        /// </summary>
        /// <returns>The data stride.</returns>
        int GetStride();
	}
}
