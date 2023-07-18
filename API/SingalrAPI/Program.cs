using SingalrAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string signalR_ConnectionString = builder.Configuration.GetSection("signalR_ConnectionString").Value;

builder.Services.AddSignalR()
        .AddAzureSignalR(options =>
        {
            options.ConnectionString = signalR_ConnectionString;
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
});

app.UseRouting();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ApplicationHub>("/ApplicationHub");
});

app.MapControllers();


app.Run();
