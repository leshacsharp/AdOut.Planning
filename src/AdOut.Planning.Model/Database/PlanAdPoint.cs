using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("PlanAdPoints")]
    public class PlanAdPoint
    {
        [ForeignKey(nameof(Plan))]
        public int PlanId { get; set; }

        [ForeignKey(nameof(AdPoint))]
        public int AdPointId { get; set; }

        [Required]
        public virtual Plan Plan { get; set; }

        [Required]
        public virtual AdPoint AdPoint { get; set; }
    }
}
