using System;
using System.Collections.Generic;

namespace shopme.Models;

public partial class Commande
{
    public int IdCommande { get; set; }

    public string CodeCommande { get; set; } = null!;

    public DateTime DateCommande { get; set; }

    public bool StatutCommande { get; set; }

    public int IdUtilisareur { get; set; }

    public virtual Utilisateur IdUtilisareurNavigation { get; set; } = null!;
}
