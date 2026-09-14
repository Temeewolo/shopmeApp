using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using shopme.Models;

namespace shopme.Controllers
{
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly ShopmeContext _context;

        public NotificationController(ShopmeContext context)
        {
            _context = context;
        }


        // ==========================================
        // AFFICHER LES NOTIFICATIONS
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Récupérer l'utilisateur connecté
            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            int idUtilisateur = int.Parse(userId);


            // Récupérer les notifications de l'utilisateur
            var notifications = await _context.Recevoirs
                .Include(r => r.IdNotificationNavigation)
                .Where(r => r.IdUtilisareur == idUtilisateur)
                .OrderByDescending(
                    r => r.IdNotificationNavigation.DateNptification
                )
                .Select(r => r.IdNotificationNavigation)
                .ToListAsync();


            return View(notifications);
        }
    }
}