using RoyalVilla.DTO;

namespace RoyalVillaWeb.Services
{
    public interface IVillaService
    {
        Task<T?> GetAllAsync<T>();
        Task<T?> GetAsync<T>(int id);
        Task<T?> CreateAsync<T>(VillaCreateDTO villaCreateDTO);
        Task<T?> UpdateAsync<T>(VillaUpdateDTO villaUpdateDTOn);
        Task<T?> DeleteAsync<T>(int id);

    }
}
