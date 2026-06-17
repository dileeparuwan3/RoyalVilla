using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla_API.Data;

namespace RoyalVilla_API.Controllers.v2
{
    [Route("api/v{version:apiVersion}/villa")]
    [ApiVersion("2.0")]
    //[ApiExplorerSettings(GroupName = "v2")]
    [ApiController]
    //[Authorize(Roles ="Admin, Customer")]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public VillaController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<string>> GetVillas()
        {
            return "This is V2";
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<string>> GetVillaById(int id)
        {
            return "This is V2 " + id;
        }

    }

}

