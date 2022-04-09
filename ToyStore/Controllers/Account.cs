using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;
using ToyStore.EmailServices;
using ToyStore.Models;
using ToyStore.Models.PasswordChange;
using ToyStore.ViewsModel;

namespace ToyStore.Controllers
{
    public class Account : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private ApplicationContext db;

        public Account(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            db = context;
                }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.UserName };
                // добавляем пользователя
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // генерация токена для пользователя
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new EmailService();
                    await emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");

                        return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
           
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
                if (result.Succeeded)
                {
                    //   var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();
                    var profile = db.Users.Find(userId).Area;
                    if (profile==String.Empty)
                    {
                        return RedirectToAction("Edit", "Profile", new {Id = userId });
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View("Error");
                }

            }
            catch (Exception ex) { }
            return View("Error");

        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            
            LoginModel model = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
     
                return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                    new { ReturnUrl = returnUrl });

            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult>
            ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginModel loginViewModel = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                        (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false,  bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
          
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Name),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on ";

                return View("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(model.Email, "Reset Password",
                    $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl=null)
        
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";


            if (model.ExternalLogins==null)
            {
                if (Regex.IsMatch(model.Email, pattern, RegexOptions.IgnoreCase))
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                var password = await _userManager.CheckPasswordAsync(user, model.Password);
                if (user != null)
                    {
                        // проверяем, подтвержден ли email
                        if (!await _userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                            return View(model);
                        }
                    }
                if (password)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        // проверяем, принадлежит ли URL приложению
                        if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
               }
            }
            LoginModel model_ = new LoginModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            return View(model_);
        }

   

    }
}
