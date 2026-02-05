using Duende.IdentityModel.Client;

namespace Client
{
	internal class Program
	{
		static async Task Main(string[] args)
		{
			var client = new HttpClient();


			var disco = await client.GetDiscoveryDocumentAsync();
		}
	}
}
