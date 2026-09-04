using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace LearningPlatform.Controllers
{
    public class MyCoursesController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var purchases = await _context.Purchases
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .Include(x => x.SubscriptionData)
                .Where(x =>
                    x.UserId == userId &&
                    x.PaymentStatus == "Success")
                .ToListAsync();

            ViewBag.ExpiryDates = new Dictionary<int, DateTime>();
            ViewBag.DaysLeft = new Dictionary<int, int>();

            foreach (var purchase in purchases)
            {
                int validityDays = 30;

                if (purchase.SubscriptionData != null)
                {
                    validityDays = purchase.SubscriptionData.Validity;
                }

                var expiryDate = purchase.PurchaseDate
                    .AddDays(validityDays);

                var daysLeft = Math.Max(
                    0,
                    (expiryDate.Date - DateTime.Now.Date).Days);

                ViewBag.ExpiryDates[purchase.PurchaseId] = expiryDate;
                ViewBag.DaysLeft[purchase.PurchaseId] = daysLeft;
            }

            return View(purchases);
        }

        public async Task<IActionResult> Learn(int id, int? topicId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var subCourse = await _context.SubCourse
                .FirstOrDefaultAsync(x => x.Id == id);

            if (subCourse == null)
            {
                return NotFound();
            }

            var purchases = await _context.Purchases
                .Include(x => x.SubscriptionData)
                .Where(x =>
                    x.UserId == userId &&
                    x.PaymentStatus == "Success" &&
                    (
                        x.SubCourseId == id ||
                        x.MasterCourseId == subCourse.MasterCourseId ||
                        x.SubscriptionData.SubCourseId == id
                    ))
                .ToListAsync();

            var validPurchase = purchases
                .FirstOrDefault(x => IsPurchaseValid(x));

            if (validPurchase == null)
            {
                TempData["ErrorMessage"] = "Your course access has expired.";
                return RedirectToAction("Index");
            }

            var validityDays = 30;

            if (validPurchase.SubscriptionData != null)
            {
                validityDays = validPurchase.SubscriptionData.Validity;
            }

            var expiryDate = validPurchase.PurchaseDate.AddDays(validityDays);

            ViewBag.ExpiryDate = expiryDate;
            ViewBag.DaysLeft = Math.Max(
                0,
                (expiryDate.Date - DateTime.Now.Date).Days);

            var topics = await _context.AddTopics
                .Where(x => x.SubCourseId == id && x.Status == "Active")
                .OrderBy(x => x.Id)
                .ToListAsync();

            var progress = await _context.CourseProgress
                .Where(x =>
                    x.UserId == userId &&
                    x.MasterCourseId == subCourse.MasterCourseId)
                .ToListAsync();

            var unlockedTopicId = topics.FirstOrDefault()?.Id;

            foreach (var topic in topics)
            {
                var topicProgress = progress
                    .FirstOrDefault(x => x.TopicId == topic.Id);

                if (topicProgress == null || !topicProgress.McqCompleted)
                {
                    unlockedTopicId = topic.Id;
                    break;
                }

                unlockedTopicId = topics.LastOrDefault()?.Id;
            }

            var currentTopic = topics
                .FirstOrDefault(x => x.Id == unlockedTopicId);

            if (topicId != null)
            {
                var selectedTopic = topics
                    .FirstOrDefault(x => x.Id == topicId);

                if (selectedTopic != null)
                {
                    var selectedIndex = topics.IndexOf(selectedTopic);
                    var unlockedIndex = topics.FindIndex(
                        x => x.Id == unlockedTopicId);

                    if (selectedIndex <= unlockedIndex)
                    {
                        currentTopic = selectedTopic;
                    }
                }
            }

            if (currentTopic == null)
            {
                return NotFound();
            }

            var material = await _context.AddMaterials
                .FirstOrDefaultAsync(x =>
                    x.TopicId == currentTopic.Id &&
                    x.Status == "Active");

            ViewBag.Topics = topics;
            ViewBag.Progress = progress;
            ViewBag.CurrentTopic = currentTopic;
            ViewBag.Material = material;
            ViewBag.UnlockedTopicId = unlockedTopicId;

            return View(subCourse);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitMcq(
     int id,
     int topicId,
     string mcq1,
     string mcq2,
     string mcq3)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var subCourse = await _context.SubCourse
                .FirstOrDefaultAsync(x => x.Id == id);

            if (subCourse == null)
            {
                return NotFound();
            }

            var topic = await _context.AddTopics
                .FirstOrDefaultAsync(x => x.Id == topicId && x.SubCourseId == id);

            if (topic == null)
            {
                return NotFound();
            }

            var material = await _context.AddMaterials
                .FirstOrDefaultAsync(x => x.TopicId == topicId && x.Status == "Active");

            if (material == null)
            {
                return NotFound();
            }

            if (mcq1 == material.MCQ1Answer &&
                mcq2 == material.MCQ2Answer &&
                mcq3 == material.MCQ3Answer)
            {
                var progress = await _context.CourseProgress
                    .FirstOrDefaultAsync(x =>
                        x.UserId == userId &&
                        x.MasterCourseId == subCourse.MasterCourseId &&
                        x.TopicId == topicId);

                if (progress == null)
                {
                    progress = new CourseProgress
                    {
                        UserId = userId.Value,
                        MasterCourseId = subCourse.MasterCourseId,
                        TopicId = topicId,
                        IsCompleted = true,
                        McqCompleted = true,
                        CompletedAt = DateTime.Now
                    };

                    _context.CourseProgress.Add(progress);
                }
                else
                {
                    progress.IsCompleted = true;
                    progress.McqCompleted = true;
                    progress.CompletedAt = DateTime.Now;
                }

                await _context.SaveChangesAsync();

                var topicIds = await _context.AddTopics
                    .Where(x => x.SubCourseId == id && x.Status == "Active")
                    .Select(x => x.Id)
                    .ToListAsync();

                var completedTopicIds = await _context.CourseProgress
                    .Where(x =>
                        x.UserId == userId &&
                        x.MasterCourseId == subCourse.MasterCourseId &&
                        x.McqCompleted)
                    .Select(x => x.TopicId)
                    .ToListAsync();

                bool courseCompleted = topicIds.Count > 0 &&
                                       topicIds.All(x => completedTopicIds.Contains(x));

                if (courseCompleted)
                {
                    TempData["CourseCompleted"] = "true";
                    TempData["SuccessMessage"] =
                        "Congratulations! You have completed the course.";
                }
                else
                {
                    TempData["SuccessMessage"] =
                        "All answers are correct. Next topic is unlocked.";
                }
            }
            else
            {
                TempData["ErrorMessage"] =
                    "Some answers are incorrect. Please try again.";
            }

            return RedirectToAction("Learn", new
            {
                id = id,
                topicId = topicId
            });
        }
        public async Task<IActionResult> DownloadAssignment(int id)
        {
            var material = await _context.AddMaterials
                .FirstOrDefaultAsync(x => x.MaterialId == id);

            if (material == null || string.IsNullOrEmpty(material.AssignmentAttachment))
            {
                return NotFound();
            }

            var filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                material.AssignmentAttachment);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(
                fileBytes,
                "application/octet-stream",
                material.AssignmentAttachment);
        }
        

        [HttpPost]
        public async Task<IActionResult> UploadSolution(int id,int topicId,int materialId,IFormFile solutionFile)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (solutionFile == null || solutionFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please select a file.";

                return RedirectToAction("Learn", new
                {
                    id = id,
                    topicId = topicId
                });
            }

            var extension = Path.GetExtension(solutionFile.FileName).ToLower();

            if (extension != ".pdf" &&
                extension != ".doc" &&
                extension != ".docx")
            {
                TempData["ErrorMessage"] = "Only PDF and Word files are allowed.";

                return RedirectToAction("Learn", new
                {
                    id = id,
                    topicId = topicId
                });
            }

            var fileName = Guid.NewGuid().ToString() + extension;

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await solutionFile.CopyToAsync(stream);
            }

            var submission = new AssignmentSubmission
            {
                UserId = userId.Value,
                MaterialId = materialId,
                SolutionFile = fileName,
                SubmittedAt = DateTime.Now,
                Status = "Submitted"
            };

            _context.AssignmentSubmissions.Add(submission);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Solution uploaded successfully.";

            return RedirectToAction("Learn", new
            {
                id = id,
                topicId = topicId
            });
        }

        public async Task<IActionResult> GenerateCertificate(int subCourseId)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var subCourse = await _context.SubCourse
    .FirstOrDefaultAsync(x => x.Id == subCourseId);

            if (user == null || subCourse == null)
            {
                return NotFound();
            }

            var course = await _context.MasterCourse
                .FirstOrDefaultAsync(x => x.Id == subCourse.MasterCourseId);

            if (user == null || course == null)
            {
                return NotFound();
            }

            var topics = await _context.AddTopics
    .Where(x => x.SubCourseId == subCourseId && x.Status == "Active")
    .ToListAsync();

            var completedTopics = await _context.CourseProgress
                .Where(x =>
                    x.UserId == userId &&
                    x.MasterCourseId == subCourse.MasterCourseId &&
                    x.McqCompleted &&
                    topics.Select(t => t.Id).Contains(x.TopicId))
                .CountAsync();

            if (completedTopics != topics.Count)
            {
                return BadRequest("Course is not completed yet.");
            }
            if (completedTopics != topics.Count)
            {
                return BadRequest("Course is not completed yet.");
            }

            var certificateNumber = "CERT-" +
                         subCourseId.ToString("000") +
                         "-" +
                         userId.Value.ToString("000") +
                         "-" +
                         DateTime.Now.ToString("yyyyMMdd");

            var existingCertificate = await _context.Certificates
                .FirstOrDefaultAsync(x =>
    x.UserId == userId &&
    x.MasterCourseId == subCourse.MasterCourseId);

            if (existingCertificate == null)
            {
                existingCertificate = new Certificate
                {
                    UserId = userId.Value,
                    MasterCourseId = subCourse.MasterCourseId,
                    CertificateNumber = certificateNumber,
                    IssuedAt = DateTime.Now,
                    CertificateFile = certificateNumber + ".pdf"
                };

                _context.Certificates.Add(existingCertificate);

                await _context.SaveChangesAsync();
            }
            else
            {
                certificateNumber = existingCertificate.CertificateNumber;
            }

            var issuedDate = DateTime.Now.ToString("dd-MM-yyyy");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);

                    page.PageColor(Colors.White);

                    page.Content()
                        .Border(5)
                        .BorderColor(Colors.Blue.Medium)
                        .Padding(40)
                        .Column(column =>
                        {
                            column.Spacing(20);

                            column.Item()
                                .AlignCenter()
                                .Text("LEARNING PLATFORM")
                                .FontSize(18)
                                .Bold()
                                .FontColor(Colors.Blue.Medium);

                            column.Item()
                                .AlignCenter()
                                .Text("CERTIFICATE OF COMPLETION")
                                .FontSize(32)
                                .Bold();

                            column.Item()
                                .AlignCenter()
                                .Text("This certificate is proudly presented to")
                                .FontSize(16);

                            column.Item()
                                .AlignCenter()
                                .Text(user.Name)
                                .FontSize(30)
                                .Bold()
                                .FontColor(Colors.Blue.Medium);

                            column.Item()
                                .AlignCenter()
                                .Text("for successfully completing the course")
                                .FontSize(16);

                            column.Item()
                                .AlignCenter()
                                .Text(course.MasterCourseName)
                                .FontSize(25)
                                .Bold();

                            column.Item()
                                .PaddingTop(20)
                                .Row(row =>
                                {
                                    row.RelativeItem()
                                        .AlignCenter()
                                        .Column(x =>
                                        {
                                            x.Item()
                                                .Text("Certificate Number")
                                                .FontSize(12)
                                                .Bold();

                                            x.Item()
                                                .Text(certificateNumber)
                                                .FontSize(12);
                                        });

                                    row.RelativeItem()
                                        .AlignCenter()
                                        .Column(x =>
                                        {
                                            x.Item()
                                                .Text("Issue Date")
                                                .FontSize(12)
                                                .Bold();

                                            x.Item()
                                                .Text(issuedDate)
                                                .FontSize(12);
                                        });
                                });

                            column.Item()
                                .PaddingTop(25)
                                .AlignCenter()
                                .Text("Congratulations on successfully completing the course!")
                                .FontSize(14);
                        });
                });
            });

            var pdfBytes = document.GeneratePdf();

            return File(
                pdfBytes,
                "application/pdf",
                certificateNumber + ".pdf");
        }

        private bool IsPurchaseValid(Purchase purchase)
        {
            int validityDays = 30;

            if (purchase.SubscriptionData != null)
            {
                validityDays = purchase.SubscriptionData.Validity;
            }

            DateTime expiryDate = purchase.PurchaseDate.AddDays(validityDays);

            return DateTime.Now <= expiryDate;
        }
    }
}