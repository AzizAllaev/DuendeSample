using Duende.IdentityModel.Client;

namespace Client
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var client = new HttpClient();

			var disco = await client.GetDiscoveryDocumentAsync();

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
			if(tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				Console.WriteLine(tokenResponse.ErrorDescription);
				return;
			}
			Console.WriteLine(tokenResponse.AccessToken);

			var apiClient = new HttpClient();
			apiClient.SetBearerToken(tokenResponse.AccessToken!);

			var response = await apiClient.GetAsync("https://localhost:5001");

			if (!response.IsSuccessStatusCode)
			{

			}
		}
	}
}
