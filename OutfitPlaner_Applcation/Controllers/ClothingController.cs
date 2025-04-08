using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutfitPlaner_Applcation.Data;
using OutfitPlaner_Applcation.Models;
using System.Security.Claims;

[Authorize]
[Route("ProfileWardrobe/Clothing")]
public class ClothingController : Controller
{
    private readonly WardrobeDbContext _context;
    private readonly ILogger<ClothingController> _logger;

    public ClothingController(
        WardrobeDbContext context,
        ILogger<ClothingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Попытка загрузки страницы с одеждой");

        try
        {
            // Получаем email текущего пользователя
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                _logger.LogWarning("Email пользователя не найден");
                return Unauthorized();
            }

            _logger.LogInformation($"Найден email пользователя: {userEmail}");

            // Ищем пользователя в БД
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail);

            if (dbUser == null)
            {
                _logger.LogWarning("Пользователь не найден для email: {Email}", userEmail);
                return Unauthorized("Пользователь не найден");
            }

            _logger.LogInformation($"Найден пользователь с ID: {dbUser.Id}");

            // Загружаем только одежду пользователя, без капсул
            var clothingItems = await _context.Clothing
                .Where(c => c.IdUser == dbUser.Id)
                .OrderByDescending(c => c.AddedAt)
                .ToListAsync();

            return View("~/Views/ProfileWardrobe/Clothing.cshtml", clothingItems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка в Clothing/Index");
            throw;
        }
    }
}