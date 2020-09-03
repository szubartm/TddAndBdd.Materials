using System.Collections.Generic;
using Mono.Options;

namespace TradingSystemConsole.Parsers
{
    public class RejectOrderParser
    {
        public string Id { get; private set; }

        public void Parse(IEnumerable<string> input)
        {
            string id = null;

            var option = new OptionSet
            {
                {
                    "id|Id=", "Id of tsOrder to be rejected",
                    v => id = v
                }
            };

            option.Parse(input);
            Id = id;
        }
    }
}