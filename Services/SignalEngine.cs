using InstitutionalPoC.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionalPoC.Services;

public static class SignalEngine
{
    public static void DetectExhaustionAbsorption(Candle c)
    {
        var delta = c.Delta;
        var totalVolume = c.BuyVolume + c.SellVolume;

        bool buyingExhaustion =
            c.Close > c.Open &&
            delta < totalVolume * 0.1m;     // price moved up && buy volumn is less then 10% of total volume

        bool absorption =
            Math.Abs(c.Close - c.Open) < 0.2m &&
            c.SellVolume > c.BuyVolume * 1.5m;      // price is relatively flat && sell volume is significantly higher than buy volume (at least 50% stronger than buyers)

        Console.WriteLine($"[{c.StartTime}] Exhaustion: {buyingExhaustion}, Absorption: {absorption}");
    }
    public static void DetectExhaustionAbsorptionComplete(Candle c)
    {
        var delta = c.Delta;
        var totalVolume = c.BuyVolume + c.SellVolume;

        if (totalVolume == 0)
            return;

        var body = Math.Abs(c.Close - c.Open);

        // =========================
        // EXHAUSTION (both sides)
        // =========================

        bool buyingExhaustion =
            c.Close > c.Open &&
            delta < totalVolume * 0.1m;

        bool sellingExhaustion =
            c.Close < c.Open &&
            Math.Abs(delta) < totalVolume * 0.1m;

        // =========================
        // ABSORPTION (both sides)
        // =========================

        bool buyAbsorption =
            body < 0.2m &&
            c.SellVolume > c.BuyVolume * 1.5m;

        bool sellAbsorption =
            body < 0.2m &&
            c.BuyVolume > c.SellVolume * 1.5m;

        Console.WriteLine(
            $"[{c.StartTime}] " +
            $"BuyExh: {buyingExhaustion}, SellExh: {sellingExhaustion}, " +
            $"BuyAbs: {buyAbsorption}, SellAbs: {sellAbsorption}"
        );
    }

    public static void DetectStackedImbalance(List<Candle> candles)
    {
        for (int i = 2; i < candles.Count; i++)
        {
            var c1 = candles[i - 2];
            var c2 = candles[i - 1];
            var c3 = candles[i];

            bool cond =
                IsImbalance(c1) &&
                IsImbalance(c2) &&
                IsImbalance(c3);

            if (cond)
                Console.WriteLine($"STACKED IMBALANCE at {c3.StartTime}");
        }
    }

    private static bool IsImbalance(Candle c)
    {
        var total = c.BuyVolume + c.SellVolume;
        if (total == 0) return false;

        var ratio = c.BuyVolume / total;
        return ratio > 0.65m || ratio < 0.35m;
    }

    public static decimal CalculateRVOL(Candle current, List<Candle> history)
    {
        var avg = history.Average(x => x.BuyVolume + x.SellVolume);
        var currentVol = current.BuyVolume + current.SellVolume;

        return avg == 0 ? 1 : currentVol / avg;
    }

    public static decimal GetPOC(Candle c)
    {
        return c.VolumeProfile
            .OrderByDescending(x => x.Value)
            .First().Key;
    }
}
