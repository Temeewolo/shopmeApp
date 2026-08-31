using System;
using System.Collections.Generic;

namespace shopme.Models;

public partial class Recevoir
{
    public int IdUtilisareur { get; set; }

    public int IdNotification { get; set; }

    public virtual Notification IdNotificationNavigation { get; set; } = null!;

    public virtual Utilisateur IdUtilisareurNavigation { get; set; } = null!;
}
