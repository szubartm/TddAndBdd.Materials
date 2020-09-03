using System.Collections.Generic;
using Mono.Options;

namespace TradingSystemConsole.Parsers
{
    public class InsertMarketDataParser
    {
        public string Symbol { get; private set; }
        public decimal Bid { get; private set; }
        public decimal Ask { get; private set; }
        public int BidSize { get; private set; }
        public int AskSize { get; private set; }

        public void Parse(IEnumerable<string> input)
        {
            string symbol = null, bid = null, ask = null, bidSize = null, askSize = null;

            var option = new OptionSet
            {
                {
                    "s|S|symbol|Symbol=", "Symbol",
                    v => symbol = v
                },
                {
                    "b|B|bid|Bid=", "Bid",
                    v => bid = v
                },
                {
                    "a|A|ask|Ask=", "Ask",
                    v => ask = v
                },
                {
                    "bs|BS|bidsize|BidSize=", "BidSize",
                    v => bidSize = v
                },
                {
                    "as|AS|asksize|AskSize=", "AskSize",
                    v => askSize = v
                }
            };

            option.Parse(input);

            if (symbol != null) Symbol = symbol;
            if (bid != null)
            {
                decimal.TryParse(bid, out var result);
                Bid = result;
            }

            if (ask != null)
            {
                decimal.TryParse(ask, out var result);
                Ask = result;
            }

            if (bidSize != null)
            {
                int.TryParse(bidSize, out var result);
                BidSize = result;
            }

            if (askSize != null)
            {
                int.TryParse(askSize, out var result);
                AskSize = result;
            }
        }
    }
}