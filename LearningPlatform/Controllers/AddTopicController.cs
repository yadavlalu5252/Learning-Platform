using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class AddTopicController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment environment;

        public AddTopicController(AppDbContext db, IWebHostEnvironment environment)
        {
            this.db = db;
            this.environment = environment;
        }

        // INDEX - TOPIC LIST
        [HttpGet]
        public IActionResult Index()
        {
            var topics = db.AddTopics
                .Include(t => t.MasterCourseData)
                .Include(t => t.SubCourseData)
                .OrderByDescending(t => t.Id)
                .ToList();
            return View(topics);
        }

        // ADD TOPIC - GET
        [HttpGet]
        public IActionResult AddTopic()
        {
            LoadDropdowns();
            return View();
        }

        // ADD TOPIC - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTopic(AddTopic model, IFormFile? ThumbnailFile)
        {
            ModelState.Remove("MasterCourseData");
            ModelState.Remove("SubCourseData");

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                var topics = db.AddTopics
                    .Include(t => t.MasterCourseData)
                    .Include(t => t.SubCourseData)
                    .OrderByDescending(t => t.Id)
                    .ToList();
                return View("Index", topics);
            }

            // UPLOAD THUMBNAIL
            string thumbnailName = "";

            if (ThumbnailFile != null && ThumbnailFile.Length > 0)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "uploads", "topics");
                Directory.CreateDirectory(uploadFolder);
                thumbnailName = Guid.NewGuid().ToString() + Path.GetExtension(ThumbnailFile.FileName);
                string filePath = Path.Combine(uploadFolder, thumbnailName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ThumbnailFile.CopyTo(stream);
                }
            }

            // CREATE TOPIC
            var topic = new AddTopic
            {
                MasterCourseId = model.MasterCourseId,
                SubCourseId = model.SubCourseId,
                TopicName = model.TopicName,
                VideoUrl = model.VideoUrl,
                Status = model.Status,
                Thumbnail = thumbnailName,
                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };

            db.AddTopics.Add(topic);
            db.SaveChanges();
            TempData["msg"] = "Topic added successfully!";
            return RedirectToAction("Index");
        }

        // EDIT - GET
        [HttpGet]
        public IActionResult EditTopic(int id)
        {
            var topic = db.AddTopics
                .FirstOrDefault(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            LoadDropdowns();
            return View(topic);
        }

        // EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTopic(int id, AddTopic model, IFormFile? ThumbnailFile)
        {
            ModelState.Remove("MasterCourseData");
            ModelState.Remove("SubCourseData");

            if (!ModelState.IsValid)
            {
                LoadDropdowns();
                return View(model);
            }

            // FIND EXISTING TOPIC
            var topic = db.AddTopics
                .FirstOrDefault(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            // UPDATE TOPIC DETAILS
            topic.MasterCourseId = model.MasterCourseId;
            topic.SubCourseId = model.SubCourseId;
            topic.TopicName = model.TopicName;
            topic.VideoUrl = model.VideoUrl;
            topic.Status = model.Status;

            // CHANGE THUMBNAIL
            if (ThumbnailFile != null && ThumbnailFile.Length > 0)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "uploads", "topics");
                Directory.CreateDirectory(uploadFolder);

                // Delete old image
                if (!string.IsNullOrEmpty(topic.Thumbnail))
                {
                    string oldFile = Path.Combine(uploadFolder, topic.Thumbnail);

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }

                // Save new image
                string newThumbnailName = Guid.NewGuid().ToString() + Path.GetExtension(ThumbnailFile.FileName);
                string newFilePath = Path.Combine(uploadFolder, newThumbnailName);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    ThumbnailFile.CopyTo(stream);
                }

                topic.Thumbnail = newThumbnailName;
            }

            db.SaveChanges();
            TempData["msg"] = "Topic updated successfully!";
            return RedirectToAction("Index");
        }

        // DELETE TOPIC
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTopic(int id)
        {
            var topic = db.AddTopics
                .FirstOrDefault(t => t.Id == id);

            if (topic == null)
            {
                return NotFound();
            }

            // Delete thumbnail
            if (!string.IsNullOrEmpty(topic.Thumbnail))
            {
                string filePath = Path.Combine(environment.WebRootPath, "uploads", "topics", topic.Thumbnail);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            // Delete topic
            db.AddTopics.Remove(topic);
            db.SaveChanges();
            TempData["msg"] = "Topic deleted successfully!";
            return RedirectToAction("Index");
        }

        // DROPDOWNS
        private void LoadDropdowns()
        {
            ViewBag.MasterCourses = db.MasterCourses
                .Where(m => m.Status == "Active")
                .OrderBy(m => m.MasterCourseName)
                .ToList();

            ViewBag.SubCourses = db.SubCourses
                .Where(s => s.Status == "Active")
                .OrderBy(s => s.SubCourseName)
                .ToList();
        }
    }
}