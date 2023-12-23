using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShoeStore.Extension;
using ShoeStore.Hellper;
using ShoeStore.Models;
using ShoeStore.ModelViews;
using System.Security.Claims;

namespace ShoeStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ShoeStoreContext _context;
        public AccountController(ShoeStoreContext context)
        {
            _context = context;
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ValidatePhone(string Phone)
        {

            try
            {
                var userCus = _context.Customers.SingleOrDefault(x => x.Phone.ToLower() == Phone.ToLower());
                if (userCus != null)
                    return Json(data: "Số điện thoại : " + Phone + " Đã được sử dụng ");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ValidateEmail(string Email)
        {

            try
            {
                var userCus = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.ToLower() == Email.ToLower());
                if (userCus != null)
                    return Json(data: "Emial : " + Email + " Đã được sử dụng ");
                return Json(data: true);
            }
            catch
            {
                return Json(data: true);
            }

        }
        [Route("dashboard.html", Name = "Dashboard")]
        public IActionResult Dashboard()
        {
            var cusID = HttpContext.Session.GetString("CustomerId"); 
            if (cusID != null)
            {
                var userCus = _context.Customers.AsNoTracking().SingleOrDefault( x => x.CustomerId == Convert.ToInt32(cusID));
                if(userCus != null)
                {
                    var lsOrder = _context.Orders
                        .Include(x => x.TransactStatus)
                        .AsNoTracking()
                        .Where(x => x.CustomerId == userCus.CustomerId)
                        .OrderByDescending(x => x.OrderDate)
                        .ToList();
                    ViewBag.Orders = lsOrder;
                    return View(userCus);
                }
             }
            return RedirectToAction("Login");
        }
            public IActionResult Index()
        {
            return View();
        }
            [HttpGet]
            [AllowAnonymous]
            [Route("login.html", Name = "DangNhap")]
            /*[Route("dang-nhap.html", Name = "DangNhap")]*/
            public ActionResult Login()
            {
                var userCus = HttpContext.Session.GetString("CustomerId"); 
                if (userCus != null)
                {
                    return RedirectToAction("Dashboard", "Account");
                }
                return View();
            }
            [HttpPost]
            [AllowAnonymous]
            [Route("login.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginViewModel customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isEmail = Unilities.IsValidEmail(customer.UserName);
                    if (!isEmail) return View(customer);

                    var userCus = _context.Customers.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName);
                    var userAdmin = _context.Users.AsNoTracking().SingleOrDefault(x => x.Email.Trim() == customer.UserName && x.RoleId == 3);

                    if (userCus == null && userAdmin == null)
                    {
                        return RedirectToAction("Register");
                    }

                    if (userCus != null)
                    {
                        string pass = (customer.Password + userCus.Salt.Trim()).ToMD5();
                        if (userCus.Password != pass)
                        {
                            return View(customer);
                        }

                        if (userCus.Active == false)
                        {
                            return RedirectToAction("ThongBao", "Accounts");
                        }

                        // Lưu Session Makh
                        HttpContext.Session.SetString("CustomerId", userCus.CustomerId.ToString());
                        var userCusId = HttpContext.Session.GetString("CustomerId");

                        // Identity cho Customer
                        var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userCus.FullName),
                    new Claim("CustomerId", userCus.CustomerId.ToString())
                };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync(claimsPrincipal);

                        return RedirectToAction("Dashboard", "Account");
                    }
                    else if (userAdmin != null)
                    {
                        // Identity cho Admin
                        var adminClaims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, userAdmin.FullName),
                    new Claim("UserId", userAdmin.Id.ToString()), // Thêm các claims khác nếu cần
                    new Claim(ClaimTypes.Role, "Admin"), // Đặt quyền admin
                };

                        ClaimsIdentity adminClaimsIdentity = new ClaimsIdentity(adminClaims, "login");
                        ClaimsPrincipal adminClaimsPrincipal = new ClaimsPrincipal(adminClaimsIdentity);

                        await HttpContext.SignInAsync(adminClaimsPrincipal);

                        return RedirectToAction("AdminHome", "Admin"); // Đặt đúng đường dẫn cho Dashboard của Admin
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception (Nên log ra hệ thống log hoặc debug)
                return RedirectToAction("Dashboard", "Account");
            }
            return View(customer);
        }
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("dang-ky.html", Name = "DangKy")]
        public async Task<IActionResult> Register(RegisterView account)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string salt = Unilities.GetRandomKey();
                    Customer userCus = new Customer()
                    {

                        FullName = account.FullName,
                        Phone = account.Phone.Trim().ToLower(),
                        Email = account.Email.Trim().ToLower(),
                        Password = (account.Password + salt.Trim()).ToMD5(),
                        ConfirmPassword = account.Password,
                        Gender = account.Gender,
                        Active = true,
                        Salt = salt,
                        CreateDate = DateTime.Now,
                        LastLogin = DateTime.Now
                    };
                    try
                    { 
                        _context.Add(userCus);
                        await _context.SaveChangesAsync();
                        HttpContext.Session.SetString("CustomerId", userCus.CustomerId.ToString());
                        var taikhoanID = HttpContext.Session.GetString("CustomerId");
                        //Identity
                        var claims = new List<Claim>
                                {
                                new Claim(ClaimTypes.Name, userCus.FullName),
                                new Claim("CustomerId", userCus.CustomerId.ToString())
                                };
                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "login");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        /*      var ShoppingCart = GioHang;
                              if (ShoppingCart.Count > 0) return RedirectToAction("Shipping", "Checkout");*/
                        return RedirectToAction("Dashboard", "Account");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Dashboard", "Account");
                    }
                }
                else
                {
                    return View(account);
                }
            }
            catch
            {   
              return View(account);
            }
        }

        [HttpGet]
        [Route("logout", Name = "Logout")]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("CustomerId");
            return RedirectToAction("Index", "Home");
        }
    }
}
