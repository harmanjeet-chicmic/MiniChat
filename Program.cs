using MiniChat.Server.Data;
using MiniChat.Server.Hubs;
using MiniChat.Server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(_ => true);
    });
});


builder.Services.AddDbContext<ChatDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton<UserConnectionService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
