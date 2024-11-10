using Microsoft.AspNetCore.Mvc;

namespace Assignment2.Controllers
{
    public abstract class BaseController : Controller
    {
        public string GenerateWelcomeMessage()
        {
            var finalMessage = string.Empty;

            // Check if the "FirstVisit" cookie exists
            if (!Request.Cookies.ContainsKey("FirstVisit"))
            {
                // If not, set it with the current timestamp
                var firstVisitTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Response.Cookies.Append("FirstVisit", firstVisitTime, new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1) // Set the cookie to expire in 1 year
                });

                // Set the welcome message for the first visit
                finalMessage = "Hey, welcome to the Course Manager App!";
            }
            else
            {
                // If the cookie exists, retrieve the timestamp and set the welcome back message
                var firstVisitTime = Request.Cookies["FirstVisit"];
                finalMessage = $"Welcome back! You first used this app on {firstVisitTime}.";
            }



            return finalMessage;
        }
    }
}
