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
using Microsoft.AspNetCore.Authorization;

namespace CarMinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "A user with the same email already exists.");
                return BadRequest(ModelState);
            }

            var user = new User { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var confirmationLink = Url.Action("ConfirmEmail", "Authentication",
                    new { userId = user.Id, token = encodedToken }, Request.Scheme);

                // Send the email confirmation link to the user's email address
                var emailService = new EmailService(_configuration); // Inject IConfiguration into the controller's constructor
                var subject = "Email Confirmation";
                var body = $"Please confirm your email by clicking the link: {confirmationLink}";

                emailService.SendEmail(user.Email, subject, body, isBodyHtml: true);

                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Attempt to sign in the user
            var user = await _userManager.FindByNameAsync(model.Identifier) ??
                       await _userManager.FindByEmailAsync(model.Identifier);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("EmailNotConfirmed", "Email is not confirmed.");
                    return BadRequest(ModelState);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, lockoutOnFailure: false, isPersistent: false);

                if (result.Succeeded)
                {
                    var secretKey = _configuration["Jwt:SecretKey"];
                    var issuer = _configuration["Jwt:Issuer"];
                    var audience = _configuration["Jwt:Audience"];

                    var token = JwtHelper.GenerateJwtToken((user.Id).ToString(), user.UserName, secretKey, issuer, audience, 60);

                    var userEmail = user.Email;
                    var username = user.UserName;

                    return Ok(new { token, username, userEmail });
                }
            }

            return Unauthorized();
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            // Decode the token
            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // Confirm the email
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                // User not found, return an error
                return BadRequest("Invalid user.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }

        // NEW CODE THAT NEEDS TO BE VERIFIED ========================================================================================

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto model)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Username);

            if (user != null)
            {
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("EmailNotConfirmed", "Email is not confirmed.");
                    return BadRequest(ModelState);
                }

                // Verify the old token
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = false  // Disable lifetime validation to allow expired tokens
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    tokenHandler.ValidateToken(model.OldToken, tokenValidationParameters, out SecurityToken validatedToken);
                }
                catch (SecurityTokenException)
                {
                    // Invalid old token
                    return Unauthorized();
                }

                // Generate a new token
                var secretKey = _configuration["Jwt:SecretKey"];
                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];

                var newToken = JwtHelper.GenerateJwtToken((user.Id).ToString(), user.UserName, secretKey, issuer, audience, 60);

                return Ok(new { token = newToken });
            }

            return Unauthorized();
        }


    }
}