using InstitutionalPoC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionalPoC.Services;

public static class CandleBuilder
{
    public static List<Candle> Build(List<Trade> trades)
    {
        var candles = new List<Candle>();

        var grouped = trades
            .GroupBy(t => new DateTime(t.Timestamp.Year, t.Timestamp.Month, t.Timestamp.Day,
                                       t.Timestamp.Hour, t.Timestamp.Minute, 0));

        foreach (var group in grouped)
        {
            var candle = new Candle
            {
                StartTime = group.Key,
                Open = group.First().Price,
                Close = group.Last().Price,
                High = group.Max(x => x.Price),
                Low = group.Min(x => x.Price)
            };

            foreach (var t in group)
            {
                if (t.Side == "buy")
                    candle.BuyVolume += t.Volume;
                else
                    candle.SellVolume += t.Volume;

                if (!candle.VolumeProfile.ContainsKey(t.Price))
                    candle.VolumeProfile[t.Price] = 0;

                candle.VolumeProfile[t.Price] += t.Volume;
            }

            candles.Add(candle);
        }

        return candles;
    }
}
