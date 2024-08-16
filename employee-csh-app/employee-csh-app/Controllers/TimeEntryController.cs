using employee_csh_app.Services;
using Microsoft.AspNetCore.Mvc;

namespace employee_csh_app.Controllers
{
    // MVC Controller responsible for handling HTTP requests related to time entries.
    public class TimeEntryController : Controller
    {
        private readonly TimeEntryService _timeEntryService;

        // Constructor that initializes the TimeEntryService used to retrieve time entry data.
        public TimeEntryController(TimeEntryService timeEntryService)
        {
            _timeEntryService = timeEntryService;
        }

        /**
         * Handles the HTTP GET request for the Index action. 
         * Retrieves time entry data and passes it to the view for display.
         */ 
        public async Task<IActionResult> Index()
        {
            var timeEntries = await _timeEntryService.GetTimeEntriesAsync();
            return View(timeEntries);
        }
    }

}
