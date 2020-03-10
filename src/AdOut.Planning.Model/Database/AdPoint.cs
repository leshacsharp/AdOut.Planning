using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("AdPoints")]
    public class AdPoint
    {
        [Key]
        public int Id { get; set; }

        public TimeSpan StartWorkingTime { get; set; }

        public TimeSpan EndWorkingTime { get; set; }

        public virtual ICollection<PlanAdPoint> PlanAdPoints { get; set; }
        public virtual ICollection<AdPointWeekend> AdPointWeekends { get; set; }
    }
}
