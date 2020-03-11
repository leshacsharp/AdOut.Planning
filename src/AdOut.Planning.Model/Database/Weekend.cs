using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    //Weekends are supplied from AdOut.AdPoint microservice

    [Table("Weekends")]
    public class Weekend
    {
        [Key]
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }

        public virtual ICollection<AdPointWeekend> AdPointWeekends { get; set; }
    }
}
