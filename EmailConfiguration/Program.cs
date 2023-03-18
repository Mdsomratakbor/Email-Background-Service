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
app.MapMethods("/background", new[] { "PATCH" }, (
    PeriodicHostedServiceState state,
    EmailBackgroundService service) =>
{
    service.IsEnabled = state.IsEnabled;
});

app.MapMethods("/cancel", new[] {"PUT"}, (PeriodicHostedServiceState state,
    EmailBackgroundService service) =>
{
    service.IsEnabled = state.IsEnabled;
    service.Second = TimeSpan.FromSeconds(state.Time);
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
