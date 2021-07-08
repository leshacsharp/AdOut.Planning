using AdOut.Extensions.Communication.Attributes;
using AdOut.Extensions.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Replication]
    [Table("PlanAdPoints")]
    public class PlanAdPoint : PersistentEntity
    {
        [ForeignKey(nameof(Plan))]
        public string PlanId { get; set; }

        [ForeignKey(nameof(AdPoint))]
        public string AdPointId { get; set; }

        [Required]
        public virtual Plan Plan { get; set; }

        [Required]
        public virtual AdPoint AdPoint { get; set; }
    }
}
