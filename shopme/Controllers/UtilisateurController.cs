using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopme.Models;

namespace shopme.Controllers
{
    public class UtilisateurController : Controller
    {
        private readonly ShopmeContext _context;

        public UtilisateurController(ShopmeContext context)
        {
            _context = context;
        }

        // GET: /Utilisateur/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Utilisateur/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                utilisateur.InscriptionUtilisateur = DateTime.Now;

                _context.Utilisateurs.Add(utilisateur);
                await _context.SaveChangesAsync();

                return RedirectToAction("Success");
            }

            return View(utilisateur);
        }

        // GET: /Utilisateur/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Utilisateur/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nomUtilisateur, string motDePasse)
        {
            var utilisateur = await _context.Utilisateurs
                .FirstOrDefaultAsync(u =>
                    u.NomUtilisateur == nomUtilisateur &&
                    u.MotDePasseUtilisateur == motDePasse);

            if (utilisateur == null)
            {
                ViewBag.Message =
                    "Nom d'utilisateur ou mot de passe incorrect.";

                return View();
            }

            HttpContext.Session.SetString(
                "NomUtilisateur",
                utilisateur.NomUtilisateur
            );

            return RedirectToAction("Index", "Home");   
        }

        // GET: /Utilisateur/Success
        public IActionResult Success()
        {
            return View();
        }
    }
}