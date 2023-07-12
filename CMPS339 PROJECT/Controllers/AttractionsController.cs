using CMPS339_PROJECT.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CMPS339_PROJECT.Models;


namespace CMPS339_PROJECT.Controllers
{

    [ApiController]
    [Route("/api/attractions")]
    public class AttractionsController : ControllerBase
    {
        private readonly IAttractionsService _attractionsService;
        private readonly ILogger<AttractionsController> _logger;

        public AttractionsController(IAttractionsService attractionsService, ILogger<AttractionsController> logger)
        {
            _attractionsService = attractionsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<Attraction> attractions = await _attractionsService.GetAllAsync();

            return Ok(attractions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(int id)
        {
            Attraction? attraction = await _attractionsService.GetByIdAsync(id);

            if (attraction != null)
            {
                return Ok(attraction);
            }
            return NotFound();

        }
    }
}
