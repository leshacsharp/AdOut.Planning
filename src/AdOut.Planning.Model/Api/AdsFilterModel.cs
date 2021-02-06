using AdOut.Planning.Model.Enum;
using System;

namespace AdOut.Planning.Model.Api
{
    public class AdsFilterModel
    {
        public string Title { get; set; }

        public ContentType? ContentType { get; set; }

        public AdStatus? Status { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
