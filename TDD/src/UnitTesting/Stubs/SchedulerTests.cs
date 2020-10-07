using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common.TestUtils;
using NUnit.Framework;

namespace UnitTesting.Stubs
{
    [IntegrationTest]
    [TestFixture("10:00:00", "10:00:15", TypeArgs = new[] { typeof(HighResolutionTickProvider) }, Description = "Real timer tests")]
    [TestFixture("10:00:00", "10:00:05", TypeArgs = new[] { typeof(AutoSteppingTickProvider) }, Description = "Simulated tick provider tests")]
    public sealed class SchedulerTests<TTickProvider> where TTickProvider : ITickProvider, new()
    {
        private readonly DateTime _startTime;
        private readonly DateTime _stopTime;
        private readonly ITickProvider _tickProvider;

        public SchedulerTests(string startTime, string stopTime)
        {
            _startTime = Convert.ToDateTime(startTime);
            _stopTime = Convert.ToDateTime(stopTime);
            _tickProvider = new TTickProvider();
        }

        [Test]
        public void Scheduler_RunSynchronouslyShouldSpeedUpAccordinglyToSpeed()
        {
            //arrange
            var speeds = new[] { 0.5, 1.0, 2.0, 5.0, 10.0, 20.0 };


            var schedulers = (
                         from speed in speeds
                         let scheduler = GetScheduler(new SchedulerSettings(_startTime, _stopTime, 1000, speed))
                         let expectedDuration = (_stopTime - _startTime).Divide(speed)
                         select new { speed, expectedDuration, scheduler }).ToList();

            //act
            var results = (
                from s in schedulers
                let testResult = Measure(() => s.scheduler.RunSynchronously())
                let normalizedResult = testResult.Multiply(s.speed)
                select new { s.speed, testResult, s.expectedDuration, normalizedResult }).ToList();

            //assert + assume
            var minNormalizedResult = results.Min(r => r.normalizedResult).TotalSeconds;
            var maxNormalizedResult = results.Max(r => r.normalizedResult).TotalSeconds;
            var skippedNormalizedResults = results.Select(r => r.normalizedResult.TotalSeconds).Where(nr => nr > minNormalizedResult && nr < maxNormalizedResult).ToList();

            //deviation
            Assume.That(skippedNormalizedResults.Min(), Is.EqualTo(skippedNormalizedResults.Max()).Within(30).Percent);

            var measurementWithinMargin = (
                from r in results
                let success = Is.EqualTo(r.expectedDuration.TotalSeconds).Within(30).Percent.ApplyTo(r.testResult.TotalSeconds)//in nunit 3.0 use ApplyTo(...).IsSuccess. Maybe support for TimeSpan constraints would be fixed in 3.0+
                select success).ToList();
            var numberOfSuccesfulMeasurements = measurementWithinMargin.Count(m => m.IsSuccess);
            Assert.That(numberOfSuccesfulMeasurements, Is.GreaterThanOrEqualTo(2), "Minimum 2 measurements need to succeed");
        }

        [TestCase("10:00:00", "10:00:15", 00100, 20.0, 151)]
        [TestCase("10:00:00", "10:00:15", 01000, 20.0, 16)]
        [TestCase("10:00:00", "10:00:15", 02000, 20.0, 8)]
        [TestCase("10:00:00", "10:00:15", 04000, 20.0, 4)]
        [TestCase("10:00:00", "10:00:15", 05000, 20.0, 4)]
        [TestCase("10:00:00", "10:00:15", 05000, 10.0, 4)]
        [TestCase("10:00:00", "10:00:15", 15000, 20.0, 2)]
        [TestCase("10:00:00", "10:00:15", 16000, 20.0, 1)]
        [TestCase("10:00:00", "10:00:16", 16000, 20.0, 2)]
        public void Scheduler_RunSynchronouslyShouldIterateWholeInterval(string realStart, string realStop, long intervalMs, double speed, int expectedSteps)
        {
            //arrange
            var steps = new List<DateTime>();
            var start = Convert.ToDateTime(realStart);
            var stop = Convert.ToDateTime(realStop);
            var scheduler = GetScheduler(new SchedulerSettings(start, stop, intervalMs, speed));
            scheduler.TickHandler = time =>
            {
                steps.Add(time);
                //Console.WriteLine(time.ToString("HH:mm:ss.ffff"));
            };

            //act
            //var sw = Stopwatch.StartNew();
            scheduler.RunSynchronously();
            //sw.Stop();
            //Console.WriteLine($"Took {sw.Elapsed}");

            //assert
            Assert.That(steps, Has.Count.EqualTo(expectedSteps));
            Assert.That(steps.FirstOrDefault(), Is.Not.Null.And.EqualTo(start));
        }

        private Scheduler GetScheduler(SchedulerSettings ss) => new Scheduler(ss, _tickProvider) { TickHandler = time => { } };

        private static TimeSpan Measure(Action actionToMeasure)
        {
            var stopwatch = Stopwatch.StartNew();
            actionToMeasure();
            stopwatch.Stop();
            return stopwatch.Elapsed;
        }
    }
}
