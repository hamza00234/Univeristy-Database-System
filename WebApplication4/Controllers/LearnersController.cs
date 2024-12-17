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
    public class LearnersController : Controller
    {
        private readonly ProjectContext _context;

        public LearnersController(ProjectContext context)
        {
            _context = context;
        }

        // GET: Learners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Learners.ToListAsync());
        }

        // GET: Learners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .Include(l => l.PersonalizationProfiles)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // GET: Learners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { id = learner.LearnerId });
            }
            return View(learner);
        }

        // GET: Learners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners.FindAsync(id);
            if (learner == null)
            {
                return NotFound();
            }
            return View(learner);
        }

        // POST: Learners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerId,FirstName,LastName,Gender,BirthDate,Country,CulturalBackground")] Learner learner)
        {
            if (id != learner.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = learner.LearnerId });
            }
            return View(learner);
        }

        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learners
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return View(learner);
        }

        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var learner = await _context.Learners.FindAsync(id);
            if (learner != null)
            {
                _context.Learners.Remove(learner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(int id)
        {
            return _context.Learners.Any(e => e.LearnerId == id);
        }


        public IActionResult AddPost(int? learnerId)
        {
            if (learnerId == null)
            {
                return NotFound();
            }

            // Pass LearnerID to the view
            ViewData["LearnerID"] = learnerId;

            return View();
        }

        // POST: Learners/AddPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPost(int DiscussionID, int LearnerID, string Post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call the stored procedure to insert the post
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC Post @LearnerID, @DiscussionID, @Post",
                        new SqlParameter("@LearnerID", LearnerID),
                        new SqlParameter("@DiscussionID", DiscussionID),
                        new SqlParameter("@Post", Post)
                    );

                    // Redirect to the Learner's details page after submitting the post
                    return RedirectToAction("Details", new { id = LearnerID });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            // Return to the view if the model state is not valid
            ViewData["DiscussionID"] = DiscussionID;
            ViewData["LearnerID"] = LearnerID;
            return View();
        }


        //// GET: Learners/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Learners/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(int LearnerId)
        {
            if (LearnerId <= 0)
            {
                ViewData["ErrorMessage"] = "Invalid Learner ID.";
                return View();
            }

            var learner = await _context.Learners
                .FirstOrDefaultAsync(m => m.LearnerId == LearnerId);

            if (learner != null)
            {
                // Redirect to learner details page if LearnerID exists
                return RedirectToAction("Details", new { id = LearnerId });
            }
            else
            {
                // Display error if learner ID is not found
                ViewData["ErrorMessage"] = "Learner ID not found. Please try again.";
                return View();
            }
        }

        //



    }
}
