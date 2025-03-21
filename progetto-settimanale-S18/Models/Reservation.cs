using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace progetto_settimanale_S18.Models
{
    public class Reservation
    {
        [Key]
        public Guid ReservationId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid RoomId { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required] 
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public bool Ended { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [ForeignKey("RoomId")]
        public Room Room { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
