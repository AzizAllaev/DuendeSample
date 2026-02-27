namespace WebApp
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

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();
			app.UseAuthorization();

			app.MapRazorPages().RequireAuthorization();

			app.MapStaticAssets();
			app.MapRazorPages()
			   .WithStaticAssets();

			app.Run();
		}
	}
}
