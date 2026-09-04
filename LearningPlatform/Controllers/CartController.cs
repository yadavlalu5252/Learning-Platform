using LearningPlatform.Data;
using LearningPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace LearningPlatform.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext db;
        private readonly IConfiguration configuration;

        public CartController(
            AppDbContext context,
            IConfiguration config)
        {
            db = context;
            configuration = config;
        }

        public IActionResult Index(int userId)
        {
            var cart = db.Cart
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .Include(x => x.SubscriptionData)
                .Where(x => x.UserId == 1)
                .ToList();

            return View(cart);
        }

        [HttpPost]
        public IActionResult Add(int subscriptionId)
        {
            var subscription = db.Subscription
                .Include(x => x.MasterCourseData)
                .Include(x => x.SubCourseData)
                .FirstOrDefault(x => x.sid == subscriptionId);

            if (subscription == null)
            {
                TempData["Message"] = "Subscription not found.";
                return RedirectToAction("Index");
            }

            var existingCart = db.Cart
                .FirstOrDefault(x =>
                    x.UserId == 1 &&
                    x.SubscriptionId == subscriptionId);

            if (existingCart != null)
            {
                TempData["Message"] =
                    "This subscription is already in your cart.";

                return RedirectToAction("Index");
            }

            Cart cart = new Cart
            {
                UserId = 1,

                MasterCourseId =
                    subscription.MasterCourseId,

                SubCourseId =
                    subscription.SubCourseId,

                SubscriptionId =
                    subscription.sid,

                Amount =
                    subscription.amount,

                CreatedAt =
                    DateTime.Now
            };

            db.Cart.Add(cart);

            db.SaveChanges();

            TempData["Message"] =
                "Subscription added to cart.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int cartId)
        {
            var cart = db.Cart
                .FirstOrDefault(x =>
                    x.CartId == cartId &&
                    x.UserId == 1);

            if (cart == null)
            {
                TempData["Message"] =
                    "Cart item not found.";

                return RedirectToAction("Index");
            }

            db.Cart.Remove(cart);

            db.SaveChanges();

            TempData["Message"] =
                "Item removed from cart.";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Payment(int cartId)
        {
            var cart = db.Cart
                .FirstOrDefault(x =>
                    x.CartId == cartId &&
                    x.UserId == 1);

            if (cart == null)
            {
                TempData["Message"] = "Cart item not found.";
                return RedirectToAction("Index");
            }

            string keyId = configuration["Razorpay:KeyId"] ?? "";
            string keySecret = configuration["Razorpay:KeySecret"] ?? "";

            RazorpayClient razorpayClient =
                new RazorpayClient(
                    keyId,
                    keySecret);

            Dictionary<string, object> options =
                new Dictionary<string, object>();

            options.Add(
                "amount",
                (int)(cart.Amount * 100));

            options.Add(
                "currency",
                "INR");

            options.Add(
                "receipt",
                "cart_" + cart.CartId);

            options.Add(
                "payment_capture",
                1);

            Razorpay.Api.Order order =
                razorpayClient.Order.Create(options);

            string orderId =
                order["id"].ToString();

            ViewBag.KeyId = keyId;
            ViewBag.OrderId = orderId;
            ViewBag.Amount = (int)(cart.Amount * 100);
            ViewBag.CartId = cart.CartId;

            return View("Payment", cart);
        }

        [HttpPost]
        public IActionResult PaymentSuccess(
            int cartId,
            string paymentId)
        {
            var cart = db.Cart
                .FirstOrDefault(x =>
                    x.CartId == cartId &&
                    x.UserId == 1);

            if (cart == null)
            {
                return NotFound();
            }

            TempData["Message"] =
                "Payment successful. Payment ID: "
                + paymentId;

            db.Cart.Remove(cart);

            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}