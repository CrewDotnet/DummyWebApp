using System.Text;
using System.Text.Json;

namespace ShopifyApiClient
{
    //public interface IShopifyClient 
    public class ShopifyClient
    {
        private readonly HttpClient _httpClient;

        public ShopifyClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> GetSingleProductAsync(string productId)
        {
            var requestBody = new
            {
                product_id = productId
            };

            var jsonContent =
                new StringContent(JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json");

            var response = await _httpClient.PostAsync("getSingleProduct", jsonContent);

            if(response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            return null;
        }
    }
}
