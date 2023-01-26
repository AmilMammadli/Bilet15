using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lumiaa.DAL;
using Lumiaa.DTOs.CardDTOs;
using Lumiaa.Models;
using Lumiaa.Extentions.CreateFileExtr;

namespace Lumiaa.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CardController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;


        public CardController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
              return View(await _context.Cards.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cards == null)
            {
                return NotFound();
            }

            var cardGetDto = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cardGetDto == null)
            {
                return NotFound();
            }

            return View(cardGetDto);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CardPostDto cardPostDto)
        {

            _context.Add(new Card
            {
                Name = cardPostDto.Name,
                Description = cardPostDto.Description,
                JobTitle = cardPostDto.JobTitle,
                Image = cardPostDto.File.CreateFile(_env.WebRootPath, "assets/img")
            });
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cards == null)
            {
                return NotFound();
            }

            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }
            CardUpdateDto cardUpdateDto = new CardUpdateDto()
            {
                cardGetDto = new CardGetDto()
                {
                    Id = card.Id,
                    Name = card.Name,
                    Description = card.Description,
                    JobTitle = card.JobTitle,
                    Image = card.Image
                }
            };
            return View(cardUpdateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CardUpdateDto cardUpdateDto)
        {

            Card? card = await _context.Cards.FindAsync(cardUpdateDto.cardGetDto.Id);
            card.Name = cardUpdateDto.cardPostDto.Name;
            card.JobTitle = cardUpdateDto.cardPostDto.JobTitle;
            card.Description = cardUpdateDto.cardPostDto.Name;
            if(cardUpdateDto.cardPostDto.File != null)
            {
                card.Image = cardUpdateDto.cardPostDto.File.CreateFile(_env.WebRootPath, "assets/img");
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cards == null)
            {
                return NotFound();
            }

            var Cards = await _context.Cards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Cards == null)
            {
                return NotFound();
            }

            return View(Cards);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cards == null)
            {
                return Problem("Entity set 'AppDbContext.CardGetDto'  is null.");
            }
            var cards = await _context.Cards.FindAsync(id);
            if (cards != null)
            {
                _context.Cards.Remove(cards);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CardGetDtoExists(int id)
        {
          return _context.Cards.Any(e => e.Id == id);
        }
    }
}
