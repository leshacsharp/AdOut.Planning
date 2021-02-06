using AdOut.Extensions.Authorization;
using System.ComponentModel.DataAnnotations;

namespace AdOut.Planning.Model.Api
{
    public class UpdateAdModel
    {
        [ResourceId]
        public string AdId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }
    }
}
