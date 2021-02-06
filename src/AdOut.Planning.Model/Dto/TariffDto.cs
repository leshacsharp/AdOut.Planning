using System;

namespace AdOut.Planning.Model.Dto
{
    public class TariffDto
    {
        public double PriceForMinute { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }
    }
}
