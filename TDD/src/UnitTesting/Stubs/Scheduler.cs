using System;
using System.Threading;

namespace UnitTesting.Stubs
{
    public class Scheduler
    {
        private readonly ITickProvider _tickProvider;
        private DateTime _simulatedTime;
        public SchedulerSettings Settings { get; }

        public Scheduler(SchedulerSettings settings, ITickProvider tickProvider)
        {
            Settings = settings;
            _tickProvider = tickProvider ?? throw new ArgumentNullException(nameof(tickProvider));
        }

        public Action<DateTime> TickHandler { get; set; }

        public void RunSynchronously(CancellationToken cancellationToken = default)
        {
            if (TickHandler == null)
                throw new NullReferenceException($"{nameof(TickHandler)} cannot be null reference.");

            _simulatedTime = Settings.StartTime;
            TimeSpan simulatedInterval = TimeSpan.FromMilliseconds(Settings.IntervalMs);


            long intervalInTicks = Settings.IntervalMs * _tickProvider.TicksPerSecond / 1000;
            intervalInTicks = (long)(intervalInTicks / Settings.SpeedFactor);
            long nextTick = _tickProvider.GetTimestamp() + intervalInTicks;


            while (_simulatedTime <= Settings.StopTime)
            {
                if (cancellationToken.IsCancellationRequested) return;
                
                TickHandler(_simulatedTime);

                long timestamp;
                while ((timestamp = _tickProvider.GetTimestamp()) < nextTick)
                {
                    long tillNextTickInTicks = nextTick - timestamp;
                    if (tillNextTickInTicks <= 0) break;

                    _tickProvider.Sleep(tillNextTickInTicks);
                }
                nextTick += intervalInTicks;

                _simulatedTime += simulatedInterval;
            }
        }

        public override string ToString() => $"Tick using {_tickProvider.GetType().Name} with settings: {Settings}";
    }

    public readonly struct SchedulerSettings
    {
        public DateTime StartTime { get; }
        public DateTime StopTime { get; }
        public long IntervalMs { get; }
        public double SpeedFactor { get; }

        public SchedulerSettings(DateTime startTime, DateTime stopTime, long intervalMs, double speedFactor)
        {
            if (startTime.Kind != stopTime.Kind) throw new ArgumentException($@"{nameof(startTime)}.Kind and {nameof(stopTime)}.Kind should be equal", nameof(startTime));
            if (startTime >= stopTime) throw new ArgumentOutOfRangeException(nameof(startTime), $@"{nameof(startTime)} should be lower than {nameof(stopTime)}");

            if (intervalMs <= 0) throw new ArgumentOutOfRangeException(nameof(intervalMs), $@"{nameof(intervalMs)} should be positive number");
            if (speedFactor <= 0.0) throw new ArgumentOutOfRangeException(nameof(speedFactor), $@"{nameof(speedFactor)} should be greater than zero");

            StartTime = startTime;
            StopTime = stopTime;
            IntervalMs = intervalMs;
            SpeedFactor = speedFactor;
        }

        public override string ToString() => $"<{StartTime:o}->{StopTime:o}> every {IntervalMs}ms with speed x{SpeedFactor}";
    }
}
