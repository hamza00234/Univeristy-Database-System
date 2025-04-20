﻿using System;
using System.Collections.Generic;

namespace WebApplication4.Models;

public partial class HealthCondition
{
    public int LearnerId { get; set; }

    public int ProfileId { get; set; }

    public string Condition { get; set; } = null!;

    public virtual PersonalizationProfile PersonalizationProfile { get; set; } = null!;
}
