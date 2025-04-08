using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutfitPlaner_Applcation.Data;
using OutfitPlaner_Applcation.Models;
using System.Security.Claims;
using System.Threading.Tasks;

[Route("CapsuleWardrobe")]
[Authorize]
public class CapsuleWardrobeController : Controller
{
    private readonly WardrobeDbContext _context;

    public CapsuleWardrobeController(WardrobeDbContext context)
    {
        _context = context;
    }
    // Для проверки вывода всех изображений со страницы Clothing

    [HttpGet("GenerateCapsules")]
    public async Task<IActionResult> GenerateCapsules()
    {
        var userId = GetCurrentUserId();
        var userClothes = await _context.Clothing
            .Where(c => c.IdUser == userId)
            .ToListAsync();

        return View("~/Views/Capsule/GenerateCapsules.cshtml", userClothes);
    }

    [HttpGet("CapsuleWardrobe")]
    public IActionResult CapsuleWardrobe()
    {
        return RedirectToAction("GenerateCapsules"); //  иначе не открывается страница, другой адрес выдает
    }

    private int GetCurrentUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}