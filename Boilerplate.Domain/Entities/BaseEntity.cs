using Boilerplate.Domain.Interfaces;

namespace Boilerplate.Domain.Entities
{
    public abstract class BaseEntity : IAuditable
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
