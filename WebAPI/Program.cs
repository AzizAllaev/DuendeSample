
using System.Security.Claims;

namespace WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddRazorPages();
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultScheme = "Cookies";
				options.DefaultChallengeScheme = "oidc";
			})
				.AddCookie("Cookies")
				.AddOpenIdConnect("oidc", options =>
				{
					options.Authority = "https://localhost5001";

					options.ClientId = "web";
					options.ClientSecret = "secret";
					options.ResponseType = "code";

					options.Scope.Clear();
					options.Scope.Add("openid");
					options.Scope.Add("profile");

					options.MapInboundClaims = false;
					options.DisableTelemetry = true;

					options.SaveTokens = true;
				});

			builder.Services.AddControllers();
			// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
			builder.Services.AddOpenApi();

			builder.Services.AddAuthentication().AddJwtBearer(options =>
			{
				options.Authority = "https://localhost:5001";
				options.TokenValidationParameters.ValidateAudience = false;
			});

			builder.Services.AddAuthorization();

			var app = builder.Build();

			app.MapGet("identity", (ClaimsPrincipal user) => 
			user.Claims.Select(c => new { c.Type, c.Value })).RequireAuthorization();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.MapOpenApi();
			}


			app.UseHttpsRedirection();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
