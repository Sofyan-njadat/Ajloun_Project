﻿using System;
using System.Collections.Generic;

namespace Ajloun_Project.Models;

public partial class AssocEventRegistration
{
    public int RegistrationId { get; set; }

    public int? AssocEventId { get; set; }

    public int? UserId { get; set; }

    public DateTime? RegisteredAt { get; set; }

    public virtual AssociationEvent? AssocEvent { get; set; }

    public virtual User? User { get; set; }
}
