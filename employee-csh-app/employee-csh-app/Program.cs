using employee_csh_app.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registering MVC services to handle controllers and views
builder.Services.AddControllersWithViews();

// Registering HttpClient for use in TimeEntryService
builder.Services.AddHttpClient<TimeEntryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // Use an exception handler page in non-development environments
    app.UseExceptionHandler("/Shared/Error");
    // Use HSTS (HTTP Strict Transport Security) in non-development environments
    app.UseHsts();
}

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Enable serving static files (e.g., CSS, JavaScript, images)
app.UseStaticFiles();

// Enable routing to manage the request/response pipeline
app.UseRouting();

// Enable authorization (currently not configured)
app.UseAuthorization();

// Map the default controller route to the TimeEntry controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TimeEntry}/{action=Index}/{id?}");

// Run the web application
app.Run();