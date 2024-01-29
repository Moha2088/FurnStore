using FurnStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FurnStore.Interfaces;

public interface IRent
{
    Task<IActionResult> Rent(int? id);
    
    Task<IActionResult> CancelRent(int? id);
    
    Task<IActionResult> RentedProducts();
    
    Task<IActionResult> ClearList();
    
    Task<List<Product>> GetProducts();
    
    Task<IActionResult> GenPdf();
}