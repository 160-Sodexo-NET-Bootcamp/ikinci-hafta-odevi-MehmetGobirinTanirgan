using System.ComponentModel.DataAnnotations;

namespace SwcsAPI.Dtos
{
    public class ContainerUpdateDto
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long? Id { get; set; }

        [StringLength(50, ErrorMessage = "Container name cannot be more than 50 characters.", MinimumLength = 1)]
        public string ContainerName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
