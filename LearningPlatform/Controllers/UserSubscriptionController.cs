using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class UserSubscriptionController : Controller
    {
       
            private readonly AppDbContext db;

            public UserSubscriptionController(AppDbContext context)
            {
                db = context;
            }

            public IActionResult Index()
            {
                var data = db.Subscription
                    .Where(x => x.status == "Active")
                    .Include(x => x.MasterCourseData)
                    .Include(x => x.SubCourseData)
                    .OrderByDescending(x => x.sid)
                    .ToList();

                return View(data);
            }

            public IActionResult Details(int id)
            {
                var item = db.Subscription
                    .Include(x => x.MasterCourseData)
                    .Include(x => x.SubCourseData)
                    .FirstOrDefault(x => x.sid == id);

                if (item == null)
                    return NotFound();

                return View(item);
            }
        }
    }
