using System.ComponentModel.DataAnnotations;
using progetto_settimanale_S18.Models;

namespace progetto_settimanale_S18.ViewModels
{
    public class AddReservationViewModel
    {
        public Guid CustomerId { get; set; }

        public bool? ItExist { get; set; } = null;

        [Required(ErrorMessage = "Name is required!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "LastName is required!")]
        public string LastName { get; set; }

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
