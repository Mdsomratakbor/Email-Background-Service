using EmailConfiguration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SampleService>();
builder.Services.AddSingleton<EmailBackgroundService>();
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<EmailBackgroundService>());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapGet("/background", (
    EmailBackgroundService service) =>
{
    return new PeriodicHostedServiceState(service.IsEnabled);
});
app.MapMethods("/background", new[] { "PATCH" }, (
    PeriodicHostedServiceState state,
    EmailBackgroundService service) =>
{
    service.IsEnabled = state.IsEnabled;
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
