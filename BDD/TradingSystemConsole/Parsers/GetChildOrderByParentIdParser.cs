using System.Collections.Generic;
using Mono.Options;

namespace TradingSystemConsole.Parsers
{
    public class GetChildOrderByParentIdParser
    {
        public string Id { get; private set; }

        public void Parse(IEnumerable<string> input)
        {
            string id = null;

            var option = new OptionSet
            {
                {
                    "id|Id=", "Id of parent tsOrder",
                    v => id = v
                }
            };
            
            option.Parse(input);

            Id = id;
        }
    }
}