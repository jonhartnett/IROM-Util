namespace IROM.Util
{
	using System;
	using System.Linq;
	
    /// <summary>
    /// Simple Quaternion struct.
    /// </summary>
    public struct Quaternion
    {
    	/// <summary>
    	/// The indentity <see cref="Quaternion"/>.
    	/// </summary>
    	public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);
    	
        /// <summary>
        /// The x value.
        /// </summary>
        public double X;
        
        /// <summary>
        /// The y value.
        /// </summary>
        public double Y;
        
        /// <summary>
        /// The z value.
        /// </summary>
        public double Z;
        
        /// <summary>
        /// The w value.
        /// </summary>
        public double W;
        
        /// <summary>
        /// Returns a quaternion that rotates the given angle around the given axis.
        /// </summary>
        /// <param name="axis">The axis to rotate about.</param>
        /// <param name="angle">The amount to rotate.</param>
        /// <returns>The quaternion.</returns>
        public static Quaternion AxisAngle(Vec3D axis, double angle)
        {
        	double sin = Math.Sin(angle / 2);
        	return new Quaternion(axis.X * sin, axis.Y * sin, axis.Z * sin, Math.Cos(angle / 2));
        }
        
        /// <summary>
        /// Creates a new <see cref="Quaternion"/> with the given values.
        /// </summary>
        /// <param name="i">The x value.</param>
        /// <param name="j">The y value.</param>
        /// <param name="k">The z value.</param>
        /// <param name="l">The w value.</param>
        public Quaternion(double i, double j, double k, double l)
        {
            X = i;
            Y = j;
            Z = k;
            W = l;
        }
        
        /// <summary>
        /// Accesses the given value of this <see cref="Quaternion"/>.
        /// </summary>
        public double this[int index]
        {
        	get
        	{
        		switch(index)
        		{
        			case 0: return X;
        			case 1: return Y;
        			case 2: return Z;
        			case 3: return W;
        			default: throw new IndexOutOfRangeException(index + " out of Quaternion range.");
        		}
        	}
        	set
        	{
        		switch(index)
        		{
        			case 0: X = value; break;
        			case 1: Y = value; break;
        			case 2: Z = value; break;
        			case 3: W = value; break;
        			default: throw new IndexOutOfRangeException(index + " out of Quaternion range.");
        		}
        	}
        }
        
        public override string ToString()
		{
			return string.Format("Quaternion({0}, {1}, {2}, {3})", X, Y, Z, W);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Quaternion)
        	{
        		return this == (Quaternion)obj;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)X.GetHashCode(), (uint)Y.GetHashCode(), (uint)Z.GetHashCode(), (uint)W.GetHashCode());
        }
        
        /// <summary>
        /// Returns the squared magnitude of this <see cref="Quaternion"/>.
        /// </summary>
        /// <returns>The squared magnitude.</returns>
        public double MagnitudeSq()
        {
        	return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }
        
        /// <summary>
        /// Returns the magnitude of this <see cref="Quaternion"/>.
        /// </summary>
        /// <returns>The magnitude.</returns>
        public double Magnitude()
        {
        	return Math.Sqrt(MagnitudeSq());
        }
        
        /// <summary>
        /// Returns this <see cref="Quaternion"/> normalized.
        /// </summary>
        /// <returns>The normalized <see cref="Quaternion"/>.</returns>
        public Quaternion Normalized()
        {
        	double mag = this.Magnitude();
        	return new Quaternion(X / mag, Y / mag, Z / mag, W / mag);
        }
        
        /// <summary>
        /// Explicit cast to <see cref="Vec4D"/>.
        /// </summary>
        /// <param name="quat">The quat to cast.</param>
        /// <returns>The resulting vec.</returns>
        public static explicit operator Vec4D(Quaternion quat)
        {
        	return new Vec4D(quat.X, quat.Y, quat.Z, quat.W);
        }
        
        /// <summary>
        /// Explicit cast from <see cref="Vec4D"/>.
        /// </summary>
        /// <param name="vec">The vec to cast.</param>
        /// <returns>The resulting quat.</returns>
        public static explicit operator Quaternion(Vec4D vec)
        {
        	return new Quaternion(vec.X, vec.Y, vec.Z, vec.W);
        }

		/// <summary>
        /// Negates this vec.
        /// </summary>
        /// <param name="vec">The vec to negate.</param>
        /// <returns>The negated vec.</returns>
        public static Quaternion operator -(Quaternion vec)
        {
            return new Quaternion(vec.X, vec.Y, vec.Z, -vec.W);
        }

        /// <summary>
        /// Multiplies the given quats.
        /// </summary>
        /// <param name="quat">The first quat.</param>
        /// <param name="quat2">The second quat.</param>
        /// <returns>The product quat.</returns>
        public static Quaternion operator *(Quaternion quat, Quaternion quat2)
        {
            return new Quaternion(quat.W * quat2.X + quat.X * quat2.W + quat.Y * quat2.Z - quat.Z * quat2.Y,
        	                      quat.W * quat2.Y - quat.X * quat2.Z + quat.Y * quat2.W + quat.Z * quat2.X,
        	                      quat.W * quat2.Z + quat.X * quat2.Y - quat.Y * quat2.X + quat.Z * quat2.W,
        	                      quat.W * quat2.W - quat.X * quat2.X - quat.Y * quat2.Y - quat.Z * quat2.Z);
        }

        /// <summary>
        /// Scales the quaternion by the given scalar.
        /// </summary>
        /// <param name="quat">The quat.</param>
        /// <param name="val">The value.</param>
        /// <returns>The product quat.</returns>
        public static Quaternion operator *(Quaternion quat, double val)
        {
        	double angle = 2 * Math.Acos(quat.W);
        	Vec3D axis = new Vec3D(quat.X, quat.Y, quat.Z) / Math.Sqrt(1 - quat.W * quat.W);
        	angle *= val;
        	return AxisAngle(axis, angle);
        }
        
        /// <summary>
        /// Multiplies the given quaternion and vector.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="quat">The quaternion.</param>
        /// <returns>The resulting vector.</returns>
        public static Vec4D operator *(Vec4D vec, Quaternion quat)
        {
        	return vec * Matrix4.Rotation(quat);
        }
        
        /// <summary>
        /// Multiplies the given quaternion and vector.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="quat">The quaternion.</param>
        /// <returns>The resulting vector.</returns>
        public static Vec3D operator *(Vec3D vec, Quaternion quat)
        {
        	return vec * Matrix4.Rotation(quat);
        }
        	
        /// <summary>
        /// Scales the quaternion by the inverse of the given scalar.
        /// </summary>
        /// <param name="quat">The quat.</param>
        /// <param name="val">The value.</param>
        /// <returns>The quotient quat.</returns>
        public static Quaternion operator /(Quaternion quat, double val)
        {
        	return quat * (1 / val);
        }
        
        /// <summary>
        /// True if the quats are equal.
        /// </summary>
        /// <param name="quat">The first quat.</param>
        /// <param name="quat2">The second quat.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Quaternion quat, Quaternion quat2)
        {
        	return quat.X == quat2.X && quat.Y == quat2.Y && quat.Z == quat2.Z && quat.W == quat2.W;
        }
        
        /// <summary>
        /// True if the quats are not equal.
        /// </summary>
        /// <param name="quat">The first quat.</param>
        /// <param name="quat2">The second quat.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Quaternion quat, Quaternion quat2)
        {
        	return !(quat == quat2);
        }
    }
}
