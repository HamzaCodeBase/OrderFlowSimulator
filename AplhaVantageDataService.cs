using InstitutionalPoC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace InstitutionalPoC;


public class AlphaVantageDataService
{
    private readonly HttpClient _httpClient = new HttpClient();
    private readonly string _apiKey = "";
    public async Task<List<Candle>> GetDailyCandlesAsync(string symbol)
    {
        // Use the DAILY function
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={symbol}&apikey={_apiKey}";
        var response = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(response);
        var root = doc.RootElement.GetProperty("Time Series (Daily)");

        var candles = new List<Candle>();

        foreach (var property in root.EnumerateObject())
        {
            var data = property.Value;
            candles.Add(new Candle
            {
                StartTime = DateTime.Parse(property.Name),
                Open = decimal.Parse(data.GetProperty("1. open").GetString()),
                High = decimal.Parse(data.GetProperty("2. high").GetString()),
                Low = decimal.Parse(data.GetProperty("3. low").GetString()),
                Close = decimal.Parse(data.GetProperty("4. close").GetString()),
                BuyVolume = decimal.Parse(data.GetProperty("5. volume").GetString()) / 2,
                SellVolume = decimal.Parse(data.GetProperty("5. volume").GetString()) / 2
            });
        }
        return candles.OrderBy(c => c.StartTime).ToList();
    }
    public async Task<List<Candle>> GetCandlesAsync(string symbol)
    {
        string url = $"https://www.alphavantage.co/query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=1min&apikey={_apiKey}";
        var response = await _httpClient.GetStringAsync(url);

        using var doc = JsonDocument.Parse(response);
        var root = doc.RootElement.GetProperty("Time Series (1min)");

        var candles = new List<Candle>();

        foreach (var property in root.EnumerateObject())
        {
            var data = property.Value;
            var candle = new Candle
            {
                StartTime = DateTime.Parse(property.Name),
                Open = decimal.Parse(data.GetProperty("1. open").GetString()),
                High = decimal.Parse(data.GetProperty("2. high").GetString()),
                Low = decimal.Parse(data.GetProperty("3. low").GetString()),
                Close = decimal.Parse(data.GetProperty("4. close").GetString()),
                // Note: Alpha Vantage provides total volume; 
                // you may need a logic to split this if you don't have tick data
                BuyVolume = decimal.Parse(data.GetProperty("5. volume").GetString()) / 2,
                SellVolume = decimal.Parse(data.GetProperty("5. volume").GetString()) / 2
            };

            candles.Add(candle);
        }
        return candles.OrderBy(c => c.StartTime).ToList();
    }
}