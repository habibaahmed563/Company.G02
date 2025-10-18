using Company.G02.DAL.Models;
using Company.G02.PL.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Company.G02.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _UserManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager ,SignInManager<AppUser> signInManager)
        {
            _UserManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp

        [HttpGet] //Get: /Account/
        public IActionResult SignUp()
        {
            return View();
        }

        // P@ssWord1

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto model)
        {
            if(ModelState.IsValid) // Server side Validation
            {
               var user = await _UserManager.FindByNameAsync(model.UserName);
                if(user is null)
                {
                   user =await _UserManager.FindByEmailAsync(model.Email);
                    if(user is null)
                    {
                        // Register
                         user = new AppUser()
                        {
                            UserName = model.UserName,
                            FirstName = model.FirstName,
                            lastName = model.LastName,
                            Email = model.Email,
                            IsAgree = model.IsAgree
                        };
                        var result = await _UserManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            // Send Email To Confirm Email
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SignUp !!");
            }

            return View();
        }

        #endregion

        #region SignIn

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        // P@ssWord1

        [HttpPost] // Account/SignIn
        public async Task<IActionResult> SignIn(SignInDto model)
        {
            if(ModelState.IsValid)
            {
               var user = await _UserManager.FindByEmailAsync(model.Email);
                if(user is not null)
                {
                    var flag = await _UserManager.CheckPasswordAsync(user, model.Password);
                    if(flag)
                    {
                        // Sign In
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if(result.Succeeded)
                        {
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid Login !");
            }


            return View(model);
        }

        #endregion

        #region SignOut

        #endregion
    }
}
