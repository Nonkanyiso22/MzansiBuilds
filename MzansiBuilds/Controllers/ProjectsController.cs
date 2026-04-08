using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MzansiBuilds.Data;
using MzansiBuilds.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Linq;

namespace MzansiBuilds.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Feed
        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .Include(p => p.Comments)
                .Include(p => p.Milestones)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(projects);
        }

        // GET: Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.UserId = User.Identity.Name;
                project.CreatedAt = System.DateTime.Now;
                _context.Add(project);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        // POST: Add Comment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int ProjectId, string Content)
        {
            if (!string.IsNullOrWhiteSpace(Content))
            {
                var comment = new Comment
                {
                    ProjectId = ProjectId,
                    Content = Content,
                    UserId = User.Identity.Name,
                    CreatedAt = System.DateTime.Now
                };
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Raise Hand / Request Collaboration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestCollaboration(int ProjectId)
        {
            var project = await _context.Projects.FindAsync(ProjectId);
            if (project != null)
            {
                project.CollaborationRequested = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Add Milestone
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMilestone(int ProjectId, string Description)
        {
            if (!string.IsNullOrWhiteSpace(Description))
            {
                var milestone = new Milestone
                {
                    ProjectId = ProjectId,
                    Description = Description,
                    AchievedAt = System.DateTime.Now
                };
                _context.Milestones.Add(milestone);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Mark Project as Completed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkCompleted(int ProjectId)
        {
            var project = await _context.Projects.FindAsync(ProjectId);
            if (project != null)
            {
                project.Stage = "Completed";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Celebration Wall
        public async Task<IActionResult> CelebrationWall()
        {
            var completedProjects = await _context.Projects
                .Where(p => p.Stage == "Completed")
                .Include(p => p.Comments)
                .Include(p => p.Milestones)
                .ToListAsync();

            return View(completedProjects);
        }
    }
}