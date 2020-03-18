using AdOut.Planning.Model.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdOut.Planning.Model.Api
{
    public class AdsFilterModel
    {
        [Required]
        public string UserId { get; set; }

        public string Title { get; set; }

        public ContentType? ContentType { get; set; }

        public AdStatus? Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
