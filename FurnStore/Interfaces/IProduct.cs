using Microsoft.AspNetCore.Mvc;

namespace FurnStore.Interfaces;

public interface IProduct
{
    Task<IActionResult> Dashboard();
    
    //Task<IActionResult> LuxuryGet();
}