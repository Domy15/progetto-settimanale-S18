using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using progetto_settimanale_S18.Data;
using progetto_settimanale_S18.Models;
using progetto_settimanale_S18.ViewModels;

namespace progetto_settimanale_S18.Services
{
    public class ReservationsService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReservationsService(ApplicationDbContext context, UserManager<ApplicationUser> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

        private async Task<bool> SaveAsync()
        {
            try
            {
                var rowsAffected = await _context.SaveChangesAsync();

                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<ReservationsListViewModel> GetReservationsAsync()
        {
            var reservationsList = new ReservationsListViewModel();

            try
            {
                reservationsList.Reservations = await _context.Reservations
                    .Include(r => r.Customer)
                    .Include(r => r.Room)
                    .Include(r => r.User)
                    .Where(r => r.Ended == false)
                    .ToListAsync();
                

                return reservationsList;
            }
            catch
            {
                reservationsList.Reservations = new List<Reservation>();
                return reservationsList;
            }
        }

        public async Task<Customer> CheckEmailAsync(string email)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

            return customer;
        }

        public async Task<List<Room>> GetRoomsAsync()
        {
            var rooms = await _context.Rooms
                .Where(r => r.Reservations.All(r => r.Ended == true))
                .ToListAsync();

            return rooms;
        }

        public async Task<List<Room>> GetAllRoomsAsync()
        {
            var rooms = await _context.Rooms.ToListAsync();

            return rooms;
        }

        public async Task<bool> AddReservationAsync(AddReservationViewModel addReservationViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == addReservationViewModel.Email);
            var user = await _userManager.FindByEmailAsync(claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value);

            if (customer == null)
            {
                customer = new Customer()
                {
                    Email = addReservationViewModel.Email,
                    Name = addReservationViewModel.Name,
                    LastName = addReservationViewModel.LastName,
                    Phone = addReservationViewModel.Phone,
                    CustomerId = Guid.NewGuid(),
                };
                _context.Customers.Add(customer);
            }

            var reservation = new Reservation()
            {
                ReservationId = Guid.NewGuid(),
                CustomerId = customer.CustomerId,
                RoomId = addReservationViewModel.RoomId,
                StartDate = addReservationViewModel.StartDate,
                EndDate = addReservationViewModel.EndDate,
                UserId = user.Id,
            }; 

            _context.Reservations.Add(reservation);

            return await SaveAsync();
        }

        public async Task<Reservation?> GetReservationById(Guid id)
        {
            try
            {
                var reservation = await _context.Reservations.Include(r => r.Customer).Include(r => r.Room).FirstAsync(r => r.ReservationId == id);
                if (reservation == null) 
                {
                    return null;
                }

                return reservation;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateReservation(EditReservationViewModel editReservationViewModel)
        {
            var reservation = await _context.Reservations.Include(r => r.Customer).Include(r => r.Room).FirstAsync(r => r.ReservationId == editReservationViewModel.Id);

            if (reservation == null) 
            { 
                return false;
            }

            reservation.StartDate = editReservationViewModel.StartDate;
            reservation.EndDate = editReservationViewModel.EndDate;
            reservation.Customer.Phone = editReservationViewModel.Phone;
            reservation.Customer.Email = editReservationViewModel.Email;
            reservation.RoomId = editReservationViewModel.RoomId;

            return await SaveAsync();
        }
    }
}
