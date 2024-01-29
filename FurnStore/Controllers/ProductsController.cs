using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FurnStore.Data;
using FurnStore.Interfaces;
using FurnStore.Models;
using Microsoft.AspNetCore.Authorization;
using ScottPlot;

namespace FurnStore.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller, IProduct
    {
        private readonly FurnStoreContext _context;

        private readonly ILogger<ProductsController> _logger;

        public ProductsController(FurnStoreContext context, ILogger<ProductsController> logger) => 
            (_context, _logger) = (context, logger);
   

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewData["ProductCount"] = _context.Product.Count();

            return _context.Product != null ?
                        View(await _context.Product.ToListAsync()) :
                        Problem("Entity set 'FurnStoreContext.Product'  is null.");
        }

        public async Task<IActionResult> Dashboard()
        {

            _logger.LogInformation("New Constructor is working!");

            var users = await _context.Users
                .ToListAsync();

            var rented = await _context.Product
                .Where(x => x.Rentee != null)
                .ToListAsync();

            var leastFreqMaterial = rented
                .Select(x => x.Material)
                .ToList()
                .GroupBy(x => x.ToString())
                .OrderBy(x => x.Count())
                .First()
                .Key;

            var mosFreqMaterial = rented
                .Select(x => x.Material)
                .ToList()
                .GroupBy(material => material.ToString())
                .OrderByDescending(x => x.Count())
                .First()
                .Key;


            _logger.LogInformation($"The value of the mosFreqMaterial is: {mosFreqMaterial} Least freq: {leastFreqMaterial}");

            decimal totalSum = 0;
            rented.ForEach(x => totalSum += x.Price);

            var rentedCount = Convert.ToDouble(rented.Count());
            var totalCount = Convert.ToDouble(_context.Product.Count());
            var rentedPct = rentedCount / totalCount * 100;

            var barPlot = new Plot();
            double[] values = { rentedCount, totalCount };
            barPlot.Add.Bars(values);
            barPlot.Axes.Margins(bottom: 0);

            Tick[] states =
            {
                new (0,"Rented"),
                new (1, "Total"),
            };

            barPlot.Axes.Bottom.TickGenerator = new ScottPlot.TickGenerators.NumericManual(states);
            barPlot.Axes.Bottom.MajorTickStyle.Length = 0;
            barPlot.SavePng("wwwroot/Images/BarPlot.png", 400, 300);

            ViewData["RentedPct"] = $"{Math.Round(rentedPct)}%";
            ViewData["TotalSum"] = $"{totalSum} .kr";
            ViewData["MostPopularMat"] = mosFreqMaterial;
            ViewData["LeastPopularMat"] = leastFreqMaterial;
            ViewData["RentedItemsCount"] = rented.Count;
            return View(users);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Material,Price,ShippingPrice,ImageUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Material,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'FurnStoreContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // public async Task<IActionResult>LuxuryGet()
        // {
        //
        //     return;
        // }
    }
}