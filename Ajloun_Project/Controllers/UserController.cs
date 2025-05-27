using Ajloun_Project.Models;
using Ajloun_Project.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ajloun_Project.Controllers
{
    public class UserController : Controller
    {

        private readonly MyDbContext _Db;
        public UserController(MyDbContext db)
        {
            _Db = db;
        }

        
        public IActionResult signUp()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> checkSignUp(Models.ViewModels.User _User)
        {
            if (ModelState.IsValid)
            {
                var user = new Models.User
                {
                    FullName = _User.FullName,
                    Email = _User.Email,
                    PasswordHash = _User.Password,
                    Phone = _User.Phone,
                    Gender = _User.Gender,
                    BirthDate = _User.BirthDate,
                    Address = _User.Address,
                    NationalId = _User.NationalId,
                    ProfileImage = "profileImg.png",
                    CreatedAt = DateTime.Now
                };

                await _Db.Users.AddAsync(user);
                await _Db.SaveChangesAsync();

                return View(nameof(signIn));
            }
            else
            {

                return View("signUp", _User);

            }

        }

        public IActionResult signIn()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> checkSignIn(UserSignIn _user)
        {
            if (!ModelState.IsValid)
            {
                return View("signIn", _user);
            }
            else
            {
                if (_user.Admin)
                {
                    var admin = await _Db.Admins.FirstOrDefaultAsync(a => a.Email == _user.Email && a.PasswordHash == _user.Password);
                    if (admin != null)
                    {
                        ViewBag.role = "You are Admin";
                        HttpContext.Session.SetString("role", admin.Role);
                        HttpContext.Session.SetInt32("userId", admin.AdminId);

                       return RedirectToAction("Statistics", "Statistics");       
                            
                            }
                    else
                    {
                        ViewBag.role = "You are not Admin";
                        return View("signIn");
                    }
                }
                else
                {
                    var user = await _Db.Users.FirstOrDefaultAsync(u => u.Email == _user.Email && u.PasswordHash == _user.Password);
                    if (user != null)
                    {
                        ViewBag.role = "You are user";
                        HttpContext.Session.SetString("role", "User");
                        HttpContext.Session.SetInt32("userId", user.UserId);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.role = "You are not user";
                        return View("signIn");
                    }

                }
            }
        }
    }
}
