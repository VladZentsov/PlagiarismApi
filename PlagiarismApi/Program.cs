using AutoMapper;
using DAL.DBContext;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL;
using Plagiarism_BLL.RepositoryInterfaces;
using Plagiarism_BLL.Services;
using Plagiarism_BLL.Services.Interfaces;
using Plagiarism_BLL.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IPlagiarismDBContext, PlagiarismDBContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("PlagiarismConnectionString")));

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutomapperProfile());
    cfg.AddProfile(new PlagiarismApi.AutomapperProfile());

}).CreateMapper());

builder.Services.AddScoped<IWorkRepository, WorkRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkInfoRepository, WorkInfoRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWorkService, WorkService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
