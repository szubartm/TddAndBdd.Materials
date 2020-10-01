using System;
using Common.TestUtils;
using Moq;
using NUnit.Framework;

// ReSharper disable InconsistentNaming
namespace Prosiak.Logic.Tests
{
    internal abstract class PigFoodPriceProvider_WhenGettingDailyPriceOfFood : ContextTest<PigFoodPriceProvider>
    {
        private Exception _exception;
        protected decimal Result;
        private Mock<IPotatoPriceProvider> _potatoPriceProvider;
        private Mock<ICurrencyConverter> _ccyConverter;
        private Pig _pig;
        private decimal _potatoPrice;
        protected decimal PricePerKgInPln;
        private decimal _expectedPrice;

        protected override PigFoodPriceProvider Arrange()
        {
            _potatoPriceProvider = new Mock<IPotatoPriceProvider>();
            _potatoPrice = 100;
            _potatoPriceProvider.Setup(x => x.GetPriceOfHundredKilogramPotatoesFromStockExchange()).Returns(_potatoPrice);

            _ccyConverter = new Mock<ICurrencyConverter>();
            PricePerKgInPln = 4;
            _ccyConverter.Setup(x => x.ConvertFromUsdToPln(_potatoPrice / 100)).Returns(PricePerKgInPln);

            var data = GetData();
            _pig = data.Item1;
            _expectedPrice = data.Item2;

            return new PigFoodPriceProvider(_potatoPriceProvider.Object, _ccyConverter.Object);
        }

        protected abstract (Pig pig, decimal expectedPrice) GetData();

        protected override void Act() 
            => _exception = Trying(() => Result = ObjectUnderTest.GetDailyPriceOfFoodFor(_pig));

        [Test]
        public void ShouldNotThrowAnyException() => _exception.ShouldBeNull();

        [Test]
        public void ShouldReturnCorrectResult() => Result.ShouldBeEqualTo(_expectedPrice);
    }

    [TestFixture]
    internal class PigFoodPriceProvider_WhenGettingDailyPriceOfFoodForYoungPig
        : PigFoodPriceProvider_WhenGettingDailyPriceOfFood
    {
        protected override (Pig pig, decimal expectedPrice) GetData() 
            => (new Pig("vhytecghtr", DateTime.Today.AddDays(-15)), PricePerKgInPln * 2m);
    }

    [TestFixture]
    internal class PigFoodPriceProvider_WhenGettingDailyPriceOfFoodForYoungPigReaching100DaysOld
        : PigFoodPriceProvider_WhenGettingDailyPriceOfFood
    {
        protected override (Pig pig, decimal expectedPrice) GetData() 
            => (new Pig("vhytecghtr", DateTime.Today.AddDays(-100)), PricePerKgInPln * 3.5m);
    }

    [TestFixture]
    internal class PigFoodPriceProvider_WhenGettingDailyPriceOfFoodForPigBeingOver100DaysOld
        : PigFoodPriceProvider_WhenGettingDailyPriceOfFood
    {
        protected override (Pig pig, decimal expectedPrice) GetData() 
            => (new Pig("vhytecghtr", DateTime.Today.AddDays(-101)), PricePerKgInPln * 3.5m);
    }

    [TestFixture]
    internal class PigFoodPriceProvider_WhenGettingDailyPriceOfFoodForPigBeing200DaysOld
        : PigFoodPriceProvider_WhenGettingDailyPriceOfFood
    {
        protected override (Pig pig, decimal expectedPrice) GetData() 
            => (new Pig("vhytecghtr", DateTime.Today.AddDays(-200)), PricePerKgInPln * 3.25m);
    }

    [TestFixture]
    internal class PigFoodPriceProvider_WhenGettingDailyPriceOfFoodForPigBeingVeryOld
        : PigFoodPriceProvider_WhenGettingDailyPriceOfFood
    {
        protected override (Pig pig, decimal expectedPrice) GetData() 
            => (new Pig("vhytecghtr", DateTime.Today.AddDays(-500)), PricePerKgInPln * 3.25m);
    }
}
