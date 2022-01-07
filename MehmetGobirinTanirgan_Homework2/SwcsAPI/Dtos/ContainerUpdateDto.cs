using System.ComponentModel.DataAnnotations;

namespace SwcsAPI.Dtos
{
    public class ContainerUpdateDto
    {
        [Range(1, long.MaxValue)]// Bu attribute ile aralık sınırlaması yapabiliyoruz
        public long Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Container name cannot be more than 50 characters.", MinimumLength = 1)]
        public string ContainerName { get; set; }

        [Range(-90.0, 90.0)]
        public decimal Latitude { get; set; }

        [Range(-180.0, 180.0)]
        public decimal Longitude { get; set; }
    }
}
