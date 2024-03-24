using ChefManager.Server.Data;
using ChefManager.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Supabase;
using System.Reflection;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


// Items with * are added by me 

//* Add controllers to the services

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore; // If needed
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//* Add connection to the database
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddScoped<Supabase.Client>(_=>
//    new Supabase.Client(
//        builder.Configuration["SupabaseUrl"],
//        builder.Configuration["SupabaseKey"],
//        new SupabaseOptions
//        {
//            AutoRefreshToken = true,
//            AutoConnectRealtime = true
//        }
//        )
//    );


//* Add Authorization
builder.Services.AddAuthorization();

//* Add Identity , it adds the identitiy api endpoints and tell it to use the the applicationdbcontext
builder.Services.AddIdentityApiEndpoints<AppUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.


//* Add the password requirements
builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();




//* map the identity api endpoints
app.MapIdentityApi<AppUser>();

//* Add logout endpoint as a post request using the sign in manager to sign out the user
app.MapPost("/logout", async (SignInManager<AppUser> signInManager) =>
{

    await signInManager.SignOutAsync();
    return Results.Ok();

}).RequireAuthorization();

//*    Add a pingauth endpoint to get the user's email and name from the claims
app.MapGet("/pingauth", (ClaimsPrincipal user) =>
{
    var id = user.FindFirstValue(ClaimTypes.NameIdentifier); // get the user's id from the claim
    var email = user.FindFirstValue(ClaimTypes.Email); // get the user's email from the claim
    var name = user.FindFirstValue(ClaimTypes.Name); // get the user's name from the claim
    return Results.Json(new { Email = email, Name = name, Id = id }); ; // return the email as a plain text response
}).RequireAuthorization();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//* Map the controllers to the endpoints
app.MapControllers();

app.MapFallbackToFile("/index.html");


app.Run();
