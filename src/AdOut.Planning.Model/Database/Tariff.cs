using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("Tariffs")]
    public class Tariff
    {
        [Key]
        public string Id { get; set; }

        public double PriceForMinute { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        [ForeignKey(nameof(AdPoint))]
        public string AdPointId { get; set; }

        [Required]
        public virtual AdPoint AdPoint { get; set; }
    }
}
