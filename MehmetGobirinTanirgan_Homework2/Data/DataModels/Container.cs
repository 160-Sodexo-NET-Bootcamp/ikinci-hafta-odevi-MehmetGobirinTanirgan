using Data.DataModels.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.DataModels
{
    public class Container : BaseEntity
    {
        public string ContainerName { get; set; }

        //Database'e kaydederken virgülden sonra 2 haneye yuvarlıyordu.Bu attribute ile yuvarlaması engellendi.
        [Column(TypeName = "decimal(10, 6)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(10, 6)")]
        public decimal Longitude { get; set; }
        public long VehicleId { get; set; }

        [ForeignKey("VehicleId")] //İki entity arasında ilişki belirlemek için kullanılan attribute.
        public virtual Vehicle Vehicle { get; set; } //Navigation property
    }
}
