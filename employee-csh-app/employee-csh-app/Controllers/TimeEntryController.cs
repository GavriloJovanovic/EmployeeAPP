using employee_csh_app.Services;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using System.Drawing;
using System.Drawing.Imaging;

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


        // Updated action to generate a pie chart with names and percentages
        public async Task<IActionResult> PieChart()
        {
            var timeEntries = await _timeEntryService.GetTimeEntriesAsync();

            // Calculate total hours worked by all employees
            var totalHours = timeEntries.Sum(te => te.TotalHoursWorked);

            // Create an SKBitmap image
            using var bitmap = new SKBitmap(800, 600);
            using var canvas = new SKCanvas(bitmap);
            canvas.Clear(SKColors.White);

            // Define colors for the pie slices
            var colors = new[] { SKColors.Red, SKColors.Blue, SKColors.Green, SKColors.Yellow, SKColors.Purple, SKColors.Orange, SKColors.Pink, SKColors.Brown, SKColors.Cyan, SKColors.Magenta };
            var colorIndex = 0;

            // Draw the pie chart
            var currentAngle = 0f;
            var radius = 150f;
            var centerX = 400f; 
            var centerY = 300f; 

            foreach (var entry in timeEntries)
            {
                var percentage = (float)(entry.TotalHoursWorked / totalHours * 100);
                var sweepAngle = (float)(entry.TotalHoursWorked / totalHours * 360);

                // Draw pie slice
                using var paint = new SKPaint { Color = colors[colorIndex], IsAntialias = true };
                canvas.DrawArc(new SKRect(centerX - radius, centerY - radius, centerX + radius, centerY + radius), currentAngle, sweepAngle, true, paint);

                // Calculate label position (midpoint of the slice, adjusted radius)
                var midAngle = currentAngle + sweepAngle / 2;
                var labelX = centerX + (float)((radius + 40) * Math.Cos(midAngle * Math.PI / 180));
                var labelY = centerY + (float)((radius + 40) * Math.Sin(midAngle * Math.PI / 180)); 

                // Draw employee name and percentage
                using var textPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    TextSize = 12, // Reduced text size
                    IsAntialias = true,
                    TextAlign = SKTextAlign.Center
                };
                canvas.DrawText($"{entry.EmployeeName} - {percentage:F1}%", labelX, labelY, textPaint);

                // Move to the next slice
                currentAngle += sweepAngle;
                colorIndex = (colorIndex + 1) % colors.Length;
            }

            // Save the image to a memory stream
            using var ms = new System.IO.MemoryStream();
            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            data.SaveTo(ms);
            ms.Seek(0, System.IO.SeekOrigin.Begin);

            // Return the image as a FileResult
            return File(ms.ToArray(), "image/png");
        }
    }

}
