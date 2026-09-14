using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shopme.Models;
using System.Security.Claims;

namespace shopme.Controllers
{
    [Authorize]
    public class PanierController : Controller
    {
        private readonly ShopmeContext _context;

        public PanierController(ShopmeContext context)
        {
            _context = context;
        }

        // ================================
        // AFFICHER LE PANIER
        // ================================

        [HttpGet]
        public IActionResult Index()
        {
            var panier = PanierSession.GetPanier(HttpContext.Session);

            return View(panier);
        }

        // ================================
        // NOMBRE TOTAL D'ARTICLES DU PANIER
        // ================================

        [HttpGet]
        public IActionResult Compteur()
        {
            var panier = PanierSession.GetPanier(HttpContext.Session);

            int nombreArticles = panier.Sum(p => p.Quantite);

            return Json(new
            {
                nombreArticles = nombreArticles
            });
        }


        // ================================
        // AJOUTER UN PRODUIT
        // ================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ajouter(int id)
        {
            var produit = await _context.Produits
                .FirstOrDefaultAsync(p => p.IdProduit == id);

            if (produit == null)
            {
                return NotFound();
            }

            var panier = PanierSession.GetPanier(HttpContext.Session);

            var article = panier.FirstOrDefault(
                p => p.IdProduit == produit.IdProduit
            );

            if (article == null)
            {
                panier.Add(new PanierItem
                {
                    IdProduit = produit.IdProduit,
                    NomProduit = produit.NomProduit,
                    PrixUnitaire = produit.PrixProduit,
                    Quantite = 1
                });
            }
            else
            {
                article.Quantite++;
            }

            PanierSession.SavePanier(
                HttpContext.Session,
                panier
            );

            // Calcul du nombre total d'articles
            int nombreArticles = panier.Sum(p => p.Quantite);

            // Calcul du montant total
            int totalPanier = panier.Sum(p => p.Total);

            return Json(new
            {
                success = true,
                message = produit.NomProduit + " ajouté au panier",
                nombreArticles = nombreArticles,
                totalPanier = totalPanier
            });
        }


        // ================================
        // AUGMENTER LA QUANTITÉ
        // ================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Augmenter(int id)
        {
            var panier = PanierSession.GetPanier(HttpContext.Session);

            var article = panier.FirstOrDefault(
                p => p.IdProduit == id
            );

            if (article != null)
            {
                article.Quantite++;
            }

            PanierSession.SavePanier(
                HttpContext.Session,
                panier
            );

            return RedirectToAction("Index");
        }


        // ================================
        // DIMINUER LA QUANTITÉ
        // ================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Diminuer(int id)
        {
            var panier = PanierSession.GetPanier(HttpContext.Session);

            var article = panier.FirstOrDefault(
                p => p.IdProduit == id
            );

            if (article != null)
            {
                article.Quantite--;

                if (article.Quantite <= 0)
                {
                    panier.Remove(article);
                }
            }

            PanierSession.SavePanier(
                HttpContext.Session,
                panier
            );

            return RedirectToAction("Index");
        }


        // ================================
        // SUPPRIMER UN PRODUIT
        // ================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Supprimer(int id)
        {
            var panier = PanierSession.GetPanier(HttpContext.Session);

            var article = panier.FirstOrDefault(
                p => p.IdProduit == id
            );

            if (article != null)
            {
                panier.Remove(article);
            }

            PanierSession.SavePanier(
                HttpContext.Session,
                panier
            );

            return RedirectToAction("Index");
        }


        // ================================
        // VIDER LE PANIER
        // ================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Vider()
        {
            var panier = new List<PanierItem>();

            PanierSession.SavePanier(
                HttpContext.Session,
                panier
            );

            return RedirectToAction("Index");
        }

        // ================================
        // PASSER LA COMMANDE
        // ================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PasserCommande()
        {
            // ==========================================
            // 1. RÉCUPÉRER LE PANIER
            // ==========================================

            var panier = PanierSession.GetPanier(HttpContext.Session);

            if (panier == null || panier.Count == 0)
            {
                return RedirectToAction(nameof(Index));
            }


            // ==========================================
            // 2. RÉCUPÉRER L'UTILISATEUR CONNECTÉ
            // ==========================================

            var userId = User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            int idUtilisateur = int.Parse(userId);


            // ==========================================
            // 3. COMMENCER LA TRANSACTION
            // ==========================================

            await using var transaction =
                await _context.Database.BeginTransactionAsync();


            try
            {
                // ==========================================
                // 4. CRÉER LA COMMANDE
                // ==========================================

                var commande = new Commande
                {
                    CodeCommande = GenererCodeCommande(),

                    DateCommande = DateTime.Now,

                    StatutCommande = false,

                    IdUtilisareur = idUtilisateur
                };

                _context.Commandes.Add(commande);

                await _context.SaveChangesAsync();


                // ==========================================
                // 5. CRÉER LES LIGNES DE COMMANDE
                // ==========================================

                foreach (var article in panier)
                {
                    var ligneCommande = new Lignecommande
                    {
                        ProduitLigneCommande = article.NomProduit,

                        StatutLigneCommande = true,

                        IdProduit = article.IdProduit
                    };

                    _context.Lignecommandes.Add(ligneCommande);

                    await _context.SaveChangesAsync();


                    // ==========================================
                    // 6. CRÉER LE LIEN DANS COMPORTE
                    // ==========================================

                    await _context.Database.ExecuteSqlInterpolatedAsync($@"
                INSERT INTO comporte
                (
                    idCommande,
                    idLigneCommande,
                    quantiteLigneCommande,
                    prixLigneCommande
                )
                VALUES
                (
                    {commande.IdCommande},
                    {ligneCommande.IdLigneCommande},
                    {article.Quantite},
                    {article.Total}
                )
            ");
                }


                // ==========================================
                // 7. CRÉER LA NOTIFICATION
                // ==========================================

                var notification = new Notification
                {
                    MessageNotification =
                        $"Votre commande {commande.CodeCommande} a été enregistrée avec succès.",

                    DateNptification = DateTime.Now,

                    StatutNotification = false,

                    TypeNotification = "Commande"
                };

                _context.Notifications.Add(notification);

                await _context.SaveChangesAsync();


                // ==========================================
                // 8. ASSOCIER LA NOTIFICATION À L'UTILISATEUR
                // ==========================================

                await _context.Database.ExecuteSqlInterpolatedAsync($@"
    INSERT INTO recevoir
    (
        idUtilisareur,
        idNotification
    )
    VALUES
    (
        {idUtilisateur},
        {notification.IdNotification}
    )
");


                // ==========================================
                // 9. VALIDER TOUTES LES OPÉRATIONS
                // ==========================================

                await transaction.CommitAsync();


                // ==========================================
                // 10. VIDER LE PANIER
                // ==========================================

                PanierSession.SavePanier(
                    HttpContext.Session,
                    new List<PanierItem>()
                );


                // ==========================================
                // 11. AFFICHER LA CONFIRMATION
                // ==========================================

                return RedirectToAction(
                    "Confirmation",
                    "Panier",
                    new
                    {
                        id = commande.IdCommande
                    }
                );
            }
            catch
            {
                // Une erreur est survenue AVANT le Commit
                // donc la transaction peut être annulée.

                await transaction.RollbackAsync();

                throw;
            }
        }
        // ================================
        // GÉNÉRER UN CODE DE COMMANDE
        // ================================

        private string GenererCodeCommande()
        {
            const string caracteres =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var random = new Random();

            return new string(
                Enumerable.Range(0, 6)
                    .Select(_ =>
                        caracteres[random.Next(caracteres.Length)])
                    .ToArray()
            );
        }

        // ================================
        // PAGE DE CONFIRMATION
        // ================================

        [HttpGet]
        public async Task<IActionResult> Confirmation(int id)
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


            // Récupérer la commande
            var commande = await _context.Commandes
                .FirstOrDefaultAsync(c =>
                    c.IdCommande == id &&
                    c.IdUtilisareur == idUtilisateur
                );

            if (commande == null)
            {
                return NotFound();
            }


            // Récupérer les lignes de cette commande
            var lignes = await _context.Comportes
                .Where(c => c.IdCommande == id)
                .ToListAsync();


            // Récupérer les informations des produits
            var produits = new List<PanierItem>();

            foreach (var ligne in lignes)
            {
                var ligneCommande = await _context.Lignecommandes
                    .Include(l => l.IdProduitNavigation)
                    .FirstOrDefaultAsync(
                        l => l.IdLigneCommande == ligne.IdLigneCommande
                    );

                if (ligneCommande != null)
                {
                    produits.Add(new PanierItem
                    {
                        IdProduit = ligneCommande.IdProduit,

                        NomProduit =
                            ligneCommande.IdProduitNavigation.NomProduit,

                        PrixUnitaire =
                            ligne.PrixLigneCommande /
                            ligne.QuantiteLigneCommande,

                        Quantite =
                            ligne.QuantiteLigneCommande
                    });
                }
            }


            ViewBag.Commande = commande;

            return View(produits);
        }
    }
}