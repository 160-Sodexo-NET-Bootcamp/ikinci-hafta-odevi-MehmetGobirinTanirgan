using System.ComponentModel.DataAnnotations;

namespace SwcsAPI.Dtos
{
    public class VehicleUpdateDto
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long? Id { get; set; }

        [StringLength(50, ErrorMessage = "Vehicle name cannot be more than 50 characters.")]
        public string VehicleName { get; set; }

        [StringLength(14, ErrorMessage = "Vehicle plate cannot be more than 14 characters.")]
        public string VehiclePlate { get; set; }
    }
}
