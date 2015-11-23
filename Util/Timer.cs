namespace IROM.Util
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	
	/// <summary>
	/// Class for handling timing.
	/// </summary>
    public static class Timer
    {
        /// <summary>
        /// The current list of timers.
        /// </summary>
        private static readonly Dictionary<string, Stopwatch> Timers = new Dictionary<string, Stopwatch>();

        /// <summary>
        /// Starts a millisecond timer of the given name.
        /// </summary>
        /// <param name="name">The timer name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StartTimer(string name)
        {
            Stopwatch watch = new Stopwatch();
            Timers[name] = watch;
            watch.Start();
        }

        /// <summary>
        /// Ends a millisecond timer of the given name and outputs to console.
        /// </summary>
        /// <param name="name">The timer name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndTimer(string name)
        {
            Timers[name].Stop();
            Console.WriteLine(name + ": " + Timers[name].ElapsedMilliseconds + " ms");
        }

        /// <summary>
        /// Starts a nanosecond timer of the given name.
        /// </summary>
        /// <param name="name">The timer name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StartNTimer(string name)
        {
            StartTimer(name);
        }

        /// <summary>
        /// Ends a nanosecond timer of the given name and outputs to console.
        /// </summary>
        /// <param name="name">The timer name.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EndNTimer(string name)
        {
            Timers[name].Stop();
            Console.WriteLine(name + ": " + (long)(Timers[name].ElapsedTicks * 1E9 / Stopwatch.Frequency) + " ns");
        }
    }
}
