using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Behavioral.Tools.PlainTypes;
using Common;
using OrderManagementSystem;
using TradingSystemWebApiContract;

namespace Behavioral.Tools.SeparateProcess
{
    public class OrderManagementSystemSimulator : IOrderManagementSystemSimulator
    {
        private readonly HttpClient _client;

        public OrderManagementSystemSimulator(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> Acknowledge(string id)
        {
            var result = await _client.PostAsync("order/acknowledge", Tools.PrepareStringContent(id));
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<bool>();
            return response;
        }

        public async Task<bool> Cancel(string id)
        {
            var request = $"id={id}";
            var result = await _client.PostAsync("order/cancel", Tools.PrepareStringContent(id));
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<bool>();
            return response;
        }

        public async Task<bool> Fill(string id, int quantity)
        {
            var result = await _client.PostAsync("order/fill", Tools.PrepareStringContent(new OrderFillDto() {Id = id, Quantity = quantity}));
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<bool>();
            return response;
        }

        public async Task<IEnumerable<IOrder>> GetAll()
        {
            IList<IOrder> orders = null;
            HttpResponseMessage response = await _client.GetAsync("Order/GetAll");
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<IList<IOrder>>();
            }
            return orders;
        }

        public async Task<IEnumerable<IOrder>> GetAllChilds(string id)
        {
            IEnumerable<OmsOrder> orders = null;
            HttpResponseMessage response = await _client.GetAsync( $"Order/getChilds?id={id}");
            if (response.IsSuccessStatusCode)
            {
                orders = await response.Content.ReadAsAsync<IEnumerable<OmsOrder>>();
            }
            return orders;
        }

        public async Task<IOrder> GetById(string id)
        {
            OmsOrder order = null;
            HttpResponseMessage response = await _client.GetAsync($"Order/get?id={id}");
            if (response.IsSuccessStatusCode)
            {
                order = await response.Content.ReadAsAsync<OmsOrder>();
            }
            return order;
        }

        public async Task<string> Insert(string symbol, Side side, decimal price, int quantity, TimeInForce timeInForce,
            string parentOrderId, StrategyTypes strategy)
        {
            var request = new OrderDto()
            {
                ParentOrderId = null, Price = price, Quantity = quantity, Side = side, Strategy = strategy,
                TimeInForce = timeInForce, Symbol = symbol
            };
            var todoItemJson = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.Unicode,
                "application/json");

            var result = await _client.PostAsync(@"order/insert", todoItemJson);
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsStringAsync();
            return response;
        }

        public async Task<bool> Reject(string id)
        {
            var result = await _client.PostAsync("order/reject", Tools.PrepareStringContent(id));
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<bool>();
            return response;
        }

        public async Task<bool> Update(string id, decimal newPrice, int newQuantity)
        {
            var result = await _client.PostAsync("order/update", Tools.PrepareStringContent(new OrderUpdateDto(){Id = id, NewPrice = newPrice, NewQuantity = newQuantity}));
            result.EnsureSuccessStatusCode();
            var response = await result.Content.ReadAsAsync<bool>();
            return response;
        }
    }
}