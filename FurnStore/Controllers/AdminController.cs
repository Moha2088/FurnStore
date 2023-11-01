using FurnStore.Data;
using Microsoft.AspNetCore.Mvc;

namespace FurnStore.Controllers;

public class AdminController : Controller
{
    private FurnStoreContext _context;

    public AdminController(FurnStoreContext context)
    {
        _context = context;
    }
    
    // GET
    public IActionResult Index()
    {
        ViewData["ProductCount"] = _context.Product.Count();
            
        return View();
    }
}