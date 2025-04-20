﻿using System;
using System.Collections.Generic;

namespace WebApplication4.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? Title { get; set; }

    public string? LearningObjective { get; set; }

    public int? CreditPoints { get; set; }

    public int? DifficultyLevel { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<CourseEnrollment> CourseEnrollments { get; set; } = new List<CourseEnrollment>();

    public virtual ICollection<CoursePrerequisite> CoursePrerequisites { get; set; } = new List<CoursePrerequisite>();

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();

    public virtual ICollection<Ranking> Rankings { get; set; } = new List<Ranking>();

    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
}
