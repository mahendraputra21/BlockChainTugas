using Microsoft.AspNetCore.Mvc;

public class OwnerController : Controller
{
    public IActionResult Dashboard()
    {
        // Menonaktifkan cache di browser
        Response.Headers["Cache-Control"] = "no-store";
        Response.Headers["Pragma"] = "no-cache";
        Response.Headers["Expires"] = "-1";

        if (string.IsNullOrEmpty(HttpContext.Session.GetString("Username")))
            return RedirectToAction("Login", "Account");

        return View();
    }
}
