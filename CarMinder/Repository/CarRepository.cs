using CarMinder.Data;
using CarMinder.Mobile;
using CarMinder.Models;
using CarMinder.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Web.Http.Results;

namespace CarMinder.Repository
{
    public class CarRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CarRepository(UserManager<User> userManager, AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<OperationResult> editCar(Car editedCar)
        {
            var carToEdit = _context.Car.FirstOrDefault(c => c.car_local_id == editedCar.car_local_id && c.UserId == editedCar.UserId);

            if (carToEdit == null)
            {
                return new OperationResult(false, "Car with the specified identifier not found");
            }

            var carProperties = typeof(Car).GetProperties();
            foreach (var property in carProperties)
            {
                // sprawdzanie czy wartość ma set
                if (property.CanWrite && property.Name != "Id") // bez Id bo to ma zostać i nie ma też tego w editedCar
                {
                    // pobieranie wartości z editedCar
                    var valueFromEditedCar = property.GetValue(editedCar);

                    // przypisywanie wartości do carToEdit
                    property.SetValue(carToEdit, valueFromEditedCar);
                }
            }

            return await carUpdateResult("Car edited successfully", "Failed to edit the car");
        }

        public async Task<OperationResult> removeCar(Car removedCar)
        {
            var carToDelete = _context.Car.FirstOrDefault(c => c.car_local_id == removedCar.car_local_id && c.UserId == removedCar.UserId);

            if (carToDelete == null)
            {
                return new OperationResult(false, "Car with the specified identifier not found");
            }

            _context.Car.Remove(carToDelete);

            return await carUpdateResult("Car removed successfully", "Failed to remove the car");
        }

        public async Task<GetCarsResult> getAllUserCars(string userId)
        {
            try
            {
                var allUserCars = await _context.Car.Where(c => c.UserId == userId).Select(c => new MobileAppCarModel
                {
                    id = c.car_local_id,
                    model_id = c.model_id,
                    title = c.title,
                    model_year = c.model_year,
                    model_name = c.model_name,
                    make_display = c.make_display,
                    model_body = c.model_body,
                    model_engine_position = c.model_engine_position,
                    model_engine_cc = c.model_engine_cc,
                    model_engine_type = c.model_engine_type,
                    model_engine_power_ps = c.model_engine_power_ps,
                    model_top_speed_kph = c.model_top_speed_kph,
                    model_drive = c.model_drive,
                    model_transmission_type = c.model_transmission_type,
                    model_seats = c.model_seats,
                    model_weight_kg = c.model_weight_kg,
                    make_country = c.make_country,
                    model_lkm_hwy = c.model_lkm_hwy,
                    model_lkm_mixed = c.model_lkm_mixed,
                    model_lkm_city = c.model_lkm_city,
                    model_fuel_cap_l = c.model_fuel_cap_l,
                    car_image_url = c.car_image_url,
                    vehicle_technical_inspection_deadline = c.vehicle_technical_inspection_deadline,
                    vehicle_technical_inspection_deadline_notification_id = c.vehicle_technical_inspection_deadline_notification_id,
                }).ToListAsync();
                return new GetCarsResult(true, allUserCars);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while retrieving user cars: {ex}");

                return new GetCarsResult(false, null);
            }
        }


        public async Task<OperationResult> carUpdateResult(string succeededMessage, string failedMessage)
        {
            var result = await _context.SaveChangesAsync(); // liczba zaktualizowanych wpisów w bazie danych

            if (result > 0)
            {
                return new OperationResult(true, succeededMessage);
            }
            else
            {
                return new OperationResult(false, failedMessage);
            }
        }
    }
}
