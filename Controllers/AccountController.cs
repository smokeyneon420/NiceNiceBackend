using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using nicenice.Server.Models;
using System.Security.Claims;

namespace nicenice.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<NiceUser> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<NiceUser> _signInManager;
        private readonly ILogger<AccountController> _logger; // Inject ILogger

        public AccountController(UserManager<NiceUser> userManager, RoleManager<Role> roleManager, SignInManager<NiceUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _logger = logger;
        }
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User model)
        {
            if (model == null)
                return BadRequest("Invalid signup data");

            var user = new NiceUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, DateOfCreation = DateTime.Now };
            var results = await _userManager.CreateAsync(user, model.Password);
            if (results.Succeeded)
            {
                if (!string.IsNullOrEmpty(model.Role))
                {
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new Role { Name = model.Role });
                    }

                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                var signInResults = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);
                if (signInResults.Succeeded)
                {

                    return Ok(new { message = "User registered successfully", user.Email });

                }
                else
                {
                    return BadRequest("Failed to sign in after registration.");
                }
            }
            // Handle errors
            else
            {
                foreach (var error in results.Errors)
                {
                    _logger.LogError("Error during signup: {Error}", error.Description); // Log each error
                }
                // Return validation errors in the response
                var errorMessages = results.Errors.Select(e => e.Description).ToList();
                return BadRequest(new { Errors = errorMessages }); // Return errors in a structured way

            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] Login model)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Invalid login details", error });
            }
            var signInResults = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (signInResults.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var roles = await _userManager.GetRolesAsync(user);

                // Create claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                };

                // Add roles as claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20) // Adjust as needed
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), authProperties);

                foreach (var claim in identity.Claims)
                {
                    Console.WriteLine($"Claim: {claim.Type} = {claim.Value}");
                }
                return Ok(new
                {
                    message = "Logged in successfully",
                    user = new
                    {
                        user.Id,
                        user.Email,
                        user.FirstName,
                        user.LastName,
                        Role = roles.FirstOrDefault()
                    }
                });
            }
            else if (signInResults.IsLockedOut)
            {
                return Unauthorized(new { message = "Your account has been locked due to multiple login attempts. Please try again later" });
            }

            else
            {
                return Unauthorized(new { message = "Invalid credentials. Please check your email and password" });
            }

        }
        [HttpPost("extend-Session")]
        [Authorize(Policy = "RequireLoggedIn")]
        public async Task<IActionResult> ExtendSession()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return Unauthorized(new { message = "User not found" });
            }
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
        };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var expirationTime = DateTimeOffset.UtcNow.AddMinutes(30);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = expirationTime
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), authProperties);

            var remainingTime = (expirationTime - DateTimeOffset.UtcNow).TotalSeconds;

            return Ok(new
            {
                message = "Session extended successfully",
                remainingTime
            });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
