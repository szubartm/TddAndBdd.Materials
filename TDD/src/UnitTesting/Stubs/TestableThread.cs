using System;
using System.Threading;
using Moq;
using NUnit.Framework;

namespace UnitTesting.Stubs
{
    public class OldSchoolScheduler : IDisposable
    {
        private readonly Timer _timer;
        public event EventHandler<EventArgs> OnBeat;

        public OldSchoolScheduler() => _timer = new Timer(o => { OnBeat?.Invoke(this, EventArgs.Empty); }, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

        public void Dispose() => _timer.Dispose();
    }

    [TestFixture]
    public class OldSchoolSchedulerTests
    {
        [Test]
        public void HeartbeatTest()
        {
            int numberOfBeats = 0;

            var sut = new OldSchoolScheduler();
            sut.OnBeat += (s, e) => { Interlocked.Increment(ref numberOfBeats); };

            Thread.Sleep(TimeSpan.FromSeconds(2 * 5 + 2)); //we are interested in 2 messages ->> 2*5sec + 2sec to be "sure"

            Assert.That(numberOfBeats, Is.GreaterThanOrEqualTo(2));

            sut.Dispose(); //what if we forget to dispose in test/production code ?
        }
    }



    [TestFixture]
    public class HeartbeatPublisherTests
    {
        [Test]
        public void HeartbeatTest()
        {
            var messagingService = new Mock<IMessagingService>();
            messagingService.Setup(ms => ms.SendMessage(It.IsAny<HeartbeatMessage>()));

            var scheduler = new Mock<IScheduler>();
            var job = new Mock<ITimerJob>();
            Action scheduledAction = null;
            scheduler.Setup
            (s =>
                 s.Schedule(It.IsAny<Action>(),
                            It.Is((TimeSpan ts) => ts == TimeSpan.FromSeconds(5)),
                            It.Is((TimeSpan ts) => ts == TimeSpan.FromSeconds(5))
                           )
            )
            .Returns(job.Object)
            .Callback
            (
                (Action action, TimeSpan dueTime, TimeSpan period) => { scheduledAction = action; }
            );
            var sut = new HeartbeatPublisher(messagingService.Object, scheduler.Object);
            sut.Start();
            scheduledAction();
            scheduledAction();
            scheduledAction();
            sut.Stop();

            messagingService.Verify(ms => ms.SendMessage(It.IsAny<HeartbeatMessage>()), Times.Exactly(3));
            job.Verify(j => j.Cancel(), Times.Once);
        }
    }

    public interface IScheduler
    {
        ITimerJob Schedule(Action action, TimeSpan dueTime, TimeSpan period);
    }

    public interface ITimerJob
    {
        void Cancel();
    }

    public class TimerScheduler : IScheduler
    {
        public ITimerJob Schedule(Action action, TimeSpan dueTime, TimeSpan period) => new TimerJob(action, dueTime, period);

        sealed class TimerJob : ITimerJob, IDisposable
        {
            private readonly Timer _timer;

            public TimerJob(Action action, TimeSpan dueTime, TimeSpan period) => _timer = new Timer(o => action(), null, dueTime, period);

            public void Cancel() => _timer.Dispose();

            public void Dispose() => _timer.Dispose();
        }
    }

    public class HeartbeatPublisher
    {
        private readonly IMessagingService _messagingService;
        private readonly IScheduler _scheduler;
        private ITimerJob _timerJob;

        public HeartbeatPublisher(IMessagingService messagingService) : this(messagingService, new TimerScheduler()) { }

        public HeartbeatPublisher(IMessagingService messagingService, IScheduler scheduler)
        {
            _messagingService = messagingService;
            _scheduler = scheduler;
        }

        public void Start() => _timerJob = _scheduler.Schedule(SendHeartbeat, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));

        private void SendHeartbeat() => _messagingService.SendMessage(new HeartbeatMessage());

        public void Stop() => _timerJob.Cancel();
    }

    public interface IMessagingService
    {
        void SendMessage(IMessage message);
    }

    public interface IMessage { }

    public class HeartbeatMessage : IMessage { }
}
