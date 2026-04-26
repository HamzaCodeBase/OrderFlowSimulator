using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace InstitutionalPoC
{
    public class FinnhubTradeGenerator
    {
        private ClientWebSocket _ws = new();

        public async Task ConnectAsync(string apiKey)
        {
            var url = $"wss://ws.finnhub.io?token={apiKey}";
            await _ws.ConnectAsync(new Uri(url), CancellationToken.None);

            await Subscribe("AAPL");

            _ = ReceiveLoop();
        }

        private async Task Subscribe(string symbol)
        {
            var msg = $"{{\"type\":\"subscribe\",\"symbol\":\"{symbol}\"}}";
            var bytes = Encoding.UTF8.GetBytes(msg);

            await _ws.SendAsync(
                new ArraySegment<byte>(bytes),
                WebSocketMessageType.Text,
                true,
                CancellationToken.None);
        }

        private async Task ReceiveLoop()
        {
            var buffer = new byte[8192];

            while (_ws.State == WebSocketState.Open)
            {
                var result = await _ws.ReceiveAsync(buffer, CancellationToken.None);

                var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine(msg);
            }
        }
    }
}
