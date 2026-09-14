namespace shopme.Models
{
    public class PanierItem
    {
        public int IdProduit { get; set; }

        public string NomProduit { get; set; } = null!;

        public int PrixUnitaire { get; set; }

        public int Quantite { get; set; }

        public int Total
        {
            get
            {
                return PrixUnitaire * Quantite;
            }
        }
    }
}