using CarMinder.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CarMinder.DTOs;
using Microsoft.AspNetCore.WebUtilities;
using CarMinder.Helpers;
using System.Text;
using CarMinder.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Org.BouncyCastle.Asn1.Pkcs;
using CarMinder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CarMinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly CarService _carService;

        public CarController(UserManager<User> userManager, AppDbContext context, IConfiguration configuration, CarService carService)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _carService = carService;
        }


        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("add-car")]
        public async Task<IActionResult> AddCar(CarDataDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _carService.AddCarAsync(model);

            // Sprawdzanie wyników operacji
            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                ModelState.AddModelError("Message", result.Message);
                return BadRequest(ModelState);
            }
           
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("edit-car")]
        public async Task<IActionResult> EditCar(CarDataDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _carService.EditCarAsync(model);

            // Sprawdzanie wyników operacji
            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                ModelState.AddModelError("Message", result.Message);
                return BadRequest(ModelState);
            }
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("remove-car")]
        public async Task<IActionResult> RemoveCar(CarDataDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _carService.RemoveCarAsync(model);

            // Sprawdzanie wyników operacji
            if (result.Success)
            {
                return Ok(result.Message);
            }
            else
            {
                ModelState.AddModelError("Message", result.Message);
                return BadRequest(ModelState);
            }

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("get-all-cars")]
        public async Task<IActionResult> AllCarsData()
        {
            var result = await _carService.AllCarsData();

            // Sprawdzanie wyników operacji
            if (result.Success)
            {
                return Ok(new { result.CarsList });
            }
            else
            {
                ModelState.AddModelError("Message", "Error");
                return BadRequest(ModelState);
            }
        }


    }
}