using System;
using System.Collections.Generic;

namespace shopme.Models;

public partial class Lignecommande
{
    public int IdLigneCommande { get; set; }

    public string ProduitLigneCommande { get; set; } = null!;

    public bool StatutLigneCommande { get; set; }

    public int IdProduit { get; set; }

    public virtual Produit IdProduitNavigation { get; set; } = null!;
}
