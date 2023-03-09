using System.ComponentModel.DataAnnotations;

namespace ServerMessager.Models.Entitys
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
