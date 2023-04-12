using Microsoft.AspNetCore.Mvc;

public class Blackjack : Controller
{
    public IActionResult Index()
    {
        return View("Index");
    }
}