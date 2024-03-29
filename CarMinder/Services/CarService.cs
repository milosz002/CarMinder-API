namespace CarMinder.Services;

using System.Threading.Tasks;
using CarMinder.Models;
using CarMinder.DTOs;
using Microsoft.AspNetCore.Http;
using CarMinder.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using CarMinder.Utilities;
using Microsoft.AspNetCore.Identity;

public class CarService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CarRepository _carRepository;
    private readonly UserRepository _userRepository;

    public CarService(IHttpContextAccessor httpContextAccessor, CarRepository carRepository, UserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _carRepository = carRepository;
        _userRepository = userRepository;
    }

    public async Task<OperationResult> AddCarAsync(CarDataDto carData)
    {
        // Pobierz użytkownika z kontekstu
        var user = _httpContextAccessor.HttpContext.Items["UserData"] as User;

        // konwertowanie danych z DTO na Car
        var car = ConvertToCar(carData, user.Id);

        // Wywołanie metody z CarRepository
        var result = await _userRepository.addCar(car, user);

        return result;
    }

    public async Task<OperationResult> EditCarAsync(CarDataDto carData)
    {
        var user = _httpContextAccessor.HttpContext.Items["UserData"] as User;

        // konwertowanie danych z DTO na Car
        var car = ConvertToCar(carData, user.Id);

        // Wywołanie metody z CarRepository
        var result = await _carRepository.editCar(car);

        return result;
    }

    
    public async Task<OperationResult> RemoveCarAsync(CarDataDto carData)
    {
        var user = _httpContextAccessor.HttpContext.Items["UserData"] as User;

        // konwertowanie danych z DTO na Car
        var car = ConvertToCar(carData, user.Id);

        // Wywołanie metody z CarRepository
        var result = await _carRepository.removeCar(car);

        return result;
    }

    public async Task<GetCarsResult> AllCarsData()
    {
        var user = _httpContextAccessor.HttpContext.Items["UserData"] as User;

        // Wywołanie metody z CarRepository
        var result = await _carRepository.getAllUserCars(user.Id);

        return result;
    }

    public Car ConvertToCar(CarDataDto model, string userId)
    {
        var newCar = new Car
        {
            UserId = userId,
            car_local_id = model.id,
            model_id = model.model_id,
            title = model.title,
            model_year = model.model_year,
            model_name = model.model_name,
            make_display = model.make_display,
            model_body = model.model_body,
            model_engine_position = model.model_engine_position,
            model_engine_cc = model.model_engine_cc,
            model_engine_type = model.model_engine_type,
            model_engine_power_ps = model.model_engine_power_ps,
            model_top_speed_kph = model.model_top_speed_kph,
            model_drive = model.model_drive,
            model_transmission_type = model.model_transmission_type,
            model_seats = model.model_seats,
            model_weight_kg = model.model_weight_kg,
            make_country = model.make_country,
            model_lkm_hwy = model.model_lkm_hwy,
            model_lkm_mixed = model.model_lkm_mixed,
            model_lkm_city = model.model_lkm_city,
            model_fuel_cap_l = model.model_fuel_cap_l,
            car_image_url = model.car_image_url,
            vehicle_technical_inspection_deadline = model.vehicle_technical_inspection_deadline,
            vehicle_technical_inspection_deadline_notification_id = model.vehicle_technical_inspection_deadline_notification_id,
       };

        return newCar;
    }

}

