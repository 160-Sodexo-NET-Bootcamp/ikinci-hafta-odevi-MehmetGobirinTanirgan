using Data.DataModels.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.DataModels
{
    public class Container : BaseEntity
    {
        public string ContainerName { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long VehicleId { get; set; }
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }
    }
}
