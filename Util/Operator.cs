namespace IROM.Util
{
	using System;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	
	/// <summary>
	/// Performs operator functions on objects.
	/// </summary>
	public static class Operator
	{
		private const BindingFlags opFlags = BindingFlags.Static | BindingFlags.Public;
		
		public static object Plus(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_UnaryPlus", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object Negate(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_UnaryNegation", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object Not(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_LogicalNot", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object Complement(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_OnesComplement", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object Increment(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_Increment", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object Decrement(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_Decrement", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object IsTrue(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_True", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object IsFalse(object obj)
		{
			MethodInfo info = obj.GetType().GetMethod("op_False", opFlags);
			if(info != null) return info.Invoke(null, new []{obj});
			throw new MissingMethodException();
		}
		
		public static object Add(object obj, object obj2)
		{
			const string opName = "op_Addition";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Subtract(object obj, object obj2)
		{
			const string opName = "op_Subtraction";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Multiply(object obj, object obj2)
		{
			const string opName = "op_Multiply";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Divide(object obj, object obj2)
		{
			const string opName = "op_Division";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Modulus(object obj, object obj2)
		{
			const string opName = "op_Modulus";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object And(object obj, object obj2)
		{
			const string opName = "op_BitwiseAnd";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Or(object obj, object obj2)
		{
			const string opName = "op_BitwiseOr";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Xor(object obj, object obj2)
		{
			const string opName = "op_ExclusiveOr";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object LeftShift(object obj, object obj2)
		{
			const string opName = "op_LeftShift";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object RightShift(object obj, object obj2)
		{
			const string opName = "op_RightShift";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Equality(object obj, object obj2)
		{
			const string opName = "op_Equality";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Inequality(object obj, object obj2)
		{
			const string opName = "op_Inequality";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Greater(object obj, object obj2)
		{
			const string opName = "op_GreaterThan";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object Less(object obj, object obj2)
		{
			const string opName = "op_LessThan";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object GreaterEquals(object obj, object obj2)
		{
			const string opName = "op_GreaterThanOrEqual";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
		
		public static object LessEquals(object obj, object obj2)
		{
			const string opName = "op_LessThanOrEqual";
			MethodInfo info = obj.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			info = obj2.GetType().GetMethod(opName, new []{obj.GetType(), obj2.GetType()});
			if(info != null) return info.Invoke(null, new []{obj, obj2});
			throw new MissingMethodException();
		}
	}
	
    /// <summary>
    /// Performs operator functions on generic types.
    /// </summary>
    /// <typeparam name="T">The generic type.</typeparam>
    public static class Operator<T>
    {
        //stored functions
        private static Func<T, T> NegateFunc;
        private static Func<T, T> NotFunc;
        private static Func<T, T> BitCompFunc;
        private static Func<T, bool> TrueFunc;
        private static Func<T, bool> FalseFunc;
        private static Func<T, T, T> AddFunc;
        private static Func<T, T, T> SubtractFunc;
        private static Func<T, T, T> MultiplyFunc;
        private static Func<T, T, T> DivideFunc;
        private static Func<T, T, T> ModuloFunc;
        private static Func<T, T, T> AndFunc;
        private static Func<T, T, T> OrFunc;
        private static Func<T, T, T> XorFunc;
        private static Func<T, T, T> LeftShiftFunc;
        private static Func<T, T, T> RightShiftFunc;
        private static Func<T, T, bool> EqualsFunc;
        private static Func<T, T, bool> NotEqualsFunc;
        private static Func<T, T, bool> LessFunc;
        private static Func<T, T, bool> GreaterFunc;
        private static Func<T, T, bool> LessEqualsFunc;
        private static Func<T, T, bool> GreaterEqualsFunc;

        /// <summary>
        /// Creates the function with the given name.
        /// </summary>
        /// <param name="name">The name of the function to init.</param>
        public static void Init(String name)
        {
            ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
            ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
            switch (name)
            {
                case "Negate":
                {
                    UnaryExpression negate_body = Expression.Negate(paramA);
                    NegateFunc = Expression.Lambda<Func<T, T>>(negate_body, paramA).Compile();
                    break;
                }
                case "Not":
                {
                    UnaryExpression not_body = Expression.Not(paramA);
                    NotFunc = Expression.Lambda<Func<T, T>>(not_body, paramA).Compile();
                    break;
                }
                case "BitComp":
                {
                    UnaryExpression bit_comp_body = Expression.OnesComplement(paramA);
                    BitCompFunc = Expression.Lambda<Func<T, T>>(bit_comp_body, paramA).Compile();
                    break;
                }
                case "IsTrue":
                {
                    UnaryExpression true_body = Expression.IsTrue(paramA);
                    TrueFunc = Expression.Lambda<Func<T, bool>>(true_body, paramA).Compile();
                    break;
                }
                case "IsFalse":
                {
                    UnaryExpression false_body = Expression.IsFalse(paramA);
                    FalseFunc = Expression.Lambda<Func<T, bool>>(false_body, paramA).Compile();
                    break;
                }
                case "Add":
                {
                    BinaryExpression add_body = Expression.Add(paramA, paramB);
                    AddFunc = Expression.Lambda<Func<T, T, T>>(add_body, paramA, paramB).Compile();
                    break;
                }
                case "Subtract":
                {
                    BinaryExpression subtract_body = Expression.Subtract(paramA, paramB);
                    SubtractFunc = Expression.Lambda<Func<T, T, T>>(subtract_body, paramA, paramB).Compile();
                    break;
                }
                case "Multiply":
                {
                    BinaryExpression multiply_body = Expression.Multiply(paramA, paramB);
                    MultiplyFunc = Expression.Lambda<Func<T, T, T>>(multiply_body, paramA, paramB).Compile();
                    break;
                }
                case "Divide":
                {
                    BinaryExpression divide_body = Expression.Divide(paramA, paramB);
                    DivideFunc = Expression.Lambda<Func<T, T, T>>(divide_body, paramA, paramB).Compile();
                    break;
                }
                case "Modulo":
                {
                    BinaryExpression modulo_body = Expression.Modulo(paramA, paramB);
                    ModuloFunc = Expression.Lambda<Func<T, T, T>>(modulo_body, paramA, paramB).Compile();
                    break;
                }
                case "And":
                {
                    BinaryExpression and_body = Expression.And(paramA, paramB);
                    AndFunc = Expression.Lambda<Func<T, T, T>>(and_body, paramA, paramB).Compile();
                    break;
                }
                case "Or":
                {
                    BinaryExpression or_body = Expression.Or(paramA, paramB);
                    OrFunc = Expression.Lambda<Func<T, T, T>>(or_body, paramA, paramB).Compile();
                    break;
                }
                case "Xor":
                {
                    BinaryExpression xor_body = Expression.ExclusiveOr(paramA, paramB);
                    XorFunc = Expression.Lambda<Func<T, T, T>>(xor_body, paramA, paramB).Compile();
                    break;
                }
                case "LeftShift":
                {
                    BinaryExpression left_shift_body = Expression.LeftShift(paramA, paramB);
                    LeftShiftFunc = Expression.Lambda<Func<T, T, T>>(left_shift_body, paramA, paramB).Compile();
                    break;
                }
                case "RightShift":
                {
                    BinaryExpression right_shift_body = Expression.RightShift(paramA, paramB);
                    RightShiftFunc = Expression.Lambda<Func<T, T, T>>(right_shift_body, paramA, paramB).Compile();
                    break;
                }
                case "Equals":
                {
                    BinaryExpression equals_body = Expression.Equal(paramA, paramB);
                    EqualsFunc = Expression.Lambda<Func<T, T, bool>>(equals_body, paramA, paramB).Compile();
                    break;
                }
                case "NotEquals":
                {
                    BinaryExpression not_equals_body = Expression.NotEqual(paramA, paramB);
                    NotEqualsFunc = Expression.Lambda<Func<T, T, bool>>(not_equals_body, paramA, paramB).Compile();
                    break;
                }
                case "Less":
                {
                    BinaryExpression less_body = Expression.LessThan(paramA, paramB);
                    LessFunc = Expression.Lambda<Func<T, T, bool>>(less_body, paramA, paramB).Compile();
                    break;
                }
                case "Greater":
                {
                    BinaryExpression greater_body = Expression.GreaterThan(paramA, paramB);
                    GreaterFunc = Expression.Lambda<Func<T, T, bool>>(greater_body, paramA, paramB).Compile();
                    break;
                }
                case "LessEquals":
                {
                    BinaryExpression less_equals_body = Expression.LessThanOrEqual(paramA, paramB);
                    LessEqualsFunc = Expression.Lambda<Func<T, T, bool>>(less_equals_body, paramA, paramB).Compile();
                    break;
                }
                case "GreaterEquals":
                {
                    BinaryExpression greater_equals_body = Expression.GreaterThanOrEqual(paramA, paramB);
                    GreaterEqualsFunc = Expression.Lambda<Func<T, T, bool>>(greater_equals_body, paramA, paramB).Compile();
                    break;
                }
            }
        }

        public static T Negate(T a)
        {
            if (NegateFunc == null) Init("Negate");
            return NegateFunc(a);
        }

        public static T Not(T a)
        {
            if (NotFunc == null) Init("Not");
            return NotFunc(a);
        }

        public static T BitComp(T a)
        {
            if (BitCompFunc == null) Init("BitComp");
            return BitCompFunc(a);
        }

        public static bool IsTrue(T a)
        {
            if (TrueFunc == null) Init("IsTrue");
            return TrueFunc(a);
        }

        public static bool IsFalse(T a)
        {
            if (FalseFunc == null) Init("IsFalse");
            return FalseFunc(a);
        }

        public static T Add(T a, T b)
        {
            if (AddFunc == null) Init("Add");
            return AddFunc(a, b);
        }

        public static T Subtract(T a, T b)
        {
            if (SubtractFunc == null) Init("Subtract");
            return SubtractFunc(a, b);
        }

        public static T Multiply(T a, T b)
        {
            if (MultiplyFunc == null) Init("Multiply");
            return MultiplyFunc(a, b);
        }

        public static T Divide(T a, T b)
        {
            if (DivideFunc == null) Init("Divide");
            return DivideFunc(a, b);
        }

        public static T Modulo(T a, T b)
        {
            if (ModuloFunc == null) Init("Modulo");
            return ModuloFunc(a, b);
        }

        public static T And(T a, T b)
        {
            if (AndFunc == null) Init("And");
            return AndFunc(a, b);
        }

        public static T Or(T a, T b)
        {
            if (OrFunc == null) Init("Or");
            return OrFunc(a, b);
        }

        public static T Xor(T a, T b)
        {
            if (XorFunc == null) Init("Xor");
            return XorFunc(a, b);
        }

        public static T LeftShift(T a, T b)
        {
            if (LeftShiftFunc == null) Init("LeftShift");
            return LeftShiftFunc(a, b);
        }

        public static T RightShift(T a, T b)
        {
            if (RightShiftFunc == null) Init("RightShift");
            return RightShiftFunc(a, b);
        }

        public static bool Equals(T a, T b)
        {
            if (EqualsFunc == null) Init("Equals");
            return EqualsFunc(a, b);
        }

        public static bool NotEquals(T a, T b)
        {
            if (NotEqualsFunc == null) Init("NotEquals");
            return NotEqualsFunc(a, b);
        }

        public static bool LessThan(T a, T b)
        {
            if (LessFunc == null) Init("Less");
            return LessFunc(a, b);
        }

        public static bool GreaterThan(T a, T b)
        {
            if (GreaterFunc == null) Init("Greater");
            return GreaterFunc(a, b);
        }

        public static bool LessThanOrEquals(T a, T b)
        {
            if (LessEqualsFunc == null) Init("LessEquals");
            return LessEqualsFunc(a, b);
        }

        public static bool GreaterThanOrEquals(T a, T b)
        {
            if (GreaterEqualsFunc == null) Init("GreaterEquals");
            return GreaterEqualsFunc(a, b);
        }
    }
}