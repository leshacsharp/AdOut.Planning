using AdOut.Planning.Model.Enum;
using System;
using System.Collections.Generic;

namespace AdOut.Planning.Model.Dto
{
    public class AdDto
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public ContentType ContentType { get; set; }

        public AdStatus Status { get; set; }

        public string Path { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime? ConfirmationDate { get; set; }

        public IEnumerable<AdPlanDto> Plans { get; set; }

        public IEnumerable<AdPointDto> AdPoints { get; set; }
    }
}
