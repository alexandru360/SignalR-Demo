using SignalRDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI
// at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors(b => b
    .SetIsOriginAllowed(x => true)
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.MapHub<MessageHub>("/MessageHub")
    .RequireCors(h =>
        h.SetIsOriginAllowed(x => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

app.MapControllers();

app.Run();