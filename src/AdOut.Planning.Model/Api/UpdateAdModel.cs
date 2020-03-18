using System.ComponentModel.DataAnnotations;

namespace AdOut.Planning.Model.Api
{
    public class UpdateAdModel
    {
        public int AdId { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
