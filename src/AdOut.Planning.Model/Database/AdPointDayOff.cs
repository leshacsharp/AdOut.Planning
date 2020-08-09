using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("AdPointDayOff")]
    public class AdPointDayOff
    {
        [ForeignKey(nameof(AdPoint))]
        public string AdPointId { get; set; }

        [ForeignKey(nameof(DayOff))]
        public string DayOffId { get; set; }

        [Required]
        public virtual AdPoint AdPoint { get; set; }

        [Required]
        public virtual DayOff DayOff { get; set; }
    }
}
