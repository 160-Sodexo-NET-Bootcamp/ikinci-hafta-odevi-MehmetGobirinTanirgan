using System.ComponentModel.DataAnnotations;

namespace Data.DataModels.Base
{
    public class BaseEntity : IEntity
    {
        [Key]//Primary key attribute
        public long Id { get; set; }
    }
}
