﻿using AdOut.Extensions.Communication.Attributes;
using AdOut.Extensions.Repositories;
using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Replication]
    [Table("Plans")]
    public class Plan : PersistentEntity
    {
        public Plan()
        {
            Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        [StringLength(70, MinimumLength = 1)]
        public string Title { get; set; }

        public PlanStatus Status { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }

        public virtual ICollection<PlanAd> PlanAds { get; set; }

        public virtual ICollection<PlanAdPoint> PlanAdPoints { get; set; }
    }
}
