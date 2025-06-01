using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StudentAssignmentManager.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        ViewBag.Message = "Student Assignment Management System.";
        return View();
    }

    public IActionResult Contact()
    {
        ViewBag.Message = "Your contact page.";
        return View();
    }
}