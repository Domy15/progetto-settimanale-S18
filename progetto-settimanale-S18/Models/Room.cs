using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S18.Models
{
    public class Room
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public string Type {  get; set; }

        [Required]
        public double Price { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
