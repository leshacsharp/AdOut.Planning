using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("AdPointWeekends")]
    public class AdPointWeekend
    {
        [ForeignKey(nameof(Weekend))]
        public int AdPointId { get; set; }

        [ForeignKey(nameof(Weekend))]
        public int WeekendId { get; set; }

        [Required]
        public virtual AdPoint AdPoint { get; set; }

        [Required]
        public virtual Weekend Weekend { get; set; }
    }
}
