using System.Security.Claims;
using FurnStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FurnStore.Controllers;

public class AdminController : Controller
{
    private FurnStoreContext _context;

    public AdminController(FurnStoreContext context)
    {
        _context = context;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var product = await _context.Product
        .Where(x => x.Rentee != null)
        .AsNoTracking()
        .ToListAsync();
        
        ViewData["RentCount"] = product.Count;
        return View(product);
    }
}