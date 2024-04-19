using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesApp.Data;
using JokesApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace JokesApp.Controllers
{
    [Authorize]
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public JokesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jokes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Joke.ToListAsync());
        }

        // POST: Jokes/ShowResults
        public async Task<IActionResult> ShowResults(String searchPhrase)
        {
            return View("Index", await _context.Joke.Where(search => search.JokesQuestion.Contains(searchPhrase)).ToListAsync()); 
        }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .Include(j => j.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: Jokes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JokesQuestion,JokesAnswer, UserId")] Joke joke)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            joke.UserId = currentUser.Id;
            Console.WriteLine(currentUser);
            Console.WriteLine(currentUser.Id);
            Console.WriteLine(joke.ToString);

            if (ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Jika validasi gagal, cetak pesan kesalahan
                foreach (var modelStateEntry in ModelState.Values)
                {
                    foreach (var error in modelStateEntry.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        Console.WriteLine(errorMessage);
                        // Lakukan sesuatu dengan pesan kesalahan...
                    }
                }

                // Kembalikan View dengan model yang diterima
                return View(joke);
            }
            return View(joke);
        }

        // GET: Jokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JokesQuestion,JokesAnswer")] Joke joke)
        {
            if (id != joke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var joke = await _context.Joke.FindAsync(id);
            if (joke != null)
            {
                _context.Joke.Remove(joke);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
            return _context.Joke.Any(e => e.Id == id);
        }
    }
}
