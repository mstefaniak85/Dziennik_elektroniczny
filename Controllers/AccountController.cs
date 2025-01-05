using Dziennik_elektroniczny.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dziennik_elektroniczny.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
           
            ViewBag.Roles = new List<string> { "Uczen", "Nauczyciel", "Administrator" };
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new List<string> { "Uczen", "Nauczyciel", "Administrator" }; 
            }

    
            if (string.IsNullOrWhiteSpace(model.Role) || !await _roleManager.RoleExistsAsync(model.Role))
            {
                ModelState.AddModelError("", $"Rola '{model.Role ?? "null"}' nie istnieje.");
                ViewBag.Roles = new List<string> { "Uczen", "Nauczyciel", "Administrator" }; 
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                ModelState.AddModelError("", "Hasło nie może być puste.");
                ViewBag.Roles = new List<string> { "Uczen", "Nauczyciel", "Administrator" }; 
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);

              
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action("ConfirmEmail", "Account",
                    new { userId = user.Id, token = token }, Request.Scheme);

                _logger.LogInformation($"Link aktywacyjny: {confirmationLink}");

                ViewBag.ConfirmationLink = confirmationLink;
                ViewBag.Message = "Twoje konto zostało zarejestrowane. Sprawdź e-mail, aby potwierdzić.";
                return View("RegistrationConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Roles = new List<string> { "Uczen", "Nauczyciel", "Administrator" }; 
            return View(model);
        }

     
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Nie znaleziono użytkownika.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.Message = "Twoje konto zostało potwierdzone. Możesz się zalogować.";
                return View("EmailConfirmed");
            }

            ViewBag.Message = "Wystąpił problem podczas potwierdzania konta.";
            return View("Error");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("", "Nazwa użytkownika i hasło są wymagane.");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    ModelState.AddModelError("", "Nie znaleziono użytkownika.");
                    return View();
                }

                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrator"))
                {
                    return RedirectToAction("Dashboard", "Administrator");
                }
                else if (roles.Contains("Nauczyciel"))
                {
                    return RedirectToAction("Dashboard", "Nauczyciele");
                }
                else if (roles.Contains("Uczen")) // Obsługa ucznia
                {
                    return RedirectToAction("Dashboard", "Uczen");
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Konto zostało zablokowane. Spróbuj ponownie później.");
                return View();
            }

            ModelState.AddModelError("", "Nieprawidłowe dane logowania.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
