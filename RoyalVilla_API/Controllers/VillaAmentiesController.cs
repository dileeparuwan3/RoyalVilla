using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla_API.Models.DTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoyalVilla_API.Controllers
{
    [Route("api/villa-amenities")]
    [ApiController]
    public class VillaAmentiesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaAmentiesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>),StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetVillaAmenties()
        {
            var amenties = await _db.villaAmenities.ToListAsync();
            var dtoResponseVillaAmenities = _mapper.Map<List<VillaAmenitiesDTO>>(amenties);
            var response = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.Ok(dtoResponseVillaAmenities, "VillaAmenties retieved successfully");
            return Ok(response);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> GetVillaAmentiesById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return NotFound(ApiResponse<object>.NotFound("VillaAmenties ID must be greater than 0"));
                }
                var amenties = _db.villaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (amenties == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"VillaAmenties ID with {id} was not found"));
                }

                var dtoResponseVillaAmenities = _mapper.Map<VillaAmenitiesDTO>(amenties);
                var response = ApiResponse<VillaAmenitiesDTO>.Ok(dtoResponseVillaAmenities, "VillaAmenties retieved successfully");

                return Ok(response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    $"An error occurred while retrieving VillaAmenties with ID {id}: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<ApiResponse<object>>> CreateVillaAmenties(VillaAmenitiesCreateDTO villaAmenitiesCreateDTO)
        {

            try
            {

                if (villaAmenitiesCreateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("VillaAmenities data is required"));
                }

                var duplicateAmenties = await _db.villaAmenities.FirstOrDefaultAsync(u => u.Name.ToLower() == villaAmenitiesCreateDTO.Name.ToLower());

                if (duplicateAmenties != null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"A VillaAmenities with the name '{villaAmenitiesCreateDTO.Name}' already exists"));
                }

                VillaAmenities villaAmenities = _mapper.Map<VillaAmenities>(villaAmenitiesCreateDTO);

                await _db.villaAmenities.AddAsync(villaAmenities);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateVillaAmenties), new { id = villaAmenities.Id }, villaAmenities);
            }
            catch (Exception ex) 
            {
                var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while creating the VillaAmenities :", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }

        }
    }   
    
}
