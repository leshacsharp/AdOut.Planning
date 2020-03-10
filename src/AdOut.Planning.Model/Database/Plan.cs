﻿using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdOut.Planning.Model.Database
{
    [Table("Plans")]
    public class Plan
    {
        [Key]
        public int Id { get; set; }

        //this is foreign key for table "Users" that exists in AdOut.Identity database (another microservice)
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public PlanType Type { get; set; }

        public PlanStatus Status { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
        public virtual ICollection<PlanAd> PlanAds { get; set; }
    }
}
