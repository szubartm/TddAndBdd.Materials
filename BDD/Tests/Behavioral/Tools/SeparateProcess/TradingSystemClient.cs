using System;
using System.Net.Http;
using System.Threading.Tasks;


namespace Behavioral.Tools.SeparateProcess
{
    public class TradingSystemClient : ITradingSystemClient
    {
        private readonly HttpClient _client;

        public TradingSystemClient(HttpClient client)
        {
            _client = client;
        }

        public async Task SetTimeTo(DateTime time)
        {
            var result = await _client.PutAsync(@"/Time/set", Tools.PrepareStringContent(time));
            result.EnsureSuccessStatusCode();
        }


        public async Task UpdateTimeBy(TimeSpan time)
        {
            var result = await _client.PutAsync(@"/Time/update", Tools.PrepareStringContent(time));
            result.EnsureSuccessStatusCode();
        }
    }
}