using Microsoft.EntityFrameworkCore;
using FinancePlannerAPI.Domain.Interfaces;
using FinancePlannerApi.Infrastructure;
using FinancePlannerAPI.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<ITransactionService, TransactionService>();
builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
builder.Services.AddTransient<IFinancialGoalRepository, FinancialGoalRepository>();
builder.Services.AddTransient<IFinancialGoalService, FinancialGoalService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddDbContext<FinancePlannerContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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
