namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// A manager for the data channels of a <see cref="DataMap2D{T}">DataMap2D</see>. Data channels must be named by arbitrary unique string types.
	/// Data channels are simply <see cref="DataMap2D{T}">DataMap2D</see>s themselves and may even have channels of their own.
	/// Strongly-supported data channels are the primary (data-storage) channels of a Map (e.g. RED for a <see cref="IROM.Render.Images.Image"/>).
    /// Weakly-supported data channels are the secondary (logical) channels of a Map (e.g. GREY for a <see cref="IROM.Render.Images.Image"/>).
    /// Types are by convention completely capitalized and have underscores for spaces. 
    /// Multiple types may refer to one channel (e.g. VALUE or LIGHTNESS) but one type may not refer to multiple channels.
	/// See a specific DataMap for its list of supported channels.
	/// </summary>
	/// <typeparam name="T">The data type of the channels.</typeparam>
	public interface IChannelManager2D<T> where T : struct
    {
    	/// <summary>
    	/// Returns true if this <see cref="DataMap2D{T}">DataMap2D</see> has a strongly-supported data channel of the given type.
    	/// See <see cref="IChannelManager2D{T}">IChannelManager2D</see> for information on strong/weak types and type naming conventions.
    	/// </summary>
    	/// <param name="type">The type.</param>
    	/// <returns>True if the given channel is strongly supported.</returns>
    	bool HasType(string type);
    	
    	/// <summary>
    	/// Returns true if this <see cref="DataMap2D{T}">DataMap2D</see> has a weakly-supported data channel of the given type.
    	/// See <see cref="IChannelManager2D{T}">IChannelManager2D</see> for information on strong/weak types and type naming conventions.
    	/// </summary>
    	/// <param name="type">The type.</param>
    	/// <returns>True if the given channel is weakly supported.</returns>
    	bool HasWeakType(string type);
    	
    	/// <summary>
    	/// Returns an array of strongly-supported types of this <see cref="DataMap2D{T}">DataMap2D</see>. 
    	/// See <see cref="IChannelManager2D{T}">IChannelManager2D</see> for information on strong/weak types and type naming conventions.
    	/// </summary>
    	/// <returns>An array of strongly supported types.</returns>
    	string[] GetTypes();
    	
    	/// <summary>
    	/// Returns an array of both strongly and weakly supported types of this <see cref="DataMap2D{T}">DataMap2D</see>. 
    	/// See <see cref="IChannelManager2D{T}">IChannelManager2D</see> for information on strong/weak types and type naming conventions.
    	/// </summary>
    	/// <returns>An array of both strongly and weakly supported types.</returns>
    	string[] GetAllTypes();
    	
    	/// <summary>
    	/// Returns the channel of the specified type.
    	/// See <see cref="IChannelManager2D{T}">IChannelManager2D</see> for information on strong/weak types and type naming conventions.
    	/// </summary>
    	/// <param name="type">The type.</param>
    	/// <returns>The channel of the specified type.</returns>
    	/// <exception cref="NotSupportedException">If the channel type given is unsupported.</exception>
    	DataMap2D<T> GetChannel(string type);
    }
}
