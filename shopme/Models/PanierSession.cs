using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace shopme.Models
{
    public static class PanierSession
    {
        private const string PanierKey = "PANIER";

        public static List<PanierItem> GetPanier(ISession session)
        {
            var panier = session.GetString(PanierKey);

            if (string.IsNullOrEmpty(panier))
            {
                return new List<PanierItem>();
            }

            return JsonSerializer.Deserialize<List<PanierItem>>(panier)
                   ?? new List<PanierItem>();
        }

        public static void SavePanier(
            ISession session,
            List<PanierItem> panier)
        {
            session.SetString(
                PanierKey,
                JsonSerializer.Serialize(panier)
            );
        }
    }
}