using AdOut.Planning.Model.Enum;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("Weekends")]
    public class Weekend
    {
        public int Id { get; set; }
        public Day Day { get; set; }

        public virtual ICollection<AdPointWeekend> AdPointWeekends { get; set; }
    }
}
