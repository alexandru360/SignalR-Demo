using SignalRDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
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

app.UseRouting();

//Not working yet
// app.UseCors(b => b
//     .AllowAnyOrigin()
//     .AllowAnyMethod()
//     .AllowAnyHeader());

app.UseCors(b => b
    .WithOrigins("https://localhost:3000")
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseEndpoints(e => e.MapControllers());
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<MessageHub>("/MessageHub");
    // endpoints.MapHub<MessageHub>("/MessageHub")
    //     .RequireCors(h =>
    //         h.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

app.MapControllers();

app.Run();