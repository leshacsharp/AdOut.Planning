using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("Ads")]
    public class Ad
    {
        public Ad()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        //Foreign key of table "Users" that is existed in the AdOut.Identity (another microservice)
        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(50,  MinimumLength = 2)]
        public string Title { get; set; }

        public ContentType ContentType { get; set; }

        public AdStatus Status { get; set; }

        [Required]
        public string Path { get; set; }

        [Required]
        public string PreviewPath { get; set; }

        //todo: make initialization of the prop in the constructor instean of init in the codebase!
        public DateTime AddedDate { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public virtual ICollection<PlanAd> PlanAds { get; set; }
    }
}
