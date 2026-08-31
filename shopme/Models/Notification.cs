using System;
using System.Collections.Generic;

namespace shopme.Models;

public partial class Notification
{
    public int IdNotification { get; set; }

    public string MessageNotification { get; set; } = null!;

    public DateTime DateNptification { get; set; }

    public bool StatutNotification { get; set; }

    public string TypeNotification { get; set; } = null!;
}
