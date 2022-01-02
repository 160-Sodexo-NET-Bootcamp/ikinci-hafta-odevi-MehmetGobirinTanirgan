using System.ComponentModel.DataAnnotations;

namespace SwcsAPI.Dtos
{
    public class ContainerUpdateDto
    {
        [Required]// Null olamaz demek
        [Range(1, long.MaxValue)]// Bu attribute ile aralık sınırlaması yapabiliyoruz
        public long? Id { get; set; }
        // Burda da string için uzunluk sınırlaması yapabiliyoruz
        [StringLength(50, ErrorMessage = "Container name cannot be more than 50 characters.", MinimumLength = 1)]
        public string ContainerName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
