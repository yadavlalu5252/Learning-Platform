using LearningPlatform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class UserDashboardController : Controller
    {
        private readonly AppDbContext _context;

        public UserDashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            var purchases = await _context.Purchases
                .Where(x =>
                    x.UserId == userId &&
                    x.PaymentStatus == "Success" &&
                    x.Status == "Active")
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .Include(x => x.SubscriptionData)
                .ToListAsync();

            var expiryDates = new Dictionary<int, DateTime>();
            var daysLeft = new Dictionary<int, int>();
            var totalTopics = new Dictionary<int, int>();
            var completedTopics = new Dictionary<int, int>();
            var progressPercent = new Dictionary<int, int>();

            foreach (var purchase in purchases)
            {
                int masterCourseId = 0;

                if (purchase.MasterCourseId != null)
                {
                    masterCourseId = purchase.MasterCourseId.Value;
                }
                else if (purchase.SubCourseId != null)
                {
                    masterCourseId = purchase.SubCourseData.MasterCourseId;
                }
                else if (purchase.SubscriptionData != null)
                {
                    masterCourseId = purchase.SubscriptionData.MasterCourseId;
                }

                var topics = await _context.AddTopics
                    .Where(x =>
                        x.MasterCourseId == masterCourseId &&
                        x.Status == "Active")
                    .Select(x => x.Id)
                    .ToListAsync();

                var completed = await _context.CourseProgress
                    .Where(x =>
                        x.UserId == userId &&
                        x.MasterCourseId == masterCourseId &&
                        x.McqCompleted &&
                        topics.Contains(x.TopicId))
                    .CountAsync();

                totalTopics[purchase.PurchaseId] = topics.Count;
                completedTopics[purchase.PurchaseId] = completed;

                if (topics.Count > 0)
                {
                    progressPercent[purchase.PurchaseId] =
                        (completed * 100) / topics.Count;
                }
                else
                {
                    progressPercent[purchase.PurchaseId] = 0;
                }

                int validityDays = 30;

                if (purchase.SubscriptionData != null)
                {
                    validityDays = purchase.SubscriptionData.Validity;
                }

                var expiryDate = purchase.PurchaseDate
                    .AddDays(validityDays);

                expiryDates[purchase.PurchaseId] = expiryDate;

                daysLeft[purchase.PurchaseId] = Math.Max(
                    0,
                    (expiryDate.Date - DateTime.Now.Date).Days);
            }

            ViewBag.User = user;
            ViewBag.ExpiryDates = expiryDates;
            ViewBag.DaysLeft = daysLeft;
            ViewBag.TotalTopics = totalTopics;
            ViewBag.CompletedTopics = completedTopics;
            ViewBag.ProgressPercent = progressPercent;

            return View(purchases);
        }
    }
}