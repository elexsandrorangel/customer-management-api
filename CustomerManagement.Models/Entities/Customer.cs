using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Models.Entities
{
    public class Customer : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public ICollection<Phones> PhoneNumbers { get; set; } = new List<Phones>();
    }
}
