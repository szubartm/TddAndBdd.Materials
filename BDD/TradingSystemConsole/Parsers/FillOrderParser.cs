using System.Collections.Generic;
using Mono.Options;

namespace TradingSystemConsole.Parsers
{
    public class FillOrderParser
    {
        public string Id { get; private set; }
        public int Quantity { get; private set; }

        public void Parse(IEnumerable<string> input)
        {
            string id = null, qty = null;

            var option = new OptionSet
            {
                {
                    "id|Id=", "Id of tsOrder to be rejected",
                    v => id = v
                },
                {
                    "q|Q|quantity|Quantity=", "Quantity",
                    v => qty = v
                }
            };

            option.Parse(input);

            Id = id;
            if (qty != null)
            {
                int.TryParse(qty, out var result);
                Quantity = result;
            }
        }
    }
}