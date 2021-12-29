using System.ComponentModel.DataAnnotations;

namespace Data.DataModels.Base
{
    public class BaseEntity : IEntity
    {
        [Key]
        public long Id { get; set; }
    }
}
