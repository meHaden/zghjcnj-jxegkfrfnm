using Microsoft.AspNetCore.Mvc;

[Route("ProfileWardrobe/Clothing")]
public class ClothingController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        return View("~/Views/ProfileWardrobe/Clothing.cshtml");
    }
}