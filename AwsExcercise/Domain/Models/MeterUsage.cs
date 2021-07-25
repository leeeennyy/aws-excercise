using System;

namespace Domain.Models
{
    public class MeterUsage
    {
        public string Meter { get; set; }
        public DateTime DateTime { get; set; }
        public int Usage { get; set; }
    }
}
