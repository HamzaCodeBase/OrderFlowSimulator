using InstitutionalPoC;
using InstitutionalPoC.Services;

var trades = TradeGenerator.Generate(500000);

var candles = CandleBuilder.Build(trades);

foreach (var c in candles)
{
    SignalEngine.DetectExhaustionAbsorptionComplete(c);

    var poc = SignalEngine.GetPOC(c);
    Console.WriteLine($"POC: {poc}\n");
}

SignalEngine.DetectStackedImbalance(candles);

