using System;
using System.Diagnostics;
using System.Threading;

namespace UnitTesting.Stubs
{
    public interface ITickProvider
    {
        bool IsProviderSupported { get; }

        long TicksPerSecond { get; }

        long GetTimestamp();

        void Sleep(long numberOfTicks);
    }

    public class HighResolutionTickProvider : ITickProvider
    {
        public bool IsProviderSupported => Stopwatch.IsHighResolution;
        public long TicksPerSecond => Stopwatch.Frequency;
        public long GetTimestamp() => Stopwatch.GetTimestamp();
        public void Sleep(long numberOfTicks)
        {
            double tillNextTickInSec = (double)numberOfTicks / TicksPerSecond;
            int tillNextTickInMs = (int)(tillNextTickInSec * 1000);
            Thread.Sleep(tillNextTickInMs);
        }
    }

    public class DateTimeTickProvider : ITickProvider
    {
        public bool IsProviderSupported => true;
        public long TicksPerSecond => TimeSpan.TicksPerSecond;
        public long GetTimestamp() => DateTime.Now.Ticks;
        public void Sleep(long numberOfTicks)
        {
            double tillNextTickInSec = (double)numberOfTicks / TicksPerSecond;
            int tillNextTickInMs = (int)(tillNextTickInSec * 1000);
            Thread.Sleep(tillNextTickInMs);
        }
    }

    public class MockTickProvider : ITickProvider
    {
        public long Ticks { get; set; }

        public bool IsProviderSupported => true;
        public long TicksPerSecond => 1000;
        public long GetTimestamp() => Ticks;
        public void Sleep(long numberOfTicks) => Thread.Sleep((int)numberOfTicks);
    }

    public class AutoSteppingTickProvider : ITickProvider
    {
        private long _ticks;
        public int TicksPerStep { get; }

        public AutoSteppingTickProvider() : this(10) { }

        public AutoSteppingTickProvider(int ticksPerStep)
        {
            if (ticksPerStep <= 0) throw new ArgumentOutOfRangeException(nameof(ticksPerStep));
            TicksPerStep = ticksPerStep;
        }

        public bool IsProviderSupported => true;
        public long TicksPerSecond => 1000;
        public long GetTimestamp()
        {
            var ticksLocal = _ticks;
            _ticks += TicksPerStep;
            return ticksLocal;


        }

        public void Sleep(long numberOfTicks)
        {
            _ticks += numberOfTicks;
            Thread.Sleep((int)numberOfTicks);
        }
    }
}
