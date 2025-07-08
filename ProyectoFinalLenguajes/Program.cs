using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProyectoFinalLenguajes.Data;
using ProyectoFinalLenguajes.Data.Repository;
using ProyectoFinalLenguajes.Data.Repository.Interface;
using ProyectoFinalLenguajes.Models;
using ProyectoFinalLenguajes.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

//cors
builder.Services.AddCors(opts =>
  opts.AddPolicy("ClienteWeb", p => p
    .WithOrigins("https://clientewebddf.netlify.app", "https://cliente-web-jade.vercel.app")
    .AllowAnyHeader()
    .AllowAnyMethod()
  )
);


builder.Services.AddAuthentication()
  .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOpts =>
  {
      jwtOpts.SaveToken = true;
      jwtOpts.RequireHttpsMetadata = true;
      jwtOpts.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = builder.Configuration["Jwt:Issuer"],
          ValidAudience = builder.Configuration["Jwt:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(
                                     Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
      };
  });

builder.Services.AddAuthorization();
builder.Services.AddControllers();    
builder.Services.AddRazorPages();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("ClienteWeb");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Kitchen}/{controller=Home}/{action=Index}/{id?}");

app.Run();
