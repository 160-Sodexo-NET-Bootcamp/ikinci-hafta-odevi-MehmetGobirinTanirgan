using System.ComponentModel.DataAnnotations;

namespace SwcsAPI.Dtos
{
    public class ContainerCreateDto // Data annotation validatorlar ile model doğruluğu sağlama
    {
        [Required] // Null göndermek demek
        // Burda da string için uzunluk sınırlaması yapabiliyoruz
        [StringLength(50, ErrorMessage = "Container name cannot be more than 50 characters.", MinimumLength = 1)]
        public string ContainerName { get; set; }

        [Range(-90.0, 90.0)]
        public decimal Latitude { get; set; }

        [Range(-180.0, 180.0)]
        public decimal Longitude { get; set; }

        [Range(1, long.MaxValue)]
        public long VehicleId { get; set; }
    }
}
