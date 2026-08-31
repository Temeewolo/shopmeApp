using System;
using System.Collections.Generic;

namespace shopme.Models;

public partial class Comporte
{
    public int IdCommande { get; set; }

    public int IdLigneCommande { get; set; }

    public int QuantiteLigneCommande { get; set; }

    public int PrixLigneCommande { get; set; }

    public virtual Commande IdCommandeNavigation { get; set; } = null!;

    public virtual Lignecommande IdLigneCommandeNavigation { get; set; } = null!;
}
