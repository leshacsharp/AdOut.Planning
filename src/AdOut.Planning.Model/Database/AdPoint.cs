using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{   
    //AdPoints are supplied from AdOut.AdPoint microservice

    [Table("AdPoints")]
    public class AdPoint
    {
        public AdPoint()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Location { get; set; }

        public TimeSpan StartWorkingTime { get; set; }

        public TimeSpan EndWorkingTime { get; set; }

        public virtual ICollection<PlanAdPoint> PlanAdPoints { get; set; }

        public virtual ICollection<AdPointDayOff> AdPointsDaysOff { get; set; }
    }
}