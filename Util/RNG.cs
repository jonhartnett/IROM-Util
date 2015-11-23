namespace IROM.Util
{
	using System;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// WARNING: Instances of this class should not be accessed in a multithreaded way without a lock!
	/// A simple, robust, minimalistic, and extremely fast RNG based on http://heliosphan.org/fastrandom.html.
	/// The period is 2^128-1.
	/// Implementation is guaranteed to never change, that is, it will always produce the same results given the same seed even across library versions.
	/// </summary>
	public sealed class RNG
	{
		//constants
		private const double DOUBLE_UNIT_INT = 1.0 / ((double)int.MaxValue+1.0);
		private const float FLOAT_UNIT_INT = 1.0F / ((float)int.MaxValue + 1.0F);
		private const uint RESET_Y = 842502087;
		private const uint RESET_Z = 3579807591;
		private const uint RESET_W = 273326509;
		
		//state variables
		private uint x; 
		private uint y; 
		private uint z; 
		private uint w;
		
		//storage for NextBool to improve performance
		private uint bitBuffer;
		private uint bitMask;
		
		//storage for NextByte to improve performance
		private uint byteBuffer;
		private byte byteBufferState;
		
		//"local" variables
		private uint t;
		private int i;
		private int bound;
		
		//seed
		private uint baseSeed;
		public uint Seed
		{
			get
			{
				return baseSeed;
			}
			set
			{
				baseSeed = value;
				x = ((baseSeed * 1431655781) 
		           + (baseSeed * 1183186591)
		           + (baseSeed * 622729787)
		           + (baseSeed * 338294347));
			
				y = RESET_Y;
				z = RESET_Z;
				w = RESET_W;
				
				bitBuffer = 0;
				bitMask = 0;
				byteBuffer = 0;
				byteBufferState = 0;
			}
		}
		
		/// <summary>
		/// Creates a new <see cref="RNG"/> with the given seed.
		/// </summary>
		/// <param name="seed">The seed.</param>
		public RNG(uint seed)
		{
			Seed = seed;
		}
		
		/// <summary>
		/// Generates a random uint over the full range of uint.
		/// </summary>
		/// <returns>A random uint.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public uint Next()
		{
			t = x ^ (x << 11);
			x = y; 
			y = z;
			z = w;
			return w = (w ^ (w >> 19)) ^ (t ^ (t >> 8));
		}
		
		/// <summary>
		/// Generates a random float over the range [0,1).
		/// </summary>
		/// <returns>A random float.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextFloat()
		{   
			return FLOAT_UNIT_INT * (int)(0x7FFFFFFF & Next());
		}
		
		/// <summary>
		/// Generates a random double over the range [0,1).
		/// </summary>
		/// <returns>A random double.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public double NextDouble()
		{
			return DOUBLE_UNIT_INT * (int)(0x7FFFFFFF & Next());         
		}
		
		/// <summary>
		/// Generates a single random bit.
		/// This method's performance is improved by generating 32 bits in one operation and storing them
		/// ready for future calls.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool NextBool()
		{
			if(bitMask == 0)
			{   
				// Generate 32 more bits.
				bitBuffer = Next();
				
				// Reset the bitMask that tells us which bit to read next.
				bitMask = 0x80000000;
				return (bitBuffer & bitMask) == 0;
			}
			
			return (bitBuffer & (bitMask >>= 1)) == 0;
		}
		
		/// <summary>
		/// Generates a signle random byte with range [0,255].
		/// This method's performance is improved by generating 4 bytes in one operation and storing them
		/// ready for future calls.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public byte NextByte()
		{
			if(byteBufferState == 0)
			{
				// Generate 4 more bytes.
				byteBuffer = Next();
				byteBufferState = 0x4;
				return (byte)byteBuffer;  // Note. Masking with 0xFF is unnecessary.
			}
			byteBufferState >>= 1;
			return (byte)(byteBuffer >>= 1);
		}
		
		/// <summary>
		/// Fills the provided byte array with random bytes.
		/// This method is functionally equivalent to System.Random.NextBytes(). 
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void NextBytes(byte[] buffer)
		{
			// Fill up the bulk of the buffer in chunks of 4 bytes at a time.
			i = 0;
			bound = buffer.Length - 3;
			for(; i < bound;)
			{   
				// Generate 4 bytes. 
				// Increased performance is achieved by generating 4 random bytes per loop.
				// Also note that no mask needs to be applied to zero out the higher order bytes before
				// casting because the cast ignores thos bytes. Thanks to Stefan Trosch�tz for pointing this out.
				Next();
				
				buffer[i++] = (byte)w;
				buffer[i++] = (byte)(w >> 8);
				buffer[i++] = (byte)(w >> 16);
				buffer[i++] = (byte)(w >> 24);
			}
			
			// Fill up any remaining bytes in the buffer.
			if(i < buffer.Length)
			{
				// Generate 4 bytes.
				Next();
				
				buffer[i++] = (byte)w;
				if(i < buffer.Length)
				{
					buffer[i++]=(byte)(w >> 8);
					if(i < buffer.Length)
					{   
						buffer[i++] = (byte)(w >> 16);
						if(i  < buffer.Length)
						{   
							buffer[i] = (byte)(w >> 24);
						}
					}
				}
			}
		}
	}
}
