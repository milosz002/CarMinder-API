using CarMinder.Data;
using CarMinder.Models;
using CarMinder.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace CarMinder.Repository
{
    public class UserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;


        public UserRepository(UserManager<User> userManager, AppDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        public async Task<OperationResult> addCar(Car carToSave, User user)
        { 
            user.Cars.Add(carToSave);

            return await userUpdateResult(user, "Car added successfully");
            
        }


        public async Task<OperationResult> userUpdateResult(User user, string succeededMessage)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new OperationResult(true, succeededMessage);
            }
            else
            {
                return new OperationResult(false, result.Errors.FirstOrDefault().Description);
            }
        }


    }
}
