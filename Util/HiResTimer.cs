namespace IROM.Util
{
	using System;
	using System.Runtime.InteropServices;
	
	/// <summary>
	/// A high resolution timer for extremely accurate measuring.
	/// In the event that high resolution timing is not supported, falls back on Environment.TickCount.
	/// </summary>
	public static class HiResTimer
	{
		public static readonly bool isSupported = false;
		public static readonly long frequency;
	
	    static HiResTimer()
	    {
	        // Query the high-resolution timer only if it is supported.
	        // A returned frequency of 1000 typically indicates that it is not
	        // supported and is emulated by the OS using the same value that is
	        // returned by Environment.TickCount.
	        // A return value of 0 indicates that the performance counter is
	        // not supported.
	        bool returnVal = QueryPerformanceFrequency(out frequency);
	
	        if (returnVal && frequency != 1000)
	        {
	            // The performance counter is supported.
	            isSupported = true;
	        }else
	        {
	        	frequency = 1000;
	        }
	    }
	
	    public static double CurrentTime
	    {
	        get
	        {
	        	return CurrentTick / (double)frequency;
	        }
	    }
	    
	    public static long CurrentTick
	    {
	    	get
	    	{
	    		if (isSupported)
	            {
	            	long tickCount = 0;
	                // Get the value here if the counter is supported.
	                QueryPerformanceCounter(out tickCount);
	                return tickCount;
	            }else
	            {
	                // Otherwise, use Environment.TickCount.
	                return Environment.TickCount;
	            }
	    	}
	    }
	    
	    // Windows CE native library with QueryPerformanceCounter().
	    [DllImport("kernel32.dll")]
	    private static extern bool QueryPerformanceCounter(out long count);
	    [DllImport("kernel32.dll")]
	    private static extern bool QueryPerformanceFrequency(out long freq);
	}
}
