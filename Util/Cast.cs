namespace IROM.Util
{
	using System;
	using System.Linq;
	using System.Linq.Expressions;
	
    /// <summary>
    /// Performs casting on generic types.
    /// </summary>
    /// <typeparam name="I">The type to cast.</typeparam>
    /// <typeparam name="O">The type to cast to.</typeparam>
    public static class Cast<I, O>
    {
        //stored function
        private static readonly Func<I, O> CastFunc;

        static Cast()
        {
            ParameterExpression paramA = Expression.Parameter(typeof(I), "a");
            UnaryExpression body = Expression.Convert(paramA, typeof(O));
            CastFunc = Expression.Lambda<Func<I, O>>(body, paramA).Compile();
        }

        /// <summary>
        /// Casts the Input type to the Output type.
        /// </summary>
        /// <param name="a">The var to cast.</param>
        /// <returns>The casted var.</returns>
        public static O CastVal(I a)
        {
            return CastFunc(a);
        }
    }
}
