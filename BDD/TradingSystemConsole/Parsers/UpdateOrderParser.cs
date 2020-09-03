using System.Collections.Generic;
using Mono.Options;

namespace TradingSystemConsole.Parsers
{
    public class UpdateOrderParser
    {
        public string Id { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }

        public void Parse(IEnumerable<string> input)
        {
            string id = null, price = null, quantity = null;

            var option = new OptionSet
            {
                {
                    "id|Id=", "ClientOrder id",
                    v => id = v
                },
                {
                    "p|P|price|Price=", "New Price",
                    v => price = v
                },
                {
                    "q|Q|quantity|Quantity=", "new Quantity",
                    v => quantity = v
                }
            };

            option.Parse(input);

            if (id != null) Id = id;
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
        }
    }
}