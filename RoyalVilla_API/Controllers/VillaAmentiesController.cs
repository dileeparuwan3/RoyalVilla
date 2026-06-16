using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoyalVilla_API.Data;
using RoyalVilla_API.Models;
using RoyalVilla.DTO;

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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VillaAmenitiesDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<VillaAmenitiesDTO>>>> GetVillaAmenties()
        {
            var amenties = await _db.villaAmenities.ToListAsync();
            var dtoResponseVillaAmenities = _mapper.Map<List<VillaAmenitiesDTO>>(amenties);
            var response = ApiResponse<IEnumerable<VillaAmenitiesDTO>>.Ok(dtoResponseVillaAmenities, "VillaAmenties retieved successfully");
            return Ok(response);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> CreateVillaAmenties(VillaAmenitiesCreateDTO villaAmenitiesCreateDTO)
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


        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<VillaAmenitiesDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<ApiResponse<VillaAmenitiesDTO>>> UpdateVillaAmenities(int id, VillaAmenitiesUpdateDTO villaAmenitiesUpdateDTO)
        {
            try
            {
                if (villaAmenitiesUpdateDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities data is required"));
                }

                if (id != villaAmenitiesUpdateDTO.Id)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Villa Amenities ID in the URL does not match VillaAmenities Id in request body."));
                }

                var villExists = await _db.villas.FirstOrDefaultAsync(u => u.Id == villaAmenitiesUpdateDTO.VillaId);

                if (villExists == null)
                {
                    return Conflict(ApiResponse<object>.Conflict($"Villa with the ID {villaAmenitiesUpdateDTO.VillaId} does not exist"));
                }

                var existingVillaAmenities = await _db.villaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if (existingVillaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with the ID {id} was not found"));
                }

                _mapper.Map(villaAmenitiesUpdateDTO, existingVillaAmenities);
                existingVillaAmenities.UpdatedDate = DateTime.Now;
                await _db.SaveChangesAsync();

                var response = ApiResponse<VillaAmenitiesDTO>.Ok(_mapper.Map<VillaAmenitiesDTO>(existingVillaAmenities), "Villa Amenities updated successfully");

                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while updating the VillaAmenities :", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);

            }

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse<object>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteVillaAmenities(int id)
        {
            try
            {
                if(id<0)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("ID is not valid"));
                }

                var villaAmenities = await _db.villaAmenities.FirstOrDefaultAsync(u => u.Id == id);

                if(villaAmenities == null)
                {
                    return NotFound(ApiResponse<object>.NotFound($"Villa Amenities with ID {id} was not found"));
                }

                _db.villaAmenities.Remove(villaAmenities);
                await _db.SaveChangesAsync();
                var response = ApiResponse<object>.NotContent("Villa Amenities deleted successfully");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(StatusCodes.Status500InternalServerError, $"An error occurred while deleting the VillaAmenities :", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
        }
    }

}
