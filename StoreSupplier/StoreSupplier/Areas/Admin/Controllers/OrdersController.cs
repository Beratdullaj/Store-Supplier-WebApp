using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreSupplier.Infrastructure;
using StoreSupplier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreSupplier.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private readonly StoreSupplierContext context;
        private readonly UserManager<AppUser> userManager;

        public OrdersController(StoreSupplierContext context, UserManager<AppUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        [Area("Admin")]
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var orders = context.Orders.OrderByDescending(x => x.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            List<Product> parts = new List<Product>();

            foreach (var item in orders)
            {
                var user = await userManager.FindByIdAsync(item.UserId);
                item.UserId = user.Name;

                var product = context.Products
                               .Where(p => p.Id == item.ProductId)
                               .ToList();
                item.Product = product[0];

                parts.Add(new Product() { Name = product[0].Name, Stock = item.Quantity });
            }
            var grouped = parts.GroupBy(i => i.Name).Select(i => new { ItemId = i.Key, Total = i.Sum(item => item.Stock) });

            var test = grouped.ToList();
            List<MostSoldProduct> mostSold = new List<MostSoldProduct>();
            
            foreach (var item in test)
            {
                mostSold.Add(new MostSoldProduct() { ItemId = item.ItemId, Total = item.Total.ToString()});
            }
            var mostSoldProducts = JsonConvert.SerializeObject(mostSold);
            foreach (var item in orders)
            {
                item.MostSoldProductId = mostSoldProducts;
            }

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Orders.Count() / pageSize);

            return View(orders);
        }
        /*[Area("Admin")]
        public async Task<IActionResult> Checkout(int p = 1)
        {
            int pageSize = 6;
            var orders = context.Orders.OrderByDescending(x => x.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);
            List<Product> parts = new List<Product>();

            foreach (var item in orders)
            {
                var user = await userManager.FindByIdAsync(item.UserId);
                item.UserId = user.Name;

                var product = context.Products
                               .Where(p => p.Id == item.ProductId)
                               .ToList();
                item.Product = product[0];

                parts.Add(new Product() { Name = product[0].Name, Stock = item.Quantity });
            }
            var grouped = parts.GroupBy(i => i.Name).Select(i => new { ItemId = i.Key, Total = i.Sum(item => item.Stock) });

            

            var test = orders;
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Orders.Count() / pageSize);

            return View(grouped);
        }*/
    }
}