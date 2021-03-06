using AdOut.Extensions.Repositories;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("PlanAds")]
    public class PlanAd : PersistentEntity
    {
        [ForeignKey(nameof(Plan))]
        public string PlanId { get; set; }

        [ForeignKey(nameof(Ad))]
        public string AdId { get; set; }

        [Required]
        public virtual Plan Plan { get; set; }

        [Required]
        public virtual Ad Ad { get; set; }

        public int? Order { get; set; }
    }
}
