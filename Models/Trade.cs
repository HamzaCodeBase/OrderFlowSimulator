using System;
using System.Collections.Generic;
using System.Text;

namespace InstitutionalPoC.Models;

public class Trade
{
    public DateTime Timestamp { get; set; }
    public decimal Price { get; set; }
    public decimal Volume { get; set; }
    public string Side { get; set; } // "buy" or "sell"
}
