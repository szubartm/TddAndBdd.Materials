using System;
using System.Collections.Generic;
using Mono.Options;

namespace TradingSystemConsole.Parsers
{
    public class UpdateTimeParser
    {
        public TimeSpan Time { get; private set; }

        public void Parse(IEnumerable<string> input)
        {
            string time = null;

            var option = new OptionSet
            {
                {
                    "t|T|time|Time=", "Time",
                    v => time = v
                }
            };

            option.Parse(input);

            if (time != null)
            {
                TimeSpan.TryParse(time, out var result);
                Time = result;
            }
        }
    }
}