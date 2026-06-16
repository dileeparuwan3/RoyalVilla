using RoyalVilla.DTO;
using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private const string API_ENDPOINT = "/api/villa";
        public VillaService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
            //_villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = villaCreateDTO,
                Url = $"{API_ENDPOINT}"
            });
        }

        public Task<T?> DeleteAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.DELETE,
                Url = $"{API_ENDPOINT}/{id}"
            });
        }

        public Task<T?> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = $"{API_ENDPOINT}"
            });
        }

        public Task<T?> GetAsync<T>(int id)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.GET,
                Url = $"{API_ENDPOINT}/{id}"
            });
        }

        public Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTO)
        {

            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.PUT,
                Data = villaUpdateDTO,
                Url = $"{API_ENDPOINT}/{villaUpdateDTO.Id}"
            });
        }
    }
}
