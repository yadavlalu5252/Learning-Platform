using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class SubscriptionController : Controller
    {
        private readonly AppDbContext db;

        public SubscriptionController(AppDbContext context)
        {
            db = context;
        }

        // GET: Subscription
        public IActionResult Index()
        {
                  ViewBag.MasterCourses = db.MasterCourse
                .Where(x => x.Status == "Active")
                .ToList();
                
     
            var subscriptions = db.Subscription
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .OrderByDescending(x => x.sid)
                .ToList();

            return View(subscriptions);
        }


    
        [HttpGet]
        public IActionResult GetSubCourses(int masterCourseId)
        {
            var data = db.SubCourse
                .Where(x =>
                    x.MasterCourseId == masterCourseId &&
                    x.Status == "Active")
                .Select(x => new
                {
                    id = x.Id,
                    name = x.SubCourseName
                })
                .ToList();

            return Json(data);
        }


    
        [HttpPost]
        public IActionResult Add(
            Subscription subscription,
            List<int> selectedSubCourses,
            IFormFile thumbnailFile)
        {
     
            var master = db.MasterCourse
                .FirstOrDefault(x =>
                    x.Id == subscription.MasterCourseId);

            if (master == null)
                return NotFound();


         
            if (selectedSubCourses == null ||
                selectedSubCourses.Count == 0)
            {
                TempData["Message"] =
                    "Please select at least one subcourse.";

                return RedirectToAction("Index");
            }


         
            if (thumbnailFile == null ||
                thumbnailFile.Length == 0)
            {
                TempData["Message"] =
                    "Please select a thumbnail.";

                return RedirectToAction("Index");
            }


     
            string folder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot/images");


       
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }


         
            string fileName =
                Guid.NewGuid().ToString()
                + Path.GetExtension(
                    thumbnailFile.FileName);


     
            string filePath =
                Path.Combine(folder, fileName);


            using (var stream = new FileStream(
                filePath,
                FileMode.Create))
            {
                thumbnailFile.CopyTo(stream);
            }


         
            string thumbnailPath =
                "/images/" + fileName;


            foreach (var subId in selectedSubCourses)
            {
                var sub = db.SubCourse
                    .FirstOrDefault(x => x.Id == subId);


                if (sub == null ||
                    sub.MasterCourseId !=
                    subscription.MasterCourseId)
                {
                    continue;
                }


                db.Subscription.Add(new Subscription
                {
                    stype = subscription.stype,

                    MasterCourseId =
                        subscription.MasterCourseId,

                    SubCourseId =
                        sub.Id,

                    amount =
                        subscription.amount,

                    Validity =
                        subscription.Validity,

                    status =
                        subscription.status,

                  
                    Thumbnail =
                        thumbnailPath,

                    CreatedBy =
                        "Admin",

                    CreatedAt =
                        DateTime.Now
                });
            }


            db.SaveChanges();


            TempData["Message"] =
                "Subscription added successfully.";

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = db.Subscription
                .FirstOrDefault(x => x.sid == id);


            if (item == null)
                return NotFound();


            return Json(new
            {
                sid = item.sid,

                stype = item.stype,

                masterCourseId =
                    item.MasterCourseId,

                subCourseId =
                    item.SubCourseId,

                amount =
                    item.amount,

                validity =
                    item.Validity,

                status =
                    item.status,

                thumbnail =
                    item.Thumbnail
            });
        }


        [HttpPost]
        public IActionResult Update(
      Subscription subscription,
      List<int> selectedSubCourses,
      IFormFile thumbnailFile)
        {
            var item = db.Subscription
                .FirstOrDefault(x => x.sid == subscription.sid);

            if (item == null)
                return NotFound();

            if (selectedSubCourses == null ||
                selectedSubCourses.Count == 0)
            {
                TempData["Message"] = "Please select a subcourse.";
                return RedirectToAction("Index");
            }

            int selectedSubCourseId = selectedSubCourses[0];

            var subCourse = db.SubCourse
                .FirstOrDefault(x => x.Id == selectedSubCourseId);

            if (subCourse == null)
            {
                TempData["Message"] = "Selected subcourse does not exist.";
                return RedirectToAction("Index");
            }

            if (subCourse.MasterCourseId != subscription.MasterCourseId)
            {
                TempData["Message"] = "Selected subcourse does not belong to this master course.";
                return RedirectToAction("Index");
            }

            item.stype = subscription.stype;
            item.MasterCourseId = subscription.MasterCourseId;
            item.SubCourseId = selectedSubCourseId;
            item.amount = subscription.amount;
            item.Validity = subscription.Validity;
            item.status = subscription.status;

            if (thumbnailFile != null && thumbnailFile.Length > 0)
            {
                string folder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/images");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName =
                    Guid.NewGuid().ToString()
                    + Path.GetExtension(thumbnailFile.FileName);

                string filePath =
                    Path.Combine(folder, fileName);

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    thumbnailFile.CopyTo(stream);
                }

                item.Thumbnail = "/images/" + fileName;
            }

            db.SaveChanges();

            TempData["Message"] = "Subscription updated successfully.";

            return RedirectToAction("Index");
        }
    }
}