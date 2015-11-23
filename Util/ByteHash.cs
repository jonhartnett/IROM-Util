namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Produces extremely-fast non-cryptographic hashes for a seed and set of coordinates with an output type of byte.
	/// </summary>
	public class ByteHash
	{
		/// <summary>
		/// The permutation array.
		/// </summary>
		private byte[] Permutation = new byte[256];
		
		private uint BaseSeed;
		
		/// <summary>
		/// The seed of this <see cref="ByteHash"/> instance.
		/// </summary>
		public uint Seed
		{
			get
			{
				return BaseSeed;
			}
			set
			{
				BaseSeed = value;
				ReShuffle();
			}
		}
		
		public ByteHash() : this(0)
		{
			
		}
		
		public ByteHash(uint seed)
		{
			Seed = seed;
		}
		
		private void ReShuffle()
		{
			byte index = 0;
			do
			{
				Permutation[index] = index;
				index++;
			}while(index != 0);
			RNG rng = new RNG(Seed);
			do
			{
				Util.Swap(ref Permutation[index], ref Permutation[rng.Next() & 0xFF]);
				index++;
			}while(index != 0);
		}
		
		/// <summary>
		/// Hashs the given uint.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <returns>The hashed value.</returns>
		public byte PerformHash(uint x)
		{
			return Permutation[x & 0xFF];
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <returns>The hashed value.</returns>
		public byte PerformHash(uint x, uint y)
		{
			return Permutation[(y + PerformHash(x)) & 0xFF];
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <param name="z">The z coord.</param>
		/// <returns>The hashed value.</returns>
		public byte PerformHash(uint x, uint y, uint z)
		{
		    return Permutation[(z + PerformHash(x, y)) & 0xFF];
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <param name="z">The z coord.</param>
		/// <param name="w">The w coord.</param>
		/// <returns>The hashed value.</returns>
		public byte PerformHash(uint x, uint y, uint z, uint w)
		{
		    return Permutation[(w + PerformHash(x, y, z)) & 0xFF];
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="coords">The coords.</param>
		/// <returns>The hashed value.</returns>
		public byte PerformHash(params uint[] coords)
		{
			byte value = 0;
			for(int i = 0; i < coords.Length; i++)
			{
				value = PerformHash(coords[i] + value);
			}
			return value;
		}
		
		/// <summary>
		/// The static instance for static hashing.
		/// </summary>
		private static readonly ByteHash StaticHasher = new ByteHash(0);
		
		/// <summary>
		/// Hashs the given uint.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <returns>The hashed value.</returns>
		public static byte PerformStaticHash(uint x)
		{
			return StaticHasher.PerformHash(x);
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="x">The x coord.</param>
		/// <param name="y">The y coord.</param>
		/// <returns>The hashed value.</returns>
		public static byte PerformStaticHash(uint x, uint y)
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
		public static byte PerformStaticHash(uint x, uint y, uint z)
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
		public static byte PerformStaticHash(uint x, uint y, uint z, uint w)
		{
		    return StaticHasher.PerformHash(x, y, z, w);
		}
		
		/// <summary>
		/// Hashs the given uints.
		/// </summary>
		/// <param name="coords">The coords.</param>
		/// <returns>The hashed value.</returns>
		public static byte PerformStaticHash(params uint[] coords)
		{
		    return StaticHasher.PerformHash(coords);
		}
	}
}
