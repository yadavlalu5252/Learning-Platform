using LearningPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class CourseController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.MasterCourse
                .FirstOrDefaultAsync(x => x.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var subCourses = await _context.SubCourse
                .Where(x => x.MasterCourseId == id && x.Status == "Active")
                .ToListAsync();

            var firstTopics = await _context.AddTopics
                .Where(x => x.MasterCourseId == id && x.Status == "Active")
                .GroupBy(x => x.SubCourseId)
                .Select(x => x
                    .OrderBy(y => y.Id)
                    .First())
                .ToListAsync();

            ViewBag.SubCourses = subCourses;
            ViewBag.FirstTopics = firstTopics;

            return View(course);
        }
    }
}