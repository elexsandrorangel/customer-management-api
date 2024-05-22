using System.ComponentModel.DataAnnotations;

namespace CustomerManagement.Models.Entities
{
    public class Phones : BaseEntity
    {

        [Required]
        [MaxLength(3)]
        public string DDD { get; set; } = string.Empty;

        [Required]
        [MaxLength(9)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public PhoneType PhoneType { get; set; }

        public Customer Customer { get; set; }
    }

    public enum PhoneType
    {
        Fixo,
        Movel
    }
}
