using GuestCRUD.Data.EF;
using GuestCRUD.Data.Models;
using GuestCRUD.Data.Repository;
using GuestCRUD.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddAuthorization();

var connection = string.Format("Data Source=C:\\src\\storage\\GuestCRUD1.db");

builder.Services.AddDbContextFactory<GuestCRUDDbContext>(options =>
{
    options.UseSqlite(connection);
}); 

var options = new DbContextOptionsBuilder<GuestCRUDDbContext>()
            .UseSqlite(connection)
            .Options;

await using (var sqliteDbContext = new GuestCRUDDbContext(options))
{
    await sqliteDbContext.Database.EnsureCreatedAsync();
    await sqliteDbContext.SaveChangesAsync();
}

builder.Services.AddSingleton<IGuestRepository, GuestRepository>();
//builder.Services.AddSingleton<IProviderRepository, ProviderRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connection));

builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization(); 
app.UseEndpoints(endpoints =>
{
    app.MapGroup("/account").MapIdentityApi<ApplicationUser>();
    app.MapControllers();
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.Run();
