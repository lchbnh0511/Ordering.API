using System.ComponentModel.DataAnnotations;

namespace Ordering.API.Models.RequestModels
{
    public class CategoryRequestModel
    {
        [Required]
        [StringLength(255)]
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        [StringLength(2048)]
        public string? PictureUrl { get; set;}
    }
}
