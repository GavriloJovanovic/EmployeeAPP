using employee_csh_app.Models;

using employee_csh_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace employee_csh_app.Services
{
    /**
      * Service class responsible for fetching and processing time entry data.
      */
    public class TimeEntryService
    {
        private readonly HttpClient _httpClient;

        /**
          * Constructor that initializes the HttpClient used for making API requests.
          */
        public TimeEntryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /**
         * Asynchronously retrieves time entries from a remote API, groups them by employee,
         * and calculates the total hours worked by each employee.
         */
        public async Task<List<TimeEntry>> GetTimeEntriesAsync()
        {
            var url = "https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ==";

            // Making a GET request to the API and retrieving the response as a JSON string
            var response = await _httpClient.GetStringAsync(url);

            // Deserializing the JSON response into a list of TimeEntry objects
            var timeEntries = JsonConvert.DeserializeObject<List<TimeEntry>>(response);

            // Grouping time entries by EmployeeName, calculating total hours worked, and rounding to the nearest hour
            var groupedEntries = timeEntries
                .Where(e => !string.IsNullOrWhiteSpace(e.EmployeeName))
                .GroupBy(e => e.EmployeeName)
                .Select(g => new TimeEntry
                {
                    EmployeeName = g.Key,
                    TotalHoursWorked = Math.Round(g.Sum(e => (e.EndTimeUtc - e.StarTimeUtc).TotalHours), 0) 
                })
                .OrderByDescending(e => e.TotalHoursWorked) 
                .ToList();

            return groupedEntries;
        }
    }
}