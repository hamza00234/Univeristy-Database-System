﻿using System;
using System.Collections.Generic;

namespace WebApplication4.Models;

public partial class LearnerMastery
{
    public int LearnerId { get; set; }

    public int QuestId { get; set; }

    public string? CompletionStatus { get; set; }

    public virtual Learner Learner { get; set; } = null!;

    public virtual SkillMastery Quest { get; set; } = null!;
}
