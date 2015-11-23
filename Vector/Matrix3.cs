namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple double 3x3 matrix struct.
	/// </summary>
	public unsafe struct Matrix3
	{
		/// <summary>
		/// The identity matrix.
		/// </summary>
		public static readonly Matrix3 Identity = new Matrix3(1, 0, 0,
		                                                      0, 1, 0,
		                                                      0, 0, 1);
		
        public double v00;
        public double v10;
        public double v20;
        
        public double v01;
        public double v11;
        public double v21;
        
        public double v02;
        public double v12;
        public double v22;
        
        /// <summary>
        /// Returns a translation matrix with the given offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix3 Translation(Vec2D offset)
        {
        	return new Matrix3(1, 0, offset.X,
        	                   0, 1, offset.Y, 
        	                   0, 0, 1);
        }
        
        /// <summary>
        /// Returns a rotation matrix with the given angle.
        /// </summary>
        /// <param name="theta">The angle.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix3 Rotation(double theta)
        {
        	double c = Math.Cos(theta);
        	double s = Math.Sin(theta);
        	return new Matrix3(c, -s, 0, 
        	                   s,  c, 0, 
        	                   0,  0, 1);
        }
        
        /// <summary>
        /// Returns a scale matrix with the given scale.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>The scale matrix.</returns>
        public static Matrix3 Scale(double scale)
        {
        	return Scale((Vec2D)scale);
        }
        
        /// <summary>
        /// Returns a scale matrix with the given scale.
        /// </summary>
        /// <param name="scale">The scale vector.</param>
        /// <returns>The scale matrix.</returns>
        public static Matrix3 Scale(Vec2D scale)
        {
        	return new Matrix3(scale.X,       0, 0, 
        	                         0, scale.Y, 0, 
        	                         0,       0, 1);
        }
		
		/// <summary>
        /// Creates a new <see cref="Matrix3"/> with the given values.
        /// </summary>
        /// <param name="m00">The 0x0 value.</param>
        /// <param name="m10">The 1x0 value.</param>
        /// <param name="m20">The 2x0 value.</param>
        /// <param name="m01">The 0x1 value.</param>
        /// <param name="m11">The 1x1 value.</param>
        /// <param name="m21">The 2x1 value.</param>
        /// <param name="m02">The 0x2 value.</param>
        /// <param name="m12">The 1x2 value.</param>
        /// <param name="m22">The 2x2 value.</param>
        public Matrix3(double m00, double m10, double m20, 
                       double m01, double m11, double m21, 
                       double m02, double m12, double m22)
        {
        	v00 = m00;
        	v10 = m10;
        	v20 = m20;
        	v01 = m01;
        	v11 = m11;
        	v21 = m21;
        	v02 = m02;
        	v12 = m12;
        	v22 = m22;
        }
        
        public override string ToString()
		{
        	return string.Format("Matrix2(({0},{1},{2}),({3},{4},{5}),({6},{7},{8}))", v00, v10, v20, 
        	                     										               v01, v11, v21,  
        	                     										               v02, v12, v22);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Matrix3)
        	{
        		return this == (Matrix3)obj;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)v00.GetHashCode(), (uint)v10.GetHashCode(), (uint)v20.GetHashCode(),
        	                                   (uint)v01.GetHashCode(), (uint)v11.GetHashCode(), (uint)v21.GetHashCode(),
        	                                   (uint)v02.GetHashCode(), (uint)v12.GetHashCode(), (uint)v22.GetHashCode());
        }
        
        public double this[int x, int y]
        {
        	get
        	{
        		switch(x + (y * 3))
        		{
        			case 0: return v00;
        			case 1: return v10;
        			case 2: return v20;
        			
        			case 3: return v01;
        			case 4: return v11;
        			case 5: return v21;
        			
        			case 6: return v02;
        			case 7: return v12;
        			case 8: return v22;
        			
        			default: throw new IndexOutOfRangeException();
        		}
        	}
        	set
        	{
        		switch(x + (y * 3))
        		{
        			case 0: v00 = value; break;
        			case 1: v10 = value; break;
        			case 2: v20 = value; break;
        			
        			case 3: v01 = value; break;
        			case 4: v11 = value; break;
        			case 5: v21 = value; break;
        			
        			case 6: v02 = value; break;
        			case 7: v12 = value; break;
        			case 8: v22 = value; break;
        			
        			default: throw new IndexOutOfRangeException();
        		}
        	}
        }
        
        /// <summary>
        /// Multiplies the given matrices.
        /// </summary>
        /// <param name="mat1">The first matrix.</param>
        /// <param name="mat2">The second matrix.</param>
        /// <returns>The product matrix.</returns>
        public static Matrix3 operator *(Matrix3 mat1, Matrix3 mat2)
        {
        	Matrix3 result = default(Matrix3);
        	for(int x = 0; x < 3; x++)
        	{
        		for(int y = 0; y < 3; y++)
	        	{
        			result[x,y] = (mat1[0,y] * mat2[x,0]) + (mat1[1,y] * mat2[x,1]) + (mat1[2,y] * mat2[x,2]);
	        	}
        	}
        	return result;
        }
        
        /// <summary>
        /// Multiplies the given matrix and vector.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="mat">The matrix.</param>
        /// <returns>The resulting vector.</returns>
        public static Vec3D operator *(Vec3D vec, Matrix3 mat)
        {
        	Vec3D result = default(Vec3D);
        	for(int i = 0; i < 3; i++)
        	{
        		result[i] = (mat[0, i] * vec[0]) + (mat[1, i] * vec[1]) + (mat[2, i] * vec[2]);
        	}
        	return result;
        }
        
        /// <summary>
        /// Multiplies the given matrix and vector.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="mat">The matrix.</param>
        /// <returns>The resulting vector.</returns>
        public static Vec2D operator *(Vec2D vec, Matrix3 mat)
        {
        	return (Vec2D)(new Vec3D(vec.X, vec.Y, 1) * mat);
        }
        
        /// <summary>
        /// True if the matricies are equal.
        /// </summary>
        /// <param name="mat">The first matrix.</param>
        /// <param name="mat2">The second matrix.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Matrix3 mat, Matrix3 mat2)
        {
        	return (mat.v00 == mat2.v00) && (mat.v10 == mat2.v10) && (mat.v20 == mat2.v20) &&
        		   (mat.v01 == mat2.v01) && (mat.v11 == mat2.v11) && (mat.v21 == mat2.v21) &&
        		   (mat.v02 == mat2.v02) && (mat.v12 == mat2.v12) && (mat.v22 == mat2.v22);
        }
        
        /// <summary>
        /// True if the matricies are not equal.
        /// </summary>
        /// <param name="mat">The first matrix.</param>
        /// <param name="mat2">The second matrix.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Matrix3 mat, Matrix3 mat2)
        {
        	return !(mat == mat2);
        }
	}
}
