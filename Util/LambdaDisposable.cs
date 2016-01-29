namespace IROM.Util
{
	using System;
	
	/// <summary>
	/// Simple <see cref="IDisposable"/> implementation that wraps a lambda.
	/// </summary>
	public struct LambdaDisposable : IDisposable
	{
		public Action action;
		
		public void Dispose()
		{
			action();
		}
		
		public static implicit operator LambdaDisposable(Action action)
		{
			return new LambdaDisposable{action = action};
		}
	}
}
