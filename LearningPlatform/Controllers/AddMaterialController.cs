using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class AddMaterialController : Controller
    {
        private readonly AppDbContext db;
        private readonly IWebHostEnvironment environment;

        public AddMaterialController(
            AppDbContext db,
            IWebHostEnvironment environment)
        {
            this.db = db;
            this.environment = environment;
        }

       
        // INDEX
      

        [HttpGet]
        public IActionResult Index()
        {
            LoadDropdowns();

            var materials = db.AddMaterials
                .Include(m => m.MasterCourseData)
                .Include(m => m.SubCourseData)
                .Include(m => m.TopicData)
                .OrderByDescending(m => m.MaterialId)
                .ToList();

            return View(materials);
        }

        
        // OPEN EDIT
        

        [HttpGet]
        public IActionResult EditMaterial(int id)
        {
            var material = db.AddMaterials
                .Include(m => m.MasterCourseData)
                .Include(m => m.SubCourseData)
                .Include(m => m.TopicData)
                .FirstOrDefault(m => m.MaterialId == id);

            if (material == null)
            {
                return NotFound();
            }

            LoadDropdowns();

            var materials = db.AddMaterials
                .Include(m => m.MasterCourseData)
                .Include(m => m.SubCourseData)
                .Include(m => m.TopicData)
                .OrderByDescending(m => m.MaterialId)
                .ToList();

            ViewBag.EditMaterial = material;

            return View("Index", materials);
        }

        
        // ADD MATERIAL
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMaterial(
            AddMaterial model,
            IFormFile? AssignmentFile)
        {
            ModelState.Remove("MasterCourseData");
            ModelState.Remove("SubCourseData");
            ModelState.Remove("TopicData");

            if (!ModelState.IsValid)
            {
                LoadDropdowns();

                var materials = db.AddMaterials
                    .Include(m => m.MasterCourseData)
                    .Include(m => m.SubCourseData)
                    .Include(m => m.TopicData)
                    .OrderByDescending(m => m.MaterialId)
                    .ToList();

                ViewBag.OpenAddModal = true;

                return View("Index", materials);
            }

            string assignmentName = "";

            if (AssignmentFile != null && AssignmentFile.Length > 0)
            {
                string uploadFolder = Path.Combine(
                    environment.WebRootPath,
                    "uploads",
                    "assignments"
                );

                Directory.CreateDirectory(uploadFolder);

                assignmentName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(AssignmentFile.FileName);

                string filePath = Path.Combine(
                    uploadFolder,
                    assignmentName
                );

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    AssignmentFile.CopyTo(stream);
                }
            }

            var material = new AddMaterial
            {
                MasterCourseId = model.MasterCourseId,
                SubCourseId = model.SubCourseId,
                TopicId = model.TopicId,

                MaterialType = model.MaterialType,

                AssignmentAttachment = assignmentName,

                MCQ1Question = model.MCQ1Question,
                MCQ1OptionA = model.MCQ1OptionA,
                MCQ1OptionB = model.MCQ1OptionB,
                MCQ1OptionC = model.MCQ1OptionC,
                MCQ1OptionD = model.MCQ1OptionD,
                MCQ1Answer = model.MCQ1Answer,

                MCQ2Question = model.MCQ2Question,
                MCQ2OptionA = model.MCQ2OptionA,
                MCQ2OptionB = model.MCQ2OptionB,
                MCQ2OptionC = model.MCQ2OptionC,
                MCQ2OptionD = model.MCQ2OptionD,
                MCQ2Answer = model.MCQ2Answer,

                MCQ3Question = model.MCQ3Question,
                MCQ3OptionA = model.MCQ3OptionA,
                MCQ3OptionB = model.MCQ3OptionB,
                MCQ3OptionC = model.MCQ3OptionC,
                MCQ3OptionD = model.MCQ3OptionD,
                MCQ3Answer = model.MCQ3Answer,

                Status = string.IsNullOrEmpty(model.Status)
                    ? "Active"
                    : model.Status,

                CreatedAt = DateTime.Now,
                CreatedBy = "Admin"
            };

            db.AddMaterials.Add(material);
            db.SaveChanges();

            TempData["msg"] = "Material added successfully!";

            return RedirectToAction("Index");
        }

        
        // EDIT MATERIAL
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMaterial(
            int id,
            AddMaterial model,
            IFormFile? AssignmentFile)
        {
            ModelState.Remove("MasterCourseData");
            ModelState.Remove("SubCourseData");
            ModelState.Remove("TopicData");

            var material = db.AddMaterials
                .FirstOrDefault(m => m.MaterialId == id);

            if (material == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                LoadDropdowns();

                var materials = db.AddMaterials
                    .Include(m => m.MasterCourseData)
                    .Include(m => m.SubCourseData)
                    .Include(m => m.TopicData)
                    .OrderByDescending(m => m.MaterialId)
                    .ToList();

                var existing = db.AddMaterials
                    .AsNoTracking()
                    .FirstOrDefault(m => m.MaterialId == id);

                if (existing != null)
                {
                    model.AssignmentAttachment =
                        existing.AssignmentAttachment;
                }

                ViewBag.EditMaterial = model;

                return View("Index", materials);
            }

            // Course details
            material.MasterCourseId = model.MasterCourseId;
            material.SubCourseId = model.SubCourseId;
            material.TopicId = model.TopicId;

            material.MaterialType = model.MaterialType;

            // MCQ 1
            material.MCQ1Question = model.MCQ1Question;
            material.MCQ1OptionA = model.MCQ1OptionA;
            material.MCQ1OptionB = model.MCQ1OptionB;
            material.MCQ1OptionC = model.MCQ1OptionC;
            material.MCQ1OptionD = model.MCQ1OptionD;
            material.MCQ1Answer = model.MCQ1Answer;

            // MCQ 2
            material.MCQ2Question = model.MCQ2Question;
            material.MCQ2OptionA = model.MCQ2OptionA;
            material.MCQ2OptionB = model.MCQ2OptionB;
            material.MCQ2OptionC = model.MCQ2OptionC;
            material.MCQ2OptionD = model.MCQ2OptionD;
            material.MCQ2Answer = model.MCQ2Answer;

            // MCQ 3
            material.MCQ3Question = model.MCQ3Question;
            material.MCQ3OptionA = model.MCQ3OptionA;
            material.MCQ3OptionB = model.MCQ3OptionB;
            material.MCQ3OptionC = model.MCQ3OptionC;
            material.MCQ3OptionD = model.MCQ3OptionD;
            material.MCQ3Answer = model.MCQ3Answer;

            // Status
            material.Status =
                string.IsNullOrEmpty(model.Status)
                    ? "Active"
                    : model.Status;

            
            // REPLACE ASSIGNMENT
            

            if (AssignmentFile != null &&
                AssignmentFile.Length > 0)
            {
                string uploadFolder = Path.Combine(
                    environment.WebRootPath,
                    "uploads",
                    "assignments"
                );

                Directory.CreateDirectory(uploadFolder);

                // Delete old file
                if (!string.IsNullOrEmpty(
                    material.AssignmentAttachment))
                {
                    string oldFile = Path.Combine(
                        uploadFolder,
                        material.AssignmentAttachment
                    );

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                }

                // Save new file
                string newAssignmentName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(
                        AssignmentFile.FileName);

                string newFilePath = Path.Combine(
                    uploadFolder,
                    newAssignmentName
                );

                using (var stream = new FileStream(
                    newFilePath,
                    FileMode.Create))
                {
                    AssignmentFile.CopyTo(stream);
                }

                material.AssignmentAttachment =
                    newAssignmentName;
            }

            db.SaveChanges();

            TempData["msg"] =
                "Material updated successfully!";

            return RedirectToAction("Index");
        }

        
        // DELETE
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMaterial(int id)
        {
            var material = db.AddMaterials
                .FirstOrDefault(m => m.MaterialId == id);

            if (material == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(
                material.AssignmentAttachment))
            {
                string filePath = Path.Combine(
                    environment.WebRootPath,
                    "uploads",
                    "assignments",
                    material.AssignmentAttachment
                );

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            db.AddMaterials.Remove(material);

            db.SaveChanges();

            TempData["msg"] =
                "Material deleted successfully!";

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

            ViewBag.Topics = db.AddTopics
                .Where(t => t.Status == "Active")
                .OrderBy(t => t.TopicName)
                .ToList();
        }
    }
}