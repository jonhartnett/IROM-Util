namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple double 4x4 matrix struct.
	/// </summary>
	public unsafe struct Matrix4
	{
		/// <summary>
		/// The identity matrix.
		/// </summary>
		public static readonly Matrix4 Identity = new Matrix4(1, 0, 0, 0,
		                                                      0, 1, 0, 0,
		                                                      0, 0, 1, 0,
		                                                      0, 0, 0, 1);
		
        public double v00;
        public double v10;
        public double v20;
        public double v30;
        
        public double v01;
        public double v11;
        public double v21;
        public double v31;
        
        public double v02;
        public double v12;
        public double v22;
        public double v32;
        
        public double v03;
        public double v13;
        public double v23;
        public double v33;
        
        /// <summary>
        /// Returns a translation matrix with the given offset.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <returns>The translation matrix.</returns>
        public static Matrix4 Translation(Vec3D offset)
        {
        	return new Matrix4(1, 0, 0, offset.X,
        	                   0, 1, 0, offset.Y, 
        	                   0, 0, 1, offset.Z,
        	                   0, 0, 0, 1);
        }
        
        /// <summary>
        /// Returns a rotation matrix with the given normalized rotation quaternion.
        /// </summary>
        /// <param name="quat">The rotation.</param>
        /// <returns>The rotation matrix.</returns>
        public static Matrix4 Rotation(Quaternion quat)
        {
        	return new Matrix4(1 - (2 * (quat.Y * quat.Y + quat.Z * quat.Z)),       2 * (quat.X * quat.Y - quat.W * quat.Z),       2 * (quat.X * quat.Z + quat.W * quat.Y), 0,
        	                         2 * (quat.X * quat.Y + quat.W * quat.Z), 1 - (2 * (quat.X * quat.X + quat.Z * quat.Z)),       2 * (quat.Y * quat.Z + quat.W * quat.X), 0,
        	                         2 * (quat.X * quat.Z - quat.W * quat.Y),       2 * (quat.Y * quat.Z - quat.W * quat.X), 1 - (2 * (quat.X * quat.X + quat.Y * quat.Y)), 0,
        	                                                               0,                                             0,                                             0, 1);
        }
        
        /// <summary>
        /// Returns a scale matrix with the given scale.
        /// </summary>
        /// <param name="scale">The scale.</param>
        /// <returns>The scale matrix.</returns>
        public static Matrix4 Scale(double scale)
        {
        	return Scale((Vec3D)scale);
        }
        
        /// <summary>
        /// Returns a scale matrix with the given scale.
        /// </summary>
        /// <param name="scale">The scale vector.</param>
        /// <returns>The scale matrix.</returns>
        public static Matrix4 Scale(Vec3D scale)
        {
        	return new Matrix4(scale.X,       0,       0, 0, 
        	                         0, scale.Y,       0, 0,
        	                         0,       0, scale.Z, 0,
        	                         0,       0,       0, 1);
        }
		
		/// <summary>
        /// Creates a new <see cref="Matrix3"/> with the given values.
        /// </summary>
        /// <param name="m00">The 0x0 value.</param>
        /// <param name="m10">The 1x0 value.</param>
        /// <param name="m20">The 2x0 value.</param>
        /// <param name="m30">The 3x0 value.</param>
        /// <param name="m01">The 0x1 value.</param>
        /// <param name="m11">The 1x1 value.</param>
        /// <param name="m21">The 2x1 value.</param>
        /// <param name="m31">The 3x1 value.</param>
        /// <param name="m02">The 0x2 value.</param>
        /// <param name="m12">The 1x2 value.</param>
        /// <param name="m22">The 2x2 value.</param>
        /// <param name="m32">The 3x2 value.</param>
        /// <param name="m03">The 0x3 value.</param>
        /// <param name="m13">The 1x3 value.</param>
        /// <param name="m23">The 2x3 value.</param>
        /// <param name="m33">The 3x3 value.</param>
        public Matrix4(double m00, double m10, double m20, double m30, 
                       double m01, double m11, double m21, double m31, 
                       double m02, double m12, double m22, double m32, 
                       double m03, double m13, double m23, double m33)
        {
        	v00 = m00;
        	v10 = m10;
        	v20 = m20;
        	v30 = m30;
        	v01 = m01;
        	v11 = m11;
        	v21 = m21;
        	v31 = m31;
        	v02 = m02;
        	v12 = m12;
        	v22 = m22;
        	v32 = m32;
        	v03 = m03;
        	v13 = m13;
        	v23 = m23;
        	v33 = m33;
        }
        
        public override string ToString()
		{
        	return string.Format("Matrix2(({0},{1},{2},{3}),({4},{5},{6},{7}),({8},{9},{10},{11}),({12},{13},{14},{15}))", v00, v10, v20, v30, 
        	                     										   												   v01, v11, v21, v31,  
        	                     										                                                   v02, v12, v22, v32,
        	                     										                                                   v03, v13, v23, v33);
		}
        
        public override bool Equals(object obj)
        {
        	if(obj is Matrix4)
        	{
        		return this == (Matrix4)obj;
        	}else
        	{
        		return false;
        	}
        }
        
        public override int GetHashCode()
        {
        	// disable NonReadonlyReferencedInGetHashCode
        	return (int)Hash.PerformStaticHash((uint)v00.GetHashCode(), (uint)v10.GetHashCode(), (uint)v20.GetHashCode(), (uint)v30.GetHashCode(),
        	                                   (uint)v01.GetHashCode(), (uint)v11.GetHashCode(), (uint)v21.GetHashCode(), (uint)v31.GetHashCode(),
        	                                   (uint)v02.GetHashCode(), (uint)v12.GetHashCode(), (uint)v22.GetHashCode(), (uint)v32.GetHashCode(),
        	                                   (uint)v03.GetHashCode(), (uint)v13.GetHashCode(), (uint)v23.GetHashCode(), (uint)v33.GetHashCode());
        }
        
        public double this[int x, int y]
        {
        	get
        	{
        		switch(x + (y * 4))
        		{
        			case 0: return v00;
        			case 1: return v10;
        			case 2: return v20;
        			case 3: return v30;
        			
        			case 4: return v01;
        			case 5: return v11;
        			case 6: return v21;
        			case 7: return v31;
        			
        			case 8: return v02;
        			case 9: return v12;
        			case 10: return v22;
        			case 11: return v32;
        			
        			case 12: return v03;
        			case 13: return v13;
        			case 14: return v23;
        			case 15: return v33;
        			
        			default: throw new IndexOutOfRangeException();
        		}
        	}
        	set
        	{
        		switch(x + (y * 4))
        		{
        			case 0: v00 = value; break;
        			case 1: v10 = value; break;
        			case 2: v20 = value; break;
        			case 3: v30 = value; break;
        			
        			case 4: v01 = value; break;
        			case 5: v11 = value; break;
        			case 6: v21 = value; break;
        			case 7: v31 = value; break;
        			
        			case 8: v02 = value; break;
        			case 9: v12 = value; break;
        			case 10: v22 = value; break;
        			case 11: v32 = value; break;
        			
        			case 12: v03 = value; break;
        			case 13: v13 = value; break;
        			case 14: v23 = value; break;
        			case 15: v33 = value; break;
        			
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
        public static Matrix4 operator *(Matrix4 mat1, Matrix4 mat2)
        {
        	Matrix4 result = default(Matrix4);
        	for(int x = 0; x < 4; x++)
        	{
        		for(int y = 0; y < 4; y++)
	        	{
        			result[x,y] = (mat1[0,y] * mat2[x,0]) + (mat1[1,y] * mat2[x,1]) + (mat1[2,y] * mat2[x,2]) + (mat1[3,y] * mat2[x,3]);
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
        public static Vec4D operator *(Vec4D vec, Matrix4 mat)
        {
        	Vec4D result = default(Vec4D);
        	for(int i = 0; i < 4; i++)
        	{
        		result[i] = (mat[0, i] * vec[0]) + (mat[1, i] * vec[1]) + (mat[2, i] * vec[2]) + (mat[3, i] * vec[3]);
        	}
        	return result;
        }
        
        /// <summary>
        /// Multiplies the given matrix and vector.
        /// </summary>
        /// <param name="vec">The vector.</param>
        /// <param name="mat">The matrix.</param>
        /// <returns>The resulting vector.</returns>
        public static Vec3D operator *(Vec3D vec, Matrix4 mat)
        {
        	return (Vec3D)(new Vec4D(vec.X, vec.Y, vec.Z, 1) * mat);
        }
        
        /// <summary>
        /// True if the matricies are equal.
        /// </summary>
        /// <param name="mat">The first matrix.</param>
        /// <param name="mat2">The second matrix.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Matrix4 mat, Matrix4 mat2)
        {
        	return (mat.v00 == mat2.v00) && (mat.v10 == mat2.v10) && (mat.v20 == mat2.v20) && (mat.v30 == mat2.v30) &&
        		   (mat.v01 == mat2.v01) && (mat.v11 == mat2.v11) && (mat.v21 == mat2.v21) && (mat.v31 == mat2.v31) &&
        		   (mat.v02 == mat2.v02) && (mat.v12 == mat2.v12) && (mat.v22 == mat2.v22) && (mat.v32 == mat2.v32) &&
        		   (mat.v03 == mat2.v03) && (mat.v13 == mat2.v13) && (mat.v23 == mat2.v23) && (mat.v33 == mat2.v33);
        }
        
        /// <summary>
        /// True if the matricies are not equal.
        /// </summary>
        /// <param name="mat">The first matrix.</param>
        /// <param name="mat2">The second matrix.</param>
        /// <returns>True if not equal.</returns>
        public static bool operator !=(Matrix4 mat, Matrix4 mat2)
        {
        	return !(mat == mat2);
        }
	}
}
