using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class PersonalizationProfilesController : Controller
    {
        private readonly ProjectContext _context;

        public PersonalizationProfilesController(ProjectContext context)
        {
            _context = context;
        }

        // GET: PersonalizationProfiles
        public async Task<IActionResult> Index()
        {
            var projectContext = _context.PersonalizationProfiles.Include(p => p.Learner);
            return View(await projectContext.ToListAsync());
        }

        // GET: PersonalizationProfiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalizationProfile = await _context.PersonalizationProfiles
                .Include(p => p.Learner)
                .FirstOrDefaultAsync(m => m.ProfileId == id);
            if (personalizationProfile == null)
            {
                return NotFound();
            }

            return View(personalizationProfile);
        }

        // GET: PersonalizationProfiles/Create
        public IActionResult Create()
        {
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId");
            return View();
        }

        // POST: PersonalizationProfiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId, ProfileId, PreferredContentType, EmotionalState, PersonalityType")] PersonalizationProfile personalizationProfile)
        {
            // Construct the SQL query with parameters
            string sqlQuery = "INSERT INTO PersonalizationProfiles (LearnerId ,Preferred_content_type, emotional_state, personality_type) " +
                              "VALUES (@LearnerId, @PreferredContentType, @EmotionalState, @PersonalityType);";

            // Execute the query with the dynamic values
            await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                new SqlParameter("@LearnerId", personalizationProfile.LearnerId),
                new SqlParameter("@PreferredContentType", personalizationProfile.PreferredContentType),
                new SqlParameter("@EmotionalState", personalizationProfile.EmotionalState),
                new SqlParameter("@PersonalityType", personalizationProfile.PersonalityType)

            );

            var profileId = _context.PersonalizationProfiles
        .Where(p => p.LearnerId == personalizationProfile.LearnerId) // Adjust filtering as needed
        .OrderByDescending(p => p.ProfileId) // Ensure you get the most recently inserted profile
        .Select(p => p.ProfileId)
        .FirstOrDefault();

            // Redirect to the Details action with the correct ProfileId
            return RedirectToAction("Details", new { id = profileId });
        
    }


        // GET: PersonalizationProfiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalizationProfile = await _context.PersonalizationProfiles.FindAsync(id);
            if (personalizationProfile == null)
            {
                return NotFound();
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", personalizationProfile.LearnerId);
            return View(personalizationProfile);
        }

        // POST: PersonalizationProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerId,ProfileId,PreferredContentType,EmotionalState,PersonalityType")] PersonalizationProfile personalizationProfile)
        {
            if (id != personalizationProfile.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalizationProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalizationProfileExists(personalizationProfile.LearnerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LearnerId"] = new SelectList(_context.Learners, "LearnerId", "LearnerId", personalizationProfile.LearnerId);
            return View(personalizationProfile);
        }

        // GET: PersonalizationProfiles/Delete/5
       

        // POST: PersonalizationProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            string sqlQuery = "DELETE FROM PersonalizationProfiles WHERE PProfileId = @ProfileId";

            // Execute the query with the dynamic values
            await _context.Database.ExecuteSqlRawAsync(sqlQuery,
                new SqlParameter("@ProfileId", id));
            return RedirectToAction("Details");


        }

        private bool PersonalizationProfileExists(int id)
        {
            return _context.PersonalizationProfiles.Any(e => e.LearnerId == id);
        }
    }
}
