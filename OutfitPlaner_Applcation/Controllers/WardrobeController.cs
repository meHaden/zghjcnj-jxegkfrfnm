using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OutfitPlaner_Applcation.Data;
using OutfitPlaner_Applcation.Models;
using System.ComponentModel.DataAnnotations;

namespace OutfitPlaner_Applcation.Controllers
{
    [Authorize]
    public class WardrobeController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly WardrobeDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WardrobeController(
            IWebHostEnvironment environment,
            WardrobeDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _environment = environment;
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AddClothingItem()
        {
            return View();
        }

        // Новый метод для просмотра вещей по категориям
        [HttpGet]
        public async Task<IActionResult> Category(string category)
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null) return Unauthorized();

            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == identityUser.Email);
            if (dbUser == null) return Unauthorized("Пользователь не найден");

            var items = await _context.Clothing
                .Where(c => c.IdUser == dbUser.Id && c.Type == category)
                .OrderByDescending(c => c.AddedAt)
                .ToListAsync();

            ViewBag.CategoryName = category;
            return View(items);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddClothingItem(
            [Required] string itemType,
            [Required] IFormFile imageFile,
            [Required] string color,
            [Required] string style,
            [Required] string material,
            [Required] string season,
            [Range(1, 5)] int condition)
        {
            if (!ModelState.IsValid)
            {
                // Возвращаем ошибки 
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new
                {
                    success = false,
                    message = "Ошибки валидации",
                    errors = errors
                });
            }

            try
            {
                
                if (imageFile.Length > 5 * 1024 * 1024)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Размер изображения не должен превышать 5 МБ"
                    });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Допустимы только файлы JPG, JPEG и PNG"
                    });
                }

                // Сохранение изображения
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "clothing");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                var imageUrl = $"/uploads/clothing/{uniqueFileName}";

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

              
                var identityUser = await _userManager.GetUserAsync(User);
                if (identityUser == null)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Пользователь не авторизован"
                    });
                }

                // Находим пользователя 
                var dbUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == identityUser.Email);

                if (dbUser == null)
                {
                    return Unauthorized(new
                    {
                        success = false,
                        message = "Пользователь не найден в основной базе"
                    });
                }

                // Создаем запись об одежде
                var clothing = new Clothing
                {
                    IdUser = dbUser.Id,
                    Type = itemType,
                    ImageUrl = imageUrl,
                    Color = color,
                    Style = style,
                    Material = material,
                    Season = season,
                    Condition = condition,
                    AddedAt = DateTime.UtcNow
                };

                _context.Clothing.Add(clothing);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    success = true,
                    category = itemType,
                    message = "Вещь успешно добавлена в гардероб"
                });
            }
            catch (Exception ex)
            {
             
                Console.WriteLine($"Ошибка при добавлении вещи: {ex}");

                return StatusCode(500, new
                {
                    success = false,
                    message = "Произошла внутренняя ошибка сервера",
                    detailedError = ex.Message
                });
            }
        }

        [HttpGet]
        public IActionResult Clothing()
        {
            return View();
        }

    }
}