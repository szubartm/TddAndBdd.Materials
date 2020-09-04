using System;
using System.Collections.Generic;
using Common;
using DataProviderSystem;
using OrderManagementSystem;
using TradingSystem.Reporting;
using TradingSystem.Strategies;
using TradingSystem.Validation;

namespace TradingSystem
{
    public class TradingSystemService : IDisposable
    {
        private DateTime _currentTime;
        private int _id;
        private IMarketDataService _marketDataService;
        private readonly IOrderManager _orderManagementSystem;
        private readonly IDictionary<string, TsOrder> _orders = new Dictionary<string, TsOrder>();

        private readonly IOrderValidator _orderValidator;
        private readonly IReporter _reporter;

        private readonly IDictionary<string, IStrategy> _strategies = new Dictionary<string, IStrategy>();
        private readonly IStrategyFactory _strategyFactories;
        private readonly ITimeSimulator _timeSimulator;
        private TradingSessionType _tradingSessionType = TradingSessionType.MarketClosed;

        public TradingSystemService(int id, IOrderManager orderManagementSystem, IMarketDataService marketDataService, ITimeSimulator timeSimulator)
        : this(id, orderManagementSystem, marketDataService, new SimpleStrategyFactory(), new EmptyReporter(), timeSimulator, new SettingsValidator())
        {
        }

        public TradingSystemService(int id, IOrderManager orderManagementSystem, IMarketDataService marketDataService,
            IStrategyFactory strategyFactories, IReporter reporter, ITimeSimulator timeSimulator,
            IOrderValidator orderValidator)
        {
            _id = id;
            _orderManagementSystem = orderManagementSystem;
            _marketDataService = marketDataService;
            _strategyFactories = strategyFactories;
            _reporter = reporter;
            _timeSimulator = timeSimulator;
            _orderValidator = orderValidator;
            _currentTime = DateTime.Now;

            _orderManagementSystem.NewOrderInserted += onNewOrder;
            _orderManagementSystem.OrderChanged += onOrderChange;
            _marketDataService.NewMarketDataReceived += onNewMarketDataReceived;

            _timeSimulator.OnSetTime += onSetTime;
            _timeSimulator.OnUpdateTime += onUpdateTimeChange;
        }

        #region externalEvents
        private void onNewOrder(IOrder order)
        {
            if(order.ParentOrderId == null)
                ProcessNewOrder(order);
        }
        
        private void onOrderChange(OrderChangeType changeType, IOrder order)
        {
            var id = order.ParentOrderId == null ? order.Id : order.ParentOrderId;
            if (!_strategies.TryGetValue(id, out var strategy)) return;

            strategy.onOrderChanged(changeType, order);
        }

        private void onNewMarketDataReceived(MarketData marketData)
        {
            foreach (var strategy in _strategies) strategy.Value.onNewMarketData(marketData);
        }

        #endregion

        #region simulation events

        private void onSetTime(DateTime time)
        {
            _currentTime = time;
            CheckIfTradingSessionHasChanged(_currentTime);
        }

        private void onUpdateTimeChange(TimeSpan time)
        {
            var newTime = _currentTime.Add(time);
            if (newTime <= _currentTime) return;

            CheckIfTradingSessionHasChanged(newTime);

            _currentTime = newTime;
            foreach (var strategy in _strategies) strategy.Value.onTimeChanged(_currentTime);
        }

        private void CheckIfTradingSessionHasChanged(DateTime time)
        {
            foreach (var session in Settings.TradingSessions)
            {
                if (session.StartTime <= time.TimeOfDay && time.TimeOfDay < session.EndTime)
                {
                    if (session.SessionType != _tradingSessionType)
                    {
                        _tradingSessionType = session.SessionType;
                        onTradingSessionChanged(_tradingSessionType);
                    }

                    break;
                }
            }
        }

        private void onTradingSessionChanged(TradingSessionType session)
        {
            foreach (var strategy in _strategies) strategy.Value.onTradingSessionChanged(session);
        }

        #endregion

        #region strategy events
        private void onStrategyFinishedProcessing(string orderId)
        {
            if (_strategies.ContainsKey(orderId))
            {
                _reporter.Send(new OrderFinishedProcessing(_strategies[orderId].ParentOrder));
                _strategies.Remove(orderId);
            }
        }


        #endregion


        private void ProcessNewOrder(IOrder order)
        {
            _reporter.Send(new OrderReceivedReport(order));

            if (!ValidateOrder(order)) return;

            var strategy = CreateStrategy(order);
            strategy.Start();

            _reporter.Send(new NewOrderProcessedReport(order));
        }

        private IStrategy CreateStrategy(IOrder order)
        {
            var strategy = _strategyFactories.CreateStrategy(order, _tradingSessionType, _orderManagementSystem);
            _strategies.Add(order.Id, strategy);
            strategy.StrategyFinishedProcessing += onStrategyFinishedProcessing;
            return strategy;
        }

        private bool ValidateOrder(IOrder order)
        {
            var result = _orderValidator.Validate(order);
            if (!result)
            {
                _reporter.Send(new OrderValidationFailed(order));
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            _orderManagementSystem.NewOrderInserted -= onNewOrder;
            _orderManagementSystem.OrderChanged -= onOrderChange;
            _marketDataService.NewMarketDataReceived -= onNewMarketDataReceived;
            _timeSimulator.OnSetTime -= onSetTime;
            _timeSimulator.OnUpdateTime -= onUpdateTimeChange;
        }
    }
}