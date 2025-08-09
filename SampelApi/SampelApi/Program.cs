using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.Title = "The Sample API";
        options.Theme = ScalarTheme.Saturn;
        options.Layout = ScalarLayout.Modern;
        options.HideClientButton = true;    

    });
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

app.Run();
