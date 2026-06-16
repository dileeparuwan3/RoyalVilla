using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Services;

namespace RoyalVillaWeb.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;

        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaList = new List<VillaDTO>();

            try
            {
                var response = await _villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>();
                if (response != null && response.Success && response.Data != null)
                {
                    villaList = response.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occured:{ex.Message}";
            }
            return View(villaList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(VillaCreateDTO villaCreateDTO)
        {
            if (ModelState.IsValid)
            {
                return View(villaCreateDTO);
            }

            try
            {
                var response = await _villaService.CreateAsync<ApiResponse<VillaDTO>>(villaCreateDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }

            return View(villaCreateDTO);

        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid villa ID";
                return RedirectToAction(nameof(Index));
            }


            try
            {

                var response = await _villaService.GetAsync<ApiResponse<VillaDTO>>(id);
                if (response != null && response.Success && response.Data != null)
                {

                    return View(response.Data);
                }

            }
            catch (Exception ex)
            {

                TempData["error"] = $"An error occured: {ex.Message}";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(VillaDTO villaDTO)
        {
            try
            {

                var response = await _villaService.DeleteAsync<ApiResponse<object>>(villaDTO.Id);
                if (response != null && response.Success)
                {
                    TempData["success"] = "Villa deleted successfully";
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                TempData["error"] = "Invalid villa Id";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var response = await _villaService.GetAsync<ApiResponse<VillaDTO>>(id);
                if (response != null && response.Success && response.Data != null)
                {
                    return View(_mapper.Map<VillaUpdateDTO>(response.Data));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                var response = await _villaService.UpdateAsync<ApiResponse<object>>(villaUpdateDTO);
                if (response != null && response.Success && response.Data != null)
                {
                    TempData["success"] = "Villa Updated successfully";
                }

            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }



    }
}
