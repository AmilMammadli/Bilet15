using Lumiaa.DAL;
using Lumiaa.DTOs.CardDTOs;
using Lumiaa.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lumiaa.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context; 
        }

        public IActionResult Index()
        {
            List<Card> list = _context.Cards.ToList();
            var list2 = list.Select(x => new CardGetDto() { Name = x.Name, Description = x.Description, JobTitle = x.Description,Image = x.Image }).ToList();
            return View(list2);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}