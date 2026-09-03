using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopme.Models;

namespace shopme.Controllers
{
    [Authorize]
    public class ProduitController : Controller
    {
        private readonly ShopmeContext _context;

        public ProduitController(ShopmeContext context)
        {
            _context = context;
        }

        // GET : Produit
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var produits = await _context.Produits.ToListAsync();

            return View(produits);
        }

        // GET : Produit/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST : Produit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Produits.Add(produit);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(produit);
        }

        // GET : Produit/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }


        // POST : Produit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produit produit)
        {
            if (id != produit.IdProduit)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Produits.Update(produit);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Produits.Any(e => e.IdProduit == produit.IdProduit))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(produit);
        }

        // GET : Produit/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .FirstOrDefaultAsync(p => p.IdProduit == id);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST : Produit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.Produits
                .FindAsync(id);

            if (produit == null)
            {
                return NotFound();
            }

            _context.Produits.Remove(produit);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}