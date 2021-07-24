using System;

namespace CSVProcessor
{
    public class MeterUsage
    {
        public Guid MeterId { get; set; }
        public DateTime DateTime { get; set; }
        public int Usage { get; set; }
    }
}
