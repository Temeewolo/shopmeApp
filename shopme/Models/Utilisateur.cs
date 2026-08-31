using System;
using System.Collections.Generic;

namespace shopme.Models;

public partial class Utilisateur
{
    public int IdUtilisareur { get; set; }

    public string NomUtilisateur { get; set; } = null!;

    public int NumeroUtilisateur { get; set; }

    public string FonctionUtilisateur { get; set; } = null!;

    public DateTime InscriptionUtilisateur { get; set; }

    public string MotDePasseUtilisateur { get; set; } = null!;

    public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
}
