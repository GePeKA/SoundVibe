using Application.Extensions;
using Application.MapperProfiles;
using DataAccess.Extensions;
using Infrastructure;
using Infrastructure.Options;
using WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithBearer();
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddValidators();
builder.Services.AddAutoMapper(typeof(UserProfile), typeof(ContentProfile));
builder.Services.AddInfrastructure();
builder.Services.AddAuthorization();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCorsWithFrontendPolicy();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
