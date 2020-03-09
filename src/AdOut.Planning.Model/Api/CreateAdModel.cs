using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AdOut.Planning.Model.Api
{
    public class CreateAdModel
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        public IFormFile Content { get; set; }
    }
}
