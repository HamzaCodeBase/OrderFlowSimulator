//using InstitutionalPoC.Models;
//using System;
//using System.Collections.Generic;

//namespace InstitutionalPoC;

//public static class TradeGenerator
//{
//    public static List<Trade> Generate(int count)
//    {
//        var trades = new List<Trade>();
//        var rand = new Random();

//        decimal price = 100;

//        // 0 = chop, 1 = trend up, 2 = trend down, 3 = absorption, 4 = exhaustion
//        int regime = 0;
//        int regimeLength = rand.Next(10, 25);
//        int regimeCounter = 0;

//        for (int i = 0; i < count; i++)
//        {
//            if (regimeCounter >= regimeLength)
//            {
//                regime = rand.Next(0, 5);
//                regimeLength = rand.Next(10, 30);
//                regimeCounter = 0;
//            }

//            regimeCounter++;

//            decimal volume;
//            decimal delta;

//            switch (regime)
//            {
//                case 1: // Trend Up
//                    delta = RandomRange(rand, 0.05m, 0.5m);
//                    volume = rand.Next(5, 25);
//                    break;

//                case 2: // Trend Down
//                    delta = -RandomRange(rand, 0.05m, 0.5m);
//                    volume = rand.Next(5, 25);
//                    break;

//                case 3: // Absorption (big volume, small movement)
//                    delta = RandomRange(rand, -0.05m, 0.05m);
//                    volume = rand.Next(50, 150);
//                    break;

//                case 4: // Exhaustion (big move, low follow-through)
//                    delta = RandomRange(rand, -0.8m, 0.8m);
//                    volume = rand.Next(1, 10);
//                    break;

//                default: // Chop
//                    delta = RandomRange(rand, -0.2m, 0.2m);
//                    volume = rand.Next(5, 20);
//                    break;
//            }

//            price += delta;

//            trades.Add(new Trade
//            {
//                Timestamp = DateTime.UtcNow.AddSeconds(i),
//                Price = Math.Round(price, 2),
//                Volume = (int)volume,
//                Side = rand.NextDouble() > 0.5 ? "buy" : "sell"
//            });
//        }

//        return trades;
//    }

//    private static decimal RandomRange(Random r, decimal min, decimal max)
//    {
//        return min + (decimal)r.NextDouble() * (max - min);
//    }
//}
using InstitutionalPoC.Models;
using System;
using System.Collections.Generic;

public class TradeGenerator
{
    private static Random _rng = new Random();
    private static decimal _lastPrice = 150.00m; // Starting reference price

    public static List<Trade> Generate(int count)
    {
        var trades = new List<Trade>();

        for (int i = 0; i < count; i++)
        {
            // 1. Simulate Price Movement (Random Walk)
            // Price changes by -0.2% to +0.2%
            decimal changePercent = (decimal)(_rng.NextDouble() * 0.004 - 0.002);
            _lastPrice += _lastPrice * changePercent;

            // 2. Simulate Volume (Exponential distribution for "normal" size)
            decimal volume = (decimal)(_rng.NextDouble() * 1000 + 1);

            // 3. Randomize Side
            string side = _rng.Next(0, 2) == 0 ? "buy" : "sell";

            trades.Add(new Trade
            {
                Timestamp = DateTime.Now.AddSeconds(-1 * (count - i)),
                Price = Math.Round(_lastPrice, 2),
                Volume = Math.Round(volume, 0),
                Side = side
            });
        }
        return trades;
    }
}