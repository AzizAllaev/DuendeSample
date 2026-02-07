using Duende.IdentityModel.Client;
using System.Text.Json;

namespace Client
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var client = new HttpClient();

			var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");

			if (disco.IsError)
			{
				Console.WriteLine("Disco error");
				return;
			}
			var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
			{
				Address = disco.TokenEndpoint,
				ClientId = "client",
				ClientSecret = "secret",
				Scope = "api1"
			});
			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				Console.WriteLine(tokenResponse.ErrorDescription);
				return;
			}
			Console.WriteLine(tokenResponse.AccessToken);

			var apiClient = new HttpClient();
			apiClient.SetBearerToken(tokenResponse.AccessToken!);

			var response = await apiClient.GetAsync("https://localhost:6001/identity");

			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
				Console.WriteLine(JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true }));
			}
		}
	}
}
