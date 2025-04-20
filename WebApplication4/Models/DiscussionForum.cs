﻿using System;
using System.Collections.Generic;

namespace WebApplication4.Models;

public partial class DiscussionForum
{
    public int ForumId { get; set; }

    public int? ModuleId { get; set; }

    public int? CourseId { get; set; }

    public string? Title { get; set; }

    public DateTime? LastActive { get; set; }

    public TimeOnly? Timestamp { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<LearnerDiscussion> LearnerDiscussions { get; set; } = new List<LearnerDiscussion>();

    public virtual Module? Module { get; set; }
}
