using System;
using System.Collections.Generic;

namespace TradingSystem
{
    public static class Settings
    {
        public static IEnumerable<string> AllowedInstruments = new List<string> {"AZN.L", "BMW.DE"};
        public static int DayInAuctionMinSize => 100;
        public static int DayInAuctionMaxSize => 90000;
        public static decimal DayInAuctionMaxPrize => 1000;

        public static int DayMinSize => 100;
        public static int DayMaxSize => 90000;
        public static decimal DayMaxPrize => 1000;
        public static TimeSpan DayInterval => TimeSpan.FromMinutes(15);
        public static int DayMaxChildOrderAllowedSize => 1000;

        public static int SniperMinSize => 100;
        public static int SniperMaxSize => 90000;
        public static decimal SniperMinPrize => 10;
        public static decimal SniperMaxPrize => 1000;
        public static TimeSpan SniperWaitingForFillTimeout => TimeSpan.FromMinutes(1);

        public static int SystemId => 121;

        public static IEnumerable<ExchangeTradingSessionHours> TradingSessions = new List<ExchangeTradingSessionHours>()
        {
            new ExchangeTradingSessionHours(TradingSessionType.MarketClosed, TimeSpan.Parse("00:00:00"), TimeSpan.Parse("7:00:00")),
            new ExchangeTradingSessionHours(TradingSessionType.OpenAuction, TimeSpan.Parse("7:00:00"), TimeSpan.Parse("8:00:00")),
            new ExchangeTradingSessionHours(TradingSessionType.ContinuesTrading, TimeSpan.Parse("8:00:00"), TimeSpan.Parse("12:00:00")),
            new ExchangeTradingSessionHours(TradingSessionType.IntradayAuction, TimeSpan.Parse("12:00:00"), TimeSpan.Parse("12:05:00")),
            new ExchangeTradingSessionHours(TradingSessionType.ContinuesTrading, TimeSpan.Parse("12:05:00"), TimeSpan.Parse("16:30:00")),
            new ExchangeTradingSessionHours(TradingSessionType.IntradayAuction, TimeSpan.Parse("16:30:00"), TimeSpan.Parse("16:35:00")),
            new ExchangeTradingSessionHours(TradingSessionType.MarketClosed, TimeSpan.Parse("16:35:00"), TimeSpan.Parse("24:00:00")),
        };

        //public static IEnumerable<ExchangeTradingSessionHours> TradingSessions = new List<ExchangeTradingSessionHours>()
        //{
        //    new ExchangeTradingSessionHours(TradingSessionType.MarketClosed, TimeSpan.Parse("00:00:00"), TimeSpan.Parse("7:00:00")),
        //    new ExchangeTradingSessionHours(TradingSessionType.OpenAuction, TimeSpan.Parse("7:00:00"), TimeSpan.Parse("8:00:00")),
        //    new ExchangeTradingSessionHours(TradingSessionType.ContinuesTrading, TimeSpan.Parse("8:00:00"), TimeSpan.Parse("16:30:00")),
        //    new ExchangeTradingSessionHours(TradingSessionType.IntradayAuction, TimeSpan.Parse("16:30:00"), TimeSpan.Parse("16:35:00")),
        //    new ExchangeTradingSessionHours(TradingSessionType.MarketClosed, TimeSpan.Parse("16:35:00"), TimeSpan.Parse("24:00:00")),
        //};
    }

    public class ExchangeTradingSessionHours
    {
        public ExchangeTradingSessionHours(TradingSessionType sessionType, TimeSpan startTime, TimeSpan endTime)
        {
            SessionType = sessionType;
            StartTime = startTime;
            EndTime = endTime;
        }

        public TradingSessionType SessionType { get; }
        public TimeSpan StartTime { get; }
        public TimeSpan EndTime { get; }
    }
}