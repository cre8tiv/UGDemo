using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DemoProject.Data;

namespace DemoProject.Controllers
{
    public class SiteController : Controller
    {
        private readonly DemoProjectContext _context;

        public SiteController(DemoProjectContext context)
        {
            _context = context;
        }

        // GET: Site
        public async Task<IActionResult> Index()
        {
            return View(await _context.Site.ToListAsync());
        }

        // GET: Site/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.Site
                .SingleOrDefaultAsync(m => m.Id == id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // GET: Site/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Url")] Site site)
        {
            if (ModelState.IsValid)
            {
                _context.Add(site);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(site);
        }

        // GET: Site/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.Site.SingleOrDefaultAsync(m => m.Id == id);
            if (site == null)
            {
                return NotFound();
            }
            return View(site);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Url")] Site site)
        {
            if (id != site.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(site);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiteExists(site.Id))
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
            return View(site);
        }

        // GET: Site/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var site = await _context.Site
                .SingleOrDefaultAsync(m => m.Id == id);
            if (site == null)
            {
                return NotFound();
            }

            return View(site);
        }

        // POST: Site/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var site = await _context.Site.SingleOrDefaultAsync(m => m.Id == id);
            _context.Site.Remove(site);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiteExists(int id)
        {
            return _context.Site.Any(e => e.Id == id);
        }
    }
}
