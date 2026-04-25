using Serilog;
using Thinka.API.DependencyInjection;
using Thinka.API.Hubs;
using Thinka.API.Middlewares;
using Thinka.Application.DependencyInjection;
using Thinka.DAL.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => 
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddApiLayer();
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddChat();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chathub").RequireAuthorization();

app.Run();
