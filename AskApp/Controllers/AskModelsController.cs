using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AskApp.Data;
using AskApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace AskApp.Controllers
{
    public class AskModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AskModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AskModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AskModel.ToListAsync());
        }

        // GET: AskModels/SearchForm
        public async Task<IActionResult> SearchForm()
        {
            return View();
        }

        // POST: AskModels/SearchResults
        public async Task<IActionResult> SearchResults(string SearchPhrase)
        {
            return View("Index", await _context.AskModel.Where(
                i => i.Question.Contains(SearchPhrase)).ToListAsync());
        }

        // GET: AskModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var askModel = await _context.AskModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (askModel == null)
            {
                return NotFound();
            }

            return View(askModel);
        }

        // GET: AskModels/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AskModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] AskModel askModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(askModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(askModel);
        }

        // GET: AskModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var askModel = await _context.AskModel.FindAsync(id);
            if (askModel == null)
            {
                return NotFound();
            }
            return View(askModel);
        }

        // POST: AskModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] AskModel askModel)
        {
            if (id != askModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(askModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AskModelExists(askModel.Id))
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
            return View(askModel);
        }

        // GET: AskModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var askModel = await _context.AskModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (askModel == null)
            {
                return NotFound();
            }

            return View(askModel);
        }

        // POST: AskModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var askModel = await _context.AskModel.FindAsync(id);
            _context.AskModel.Remove(askModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AskModelExists(int id)
        {
            return _context.AskModel.Any(e => e.Id == id);
        }
    }
}
