using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace LearningPlatform.Controllers
{
    public class AllCoursesController(AppDbContext _context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var masterCourses = await _context.MasterCourse
                .Where(x => x.Status == "Active")
                .ToListAsync();

            var masterCourseAmounts = await _context.SubCourse
    .Where(x => x.Status == "Active")
    .GroupBy(x => x.MasterCourseId)
    .ToDictionaryAsync(
        x => x.Key,
        x => x.Sum(y => y.Amount)
    );

            var subCourses = await _context.SubCourse
                .Where(x => x.Status == "Active")
                .Include(x => x.MasterCourse)
                .ToListAsync();

            var subscriptions = await _context.Subscriptions
                .Where(x => x.status == "Active")
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .ToListAsync();

            ViewBag.MasterCourses = masterCourses;
            ViewBag.MasterCourseAmounts = masterCourseAmounts;
            ViewBag.SubCourses = subCourses;
            ViewBag.Subscriptions = subscriptions;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(
    string type,
    int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cart = new Cart
            {
                UserId = userId.Value,
                CreatedAt = DateTime.Now
            };

            if (type == "master")
            {
                var masterCourse = await _context.MasterCourse
                    .FirstOrDefaultAsync(x => x.Id == id && x.Status == "Active");

                if (masterCourse == null)
                {
                    return NotFound();
                }

                var amount = await _context.SubCourse
                    .Where(x => x.MasterCourseId == id && x.Status == "Active")
                    .SumAsync(x => x.Amount);

                cart.MasterCourseId = id;
                cart.Amount = amount;
            }
            else if (type == "sub")
            {
                var subCourse = await _context.SubCourse
                    .FirstOrDefaultAsync(x => x.Id == id && x.Status == "Active");

                if (subCourse == null)
                {
                    return NotFound();
                }

                cart.SubCourseId = id;
                cart.Amount = subCourse.Amount;
            }
            else if (type == "subscription")
            {
                var subscription = await _context.Subscriptions
                    .FirstOrDefaultAsync(x => x.sid == id && x.status == "Active");

                if (subscription == null)
                {
                    return NotFound();
                }

                cart.SubscriptionId = id;
                cart.Amount = subscription.amount;
            }
            else
            {
                return BadRequest();
            }

            var alreadyExists = await _context.Carts
                .AnyAsync(x =>
                    x.UserId == userId &&
                    x.MasterCourseId == cart.MasterCourseId &&
                    x.SubCourseId == cart.SubCourseId &&
                    x.SubscriptionId == cart.SubscriptionId);

            if (alreadyExists)
            {
                TempData["ErrorMessage"] = "This item is already in your cart.";
                return RedirectToAction("Index");
            }

            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Added to cart successfully.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Cart()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItems = await _context.Carts
                .Where(x => x.UserId == userId)
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .Include(x => x.SubscriptionData)
                .ToListAsync();

            return View(cartItems);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItem = await _context.Carts
                .FirstOrDefaultAsync(x =>
                    x.CartId == id &&
                    x.UserId == userId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cartItem);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Item removed from cart.";

            return RedirectToAction("Cart");
        }
        public async Task<IActionResult> Checkout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItems = await _context.Carts
                .Where(x => x.UserId == userId)
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .Include(x => x.SubscriptionData)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";

                return RedirectToAction("Cart");
            }

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPurchase()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItems = await _context.Carts
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (cartItems.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";

                return RedirectToAction("Cart");
            }

            decimal amount = cartItems.Sum(x => x.Amount);

            string keyId = "rzp_test_TXhTMZYQQQHYiW";
            string keySecret = "9L1MLzkTIWyorzbxbrrA8loZ";

            RazorpayClient razorpayClient =
                new RazorpayClient(keyId, keySecret);

            Dictionary<string, object> options =
                new Dictionary<string, object>();

            options.Add("amount", (int)(amount * 100));
            options.Add("currency", "INR");
            options.Add("receipt", "receipt_" + userId + "_" + DateTime.Now.Ticks);
            options.Add("payment_capture", 1);

            Razorpay.Api.Order order =
                razorpayClient.Order.Create(options);

            string orderId = order["id"].ToString();

            ViewBag.KeyId = keyId;
            ViewBag.OrderId = orderId;
            ViewBag.Amount = (int)(amount * 100);

            return View("Payment");
        }

        [HttpPost]
        public async Task<IActionResult> PaymentSuccess(
    string razorpay_payment_id,
    string razorpay_order_id,
    string razorpay_signature)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            string keyId = "rzp_test_TXhTMZYQQQHYiW";
            string keySecret = "9L1MLzkTIWyorzbxbrrA8loZ";

            try
            {
                Dictionary<string, string> attributes =
                    new Dictionary<string, string>();

                attributes.Add("razorpay_order_id", razorpay_order_id);
                attributes.Add("razorpay_payment_id", razorpay_payment_id);
                attributes.Add("razorpay_signature", razorpay_signature);

                Utils.verifyPaymentSignature(attributes);

                var cartItems = await _context.Carts
                    .Where(x => x.UserId == userId)
                    .ToListAsync();

                if (cartItems.Count == 0)
                {
                    TempData["ErrorMessage"] =
                        "Your cart is empty.";

                    return RedirectToAction("Cart");
                }

                foreach (var item in cartItems)
                {
                    var purchase = new Purchase
                    {
                        UserId = userId.Value,
                        MasterCourseId = item.MasterCourseId,
                        SubCourseId = item.SubCourseId,
                        SubscriptionId = item.SubscriptionId,
                        Amount = item.Amount,
                        PaymentStatus = "Success",
                        PurchaseDate = DateTime.Now,
                        Status = "Active"
                    };

                    _context.Purchases.Add(purchase);
                }

                _context.Carts.RemoveRange(cartItems);

                await _context.SaveChangesAsync();

                TempData.Remove("ErrorMessage");

                TempData["SuccessMessage"] =
                    "Payment successful. Purchase completed.";

                return RedirectToAction("Index", "MyCourses");
            }
            catch
            {
                TempData["ErrorMessage"] =
                    "Payment verification failed.";

                return RedirectToAction("Cart");
            }
        }
    }
}