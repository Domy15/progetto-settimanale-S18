using progetto_settimanale_S18.Models;
using System.ComponentModel.DataAnnotations;

namespace progetto_settimanale_S18.ViewModels
{
    public class EditReservationViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Email is required!")]
        [EmailAddress(ErrorMessage = "Email is not valid!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required!")]
        [StringLength(10, MinimumLength = 10)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Start Date is required!")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End Date is required!")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Chose a room!")]
        public Guid RoomId { get; set; }

        public List<Room> rooms { get; set; }
    }
}
