using EFCoreDatabaseFirstSample.Models;
using EFCoreDatabaseFirstSample.Models.DataManager;
using EFCoreDatabaseFirstSample.Models.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
// DB Context
builder.Services.AddDbContext<BookStoreContext>(opts => opts.UseSqlServer(builder.Configuration.GetConnectionString("BookStore")));

//Repository
builder.Services.AddScoped<IDataRepository<Author>, AuthorDataManager>();
builder.Services.AddScoped<IDataRepository<Book>, BookDataManager>();
builder.Services.AddScoped<IDataRepository<Publisher>, PublisherDataManager>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
