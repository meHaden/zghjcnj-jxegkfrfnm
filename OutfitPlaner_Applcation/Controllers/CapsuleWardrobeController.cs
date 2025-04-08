using Microsoft.AspNetCore.Mvc;

[Route("CapsuleWardrobe")]
public class CapsuleWardrobeController : Controller
{
    [HttpGet("GenerateCapsules")]
    public IActionResult GenerateCapsules()
    {
        return View("~/Views/Capsule/GenerateCapsules.cshtml");
    }

    // Удалите этот метод, если он есть
    [HttpGet("CapsuleWardrobe")]
    public IActionResult CapsuleWardrobe()
    {
        return RedirectToAction("GenerateCapsules"); // Это может вызывать цикл
    }
}