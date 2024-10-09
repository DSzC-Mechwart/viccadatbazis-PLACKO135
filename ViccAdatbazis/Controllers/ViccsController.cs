using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Controllers
{
    public class ViccsController : Controller
    {
        private readonly ViccDbContext _context;

        public ViccsController(ViccDbContext context)
        {
            _context = context;
        }

        // GET: Viccs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Viccek.ToListAsync());
        }

        // GET: Viccs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vicc = await _context.Viccek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vicc == null)
            {
                return NotFound();
            }

            return View(vicc);
        }

        // GET: Viccs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Viccs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Tartalom,Feltolto,Tetszik,NemTetszik,Aktiv")] Vicc vicc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vicc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vicc);
        }

        // GET: Viccs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vicc = await _context.Viccek.FindAsync(id);
            if (vicc == null)
            {
                return NotFound();
            }
            return View(vicc);
        }

        // POST: Viccs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Tartalom,Feltolto,Tetszik,NemTetszik,Aktiv")] Vicc vicc)
        {
            if (id != vicc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vicc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ViccExists(vicc.Id))
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
            return View(vicc);
        }

        // GET: Viccs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vicc = await _context.Viccek
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vicc == null)
            {
                return NotFound();
            }

            return View(vicc);
        }

        // POST: Viccs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vicc = await _context.Viccek.FindAsync(id);
            if (vicc != null)
            {
                _context.Viccek.Remove(vicc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ViccExists(int id)
        {
            return _context.Viccek.Any(e => e.Id == id);
        }
    }
}
