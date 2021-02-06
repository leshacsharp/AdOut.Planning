using AdOut.Extensions.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    //Days Off are supplied from AdOut.AdPoint microservice

    [Table("DaysOff")]
    public class DayOff : PersistentEntity
    {
        [Key]
        public string Id { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public virtual ICollection<AdPoint> AdPoints { get; set; }
    }
}
