using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolBlog.Data;
using SolBlog.Models;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace SolBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<BlogUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailService;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, UserManager<BlogUser> userManager, ApplicationDbContext context, IEmailSender emailService, IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _emailService = emailService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ContactMe()
        {
            string? blogUserId = _userManager.GetUserId(User);

            if ( blogUserId == null)
            {
                return NotFound();
            }

            BlogUser? blogUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == blogUserId);

            return View(blogUser);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactMe([Bind("FirstName, LastName, Email")] BlogUser blogUser, string? message)
        {
            string swalmessage = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    string? adminEmail = _configuration["AdminLoginEmail"] ?? Environment.GetEnvironmentVariable("AdminLoginEmail");
                    await _emailService.SendEmailAsync(adminEmail, $"Contact Me Message From - {blogUser.FullName}", message!);
                    swalmessage = "Email sent Successfully!";
                }
                catch (Exception)
                {

                    throw;
                }
                swalmessage = "Error : Unable to send Email";

            }                                  
                return RedirectToAction("ContactMe", new {swalmessage});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}