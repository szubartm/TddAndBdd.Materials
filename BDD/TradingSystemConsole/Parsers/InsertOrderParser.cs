using System;
using System.Collections.Generic;
using Common;
using Mono.Options;

namespace TradingSystemConsole.Parsers
{
    public class InsertOrderParser
    {
        public string Symbol { get; private set; }
        public Side Side { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public TimeInForce TimeInForce { get; private set; }
        public StrategyTypes Strategy { get; private set; }

        public void Parse(IEnumerable<string> input)
        {
            string symbol = null, side = null, price = null, quantity = null, timeInForce = null, strategy = null;

            var option = new OptionSet
            {
                {
                    "sym|Sym|symbol|Symbol=", "Symbol",
                    v => symbol = v
                },
                {
                    "s|S|side|Side=", "Side",
                    v => side = v
                },
                {
                    "p|P|price|Price=", "Price",
                    v => price = v
                },
                {
                    "q|Q|quantity|Quantity=", "Quantity",
                    v => quantity = v
                },
                {
                    "tif|TIF|timeinforce|TimeInForce=", "TimeInForce",
                    v => timeInForce = v
                },
                {
                    "str|Str|strategy|Strategy=", "Strategy Name",
                    v => strategy = v
                }
            };

            option.Parse(input);

            if (symbol != null) Symbol = symbol;
            if (side != null)
            {
                if (side.ToUpper().Contains("B"))
                    Side = Side.Buy;
                else Side = Side.Sell;
            }

            if (price != null)
            {
                decimal.TryParse(price, out var result);
                Price = result;
            }

            if (quantity != null)
            {
                int.TryParse(quantity, out var result);
                Quantity = result;
            }

            if (timeInForce != null)
            {
                Enum.TryParse(timeInForce, out TimeInForce result);
                TimeInForce = result;
            }

            if (strategy != null)
            {
                Enum.TryParse(strategy, out StrategyTypes result);
                Strategy = result;
            }
        }
    }
}