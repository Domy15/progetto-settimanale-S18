using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S18.Models
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string Phone { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
