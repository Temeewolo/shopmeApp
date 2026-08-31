using System;
using System.Collections.Generic;

namespace shopme.Models;

public partial class Produit
{
    public int IdProduit { get; set; }

    public string CodeProduit { get; set; } = null!;

    public string NomProduit { get; set; } = null!;

    public string TypeProduit { get; set; } = null!;

    public DateOnly DateFabricationProduit { get; set; }

    public DateOnly DatePeremptionProduit { get; set; }

    public bool VisibiliteProduit { get; set; }

    public int PrixProduit { get; set; }

    public virtual ICollection<Lignecommande> Lignecommandes { get; set; } = new List<Lignecommande>();
}
