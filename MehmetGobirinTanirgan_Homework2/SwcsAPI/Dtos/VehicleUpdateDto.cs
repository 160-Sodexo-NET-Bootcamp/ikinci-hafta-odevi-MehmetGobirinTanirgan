using System.ComponentModel.DataAnnotations;

namespace SwcsAPI.Dtos
{
    public class VehicleUpdateDto
    {
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Vehicle name cannot be more than 50 characters.")]
        public string VehicleName { get; set; }

        [Required]
        [StringLength(14, ErrorMessage = "Vehicle plate cannot be more than 14 characters.")]
        public string VehiclePlate { get; set; }
    }
}
