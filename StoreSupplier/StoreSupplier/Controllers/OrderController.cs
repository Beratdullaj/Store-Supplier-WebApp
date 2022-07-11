using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreSupplier.Infrastructure;
using StoreSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreSupplier.Controllers
{
    public class OrderController : Controller
    {
        private readonly StoreSupplierContext context;
        public OrderController(StoreSupplierContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            return View(cartVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier).ToString();

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };

            foreach (var item in cartVM.CartItems)
            {
                var orderStore = new Order
                {
                    City = order.City,
                    Address = order.Address,
                    CardNumber = order.CardNumber,
                    CardValidDate = order.CardValidDate,
                    CVV = order.CVV,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UserId = userId
                };

                context.Add(orderStore);
                await context.SaveChangesAsync();
                var productId = item.ProductId;
                var product = context.Products
                              .Where(p => p.Id == productId)
                              .ToList();
                if (product[0].Stock >= item.Quantity)
                {
                    product[0].Stock = (product[0].Stock - item.Quantity);

                    context.Products.Update(product[0]);
                    await context.SaveChangesAsync();
                }
                
            }
            HttpContext.Session.Remove("Cart");
            TempData["Success"] = "Order successfully!";

            return Redirect("/Products/Index");
        }
    }
}
