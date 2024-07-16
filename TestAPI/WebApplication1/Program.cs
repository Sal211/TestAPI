using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication1.Models;
using WebApplication1.Models.Connection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
// Basic Auth
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new BasicAuth());
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Connectsting
ClsConstring.Constr = builder.Configuration.GetSection("ConnectionString").Value.ToString(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
