using Data.DataModels.Base;
using System.Collections.Generic;

namespace Data.DataModels
{
    public class Vehicle : BaseEntity
    {
        public string VehicleName { get; set; }
        public string VehiclePlate { get; set; }
        public virtual List<Container> Containers { get; set; } //Navigation property
    }
}
