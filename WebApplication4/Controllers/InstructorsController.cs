using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly ProjectContext _context;

        public InstructorsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructors.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.instructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,Name,LatestQualification,ExpertiseArea,Email")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = instructor.instructorId });


            }
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorId,Name,LatestQualification,ExpertiseArea,Email")] Instructor instructor)
        {
            if (id != instructor.instructorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.instructorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = instructor.instructorId });
            }
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.instructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.Instructors.Remove(instructor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.instructorId == id);
        }




        public IActionResult AddPost()
        {
            return View();
        }

        // POST: Instructors/AddPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(int DiscussionID, int LearnerID, string Post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call the stored procedure
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC Post @LearnerID, @DiscussionID, @Post",
                        new SqlParameter("@LearnerID", LearnerID),
                        new SqlParameter("@DiscussionID", DiscussionID),
                        new SqlParameter("@Post", Post)
                    );

                    return RedirectToAction("Details", new { id = LearnerID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            // If the model is invalid, retain the values in the view
            ViewData["DiscussionID"] = DiscussionID;
            ViewData["LearnerID"] = LearnerID;
            return View();
        }
    



    public IActionResult Login()
        {
            return View();
        }

        // POST: Instructors/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(int instructorId) // Changed parameter name to instructorId
        {
            if (ModelState.IsValid)
            {
                var instructor = await _context.Instructors
                    .FirstOrDefaultAsync(i => i.instructorId == instructorId); // Use instructorId to find the instructor

                if (instructor == null)
                {
                    ViewData["ErrorMessage"] = "Invalid Instructor ID"; // Updated error message for instructorId
                    return View();
                }

                // Authentication successful, redirect to the instructor's details page
                return RedirectToAction("Details", new { id = instructor.instructorId });
            }

            // If we got this far, something failed; re-display the form.
            return View();
        }

    }

    //
}

