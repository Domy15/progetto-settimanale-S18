using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using progetto_settimanale_S18.Services;
using progetto_settimanale_S18.ViewModels;

namespace progetto_settimanale_S18.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly ReservationsService _reservationsService;

        public ReservationsController(ReservationsService reservationsService)
        {
            _reservationsService = reservationsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetReservations()
        {
            try
            {
                var reservationsList = await _reservationsService.GetReservationsAsync();

                return PartialView("_ReservationsList", reservationsList);
            }
            catch
            {
                TempData["Error"] = "Error";
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Add(AddReservationViewModel addReservationViewModel)
        {
            var rooms = await _reservationsService.GetRoomsAsync();
            addReservationViewModel.rooms = rooms;

            return View(addReservationViewModel);
        }

        public async Task<IActionResult> CheckEmail(AddReservationViewModel addReservationViewModel)
        {
            var customer = await _reservationsService.CheckEmailAsync(addReservationViewModel.Email);
            if (customer != null) 
            {
                addReservationViewModel.Phone = customer.Phone;
                addReservationViewModel.Name = customer.Name;
                addReservationViewModel.LastName = customer.LastName;
                addReservationViewModel.CustomerId = customer.CustomerId;
                addReservationViewModel.ItExist = true;
            }
            else 
            {
                addReservationViewModel.ItExist = false;
            }



            return RedirectToAction("Add", addReservationViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReservation(AddReservationViewModel addReservationViewModel)
        {
            ModelState.Remove("rooms");
            if (!ModelState.IsValid) 
            {
                return RedirectToAction("Add", addReservationViewModel);
            }

            var result = await _reservationsService.AddReservationAsync(addReservationViewModel, User);

            if(!result)
            {
                TempData["Error"] = "Error while saving reservation";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var reservation = await _reservationsService.GetReservationById(id);
            var rooms = await _reservationsService.GetAllRoomsAsync();

            if (reservation == null)
            {
                return RedirectToAction("Index");
            };

            var editReservation = new EditReservationViewModel()
            {
                rooms = rooms,
                Email = reservation.Customer.Email,
                Phone = reservation.Customer.Phone,
                StartDate = reservation.StartDate,
                EndDate = reservation.EndDate,
                RoomId = reservation.RoomId,
                Id = id
            };

            return PartialView("_EditReservation", editReservation);
        }

        [HttpPost("Reservations/Edit/Save")]
        public async Task<IActionResult> SaveEdit(EditReservationViewModel editReservationViewModel)
        {
            ModelState.Remove("rooms");
            if (!ModelState.IsValid) 
            {
                return RedirectToAction("Edit");
            }

            var result = await _reservationsService.UpdateReservation(editReservationViewModel);

            if (!result)
            {
                return Json(new
                {
                    success = false,
                    message = "Error while updating entity on database"
                });
            }

            return Json(new
            {
                success = true,
                message = "Entity updated successfully"
            });
        }
    }
}
