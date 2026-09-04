using LearningPlatform.Data;
using LearningPlatform.Dto;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class AuthController(AppDbContext _context) : Controller
    {
        public IActionResult Login()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> CreateUser(UserDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(dto.Password) || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Name))
            {
                ViewBag.ErrorMessage = "All Fields are mandatory!";
                return View("Register");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (existingUser == null)
            {
                var user = new User
                {
                    Name = dto.Name,
                    Email = dto.Email,
                    Username = dto.Username,
                    Password = dto.Password,
                    RoleId = 2
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                ViewBag.ErrorMessage = "User with this email already exists.";
                return View("Register");
            }

            TempData["SuccessMessage"] = "User created Successfully. Please login.";

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> LoginUser(UserDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Email) || string.IsNullOrEmpty(dto.Password))
            {
                ViewBag.ErrorMessage = "Email and Password are mandatory!";
                return View("Login");
            }

            var isUserExit = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (isUserExit == null)
            {
                ViewBag.ErrorMessage = "User with email does not exit";

                return View("Login");
            }
            else
            {
                if (isUserExit.Password == dto.Password)
                {
                    HttpContext.Session.SetInt32("UserId", isUserExit.UserId);
                    HttpContext.Session.SetString("Username", isUserExit.Username);
                    HttpContext.Session.SetString("Role", isUserExit.Role.RoleName);

                    TempData["SuccessMessage"] = "Login Successful!";

                    if (isUserExit.Role.RoleName == "Admin")
                    {
                        return RedirectToAction("Index", "AdminDashboard");
                    }
                    else if (isUserExit.Role.RoleName == "User")
                    {
                        return RedirectToAction("Index", "UserDashboard");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Invalid user role.";
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Incorrect Password";
                    return View("Login");
                }
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Auth");
        }
    }
}