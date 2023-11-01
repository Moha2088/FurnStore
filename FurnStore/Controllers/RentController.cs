﻿using System.Collections.Immutable;
using System.Security.Claims;
using FurnStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnStore.Controllers
{
    public class RentController : Controller
    {
        private readonly FurnStoreContext _context;

        public RentController(FurnStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _context.Product
                .Where(p => p.Rentee == null)
                .ToListAsync();

            ViewData["ProductCount"] = product.Count();
          
           return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Product.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            product.Rentee = userId;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (product.Rentee == userid)
            {
                product.Rentee = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(RentedProducts));
        }
        
        public async Task<IActionResult> RentedProducts()
        {
            string userid = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var product = await _context.Product
                .Where(p => p.Rentee == userid)
                .ToListAsync();

            ViewData["ProductCount"] = product.Count();
            return View(product);
        }
    }
}