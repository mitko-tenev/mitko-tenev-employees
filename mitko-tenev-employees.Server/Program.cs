using mitko_tenev_employees.Server.Services;
using mitko_tenev_employees.Server.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// TODO: Rename policy
var EmployeesFrontendAppOrigins = "_employeesFrontendAppOrigins";
var frontendUrl = builder.Configuration.GetValue<string>("FrontendUrl");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: EmployeesFrontendAppOrigins,
        policy =>
        {
            policy
                .WithOrigins(frontendUrl)
                .WithMethods("GET", "POST");
        });
});

// Add services to the container.
builder.Services.AddTransient<ICsvParserService, CsvParserService>();
builder.Services.AddTransient<IEmployeeProjectService, EmployeeProjectService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(EmployeesFrontendAppOrigins);

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
