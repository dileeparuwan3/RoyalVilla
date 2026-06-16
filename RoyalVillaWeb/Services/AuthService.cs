using RoyalVilla.DTO;
using RoyalVillaWeb.Models;

namespace RoyalVillaWeb.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private const string API_ENDPOINT = "/api/auth";
        public AuthService(IHttpClientFactory httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor)
        {
            //_villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T?> LoginAsync<T>(LoginRequestDTO loginRequestDTO)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDTO,
                Url = API_ENDPOINT + "/login"
            });
        }

        public Task<T?> RegisterAsync<T>(RegisterationRequestDTO registertaionRequestDTO)
        {
            return SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = registertaionRequestDTO,
                Url = API_ENDPOINT + "/register"
            });
        }
    }
}
