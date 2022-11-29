using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using PlatformService.Configurations;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly IHttpClientFactory clientFactory;
    private readonly IOptions<CommandServiceOptions> options;

    public HttpCommandDataClient(IHttpClientFactory clientFactory, IOptions<CommandServiceOptions> options)
    {
        this.clientFactory = clientFactory;
        this.options = options;
    }
    public async Task SendPlaformToCommand(PlatformReadDto dto)
    {
        var httpContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");

        var client = this.clientFactory.CreateClient();

        try
        {
            var response = await client.PostAsync($"{this.options.Value.BaseUrl}/api/c/platforms", httpContent);

            if (response.IsSuccessStatusCode)
                Console.WriteLine("--> Sync POST to CommandService was successful!");
            else
                Console.WriteLine("--> Sync POST To CommandService wasn't ok");
        }
        catch(Exception)
        {
            throw new HttpRequestException();
        }
    }
}
