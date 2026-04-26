using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionalPoC.Models;

public class Candle
{
    public DateTime StartTime { get; set; }
    public decimal Open { get; set; }
    public decimal Close { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }

    public decimal BuyVolume { get; set; }
    public decimal SellVolume { get; set; }

    public decimal Delta => BuyVolume - SellVolume;

    public Dictionary<decimal, decimal> VolumeProfile { get; set; } = new();
}