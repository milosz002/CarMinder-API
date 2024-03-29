using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarMinder.Models
{
    public class User : IdentityUser
    {
        public List<Car> Cars { get; set; }

        public User()
        {
            Cars = new List<Car>();
        }
    }
}
