namespace IROM.Util
{
	using System;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	
	/// <summary>
	/// Performs unary operations on generic types.
	/// </summary>
	public static class Unary<A, B>
	{
		public static readonly Func<A, B> Negate;
		public static readonly Func<A, B> Not;
		public static readonly Func<A, B> BitComp;
		public static readonly Func<A, B> IsTrue;
		public static readonly Func<A, B> IsFalse;
		
		static Unary()
		{
			Negate = CreateFunc(Expression.Negate);
			Not = CreateFunc(Expression.Not);
			BitComp = CreateFunc(Expression.OnesComplement);
			IsTrue = CreateFunc(Expression.IsTrue);
			IsFalse = CreateFunc(Expression.IsFalse);
		}
		
		private static Func<A, B> CreateFunc(Func<Expression, UnaryExpression> operation)
		{
			try
			{
				ParameterExpression paramA = Expression.Parameter(typeof(A), "a");
				return Expression.Lambda<Func<A, B>>(operation(paramA), paramA).Compile();
			}catch(InvalidOperationException)
			{
				return null;
			}catch(ArgumentException)
			{
				return null;
			}
		}
	}
	
	/// <summary>
	/// Performs binary operations on generic types.
	/// </summary>
	public static class Binary<A, B, C>
	{
		public static readonly Func<A, B, C> Add;
		public static readonly Func<A, B, C> Subtract;
		public static readonly Func<A, B, C> Multiply;
		public static readonly Func<A, B, C> Divide;
		public static readonly Func<A, B, C> Modulo;
		public static readonly Func<A, B, C> And;
		public static readonly Func<A, B, C> Or;
		public static readonly Func<A, B, C> Xor;
		public static readonly Func<A, B, C> LeftShift;
		public static readonly Func<A, B, C> RightShift;
		public static readonly Func<A, B, C> Equal;
        public static readonly Func<A, B, C> NotEqual;
        public static readonly Func<A, B, C> Less;
        public static readonly Func<A, B, C> Greater;
        public static readonly Func<A, B, C> LessEqual;
        public static readonly Func<A, B, C> GreaterEqual;
		
		static Binary()
		{
			Add = CreateFunc(Expression.Add);
			Subtract = CreateFunc(Expression.Subtract);
			Multiply = CreateFunc(Expression.Multiply);
			Divide = CreateFunc(Expression.Divide);
			Modulo = CreateFunc(Expression.Modulo);
			And = CreateFunc(Expression.And);
			Or = CreateFunc(Expression.Or);
			Xor = CreateFunc(Expression.ExclusiveOr);
			LeftShift = CreateFunc(Expression.LeftShift);
			RightShift = CreateFunc(Expression.RightShift);
			Equal = CreateFunc(Expression.Equal);
			NotEqual = CreateFunc(Expression.NotEqual);
			Less = CreateFunc(Expression.LessThan);
			Greater = CreateFunc(Expression.GreaterThan);
			LessEqual = CreateFunc(Expression.LessThanOrEqual);
			GreaterEqual = CreateFunc(Expression.GreaterThanOrEqual);
		}
		
		private static Func<A, B, C> CreateFunc(Func<Expression, Expression, BinaryExpression> operation)
		{
			try
			{
				ParameterExpression paramA = Expression.Parameter(typeof(A), "a");
				ParameterExpression paramB = Expression.Parameter(typeof(B), "b");
				return Expression.Lambda<Func<A, B, C>>(operation(paramA, paramB), paramA, paramB).Compile();
			}catch(InvalidOperationException)
			{
				return null;
			}catch(ArgumentException)
			{
				return null;
			}
		}
	}
}