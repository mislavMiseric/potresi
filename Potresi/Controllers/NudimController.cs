using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Potresi.Data;
using Potresi.Models;

namespace Potresi.Controllers
{
    [Authorize]
    public class NudimController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NudimController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Nudim
        public async Task<IActionResult> Index()
        {
            return View(await _context.Nudim.Where(n => n.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier))).ToListAsync());
        }

        // GET: Ponudeno
        public async Task<IActionResult> Ponudeno()
        {
            return View(await _context.Nudim.Where(n => n.Aktivno == true).ToListAsync());
        }

        // GET: Nudim/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nudim = await _context.Nudim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nudim == null || !nudim.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return NotFound();
            }

            return View(nudim);
        }

        // GET: Nudim/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Nudim/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Opis,Lokacija,Aktivno")] Nudim nudim)
        {
            if (ModelState.IsValid)
            {
                nudim.Vrijeme = DateTime.Now;
                nudim.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Add(nudim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nudim);
        }

        // GET: Nudim/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nudim = await _context.Nudim.FindAsync(id);
            if (nudim == null || !nudim.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return NotFound();
            }
            return View(nudim);
        }

        // POST: Nudim/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Opis,Lokacija,Aktivno")] Nudim nudim)
        {
            var nudimBaza = await _context.Nudim.AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nudimBaza == null)
                return NotFound();
            if (id != nudim.Id || !nudimBaza.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return NotFound();
            }
            nudim.Vrijeme = nudimBaza.Vrijeme;
            nudim.UserId = nudimBaza.UserId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nudim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NudimExists(nudim.Id))
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
            return View(nudim);
        }

        // GET: Nudim/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nudim = await _context.Nudim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nudim == null || !nudim.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return NotFound();
            }

            return View(nudim);
        }

        // POST: Nudim/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nudim = await _context.Nudim.FindAsync(id);
            if (!nudim.UserId.Equals(User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                _context.Nudim.Remove(nudim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }


        // GET: Nudim/Zatrazi/5
        public async Task<IActionResult> Zatrazi(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nudim = await _context.Nudim
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nudim == null || nudim.Aktivno == false)
            {
                return NotFound();
            }

            return View(nudim);
        }

        // POST: Nudim/Zatrazi/5
        [HttpPost, ActionName("Zatrazi")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ZatraziConfirmed(int id)
        {
            var nudim = await _context.Nudim.FindAsync(id);

            nudim.Aktivno = false;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nudim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NudimExists(nudim.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Ponudeno));
            }
            return View(nudim);
        }


        private bool NudimExists(int id)
        {
            return _context.Nudim.Any(e => e.Id == id);
        }
    }
}
