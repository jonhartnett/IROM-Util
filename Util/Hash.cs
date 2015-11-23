namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Custom C# implementation of the xxHash algorithm (https://code.google.com/p/xxhash/).
	/// Produces extremely-fast non-cryptographic hashes for a set of coordinates.
	/// Allows the use of a seed, but only for a specific instance is created.
	/// The algorithm scores a perfect 10 on the SMHasher test (https://code.google.com/p/smhasher/wiki/SMHasher).
	/// Implementation is guaranteed to never change, that is, it will always produce the same results given the same seed even across library versions.
	/// </summary>
	public class Hash
	{
		//the prime constants
		private const uint PRIME_1 = 2654435761;
		private const uint PRIME_2 = 2246822519;
		private const uint PRIME_3 = 3266489917;
		private const uint PRIME_4 = 668265263;
		private const uint PRIME_5 = 374761393;
		
		/// <summary>
		/// The seed of this <see cref="Hash"/> instance.
		/// </summary>
		public uint Seed//property just to match pattern of RNG and ByteHash
		{
			get;
			set;
		}
		
		public Hash() : this(0)
		{
			
		}
		
		public Hash(uint seed)
		{
			Seed = seed;
		}
		
		/// <summary>
		/// Hashs the given uint.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <returns>The hashed value.</returns>
		public uint PerformHash(uint x)
		{
		    uint h32 = Seed + PRIME_5 + 8;
		    
		    h32 += x * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		
		    h32 ^= h32 >> 15;
		    h32 *= PRIME_2;
		    h32 ^= h32 >> 13;
		    h32 *= PRIME_3;
		    h32 ^= h32 >> 16;
		
		    return h32;
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <returns>The hashed value.</returns>
		public uint PerformHash(uint x, uint y)
		{
		    uint h32 = Seed + PRIME_5 + 8;
		    
		    h32 += y * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		    
		    h32 += x * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		
		    h32 ^= h32 >> 15;
		    h32 *= PRIME_2;
		    h32 ^= h32 >> 13;
		    h32 *= PRIME_3;
		    h32 ^= h32 >> 16;
		
		    return h32;
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <param name="z">The z coord.</param>
		/// <returns>The hashed value.</returns>
		public uint PerformHash(uint x, uint y, uint z)
		{
		    uint h32 = Seed + PRIME_5 + 8;
		    
		    h32 += z * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		    
		    h32 += y * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		    
		    h32 += x * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		
		    h32 ^= h32 >> 15;
		    h32 *= PRIME_2;
		    h32 ^= h32 >> 13;
		    h32 *= PRIME_3;
		    h32 ^= h32 >> 16;
		
		    return h32;
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <param name="z">The z coord.</param>
		/// <param name="w">The w coord.</param>
		/// <returns>The hashed value.</returns>
		public uint PerformHash(uint x, uint y, uint z, uint w)
		{
		    uint h32 = Seed + PRIME_5 + 8;
		    
		    h32 += w * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		    
		    h32 += z * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		    
		    h32 += y * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		    
		    h32 += x * PRIME_3;
		    h32 = ((h32 << 17) | (h32 >> (32 - 17)));
		    h32 *= PRIME_4;
		
		    h32 ^= h32 >> 15;
		    h32 *= PRIME_2;
		    h32 ^= h32 >> 13;
		    h32 *= PRIME_3;
		    h32 ^= h32 >> 16;
		
		    return h32;
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="coords">The coords.</param>
		/// <returns>The hashed value.</returns>
		public uint PerformHash(params uint[] coords)
		{
		    uint h32 = Seed + PRIME_5 + 8;
		    
		    for(int i = coords.Length - 1; i >= 0; i--)
		    {
		    	 h32 += coords[i] * PRIME_3;
			     h32 = ((h32 << 17) | (h32 >> (32 - 17)));
			     h32 *= PRIME_4;
		    }
		
		    h32 ^= h32 >> 15;
		    h32 *= PRIME_2;
		    h32 ^= h32 >> 13;
		    h32 *= PRIME_3;
		    h32 ^= h32 >> 16;
		
		    return h32;
		}
		
		/// <summary>
		/// The static instance for static hashing.
		/// </summary>
		private static readonly Hash StaticHasher = new Hash(0);
		
		/// <summary>
		/// Hashs the given uint.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <returns>The hashed value.</returns>
		public static uint PerformStaticHash(uint x)
		{
			return StaticHasher.PerformHash(x);
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <returns>The hashed value.</returns>
		public static uint PerformStaticHash(uint x, uint y)
		{
		    return StaticHasher.PerformHash(x, y);
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <param name="z">The z coord.</param>
		/// <returns>The hashed value.</returns>
		public static uint PerformStaticHash(uint x, uint y, uint z)
		{
		    return StaticHasher.PerformHash(x, y, z);
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <param name="z">The z coord.</param>
		/// <param name="w">The w coord.</param>
		/// <returns>The hashed value.</returns>
		public static uint PerformStaticHash(uint x, uint y, uint z, uint w)
		{
		    return StaticHasher.PerformHash(x, y, z, w);
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="coords">The coords.</param>
		/// <returns>The hashed value.</returns>
		public static uint PerformStaticHash(params uint[] coords)
		{
		    return StaticHasher.PerformHash(coords);
		}
	}
}
